using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.GameObjects;
using RogueLike.Maze;
using RogueLike.Render;

namespace RogueLike.Game.Levels
{
    internal class MazeLevel : Level
    {
        private StaticObject _maze;
        public StaticObject Maze => _maze;

        public MazeLevel(StaticObject maze, List<GameObject> objects) : base(objects)
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

        private static StaticObject GenerateMazeObject()
        {
            // Maze
            RenderObject mazeRenderObject = new RenderObject(MazeGenerator.Shared.GetMazeRenderPattern(GameController.PlayAreaSize, 7));
            StaticObject maze = new StaticObject(mazeRenderObject, new Collider(mazeRenderObject));

            return maze;
        }
    }
}
