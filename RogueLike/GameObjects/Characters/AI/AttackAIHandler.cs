namespace RogueLike.GameObjects.Characters.AI
{
    internal abstract class AttackAIHandler : AIHandler
    {
        public AttackAIHandler(Enemy enemy) : base(enemy)
        {
        }

        protected abstract void InternalHandleAttack();

        public void HandleAttack()
        {
            if (!Enemy.WeaponInfo.CanAttack)
            {
                return;
            }

            InternalHandleAttack();
        }
    }
}
