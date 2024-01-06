namespace RogueLike.Game
{
    internal class GameLoop : GameLoopBase
    {
        public GameLoop(ushort maxFps) : base(maxFps)
        {
        }

        protected override void Update(double deltaTime)
        {
            GameController.OnGameLoopTick(deltaTime);
        }
    }
}
