using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.GameObjects.Characters.AIEnemies;

namespace RogueLike.AI
{
    internal class RangeMoveAIHandler : MoveAIHandler
    {
        private int _minDistanceToPlayer;

        public RangeMoveAIHandler(Enemy enemy, double moveCooldown, int playerFindDepth, int minDistanceToPlayer) : base(enemy, moveCooldown, playerFindDepth)
        {
            _minDistanceToPlayer = minDistanceToPlayer;
        }

        protected override void InternalHandleMove(double deltaTime)
        {
            if (GameController.Player != null)
            {
                Vector2Int direction;

                if (PathMemory.Count <= 0)
                {
                    var neighborsPositions = Enemy.Position.GetNeighborsPositions();
                    direction = neighborsPositions[Random.Shared.Next(0, neighborsPositions.Count)] - Enemy.Position;
                }
                else if (PathMemory.Count >= _minDistanceToPlayer)
                {
                    direction = PathMemory.Pop() - Enemy.Position;
                }
                else
                {
                    direction = (PathMemory.Peek() - Enemy.Position) * -1;
                }

                Enemy.Move(direction);
            }
        }
    }
}
