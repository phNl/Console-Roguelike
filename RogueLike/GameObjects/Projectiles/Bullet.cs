using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.GameObjects.Projectiles
{
    internal class Bullet : Projectile
    {
        private int _damage;
        public int Damage => _damage;

        public Bullet(RenderObject renderObject, int damage) : base(renderObject)
        {
            _damage = damage;
        }

        public override void Move(Vector2Int direction)
        {
            Position += direction;

            List<GameObject> objectsAtPoint = CollisionHandler.GetCollisionObjectsAtPoint(Position);

            objectsAtPoint.Remove(this);
            if (Owner != null)
                objectsAtPoint.Remove(Owner);

            if (objectsAtPoint.Count > 0)
            {
                OnCollision();
            }
        }

        protected override void OnCollision()
        {
            Destroy();
        }
    }
}
