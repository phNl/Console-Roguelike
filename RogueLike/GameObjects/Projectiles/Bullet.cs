using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.GameObjects.Characters.Properties;
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
            List<GameObject> excludeGOList = new List<GameObject>();
            if (Owner != null)
            {
                excludeGOList.Add(Owner);
            }

            List<GameObject> collisionObjects = CollisionHandler.GetCollisionObjects(this, excludeGameObjects: excludeGOList).Keys.ToList();

            if (collisionObjects.Count > 0)
            {
                OnCollision(collisionObjects);
            }
        }

        protected override void OnCollision(List<GameObject> collisionGameObjects)
        {
            foreach (var collisionObject in collisionGameObjects)
            {
                IDamagable? damagableObject = collisionObject as IDamagable;
                if (damagableObject != null)
                {
                    damagableObject.Damage(Damage);
                }
            }

            Destroy();
        }

        protected override void OnLaunch()
        {
            UpdateCollision();
        }

        protected override void OnInnerUpdate()
        {
            UpdateCollision();
        }
    }
}
