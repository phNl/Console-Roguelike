namespace RogueLike.Game
{
    internal class GameLoop : GameLoopBase
    {
        public event Action? OnAfterUpdate;
        public event Action? OnUpdate;

        public GameLoop(ushort maxFps) : base(maxFps)
        {
        }

        protected override void AfterUpdate()
        {
            OnAfterUpdate?.Invoke();
        }

        protected override void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
