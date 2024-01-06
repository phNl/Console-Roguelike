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
        private Weapon _weapon = new RangeWeapon(1.5, 5, 10, 25);

        private double _moveTimer = 0;
        private double _moveCooldown = 0.5;

        private Stack<Vector2Int> _lastPathMemory = new Stack<Vector2Int>();

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
            Destroy();
        }

        protected override sealed void OnUpdate(double deltaTime)
        {
            _weapon.Update(deltaTime);
            HandleAIMove(deltaTime);
            HandleAIAttack();
        }

        protected override void OnDestroy()
        {
            OnMove -= ResetMoveTimer;
        }

        private void ResetMoveTimer(Vector2Int direction)
        {
            _moveTimer = _moveCooldown;
        }

        // todo: мб вынести в отдельный скрипт
        private void HandleAIMove(double deltaTime)
        {
            if (_moveTimer > 0)
            {
                _moveTimer -= deltaTime;
            }
            else
            {
                if (GameController.Maze != null && GameController.Player != null)
                {
                    // todo: change 10 to player find area size
                    List<Vector2Int> pathPoints =
                        MazePathFinder.Shared.GetPathPoints(GameController.Maze.Collider.CollisionMap, Position, GameController.Player.Position, 30);

                    if (pathPoints.Count > 0)
                    {
                        _lastPathMemory.Clear();
                        foreach (var point in pathPoints)
                        {
                            _lastPathMemory.Push(point);
                        }
                    }

                    Vector2Int direction;

                    if (_lastPathMemory.Count <= 0)
                    {
                        var neighborsPositions = Position.GetNeighborsPositions();
                        direction = neighborsPositions[Random.Shared.Next(0, neighborsPositions.Count)] - Position;
                    }
                    else
                    {
                        direction = _lastPathMemory.Pop() - Position;
                    }

                    Move(direction);
                }
            }
        }

        private void HandleAIAttack()
        {
            if (!_weapon.CanAttack) return;

            if (_lastPathMemory.Count <= 0)
            {
                return;
            }

            if (GameController.Player != null)
            {
                // todo: change 4 to range attack
                if (_lastPathMemory.Count < 15)
                {
                    var posDifference = GameController.Player.Position - Position;
                    Vector2Int attackDirection;
                    if (Math.Abs(posDifference.x) > Math.Abs(posDifference.y))
                    {
                        attackDirection = new Vector2Int(Math.Sign(posDifference.x), 0);
                    }
                    else
                    {
                        attackDirection = new Vector2Int(0, Math.Sign(posDifference.y));
                    }

                    Attack(attackDirection);
                }
            }
        }
    }
}
