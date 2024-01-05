using RogueLike.Collision;
using RogueLike.CustomMath;

namespace RogueLike.Maze
{
    internal class MazePathFinder
    {
        public static MazePathFinder Shared => new MazePathFinder();

        /// <summary>
        /// Inclusive point
        /// </summary>
        private Vector2Int _upLeftPoint;
        /// <summary>
        /// Exclusive point
        /// </summary>
        private Vector2Int _downRightPoint;

        private int _step;
        private MazePathFinderPattern _pathFinderPattern;

        public MazePathFinder()
        {
            _pathFinderPattern = new MazePathFinderPattern(new CollisionMap(Vector2Int.Zero), Vector2Int.Zero, Vector2Int.Zero);
        }

        public List<Vector2Int> GetPathPoints(
            IReadOnlyCollisionMap mazeCollisionMap,
            Vector2Int startPos,
            Vector2Int destinationPos,
            int maxDepth = -1,
            bool excludeStartAndDestination = true
        )
        {
            List<Vector2Int> emptyList = new List<Vector2Int>();

            if (!mazeCollisionMap.IsPointInBounds(startPos))
                return emptyList;
            if (!mazeCollisionMap.IsPointInBounds(destinationPos))
                return emptyList;

            _pathFinderPattern = new MazePathFinderPattern(mazeCollisionMap, startPos, destinationPos);

            if (_pathFinderPattern[startPos] != (int)MazePathFinderCellType.Start)
                return emptyList;
            if (_pathFinderPattern[destinationPos] != (int)MazePathFinderCellType.Destination)
                return emptyList;

            _step = 0;
            // Inclusive
            _upLeftPoint = startPos;
            // Exclusive
            _downRightPoint = startPos + 1;

            while ((_step < maxDepth || maxDepth < 0) && _step < _pathFinderPattern.Count)
            {
                var upLeftPoint = _upLeftPoint;
                var downRightPoint = _downRightPoint;
                for (int x = upLeftPoint.x; x < downRightPoint.x; x++)
                {
                    for (int y = upLeftPoint.y; y < downRightPoint.y; y++)
                    {
                        if (_pathFinderPattern[x, y] == _step)
                        {
                            MakeWave(new Vector2Int(x, y));
                        }
                    }
                }

                if (IsPathFound(_pathFinderPattern, destinationPos))
                    break;

                _step++;
            }
            
            return GetPathPointFromPattern(destinationPos, excludeStartAndDestination);
        }

        private void MakeWave(Vector2Int pointPos)
        {
            List<Vector2Int> neighborsPositions = pointPos.GetNeighborsPositions();

            foreach (Vector2Int position in neighborsPositions)
            {
                if (!_pathFinderPattern.IsPointInBounds(position))
                    continue;

                if (_pathFinderPattern[position] != (int)MazePathFinderCellType.NotVisited &&
                    _pathFinderPattern[position] != (int)MazePathFinderCellType.Destination)
                {
                    continue;
                }

                _pathFinderPattern[position] = _step + 1;

                ExpandFindingRectangle(position);
            }
        }

        private void ExpandFindingRectangle(Vector2Int position)
        {
            _upLeftPoint.x = Math.Min(position.x, _upLeftPoint.x);
            _upLeftPoint.y = Math.Min(position.y, _upLeftPoint.y);

            _downRightPoint.x = Math.Max(position.x + 1, _downRightPoint.x);
            _downRightPoint.y = Math.Max(position.y + 1, _downRightPoint.y);
        }

        private bool IsPathFound(MazePathFinderPattern pathFinderPattern, Vector2Int destinationPos)
        {
            return pathFinderPattern[destinationPos] != (int)MazePathFinderCellType.Destination;
        }

        private List<Vector2Int> GetPathPointFromPattern(Vector2Int destinationPos, bool excludeStartAndDestination)
        {
            List<Vector2Int> pathPoints = new List<Vector2Int>();
            if (!IsPathFound(_pathFinderPattern, destinationPos))
            {
                return pathPoints;
            }

            Vector2Int currentPosition = destinationPos;

            int counter = 0;

            if (!excludeStartAndDestination)
            {
                pathPoints.Add(currentPosition);
            }

            while (counter < _pathFinderPattern.Count)
            {
                foreach (var position in currentPosition.GetNeighborsPositions())
                {
                    if (!_pathFinderPattern.IsPointInBounds(position))
                        continue;

                    if (_pathFinderPattern[position] == _pathFinderPattern[currentPosition] - 1)
                    {
                        currentPosition = position;
                        pathPoints.Add(currentPosition);
                        if (_pathFinderPattern[currentPosition] == (int)MazePathFinderCellType.Start)
                        {
                            if (excludeStartAndDestination)
                            {
                                pathPoints.Remove(pathPoints.Last());
                            }

                            return pathPoints;
                        }

                        break;
                    }
                }

                counter++;
            }

            throw new NotImplementedException();
        }
    }
}
