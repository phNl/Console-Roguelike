using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.GameObjects;
using RogueLike.GameObjects.Characters;
using RogueLike.Input;
using RogueLike.Input.Bindings;
using RogueLike.Maze;
using RogueLike.Render;
using RogueLike.RenderTools;

namespace RogueLike.Game
{
    internal enum GameState
    {
        InGame,
        Death
    }

    internal static class GameController
    {
        public static event Action<Level>? OnLevelLoaded;

        public static event Action<Player?, Player?>? OnPlayerChanged;

        public static Vector2Int LevelSize => _renderer != null ? _renderer.BufferSize : Vector2Int.Zero;

        private static Level? _currentLevel;
        public static Level? CurrentLevel => _currentLevel;

        private static Player? __player;
        public static Player? Player
        {
            get => __player;
            set
            {
                if (__player != value)
                {
                    OnPlayerChanged?.Invoke(__player, value);
                    __player = value;
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

        // Binds
        private static List<BindingHandler>? _bindingHandlers;

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

            _inputActionHandler.CurrentInputActionMap = InputActionMaps?[GameState.InGame];

            InputHandler.RestartReadKeyLoop();

            _gameLoop.OnUpdate += OnUpdate;

            _bindingHandlers = new List<BindingHandler>();
            InitializeBindingHandlers();

            _isInitialized = true;
        }

        private static void InitializeBindingHandlers()
        {
            // Initialize binds
            if (InputActionMaps != null)
            {
                _bindingHandlers?.Add(new InGamePlayerBindingHandler(InputActionMaps[GameState.InGame]));
            }

            foreach (var bindingHandler in _bindingHandlers)
            {
                bindingHandler.AddAllBinds();
            }
        }

        public static void LoadLevel(Level level)
        {
            _currentLevel = level;

            _inputActionHandler?.Pause();
            _gameLoop?.Pause();
            OnLoadLevel();
            _gameLoop?.Unpause();
            _inputActionHandler?.Unpause();
        }

        private static void OnLoadLevel()
        {
            // Maze walls
            MazeGenerator mazeGenerator = new MazeGenerator('#', (char)RenderBuffer.NullSymbol);
            RenderObject mazeRenderObject = new RenderObject(mazeGenerator.GetMazeRenderPattern(LevelSize, 7));
            StaticObject maze = new StaticObject(mazeRenderObject, new Collider(mazeRenderObject));
            maze.Position = Vector2Int.Zero;
            CurrentLevel?.PrepareAddObject(maze);

            // Level exit trigger
            RenderObject levelExitDoorRenderObject = new RenderObject(new RenderBuffer(new string[] { "E" }));
            Collider levelExitDoorCollider = new Collider(levelExitDoorRenderObject, true);
            LevelExitDoor levelExitDoor = new LevelExitDoor(levelExitDoorRenderObject, levelExitDoorCollider);
            levelExitDoor.Position = new Vector2Int(20, 20);
            CurrentLevel?.PrepareAddObject(levelExitDoor);

            // Player
            RenderObject playerRenderObject = new RenderObject(new RenderBuffer(new string[] { "O" }));
            Player player = new Player(playerRenderObject, new Collider(playerRenderObject));
            player.Position = new Vector2Int(16, 5);
            Player = player;
            CurrentLevel?.PrepareAddObject(player);
        }

        private static void OnUpdate()
        {
            PreUpdate();
            Update();
            AfterUpdate();
        }

        private static void PreUpdate()
        {
            CurrentLevel?.AddPreparedObjects();
        }

        private static void Update()
        {
            _inputActionHandler?.ProcessInputActions();

            if (CurrentLevel?.Objects != null)
            {
                foreach (var obj in CurrentLevel.Objects)
                {
                    if (GameLoop != null)
                        obj.Update(GameLoop.DeltaTime);
                }
            }
        }

        private static void AfterUpdate()
        {
            CurrentLevel?.RemovePreparedObjects();
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
    }
}
