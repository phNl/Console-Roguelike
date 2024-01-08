using RogueLike.CustomMath;
using RogueLike.GameObjects;
using RogueLike.Render;

namespace RogueLike.Game.Levels
{
    internal class MazeLevel : Level
    {
        private GameObjects.Maze _maze;
        public GameObjects.Maze Maze => _maze;

        public MazeLevel(GameObjects.Maze maze, List<GameObject> objects) : base(objects)
        {
            _maze = maze;
            _maze.Position = Vector2Int.Zero;

            AddObjectImmediately(_maze);
        }

        public MazeLevel(List<GameObject> objects) : this(GenerateMazeObject(), objects)
        {
        }

        public MazeLevel() : this(new List<GameObject>())
        {
        }

        private static GameObjects.Maze GenerateMazeObject()
        {
            return new GameObjects.Maze(GameController.PlayAreaSize, '#', (char)RenderBuffer.NullSymbol, null, 7);
        }
    }
}
