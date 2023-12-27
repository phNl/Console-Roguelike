using RogueLike.GameObjects;

namespace RogueLike.Game
{
    internal class Level
    {
        private List<GameObject> _objects;
        public IReadOnlyList<GameObject> Objects => _objects;

        public Level(List<GameObject> objects)
        {
            _objects = objects;
        }

        public Level() : this(new List<GameObject>())
        {
        }

        public void AddObject(GameObject obj)
        {
            _objects.Add(obj);
            obj.OnDestroyAction += RemoveObject;
        }

        public void RemoveObject(GameObject obj)
        {
            obj.OnDestroyAction -= RemoveObject;
            _objects.Remove(obj);
        }
    }
}
