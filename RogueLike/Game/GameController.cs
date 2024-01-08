using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game.Levels;
using RogueLike.GameObjects;
using RogueLike.GameObjects.Characters;
using RogueLike.GameObjects.UI;
using RogueLike.Input;
using RogueLike.Input.Bindings;
using RogueLike.Render;
using RogueLike.RenderTools;
using RogueLike.Weapons;

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

        private static Vector2Int _playAreaSize;
        public static Vector2Int PlayAreaSize => _playAreaSize;

        public static Vector2Int UIAreaPosition => new Vector2Int(0, PlayAreaSize.y);
        public static Vector2Int UIAreaSize => LevelSize - PlayAreaSize;

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
                _bindingHandlers?.Add(new DeathScreenBindingHandler(InputActionMaps[GameState.Death]));
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

        public static void GenerateAndLoadInGameLevel()
        {
            GenerateAndLoadInGameLevel(
                playerSpawnArea: new Tuple<Vector2Int, Vector2Int>(Vector2Int.Zero, new Vector2Int(LevelSize.x - 1, LevelSize.y / 8)),
                exitSpawnArea: new Tuple<Vector2Int, Vector2Int>(new Vector2Int(0, LevelSize.y / 8 * 7), LevelSize - 1),
                enemiesSpawnArea: new Tuple<Vector2Int, Vector2Int>(LevelSize / 6, LevelSize - 1),
                rangeEnemiesCount: 5,
                meleeEnemiesCount: 10
                );
        }

        public static void GenerateAndLoadInGameLevel(
            Tuple<Vector2Int, Vector2Int> playerSpawnArea,
            Tuple<Vector2Int, Vector2Int> exitSpawnArea,
            Tuple<Vector2Int, Vector2Int> enemiesSpawnArea,
            int rangeEnemiesCount,
            int meleeEnemiesCount
            )
        {
            MazeLevel level = new MazeLevel();
            List<Vector2Int> mazeEmptyCells = level.Maze.GetEmptyCells();

            // Level exit trigger
            RenderObject levelExitDoorRenderObject = new RenderObject(new RenderBuffer(new string[] { "E" }));
            Collider levelExitDoorCollider = new Collider(levelExitDoorRenderObject, true);
            LevelExitDoor levelExitDoor = new LevelExitDoor(levelExitDoorRenderObject, levelExitDoorCollider, 50);
            List<Vector2Int> mazeRandomCells = 
                mazeEmptyCells.FindAll((Vector2Int) => Vector2Int.InRectangle(exitSpawnArea.Item1, exitSpawnArea.Item2));
            levelExitDoor.Position = mazeRandomCells[Random.Shared.Next(0, mazeRandomCells.Count)];
            level.PrepareAddObject(levelExitDoor);

            // Player
            if (Player == null)
            {
                RenderObject playerRenderObject = new RenderObject(new RenderBuffer(new string[] { "O" }));
                RangeWeapon playerWeapon = new RangeWeapon(1, 5, 10, 50);
                Player = new Player(playerRenderObject, new Collider(playerRenderObject), playerWeapon, 100);
            }
            if (Player.Health.Value <= Player.Health.MinValue)
            {
                Player.Heal(Player.Health.MaxValue);
            }
            List<Vector2Int> playerRandomCells = 
                mazeEmptyCells.FindAll((Vector2Int) => Vector2Int.InRectangle(playerSpawnArea.Item1, playerSpawnArea.Item2));
            Player.Position = playerRandomCells[Random.Shared.Next(0, playerRandomCells.Count)];
            level.PrepareAddObject(Player);

            // Player UI
            TextUI healthTextUI = new TextUI("Health:");
            healthTextUI.Position = new Vector2Int(5, UIAreaPosition.y + 1);
            level.PrepareAddObject(healthTextUI);

            if (Player != null)
            {
                TextUI healthValueTextUI = new TextUI(Player.Health.Value.ToString());
                healthValueTextUI.Position = healthTextUI.Position + new Vector2Int(healthTextUI.TextLength + 1, 0);
                Player.Health.ValueChanged += (prevHP, newHP) => healthValueTextUI.Text = newHP.ToString();
                level.PrepareAddObject(healthValueTextUI);
            }

            List<Vector2Int> randomEnemyCells =
                        mazeEmptyCells.FindAll((Vector2Int) => Vector2Int.InRectangle(enemiesSpawnArea.Item1, enemiesSpawnArea.Item2));

            // Range enemies
            for (int i = 0; i < rangeEnemiesCount; i++)
            {
                var rangeEnemy = EnemyGenerator.GenerateRangeEnemy();
                rangeEnemy.Position = randomEnemyCells[Random.Shared.Next(0, randomEnemyCells.Count)];
                level.PrepareAddObject(rangeEnemy);
            }

            // Melee enemies
            for (int i = 0; i < meleeEnemiesCount; i++)
            {
                var meleeEnemy = EnemyGenerator.GenerateMeleeEnemy();
                meleeEnemy.Position = randomEnemyCells[Random.Shared.Next(0, randomEnemyCells.Count)];
                level.PrepareAddObject(meleeEnemy);
            }

            level.AddPreparedObjects();
            ChangeInputActionMap(GameState.InGame);

            LoadLevel(level);
        }

        public static void GenerateAndLoadDeathScreenLevel()
        {
            DeathScreenLevel level = new DeathScreenLevel();

            TextUI deathTextUI = new TextUI("YOU DIED");
            deathTextUI.Position = LevelSize / 2 - new Vector2Int(deathTextUI.TextLength / 2, LevelSize.y / 3);
            level.PrepareAddObject(deathTextUI);

            TextUI quitTextUI = new TextUI("QUIT");
            quitTextUI.Position = LevelSize / new Vector2Int(3, 3) - new Vector2Int(quitTextUI.TextLength / 2, 0);
            level.PrepareAddObject(quitTextUI);

            TextUI quitButtonTextUI = new TextUI("[Esc]");
            quitButtonTextUI.Position = quitTextUI.Position + new Vector2Int(0, 1);
            level.PrepareAddObject(quitButtonTextUI);

            TextUI continueTextUI = new TextUI("CONTINUE");
            continueTextUI.Position = LevelSize / new Vector2Int(3, 3) * new Vector2Int(2, 1) - new Vector2Int(continueTextUI.TextLength / 2, 0);
            level.PrepareAddObject(continueTextUI);

            TextUI continueButtonTextUI = new TextUI("[Enter]");
            continueButtonTextUI.Position = continueTextUI.Position + new Vector2Int(0, 1);
            level.PrepareAddObject(continueButtonTextUI);

            level.AddPreparedObjects();
            ChangeInputActionMap(GameState.Death);
            LoadLevel(level);
        }

        public static void ChangeInputActionMap(GameState gameState)
        {
            if (_inputActionHandler != null && InputActionMaps != null)
            {
                _inputActionHandler.CurrentInputActionMap = InputActionMaps[gameState];
            }
            else
            {
                throw new Exception("InputActionHandler or InputActionMaps is null");
            }
        }

        private static void OnLoadLevel()
        {
            
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
