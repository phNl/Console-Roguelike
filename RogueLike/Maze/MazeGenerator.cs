using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.Maze
{
    internal struct DividedMazePart
    {
        public Vector2Int DivisionAxes;
        public string[] MazePartStringPattern;
        public Vector2Int Size;

        public DividedMazePart()
        {
            DivisionAxes = new Vector2Int(-1, -1);
            MazePartStringPattern = new string[0];
            Size = Vector2Int.Zero;
        }

        public DividedMazePart(Vector2Int divisionAxes, string[] mazePartStringPattern, Vector2Int partSize)
        {
            DivisionAxes = divisionAxes;
            MazePartStringPattern = mazePartStringPattern;
            Size = partSize;
        }
    }

    internal class MazeGenerator
    {
        private char _wallSymbol;
        private char _emptySymbol;

        private const int DivisionSectionsCount = 4;

        public MazeGenerator(char wallSymbol, char emptySymbol)
        {
            _wallSymbol = wallSymbol;
            _emptySymbol = emptySymbol;
        }

        public RenderBuffer GetMazeRenderPattern(Vector2Int mazeSize, int minCorridorWide = 1, int? seed = null)
        {
            RenderBuffer mazeRenderPattern = new RenderBuffer(mazeSize);
            mazeRenderPattern.InsertPattern(GetMazeFrameStringPattern(mazeSize), Vector2Int.Zero);

            RenderBuffer innerMazeRenderPattern = GetMazeRenderPatternInner(mazeSize - new Vector2Int(2, 2), minCorridorWide, seed);
            mazeRenderPattern.InsertPattern(innerMazeRenderPattern, new Vector2Int(1, 1));
            
            return mazeRenderPattern;
        }

        private RenderBuffer GetMazeRenderPatternInner(Vector2Int mazeSize, int minCorridorWide, int? seed = null)
        {
            RenderBuffer mazeRenderPattern = new RenderBuffer(mazeSize);

            DividedMazePart dividedMazePart = GetDividedMazePart(mazeSize, minCorridorWide, seed);
            if (dividedMazePart.Size.x <= 0 || dividedMazePart.Size.y <= 0)
                return mazeRenderPattern;

            mazeRenderPattern.InsertPattern(dividedMazePart.MazePartStringPattern, Vector2Int.Zero);

            for (int i = 0; i < DivisionSectionsCount; i++)
            {
                Vector2Int sectorPos = new Vector2Int(i / 2, i % 2);
                Vector2Int partSize = (dividedMazePart.Size * sectorPos - dividedMazePart.DivisionAxes - sectorPos).Abs();

                if (partSize.x <= 2 || partSize.y <= 2) continue;

                mazeRenderPattern.InsertPattern(
                    GetMazeRenderPatternInner(partSize, minCorridorWide, seed),
                    dividedMazePart.DivisionAxes * sectorPos + sectorPos
                    );
            }

            return mazeRenderPattern;
        }

        private string[] GetMazeFrameStringPattern(Vector2Int mazeSize)
        {
            return GetFilledAxesPattern(new int[] { 0, mazeSize.x - 1 }, new int[] { 0, mazeSize.y - 1 }, mazeSize);
        }

        private DividedMazePart GetDividedMazePart(Vector2Int mazeSize, int minCorridorWide, int? seed)
        {
            if (mazeSize.x < minCorridorWide * 2 + 1 || mazeSize.y < minCorridorWide * 2 + 1)
                return new DividedMazePart();

            Random random = seed == null ? new Random() : new Random(seed.Value);
            Vector2Int randomDivisionAxes = new Vector2Int(
                random.Next(minCorridorWide, mazeSize.x - minCorridorWide), 
                random.Next(minCorridorWide, mazeSize.y - minCorridorWide)
                );

            var stringPattern = GetFilledAxesPattern(randomDivisionAxes, mazeSize);
            return new DividedMazePart(randomDivisionAxes, stringPattern, mazeSize);
        }

        private string[] GetFilledAxesPattern(int[] xAxes, int[] yAxes, Vector2Int mazeSize)
        {
            string[] mazeFrameStringPattern = new string[mazeSize.y];

            for (int yi = 0; yi < mazeSize.y; yi++)
            {
                mazeFrameStringPattern[yi] = "";

                for (int xi = 0; xi < mazeSize.x; xi++)
                {
                    if (xAxes.Contains(xi) || yAxes.Contains(yi))
                    {
                        mazeFrameStringPattern[yi] += _wallSymbol;
                    }
                    else
                    {
                        mazeFrameStringPattern[yi] += _emptySymbol;
                    }
                }
            }

            return mazeFrameStringPattern;
        }

        private string[] GetFilledAxesPattern(int xAxis, int yAxis, Vector2Int mazeSize)
        {
            return GetFilledAxesPattern(new int[] { xAxis }, new int[] { yAxis }, mazeSize);
        }

        private string[] GetFilledAxesPattern(Vector2Int axes, Vector2Int mazeSize)
        {
            return GetFilledAxesPattern(new int[] { axes.x }, new int[] { axes.y }, mazeSize);
        }
    }
}
