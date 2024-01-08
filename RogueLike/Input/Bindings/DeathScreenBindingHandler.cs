using RogueLike.Game;

namespace RogueLike.Input.Bindings
{
    internal class DeathScreenBindingHandler : BindingHandler
    {
        private InputActionMap _binds = new InputActionMap();
        public override IReadOnlyInputActionMap Binds => _binds;

        public DeathScreenBindingHandler(InputActionMap inputActionMap) : base(inputActionMap)
        {
        }

        protected override void InitializeBindsDictionary()
        {
            _binds.AddBind(ConsoleKey.Escape, GameController.QuitGame);
            _binds.AddBind(ConsoleKey.Enter, GameController.GenerateAndLoadInGameLevel);
        }
    }
}
