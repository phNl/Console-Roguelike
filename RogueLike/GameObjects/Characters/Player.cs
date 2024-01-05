using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.GameObjects.Characters
{
    internal class Player : Character
    {
        // todo: change (or remove)
        private Weapon _weapon = new RangeWeapon(1f, 5f, 5f, 100);

        public Player(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }

        public override void Attack(Vector2Int direction)
        {
            _weapon.Attack(Position, direction, this);
        }

        // todo: Сделать смену на экран смерти (просто текст и опции перезапуска/выхода из игры по нажатию биндов)
        public override void Kill()
        {
            throw new NotImplementedException();
        }

        // todo: Remove this
        public override void Update(double deltaTime)
        {
            _weapon.Update(deltaTime);
        }
    }
}
