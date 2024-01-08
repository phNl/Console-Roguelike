using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Maze;
using RogueLike.Render;

namespace RogueLike.GameObjects
{
    internal class Maze : StaticObject
    {
        public Maze(
            Vector2Int mazeSize,
            char wallSymbol,
            char emptySymbol,
            int? mazeSeed,
            int minCorridorWide
            ) : base(new RenderObject(), new Collider())
        {
            RenderBuffer renderPattern = GenerateMazeRenderPattern(mazeSize, wallSymbol, emptySymbol, mazeSeed, minCorridorWide);
            RenderObject = new RenderObject(renderPattern);
            Collider = new Collider(RenderObject, false);
        }

        public List<Vector2Int> GetEmptyCells()
        {
            List<Vector2Int> emptyCells = new List<Vector2Int>();

            IReadOnlyCollisionMap collisionMap = Collider.CollisionMap;
            for (int x = 0; x < collisionMap.ArraySize.x; x++)
            {
                for (int y = 0; y < collisionMap.ArraySize.y; y++)
                {
                    if (!collisionMap[x, y])
                    {
                        emptyCells.Add(new Vector2Int(x, y));
                    }
                }
            }

            return emptyCells;
        }

        private RenderBuffer GenerateMazeRenderPattern(Vector2Int mazeSize, char wallSymbol, char emptySymbol, int? mazeSeed, int minCorridorWide)
        {
            MazeGenerator mazeGenerator = new MazeGenerator(wallSymbol, emptySymbol);
            return mazeGenerator.GetMazeRenderPattern(mazeSize, minCorridorWide, mazeSeed);
        }
    }
}
