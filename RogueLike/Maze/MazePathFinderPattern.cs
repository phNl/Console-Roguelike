using RogueLike.AdditionalTools;
using RogueLike.Collision;
using RogueLike.CustomMath;

namespace RogueLike.Maze
{
    public enum MazePathFinderCellType
    {
        Start = 0,
        NotVisited = -1,
        Wall = -2,
        Destination = -3,
    }

    internal class MazePathFinderPattern : Array2DWrapper<int>
    {
        public MazePathFinderPattern(IReadOnlyCollisionMap collisionMap, Vector2Int startPos, Vector2Int destinationPos) : base(collisionMap.ArraySize)
        {
            for (int x = 0; x < collisionMap.ArraySize.x; x++)
            {
                for (int y = 0; y < collisionMap.ArraySize.y; y++)
                {
                    if (collisionMap[x, y])
                    {
                        this[x, y] = (int)MazePathFinderCellType.Wall;
                    }
                    else
                    {
                        var currentPos = new Vector2Int(x, y);
                        if (currentPos == startPos)
                        {
                            this[x, y] = (int)MazePathFinderCellType.Start;
                        }
                        else if (currentPos == destinationPos)
                        {
                            this[x, y] = (int)MazePathFinderCellType.Destination;
                        }
                        else
                        {
                            this[x, y] = (int)MazePathFinderCellType.NotVisited;
                        }
                    }
                }
            }
        }

        public MazePathFinderPattern(MazePathFinderPattern mazePathFinderPatter) : base(mazePathFinderPatter)
        {
        }
    }
}
