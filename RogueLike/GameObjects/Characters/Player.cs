using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.GameObjects.Projectiles;
using RogueLike.Render;

namespace RogueLike.GameObjects.Characters
{
    internal class Player : Character
    {
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

        public void Shoot(Vector2Int direction)
        {
            RenderObject renderObject = new RenderObject(new RenderBuffer(new string[] { "*" }));
            Collider bulletCollider = new Collider(CollisionMap.GetCollisionMapFromRenderPattern(renderObject.RenderPattern), true);
            Bullet bullet = new Bullet(renderObject, bulletCollider, 100);
            bullet.Collider = bulletCollider;

            bullet.Position = Position + direction;
            GameController.CurrentLevel?.PrepareAddObject(bullet);

            bullet.Launch(direction, 5f, this, 5);
        }

        public void ShootUp()
        {
            Shoot(Vector2Int.Up);
        }

        public void ShootDown()
        {
            Shoot(Vector2Int.Down);
        }

        public void ShootLeft()
        {
            Shoot(Vector2Int.Left);
        }

        public void ShootRight()
        {
            Shoot(Vector2Int.Right);
        }
    }
}
