namespace RogueLike.Input
{
    public class InputActionMap
    {
        private Dictionary<ConsoleKey, List<Action>> _actionBinds;
        public IReadOnlyDictionary<ConsoleKey, List<Action>> ActionBinds => _actionBinds;

        public InputActionMap()
        {
            _actionBinds = new Dictionary<ConsoleKey, List<Action>>();
        }

        public void AddBind(ConsoleKey key, Action action)
        {
            if (!_actionBinds.ContainsKey(key))
            {
                _actionBinds.Add(key, new List<Action>());
            }

            _actionBinds[key].Add(action);
        }

        public void RemoveBind(ConsoleKey key, Action action)
        {
            _actionBinds[key]?.Remove(action);
        }

        public void ClearKey(ConsoleKey key)
        {
            _actionBinds.Remove(key);
        }

        public void RemoveAction(Action action)
        {
            foreach (var key in _actionBinds.Keys)
            {
                _actionBinds[key].Remove(action);
            }
        }
    }
}
