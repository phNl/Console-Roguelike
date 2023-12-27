﻿namespace RogueLike.Game
{
    internal abstract class GameLoopBase
    {
        private System.Timers.Timer _gameTimer;

        private ushort _fpsMax;
        public ushort FPSMax
        {
            set => _fpsMax = value;
            get => _fpsMax;
        }

        private const long TicksPerSecond = 10000000;

        private DateTime _lastUpdatingTime;

        private float _deltaTime = 0;
        public float DeltaTime => _deltaTime;

        public GameLoopBase(ushort fpsMax)
        {
            FPSMax = fpsMax;

            _gameTimer = new System.Timers.Timer();
            _gameTimer.Interval = 1000 / FPSMax;
            _gameTimer.Elapsed += InnerUpdate;
            _gameTimer.Start();
        }

        ~GameLoopBase()
        {
            Pause();
            _gameTimer.Elapsed -= InnerUpdate;
        }

        public void Pause()
        {
            _gameTimer.Stop();
        }

        public void Unpause()
        {
            _gameTimer.Start();
        }

        protected abstract void Update();
        protected abstract void AfterUpdate();

        private void InnerUpdate(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _deltaTime = (float)(e.SignalTime - _lastUpdatingTime).Ticks / TicksPerSecond;
            _lastUpdatingTime = e.SignalTime;

            Update();
            AfterUpdate();
        }
    }
}
