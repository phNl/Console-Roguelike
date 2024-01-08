namespace RogueLike.GameObjects.Characters.AI
{
    internal abstract class AttackAIHandler : AIHandler
    {
        public AttackAIHandler(Enemy enemy) : base(enemy)
        {
        }

        public abstract void HandleAttack();
    }
}
