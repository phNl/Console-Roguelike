using RogueLike.Collision;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.GameObjects.Characters.AI
{
    internal class RangeEnemy : Enemy
    {
        private AttackAIHandler _attackAIHandler;
        protected override AttackAIHandler AttackAIHandler => _attackAIHandler;

        private MoveAIHandler _moveAIHandler;
        protected override MoveAIHandler MoveAIHandler => _moveAIHandler;

        public RangeEnemy(
            RenderObject renderObject,
            Collider collider,
            Weapon weapon,
            int attackRange,
            double moveCooldownInSeconds,
            int playerFindDepth,
            int minDistanceToPlayer
            ) : base(renderObject, collider, weapon)
        {
            _attackAIHandler = new RangeAttackAIHandler(this, attackRange);
            _moveAIHandler = new RangeMoveAIHandler(this, moveCooldownInSeconds, playerFindDepth, minDistanceToPlayer);
        }
    }
}
