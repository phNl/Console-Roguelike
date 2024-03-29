﻿using RogueLike.Game;

namespace RogueLike.Input.Bindings
{
    internal class InGameOtherBindingHandler : BindingHandler
    {
        private InputActionMap _binds = new InputActionMap();
        public override IReadOnlyInputActionMap Binds => _binds;

        public InGameOtherBindingHandler(InputActionMap inputActionMap) : base(inputActionMap)
        {
        }

        protected override void InitializeBindsDictionary()
        {
            _binds.AddBind(ConsoleKey.Escape, GameController.QuitGame);
        }
    }
}
