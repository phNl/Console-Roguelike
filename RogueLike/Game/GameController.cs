using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.GameObjects;
using RogueLike.GameObjects.Characters;
using RogueLike.GameObjects.UI;
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

        private static StaticObject? _maze;
        public static StaticObject? Maze => _maze;

        private static InputActionHandler? _inputActionHandler;
        private static Renderer? _renderer;

        private static GameLoop? _gameLoop;
        public static GameLoop? GameLoop => _gameLoop;

        private static Dictionary<GameState, InputActionMap>? _inputActionMaps;
        public static IReadOnlyDictionary<GameState, InputActionMap>? InputActionMaps => _inputActionMaps;

        private static bool _isInitialized = false;
        public static bool IsInitialized => _isInitialized;

        private static Vector2Int _playAreaSize;
        public static Vector2Int PlayAreaSize => _playAreaSize;

        private static Vector2Int UIAreaPosition => new Vector2Int(0, PlayAreaSize.y);
        private static Vector2Int UIAreaSize => LevelSize - PlayAreaSize;

        // Binds
        private static List<BindingHandler>? _bindingHandlers;

        public static void Initialize(Vector2Int screenSize, ushort maxFps = 60, string fontName = "Consolas", short fontSize = 14)
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

            _bindingHandlers = new List<BindingHandler>();
            InitializeBindingHandlers();

            _playAreaSize = LevelSize - new Vector2Int(0, 5);

            _isInitialized = true;
        }

        private static void InitializeBindingHandlers()
        {
            // Initialize binds
            if (InputActionMaps != null)
            {
                _bindingHandlers?.Add(new InGamePlayerBindingHandler(InputActionMaps[GameState.InGame]));
                _bindingHandlers?.Add(new InGameOtherBindingHandler(InputActionMaps[GameState.InGame]));
            }

            if (_bindingHandlers != null)
            {
                foreach (var bindingHandler in _bindingHandlers)
                {
                    bindingHandler.AddAllBinds();
                }
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

        public static void OnGameLoopTick(double deltaTime)
        {
            PreUpdate(deltaTime);
            Update(deltaTime);
            AfterUpdate(deltaTime);
            RenderUpdate();
        }

        public static void QuitGame()
        {
            GameLoop?.Stop();
        }

        private static void OnLoadLevel()
        {
            // Maze walls
            MazeGenerator mazeGenerator = new MazeGenerator('#', (char)RenderBuffer.NullSymbol);
            RenderObject mazeRenderObject = new RenderObject(mazeGenerator.GetMazeRenderPattern(PlayAreaSize, 7));
            _maze = new StaticObject(mazeRenderObject, new Collider(mazeRenderObject));
            _maze.Position = Vector2Int.Zero;
            CurrentLevel?.PrepareAddObject(_maze);

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

            // Test Enemy
            RenderObject enemyRenderObject = new RenderObject(new RenderBuffer(new string[] { "X" }));
            Enemy enemy = new Enemy(enemyRenderObject, new Collider(enemyRenderObject));
            enemy.Position = new Vector2Int(25, 10);
            CurrentLevel?.PrepareAddObject(enemy);

            // UI
            TextUI healthTextUI = new TextUI("Health:");
            healthTextUI.Position = new Vector2Int(5, UIAreaPosition.y + 1);
            CurrentLevel?.PrepareAddObject(healthTextUI);

            TextUI healthValueTextUI = new TextUI(Player.Health.MaxValue.ToString());
            healthValueTextUI.Position = healthTextUI.Position + new Vector2Int(healthTextUI.TextLength + 1, 0);
            // todo: Вынести в скрипт UI Handler
            Player.Health.ValueChanged += (prevHP, newHp) => healthValueTextUI.Text = Player.Health.Value.ToString();
            CurrentLevel?.PrepareAddObject(healthValueTextUI);
        }

        private static void PreUpdate(double deltaTime)
        {
            CurrentLevel?.AddPreparedObjects();
        }

        private static void Update(double deltaTime)
        {
            _inputActionHandler?.ProcessInputActions();

            if (CurrentLevel?.Objects != null)
            {
                foreach (var obj in CurrentLevel.Objects)
                {
                    obj.Update(deltaTime);
                    if (obj.IsPreparedToDestroy)
                    {
                        CurrentLevel.PrepareRemoveObject(obj);
                    }
                }
            }
        }

        private static void AfterUpdate(double deltaTime)
        {
            CurrentLevel?.RemovePreparedObjects();
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
