using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.GameObjects.Characters
{
    internal class Player : Character
    {
        private Weapon _weapon = new RangeWeapon(1f, 5f, 5f, 100);

        public Player(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }

        public Player(RenderObject renderObject) : base(renderObject)
        {
        }

        public Player(Collider collider) : base(collider)
        {
        }

        public Player()
        {
        }

        // todo: Remove this  
        public void Attack(Vector2Int direction)
        {
            _weapon.Attack(Position, direction, this);
        }

        public void AttackUp()
        {
            Attack(Vector2Int.Up);
        }

        public void AttackDown()
        {
            Attack(Vector2Int.Down);
        }

        public void AttackLeft()
        {
            Attack(Vector2Int.Left);
        }

        public void AttackRight()
        {
            Attack(Vector2Int.Right);
        }

        // todo: Remove this
        public override void Update(double deltaTime)
        {
            _weapon.Update(deltaTime);
        }
    }
}
