using RogueLike.AdditionalTools;
using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.Collision
{
    internal class CollisionMap : Array2DWrapper<bool>, IReadOnlyCollisionMap
    {
        public static CollisionMap Empty => new CollisionMap(Vector2Int.Zero);

        public CollisionMap(Vector2Int arraySize) : base(arraySize)
        {
            for (int i = 0; i < Count; i++)
            {
                Array[i] = false;
            }
        }

        public static CollisionMap GetCollisionMapFromRenderPattern(IReadOnlyRenderBuffer renderPattern)
        {
            CollisionMap collisionMap = new CollisionMap(renderPattern.ArraySize);

            for (int y = 0; y < renderPattern.ArraySize.y; y++)
            {
                for (int x = 0; x < renderPattern.ArraySize.x; x++)
                {
                    collisionMap[x, y] = (renderPattern[x, y] != RenderBuffer.NullSymbol);
                }
            }

            return collisionMap;
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
                    this[x, y] = collisionMap[x - position.x, y - position.y];
                }
            }
        }

        public void InsertPattern(IReadOnlyCollisionMap collisionMap, Vector2Int position)
        {
            InsertPattern(collisionMap, position);
        }
    }
}
