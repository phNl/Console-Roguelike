using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.GameObjects.Characters.Properties;
using RogueLike.Render;

namespace RogueLike.GameObjects.Characters
{
    internal abstract class Character : 
        GameObject, 
        IMovable, 
        IDamagable, 
        IHealable, 
        IKillable, 
        IAttack
    {
        /// <summary>
        /// Invoke when the character moves in a direction
        /// </summary>
        public event Action<Vector2Int>? OnMove;

        private Health _health = new Health();
        public IReadOnlyHealth Health => _health;

        public virtual bool CanMove => true;

        public Character(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }

        public abstract void Attack(Vector2Int direction);

        public abstract void Kill();

        public void Move(Vector2Int direction)
        {
            if (!CanMove) return;

            Position += direction;

            if (CollisionHandler.HasCollisionWithAny(this, CollisionMode.Only_Colliders))
            {
                Position -= direction;
            }
            else
            {
                OnMove?.Invoke(direction);
            }
        }

        public void MoveUp()
        {
            Move(Vector2Int.Up);
        }

        public void MoveDown()
        {
            Move(Vector2Int.Down);
        }

        public void MoveLeft()
        {
            Move(Vector2Int.Left);
        }

        public void MoveRight()
        {
            Move(Vector2Int.Right);
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

        public void Damage(int damage)
        {
            _health.Damage(damage);
        }

        public void Heal(int health)
        {
            _health.Heal(health);
        }
    }
}
