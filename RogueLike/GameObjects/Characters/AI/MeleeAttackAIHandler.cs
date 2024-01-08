using RogueLike.CustomMath;
using RogueLike.Game;

namespace RogueLike.GameObjects.Characters.AI
{
    internal class MeleeAttackAIHandler : AttackAIHandler
    {
        public MeleeAttackAIHandler(Enemy enemy) : base(enemy)
        {
        }

        protected override void InternalHandleAttack()
        {
            if (GameController.Player != null)
            {
                if (Enemy.PathMemoryCopy.Count <= 0 &&
                    Enemy.Position - GameController.Player.Position <= new Vector2Int(1, 1))
                {
                    var posDifference = GameController.Player.Position - Enemy.Position;
                    Vector2Int attackDirection;
                    if (Math.Abs(posDifference.x) > Math.Abs(posDifference.y))
                    {
                        attackDirection = new Vector2Int(Math.Sign(posDifference.x), 0);
                    }
                    else
                    {
                        attackDirection = new Vector2Int(0, Math.Sign(posDifference.y));
                    }

                    Enemy.Attack(attackDirection);
                }
            }
        }
    }
}
