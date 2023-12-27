namespace RogueLike.Input
{
    internal class InputActionHandler
    {
        private InputActionMap? _currentInputActionMap;
        public InputActionMap? CurrentInputActionMap
        {
            get => _currentInputActionMap;
            set => _currentInputActionMap = value;
        }

        public InputActionHandler()
        {
            InputHandler.OnKeyRead += ActivateInputActions;
        }

        private void ActivateInputActions(ConsoleKeyInfo keyInfo)
        {
            if (CurrentInputActionMap == null)
                return;

            foreach (var inputAction in CurrentInputActionMap.ActionBinds[keyInfo.Key])
            {
                inputAction?.Invoke();
            }
        }
    }
}
