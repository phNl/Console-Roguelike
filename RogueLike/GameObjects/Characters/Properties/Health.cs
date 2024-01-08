namespace RogueLike.GameObjects.Characters.Properties
{
    internal class Health : IReadOnlyHealth, IDamagable, IHealable
    {
        public event Action<int, int>? ValueChanged;
        public event Action? ValueChangedToMinimum;

        public event Action<int, int>? MaxValueChanged;

        private int __value;
        public int Value
        {
            get
            {
                return __value;
            }

            private set
            {
                value = Math.Clamp(value, MinValue, MaxValue);
                if (__value != value)
                {
                    ValueChanged?.Invoke(__value, value);
                    __value = value;

                    if (__value <= MinValue)
                    {
                        ValueChangedToMinimum?.Invoke();
                    }
                }
            }
        }

        public int MinValue => 0;

        private int __maxValue;
        public int MaxValue
        {
            get
            {
                return __maxValue;
            }

            private set
            {
                if (value < MinValue)
                {
                    throw new Exception($"MaxValue ({value}) cannot be less than MinValue ({MinValue})");
                }

                if (__maxValue != value)
                {
                    MaxValueChanged?.Invoke(__maxValue, value);
                    __maxValue = value;
                }
            }
        }

        public Health(int maxValue = 100)
        {
            MaxValue = maxValue;
            Value = MaxValue;
        }

        public void Damage(int damage)
        {
            Value -= damage;
        }

        public void Heal(int health)
        {
            Value += health;
        }

        public void SetHealth(int health)
        {
            Value = health;
        }
    }
}
