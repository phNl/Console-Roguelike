using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.GameObjects;
using RogueLike.GameObjects.Projectiles;
using RogueLike.Render;

namespace RogueLike.Weapons
{
    internal class RangeWeapon : Weapon
    {
        private float _bulletSpeed;
        public float BulletSpeed => _bulletSpeed;

        private float _bulletLifeTime;
        public float BulletLifeTime => _bulletLifeTime;

        private int _damage;
        public override sealed int Damage => _damage;

        public RangeWeapon(double attackCooldown, float bulletSpeed, float bulletLifeTime, int damage) : base(attackCooldown) 
        {
            _bulletLifeTime = bulletLifeTime;
            _bulletSpeed = bulletSpeed;
            _damage = damage;
        }

        protected override sealed void HandleAttack(Vector2Int position, Vector2Int direction, GameObject attacker)
        {
            RenderObject bulletRenderObject = new RenderObject(new RenderBuffer(new string[] { "*" }));
            Collider bulletCollider = new Collider(bulletRenderObject, true);
            Bullet bullet = new Bullet(bulletRenderObject, bulletCollider, Damage);
            bullet.Collider = bulletCollider;

            bullet.Position = position + direction;
            GameController.CurrentLevel?.PrepareAddObject(bullet);

            bullet.Launch(direction, _bulletSpeed, attacker, _bulletLifeTime);
        }
    }
}
