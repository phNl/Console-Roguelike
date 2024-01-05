namespace RogueLike.Input
{
    public interface IReadOnlyInputActionMap
    {
        public IReadOnlyDictionary<ConsoleKey, List<Action>> ActionBinds { get; }
    }
}