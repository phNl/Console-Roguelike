using RogueLike.Collision;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.GameObjects.Characters.AI
{
    internal class MeleeEnemy : Enemy
    {
        private AttackAIHandler _attackAIHandler;
        protected override AttackAIHandler AttackAIHandler => _attackAIHandler;

        private MoveAIHandler _moveAIHandler;
        protected override MoveAIHandler MoveAIHandler => _moveAIHandler;

        public MeleeEnemy(
            RenderObject renderObject,
            Collider collider,
            Weapon weapon,
            int maxHealthValue,
            double moveCooldownInSeconds,
            int playerFindDepth
            ) : base(renderObject, collider, weapon, maxHealthValue)
        {
            _attackAIHandler = new MeleeAttackAIHandler(this);
            _moveAIHandler = new MeleeMoveAIHandler(this, moveCooldownInSeconds, playerFindDepth);
        }
    }
}
