using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.GameObjects.Projectiles;
using RogueLike.Render;

namespace RogueLike.GameObjects.Characters
{
    internal class Player : Character
    {
        public Player()
        {
        }

        public Player(RenderObject renderObject) : base(renderObject)
        {
        }

        public Player(Collider collider) : base(collider)
        {
        }

        public Player(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }

        public void Shoot(Vector2Int direction)
        {
            RenderBuffer bulletRenderPattern = new RenderBuffer(new string[] { "*" });
            RenderObject bulletRenderObject = new RenderObject(bulletRenderPattern);
            Bullet bullet = new Bullet(bulletRenderObject, 100);
            Collider bulletCollider = new Collider(CollisionMap.GetCollisionMapFromRenderPattern(bulletRenderPattern, bullet), true);
            bullet.Collider = bulletCollider;

            bullet.Position = Position + direction;
            GameController.CurrentLevel?.AddObject(bullet);

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
