using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.Maze;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.GameObjects.Characters
{
    internal class Enemy : Character
    {
        private Weapon _weapon = new RangeWeapon(2, 5, 10, 50);

        private double _moveTimer = 0;
        private double _moveCooldown = 0.5;

        public override bool CanMove => _moveTimer <= 0;

        public Enemy(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
            OnMove += ResetMoveTimer;
        }

        public override void Attack(Vector2Int direction)
        {
            _weapon.Attack(Position, direction, this);
        }

        public override void Kill()
        {
            throw new NotImplementedException();
        }

        public override void Update(double deltaTime)
        {
            if (_moveTimer > 0)
            {
                _moveTimer -= deltaTime;
            }
            else
            {
                if (GameController.Maze != null && GameController.Player != null)
                {
                    List<Vector2Int> pathPoints = MazePathFinder.Shared.GetPathPoints(GameController.Maze.Collider.CollisionMap, Position, GameController.Player.Position, 10);
                    if (pathPoints.Count > 1)
                    {
                        Vector2Int nextPosition = pathPoints[pathPoints.Count - 1];
                        Vector2Int direction = nextPosition - Position;

                        Move(direction);
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            OnMove -= ResetMoveTimer;
        }

        private void ResetMoveTimer(Vector2Int direction)
        {
            _moveTimer = _moveCooldown;
        }
    }
}
