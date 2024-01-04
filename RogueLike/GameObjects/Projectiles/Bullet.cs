using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.GameObjects.Projectiles
{
    internal class Bullet : Projectile
    {
        private int _damage;
        public int Damage => _damage;

        public Bullet(RenderObject renderObject, Collider collider, int damage) : base(renderObject, collider)
        {
            _damage = damage;
        }

        public override void Move(Vector2Int direction)
        {
            Position += direction;
        }

        protected void UpdateCollision()
        {
            List<GameObject> excludeGO = new List<GameObject>();
            if (Owner != null)
                excludeGO.Add(Owner);

            if (CollisionHandler.HasCollisionWithAny(this, CollisionMode.Only_Colliders, excludeGO))
            {
                OnCollision();
            }
        }

        protected override void OnCollision()
        {
            Destroy();
        }

        protected override void OnLaunch()
        {
            UpdateCollision();
        }

        protected override void OnUpdate()
        {
            UpdateCollision();
        }
    }
}
