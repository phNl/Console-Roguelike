using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.GameObjects;

namespace RogueLike.Collision
{
    internal static class CollisionHandler
    {
        public static List<GameObject> GetCollisionObjectsAtPoint(Level? level, Vector2Int pointPosition)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            if (level == null)
                return gameObjects;

            foreach (var gameObject in level.Objects)
            {
                var collider = gameObject.Collider;

                Vector2Int localPointPosition = pointPosition - gameObject.Position;
                Vector2Int patternSize = collider.CollisionMap.ArraySize;

                if (localPointPosition.x < 0 || localPointPosition.x >= patternSize.x ||
                    localPointPosition.y < 0 || localPointPosition.y >= patternSize.y)
                {
                    continue;
                }

                if (!collider.IsTrigger && collider.CollisionMap[localPointPosition.x, localPointPosition.y])
                {
                    gameObjects.Add(gameObject);
                }
            }

            return gameObjects;
        }

        public static List<GameObject> GetCollisionObjectsAtPoint(Vector2Int pointPosition)
        {
            return GetCollisionObjectsAtPoint(GameController.CurrentLevel, pointPosition);
        }
    }
}
