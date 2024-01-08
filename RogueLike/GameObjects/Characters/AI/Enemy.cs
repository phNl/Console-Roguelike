using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.GameObjects.Characters.AI
{
    internal abstract class Enemy : Character
    {
        public override sealed bool CanMove => MoveAIHandler.CanMove;

        protected abstract AttackAIHandler AttackAIHandler { get; }
        protected abstract MoveAIHandler MoveAIHandler { get; }

        public Stack<Vector2Int> PathMemoryCopy => MoveAIHandler.PathMemoryCopy;

        private Weapon _weapon;
        public IReadOnlyWeapon Weapon => _weapon;

        public Enemy(RenderObject renderObject, Collider collider, Weapon weapon) : base(renderObject, collider)
        {
            _weapon = weapon;
        }

        public override sealed void Attack(Vector2Int direction)
        {
            _weapon.Attack(Position, direction, this);
        }

        public override sealed void Kill()
        {
            Destroy();
        }

        protected override sealed void OnUpdate(double deltaTime)
        {
            _weapon.Update(deltaTime);
            MoveAIHandler.HandleMove(deltaTime);
            AttackAIHandler.HandleAttack();
        }
    }
}
