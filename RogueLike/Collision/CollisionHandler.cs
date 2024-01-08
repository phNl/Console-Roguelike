using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.Game.Levels;
using RogueLike.GameObjects;

namespace RogueLike.Collision
{
    internal enum CollisionMode
    {
        All,
        Only_Colliders,
        Only_Triggers,
    }

    internal static class CollisionHandler
    {
        public static List<GameObject> GetCollisionObjectsAtPoint(Level? level, Vector2Int position, CollisionMode collisionMode = CollisionMode.All)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            if (level == null)
                return gameObjects;

            foreach (var gameObject in level.Objects)
            {
                var collider = gameObject.Collider;

                Vector2Int localPosition = position - gameObject.Position;
                Vector2Int patternSize = collider.CollisionMap.ArraySize;

                if (localPosition.x < 0 || localPosition.x >= patternSize.x ||
                    localPosition.y < 0 || localPosition.y >= patternSize.y)
                {
                    continue;
                }

                if (IsSuitableCollider(collisionMode, collider) && collider.CollisionMap[localPosition])
                {
                    gameObjects.Add(gameObject);
                }
            }

            return gameObjects;
        }

        public static List<GameObject> GetCollisionObjectsAtPoint(Vector2Int pointPosition, CollisionMode collisionMode = CollisionMode.All)
        {
            return GetCollisionObjectsAtPoint(GameController.CurrentLevel, pointPosition, collisionMode);
        }

        public static bool IsGameObjectsCollidersIntersects(GameObject firstGameObject, GameObject secondGameObject)
        {
            Vector2Int firstGOPos1 = firstGameObject.Position;
            Vector2Int firstGOPos2 = firstGameObject.Position + firstGameObject.Collider.CollisionMap.ArraySize;

            Vector2Int secondGOPos1 = secondGameObject.Position;
            Vector2Int secondGOPos2 = secondGameObject.Position + secondGameObject.Collider.CollisionMap.ArraySize;

            return (firstGOPos1.x < secondGOPos2.x && firstGOPos2.x > secondGOPos1.x &&
                firstGOPos1.y < secondGOPos2.y && firstGOPos2.y > secondGOPos1.y);
        }

        /// <summary>
        /// Get collision points on level between two objects
        /// </summary>
        public static List<Vector2Int> GetCollisionPointsBetweenObjects(GameObject firstGameObject, GameObject secondGameObject, bool onlyFirstPoint = true)
        {
            List<Vector2Int> points = new List<Vector2Int>();

            if (!IsGameObjectsCollidersIntersects(firstGameObject, secondGameObject))
            {
                return points;
            }

            double firstGOSize = firstGameObject.Collider.CollisionMap.ArraySize.Magnitude();
            double secongGOSize = secondGameObject.Collider.CollisionMap.ArraySize.Magnitude();

            GameObject smallGO = firstGOSize < secongGOSize ? firstGameObject : secondGameObject;
            GameObject bigGO = smallGO != firstGameObject ? firstGameObject : secondGameObject;

            Vector2Int localBigGOPosition = bigGO.Position - smallGO.Position;

            CollisionMap comparsionCollisionMap = new CollisionMap(smallGO.Collider.CollisionMap.ArraySize);
            comparsionCollisionMap.InsertPattern(smallGO.Collider.CollisionMap, Vector2Int.Zero);
            comparsionCollisionMap.ApplyMask(bigGO.Collider.CollisionMap, localBigGOPosition);

            Vector2Int bigGOEndPosition = localBigGOPosition + bigGO.Collider.CollisionMap.ArraySize;

            for (int x = 0; x < bigGOEndPosition.x && x < comparsionCollisionMap.ArraySize.x; x++)
            {
                for (int y = 0; y < bigGOEndPosition.y && y < comparsionCollisionMap.ArraySize.y; y++)
                {
                    if (comparsionCollisionMap[x, y])
                    {
                        points.Add(new Vector2Int(x, y));
                        if (onlyFirstPoint)
                            return points;
                    }
                }
            }

            return points;
        }

        public static Dictionary<GameObject, List<Vector2Int>> GetCollisionObjects(
            Level? level,
            GameObject gameObject,
            List<GameObject>? excludeGameObjects = null,
            CollisionMode collisionMode = CollisionMode.All
        )
        {
            var objectsWithPoints = new Dictionary<GameObject, List<Vector2Int>>();

            if (level == null)
                return objectsWithPoints;

            List<GameObject> levelObjects = new List<GameObject>(level.Objects);
            levelObjects.Remove(gameObject);
            if (excludeGameObjects != null)
            {
                foreach (var obj in excludeGameObjects)
                {
                    levelObjects.Remove(obj);
                }
            }

            foreach (var obj in levelObjects)
            {
                if (IsSuitableCollider(collisionMode, obj.Collider))
                {
                    List<Vector2Int> collisionPoints = GetCollisionPointsBetweenObjects(gameObject, obj);
                    if (collisionPoints.Count > 0)
                    {
                        objectsWithPoints.Add(obj, collisionPoints);
                    }
                }
            }

            return objectsWithPoints;
        }

        public static Dictionary<GameObject, List<Vector2Int>> GetCollisionObjects(
            GameObject gameObject,
            List<GameObject>? excludeGameObjects = null,
            CollisionMode collisionMode = CollisionMode.All
        )
        {
            return GetCollisionObjects(GameController.CurrentLevel, gameObject, excludeGameObjects, collisionMode);
        }


        public static bool HasCollisionWithAny(
            Level? level,
            GameObject gameObject,
            CollisionMode collisionMode = CollisionMode.All,
            List<GameObject>? excludeGameObjects = null
        )
        {
            if (level == null)
                return false;

            foreach (var obj in level.Objects)
            {
                if (obj == gameObject)
                    continue;

                if (excludeGameObjects != null && excludeGameObjects.Contains(obj))
                    continue;

                if (!IsSuitableCollider(collisionMode, obj.Collider))
                    continue;

                if (GetCollisionPointsBetweenObjects(gameObject, obj).Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasCollisionWithAny(
            GameObject gameObject,
            CollisionMode collisionMode = CollisionMode.All,
            List<GameObject>? excludeGameObjects = null
        )
        {
            return HasCollisionWithAny(GameController.CurrentLevel, gameObject, collisionMode, excludeGameObjects);
        }

        private static bool IsSuitableCollider(CollisionMode collisionMode, Collider collider)
        {
            switch (collisionMode)
            {
                case CollisionMode.All:
                    return true;
                case CollisionMode.Only_Colliders:
                    return !collider.IsTrigger;
                case CollisionMode.Only_Triggers:
                    return collider.IsTrigger;
            }

            throw new NotImplementedException();
        }
    }
}
