namespace RogueLike.Input.Bindings
{
    internal abstract class BindingHandler
    {
        private InputActionMap _inputActionMap;
        protected InputActionMap InputActionMap => _inputActionMap;

        public abstract IReadOnlyInputActionMap Binds { get; }

        public BindingHandler(InputActionMap inputActionMap)
        {
            _inputActionMap = inputActionMap;
            InitializeBindsDictionary();
        }

        protected abstract void InitializeBindsDictionary();

        public void AddAllBinds()
        {
            foreach (var bind in Binds.ActionBinds)
            {
                foreach (var action in bind.Value)
                {
                    InputActionMap.AddBind(bind.Key, action);
                }
            }
        }

        public void RemoveAllBinds()
        {
            foreach (var bind in Binds.ActionBinds)
            {
                foreach (var action in bind.Value)
                {
                    InputActionMap.RemoveBind(bind.Key, action);
                }
            }
        }

    }
}
