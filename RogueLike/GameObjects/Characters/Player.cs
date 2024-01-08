using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.GameObjects.Characters
{
    internal class Player : Character
    {
        public Player(
            RenderObject renderObject,
            Collider collider,
            Weapon weapon,
            int maxHealthValue
            ) : base(renderObject, collider, weapon, maxHealthValue)
        {
        }

        public override void Attack(Vector2Int direction)
        {
            Weapon.Attack(Position, direction, this);
        }

        public override void Kill()
        {
            GameController.GenerateAndLoadDeathScreenLevel();
        }
    }
}
