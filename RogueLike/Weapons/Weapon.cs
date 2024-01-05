using RogueLike.CustomMath;
using RogueLike.GameObjects;

namespace RogueLike.Weapons
{
    internal abstract class Weapon : IUpdate
    {
        public event Action<double>? TimerBeforeAttackChange;

        private double _attackCooldown;
        /// <summary>
        /// Delay between attacks in seconds.
        /// </summary>
        public double AttackCooldown => _attackCooldown;

        private double __timerBeforeAttack = 0f;
        /// <summary>
        /// Time in seconds before the next attack can be made
        /// </summary>
        public double TimerBeforeAttack
        {
            get
            {
                return __timerBeforeAttack;
            }
            private set
            {
                if (value != __timerBeforeAttack)
                {
                    __timerBeforeAttack = value;
                    TimerBeforeAttackChange?.Invoke(value);
                }
            }
        }

        public abstract int Damage { get; }

        public virtual bool CanAttack => TimerBeforeAttack <= 0f;

        public Weapon(double attackCooldown)
        {
            _attackCooldown = attackCooldown;
        }

        protected abstract void HandleAttack(Vector2Int position, Vector2Int direction, GameObject attacker);

        public void Attack(Vector2Int position, Vector2Int direction, GameObject attacker)
        {
            if (CanAttack)
            {
                HandleAttack(position, direction, attacker);

                TimerBeforeAttack = AttackCooldown;
            }
        }

        public void Update(double deltaTime)
        {
            if (TimerBeforeAttack > 0f)
            {
                TimerBeforeAttack -= deltaTime;
            }
        }
    }
}
