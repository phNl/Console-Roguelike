using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.GameObjects.Characters.AIEnemies;

namespace RogueLike.AI
{
    internal class MeleeMoveAIHandler : MoveAIHandler
    {
        public MeleeMoveAIHandler(Enemy enemy, double moveCooldown, int playerFindDepth) : base(enemy, moveCooldown, playerFindDepth)
        {
        }

        protected override void InternalHandleMove(double deltaTime)
        {
            if (GameController.Player != null)
            {
                Vector2Int direction = Vector2Int.Zero;

                if (PathMemory.Count <= 0)
                {
                    if (Enemy.Position - GameController.Player.Position <= new Vector2Int(1, 1))
                    {
                        return;
                    }
                    else
                    {
                        var neighborsPositions = Enemy.Position.GetNeighborsPositions();
                        direction = neighborsPositions[Random.Shared.Next(0, neighborsPositions.Count)] - Enemy.Position;
                    }
                }
                else if (PathMemory.Count >= 0)
                {
                    direction = PathMemory.Pop() - Enemy.Position;
                }

                Enemy.Move(direction);
            }
        }
    }
}
