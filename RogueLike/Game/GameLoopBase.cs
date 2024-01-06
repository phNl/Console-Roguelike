namespace RogueLike.Game
{
    internal abstract class GameLoopBase
    {
        private bool _isGameLoopEnded = false;
        private readonly Thread _loopThread;

        protected double LoopInterval => 1f / (double)FPSMax;

        private ushort _fpsMax;
        public ushort FPSMax
        {
            set => _fpsMax = value;
            get => _fpsMax;
        }

        private bool _isFirstUpdateAfterPause = true;
        private DateTime _lastLoopTickTime;
        private DateTime _lastUpdateTime;

        private double _timerInSeconds = 0;

        private bool _isPaused = true;
        public bool IsPaused => _isPaused;

        public GameLoopBase(ushort fpsMax)
        {
            FPSMax = fpsMax;
            _loopThread = new Thread(StartLoop);
            _loopThread.Start();
            Pause();
        }

        ~GameLoopBase()
        {
            Stop();
        }

        public void Pause()
        {
            _isFirstUpdateAfterPause = true;
            _isPaused = true;
        }

        public void Unpause()
        {
            _isPaused = false;
        }

        public void Stop()
        {
            _isGameLoopEnded = true;
        }

        protected abstract void Update(double deltaTime);

        private void InnerUpdate()
        {
            if (_isFirstUpdateAfterPause)
            {
                _isFirstUpdateAfterPause = false;
            }
            else
            {
                Update((DateTime.Now - _lastUpdateTime).TotalSeconds);
            }

            _lastUpdateTime = DateTime.Now;
        }

        private void StartLoop()
        {
            _lastLoopTickTime = DateTime.Now;

            while (!_isGameLoopEnded)
            {
                if (!IsPaused)
                {
                    _timerInSeconds += (DateTime.Now - _lastLoopTickTime).TotalSeconds;

                    while (_timerInSeconds >= LoopInterval)
                    {
                        _timerInSeconds -= LoopInterval;
                        InnerUpdate();
                    }
                }

                _lastLoopTickTime = DateTime.Now;
            }
        }
    }
}
