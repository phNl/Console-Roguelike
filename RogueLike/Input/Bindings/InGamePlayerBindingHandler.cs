using RogueLike.Game;
using RogueLike.GameObjects.Characters;

namespace RogueLike.Input.Bindings
{
    internal class InGamePlayerBindingHandler : BindingHandler
    {
        protected Player? Player => GameController.Player;

        private InputActionMap _binds = new InputActionMap();
        public override IReadOnlyInputActionMap Binds => _binds;

        public InGamePlayerBindingHandler(InputActionMap inputActionMap) : base(inputActionMap)
        {
        }

        protected override void InitializeBindsDictionary()
        {
            _binds.AddBind(ConsoleKey.W, MovePlayerUp);
            _binds.AddBind(ConsoleKey.S, MovePlayerDown);
            _binds.AddBind(ConsoleKey.A, MovePlayerLeft);
            _binds.AddBind(ConsoleKey.D, MovePlayerRight);

            _binds.AddBind(ConsoleKey.UpArrow, PlayerAttackUp);
            _binds.AddBind(ConsoleKey.DownArrow, PlayerAttackDown);
            _binds.AddBind(ConsoleKey.LeftArrow, PlayerAttackLeft);
            _binds.AddBind(ConsoleKey.RightArrow, PlayerAttackRight);
        }

        private void MovePlayerUp()
        {
            Player?.MoveUp();
        }

        private void MovePlayerDown()
        {
            Player?.MoveDown();
        }

        private void MovePlayerLeft()
        {
            Player?.MoveLeft();
        }

        private void MovePlayerRight()
        {
            Player?.MoveRight();
        }

        private void PlayerAttackUp()
        {
            Player?.AttackUp();
        }
        
        private void PlayerAttackDown()
        {
            Player?.AttackDown();
        }
        
        private void PlayerAttackLeft()
        {
            Player?.AttackLeft();
        }
        
        private void PlayerAttackRight()
        {
            Player?.AttackRight();
        }
    }
}
