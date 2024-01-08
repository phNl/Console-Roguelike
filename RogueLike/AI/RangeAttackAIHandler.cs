using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.GameObjects.Characters.AIEnemies;

namespace RogueLike.AI
{
    internal class RangeAttackAIHandler : AttackAIHandler
    {
        private int _attackRange;
        public int AttackRange => _attackRange;

        public RangeAttackAIHandler(Enemy enemy, int attackRange) : base(enemy)
        {
            _attackRange = attackRange;
        }

        protected override void InternalHandleAttack()
        {
            if (Enemy.PathMemoryCopy.Count <= 0)
            {
                return;
            }

            if (GameController.Player != null)
            {
                if (Enemy.PathMemoryCopy.Count < AttackRange)
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
