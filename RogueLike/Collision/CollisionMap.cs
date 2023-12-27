using RogueLike.AdditionalTools;
using RogueLike.CustomMath;
using RogueLike.GameObjects;
using RogueLike.Render;

namespace RogueLike.Collision
{
    internal class CollisionMap : Array2DWrapper<List<GameObject>>, IReadOnlyCollisionMap
    {
        public CollisionMap(Vector2Int arraySize) : base(arraySize)
        {
            for (int i = 0; i < Count; i++)
            {
                Array[i] = new List<GameObject>();
            }
        }

        public static CollisionMap GetCollisionMapFromRenderPattern(RenderBuffer renderPattern, GameObject gameObject)
        {
            CollisionMap collisionMap = new CollisionMap(renderPattern.ArraySize);

            for (int y = 0; y < renderPattern.ArraySize.y; y++)
            {
                for (int x = 0; x < renderPattern.ArraySize.x; x++)
                {
                    if (renderPattern[x, y] != RenderBuffer.NullSymbol)
                    {
                        collisionMap[x, y].Add(gameObject);
                    }
                }
            }

            return collisionMap;
        }

        public static CollisionMap GetCollisionMapFromRenderPattern(IReadOnlyRenderBuffer renderPattern, GameObject gameObject)
        {
            return GetCollisionMapFromRenderPattern(renderPattern, gameObject);
        }

        public static CollisionMap GetEmpty()
        {
            return new CollisionMap(Vector2Int.Zero);
        }

        public void InsertPattern(CollisionMap collisionMap, Vector2Int position)
        {
            if (position.y >= ArraySize.y || position.x >= ArraySize.x)
                return;

            for (int y = Math.Clamp(position.y, 0, ArraySize.y - 1);
                y < Math.Clamp(position.y + collisionMap.ArraySize.y, position.y, ArraySize.y);
                y++)
            {
                for (int x = Math.Clamp(position.x, 0, ArraySize.x - 1);
                    x < Math.Clamp(position.x + collisionMap.ArraySize.x, position.x, ArraySize.x);
                    x++)
                {
                    foreach (var item in collisionMap[x - position.x, y - position.y])
                    {
                        this[x, y].Add(item);
                    }
                }
            }
        }

        public void InsertPattern(IReadOnlyCollisionMap collisionMap, Vector2Int position)
        {
            InsertPattern(collisionMap, position);
        }
    }
}
