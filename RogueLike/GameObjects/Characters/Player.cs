using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.Game.Levels;
using RogueLike.GameObjects.UI;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.GameObjects.Characters
{
    internal class Player : Character
    {
        // todo: change (or remove)
        private Weapon _weapon = new RangeWeapon(1f, 5f, 5f, 50);

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
            GameController.GenerateAndLoadDeathScreenLevel();
        }

        // todo: Remove this
        protected override sealed void OnUpdate(double deltaTime)
        {
            _weapon.Update(deltaTime);
        }
    }
}
