using RogueLike.CustomMath;
using RogueLike.Game.Levels;
using RogueLike.Game;
using RogueLike.Maze;

namespace RogueLike.GameObjects.Characters.AI
{
    internal abstract class MoveAIHandler : AIHandler
    {
        public bool CanMove => _moveTimer <= 0;

        private double _moveTimer = 0;
        private double _moveCooldown;

        private int _playerFindDepth;

        protected Stack<Vector2Int> PathMemory = new Stack<Vector2Int>();
        public Stack<Vector2Int> PathMemoryCopy => new Stack<Vector2Int>(PathMemory);

        public MoveAIHandler(Enemy enemy, double moveCooldown, int playerFindDepth) : base(enemy)
        {
            _moveCooldown = moveCooldown;
            _playerFindDepth = playerFindDepth;
        }

        protected abstract void InternalHandleMove(double deltaTime);

        public void HandleMove(double deltaTime)
        {
            if (CanMove)
            {
                UpdatePathMemory();
                InternalHandleMove(deltaTime);
                ResetMoveTimer();
            }
            else
            {
                TickTimer(deltaTime);
            }
        }

        private void UpdatePathMemory()
        {
            StaticObject maze;

            var mazeLevel = GameController.CurrentLevel as MazeLevel;
            if (mazeLevel != null)
            {
                maze = mazeLevel.Maze;
            }
            else
            {
                return;
            }

            if (maze != null && GameController.Player != null)
            {
                List<Vector2Int> pathPoints =
                    MazePathFinder.Shared.GetPathPoints(maze.Collider.CollisionMap, Enemy.Position, GameController.Player.Position, _playerFindDepth);

                if (pathPoints.Count > 0)
                {
                    PathMemory.Clear();
                    foreach (var point in pathPoints)
                    {
                        PathMemory.Push(point);
                    }
                }
            }
        }

        private void ResetMoveTimer()
        {
            _moveTimer = _moveCooldown;
        }

        private void TickTimer(double deltaTime)
        {
            _moveTimer -= deltaTime;
        }
    }
}
