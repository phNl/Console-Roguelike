namespace RogueLike.Weapons
{
    internal interface IReadOnlyWeapon
    {
        public event Action<double>? TimerBeforeAttackChange;

        public double AttackCooldown { get; }

        public double TimerBeforeAttack { get; }

        public int Damage { get; }

        public bool CanAttack { get; }
    }
}
