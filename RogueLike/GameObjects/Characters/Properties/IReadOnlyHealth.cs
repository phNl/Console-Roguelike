namespace RogueLike.GameObjects.Characters.Properties
{
    internal interface IReadOnlyHealth
    {
        public event Action<int, int>? ValueChanged;
        public event Action? ValueChangedToMinimum;

        public int Value { get; }
        public int MinValue { get; }
        public int MaxValue { get; }
    }
}
