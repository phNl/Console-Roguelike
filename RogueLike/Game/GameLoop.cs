namespace RogueLike.Game
{
    internal class GameLoop : GameLoopBase
    {
        public event Action? OnUpdate;

        public GameLoop(ushort maxFps) : base(maxFps)
        {
        }

        protected override void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
