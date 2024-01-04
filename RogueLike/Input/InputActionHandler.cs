namespace RogueLike.Input
{
    internal class InputActionHandler
    {
        private InputActionMap? __currentInputActionMap;
        public InputActionMap? CurrentInputActionMap
        {
            get => __currentInputActionMap;
            set => __currentInputActionMap = value;
        }

        private Queue<Action> _inputActionsToProcess;

        public InputActionHandler()
        {
            _inputActionsToProcess = new Queue<Action>();
        }

        public void Pause()
        {
            InputHandler.OnKeyRead -= EnqueueInputActions;
        }

        public void Unpause()
        {
            InputHandler.OnKeyRead += EnqueueInputActions;
        }

        public void ProcessInputActions()
        {
            while (_inputActionsToProcess.Count > 0)
            {
                Action action = _inputActionsToProcess.Dequeue();
                action?.Invoke();
            }
        }

        public void ClearInputActionsQueue()
        {
            _inputActionsToProcess?.Clear();
        }

        private void EnqueueInputActions(ConsoleKeyInfo keyInfo)
        {
            if (CurrentInputActionMap == null)
                return;

            Action[] inputActions = new Action[CurrentInputActionMap.ActionBinds.Count];
            CurrentInputActionMap.ActionBinds[keyInfo.Key].CopyTo(inputActions);

            foreach (var inputAction in inputActions)
            {
                _inputActionsToProcess.Enqueue(inputAction);
            }
        }
    }
}
