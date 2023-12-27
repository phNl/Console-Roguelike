using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.GameObjects;
using RogueLike.GameObjects.Characters;
using RogueLike.Input;
using RogueLike.Maze;
using RogueLike.Render;
using RogueLike.RenderTools;
using System.ComponentModel.DataAnnotations;

namespace RogueLike.Game
{
    internal enum GameState
    {
        Menu,
        InGame
    }

    internal static class GameController
    {
        public static event Action<Level>? OnLevelLoaded;

        public static event Action<Player?, Player?>? OnPlayerChanged;

        public static Vector2Int LevelSize => _renderer != null ? _renderer.BufferSize : Vector2Int.Zero;

        private static Level? _currentLevel;
        public static Level? CurrentLevel => _currentLevel;

        private static Player? _player;
        public static Player? Player
        {
            get => _player;
            set
            {
                if (_player != value)
                {
                    OnPlayerChanged?.Invoke(_player, value);
                    _player = value;
                }
            }
        }

        private static InputActionHandler? _inputActionHandler;
        private static Renderer? _renderer;

        private static GameLoop? _gameLoop;
        public static GameLoop? GameLoop => _gameLoop;

        private static Dictionary<GameState, InputActionMap>? _inputActionMaps;
        public static IReadOnlyDictionary<GameState, InputActionMap>? InputActionMaps => _inputActionMaps;

        private static bool _isInitialized = false;
        public static bool IsInitialized => _isInitialized;

        public static void Initialize(Vector2Int screenSize, ushort maxFps = 30, string fontName = "Consolas", short fontSize = 14)
        {
            if (IsInitialized)
                return;

            _renderer = new Renderer(screenSize, fontName, fontSize);
            _inputActionHandler = new InputActionHandler();
            _gameLoop = new GameLoop(maxFps);

            _inputActionMaps = new Dictionary<GameState, InputActionMap>();
            foreach (GameState gameState in Enum.GetValues(typeof(GameState)))
            {
                _inputActionMaps.Add(gameState, new InputActionMap());
            }

            // TODO: временно 
            _inputActionHandler.CurrentInputActionMap = InputActionMaps?[GameState.InGame];
            OnPlayerChanged += UpdatePlayerBinds;

            InputHandler.RestartReadKeyLoop();

            InputHandler.OnKeyRead += OnInputUpdate;

            _gameLoop.OnUpdate += OnUpdate;

            _isInitialized = true;
        }

        public static void LoadLevel(Level level)
        {
            _currentLevel = level;

            _gameLoop?.Pause();
            OnLoadLevel();
            _gameLoop?.Unpause();
        }

        private static void OnLoadLevel()
        {
            MazeGenerator mazeGenerator = new MazeGenerator('#', (char)RenderBuffer.NullSymbol);
            RenderObject mazeRenderObject = new RenderObject(mazeGenerator.GetMazeRenderPattern(LevelSize, 5));
            Collider mazeCollider = new Collider(CollisionMap.GetCollisionMapFromRenderPattern(mazeRenderObject.RenderPattern));
            StaticObject maze = new StaticObject(mazeRenderObject, mazeCollider);
            maze.Position = new Vector2Int(0, 0);
            CurrentLevel?.AddObject(maze);


            RenderBuffer playerRenderBuffer  = new RenderBuffer(new string[] { "O" });
            RenderObject playerRenderObject = new RenderObject(playerRenderBuffer);
            Collider playerCollider = new Collider(CollisionMap.GetCollisionMapFromRenderPattern(playerRenderBuffer));
            Player player = new Player(playerRenderObject, playerCollider);
            player.Position = new Vector2Int(16, 5);
            Player = player;
            CurrentLevel?.AddObject(player);
        }

        private static void OnInputUpdate(ConsoleKeyInfo keyInfo)
        {
            
        }

        private static void OnUpdate()
        {
            Update();
            AfterUpdate();
        }

        private static void Update()
        {
            if (CurrentLevel?.Objects != null)
            {
                for (var i = CurrentLevel.Objects.Count - 1; i > 0; i--)
                {
                    CurrentLevel.Objects[i].Update();
                }
            }
        }

        private static void AfterUpdate()
        {
            RenderUpdate();
        }

        private static void RenderUpdate()
        {
            if (_renderer == null)
                return;

            _renderer.ClearAllBuffers();

            if (CurrentLevel != null)
            {
                var objects = new List<GameObject>(CurrentLevel.Objects);
                foreach (var gameObject in objects)
                {
                    if (gameObject != null)
                        _renderer.AddObject(gameObject);
                }
            }

            _renderer.Draw();
        }

        private static void UpdatePlayerBinds(Player? oldPlayer, Player? newPlayer)
        {
            if (oldPlayer != null)
            {
                _inputActionHandler?.CurrentInputActionMap?.RemoveBind(ConsoleKey.W, oldPlayer.MoveUp);
                _inputActionHandler?.CurrentInputActionMap?.RemoveBind(ConsoleKey.S, oldPlayer.MoveDown);
                _inputActionHandler?.CurrentInputActionMap?.RemoveBind(ConsoleKey.A, oldPlayer.MoveLeft);
                _inputActionHandler?.CurrentInputActionMap?.RemoveBind(ConsoleKey.D, oldPlayer.MoveRight);

                _inputActionHandler?.CurrentInputActionMap?.RemoveBind(ConsoleKey.UpArrow, oldPlayer.ShootUp);
                _inputActionHandler?.CurrentInputActionMap?.RemoveBind(ConsoleKey.DownArrow, oldPlayer.ShootDown);
                _inputActionHandler?.CurrentInputActionMap?.RemoveBind(ConsoleKey.LeftArrow, oldPlayer.ShootLeft);
                _inputActionHandler?.CurrentInputActionMap?.RemoveBind(ConsoleKey.RightArrow, oldPlayer.ShootRight);
            }

            if (newPlayer != null)
            {
                _inputActionHandler?.CurrentInputActionMap?.AddBind(ConsoleKey.W, newPlayer.MoveUp);
                _inputActionHandler?.CurrentInputActionMap?.AddBind(ConsoleKey.S, newPlayer.MoveDown);
                _inputActionHandler?.CurrentInputActionMap?.AddBind(ConsoleKey.A, newPlayer.MoveLeft);
                _inputActionHandler?.CurrentInputActionMap?.AddBind(ConsoleKey.D, newPlayer.MoveRight);

                _inputActionHandler?.CurrentInputActionMap?.AddBind(ConsoleKey.UpArrow, newPlayer.ShootUp);
                _inputActionHandler?.CurrentInputActionMap?.AddBind(ConsoleKey.DownArrow, newPlayer.ShootDown);
                _inputActionHandler?.CurrentInputActionMap?.AddBind(ConsoleKey.LeftArrow, newPlayer.ShootLeft);
                _inputActionHandler?.CurrentInputActionMap?.AddBind(ConsoleKey.RightArrow, newPlayer.ShootRight);
            }
        }
    }
}
