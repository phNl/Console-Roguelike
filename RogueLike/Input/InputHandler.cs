namespace RogueLike.Input
{
    internal static class InputHandler
    {
        public static event Action<ConsoleKeyInfo>? OnKeyRead;
        private static CancellationTokenSource? _cts;

        public static async void RestartReadKeyLoop()
        {
            if (_cts != null)
                StopReadKeyLoop();

            _cts = new CancellationTokenSource();
            await ReadKeyLoopAsync(_cts.Token);
        }

        public static void StopReadKeyLoop()
        {
            _cts?.Cancel();
        }

        private static async Task ReadKeyLoopAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ConsoleKeyInfo key = await Task.Run(() => Console.ReadKey(), cancellationToken);
                OnKeyRead?.Invoke(key);
            }
        }
    }
}
