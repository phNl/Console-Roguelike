namespace RogueLike.GameObjects.Characters.Properties
{
    internal interface IReadOnlyHealth
    {
        public int Value { get; }
        public int MinValue { get; }
        public int MaxValue { get; }
    }
}
