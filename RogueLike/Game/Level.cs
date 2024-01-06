using RogueLike.GameObjects;

namespace RogueLike.Game
{
    internal class Level
    {
        private List<GameObject> _objects;
        public IReadOnlyList<GameObject> Objects => new List<GameObject>(_objects);

        private List<GameObject> _objectsToRemove = new List<GameObject>();
        private List<GameObject> _objectsToAdd = new List<GameObject>();

        public Level(List<GameObject> objects)
        {
            _objects = objects;
        }

        public Level() : this(new List<GameObject>())
        {
        }

        public void PrepareAddObject(GameObject obj)
        {
            _objectsToAdd.Add(obj);
        }

        public void PrepareRemoveObject(GameObject obj)
        {
            _objectsToRemove.Add(obj);
        }

        public void RemovePreparedObjects()
        {
            foreach (GameObject obj in _objectsToRemove)
            {
                _objects.Remove(obj);
                obj.OnDestroyAction -= PrepareRemoveObject;
            }

            _objectsToRemove = new List<GameObject>();
        }

        public void AddPreparedObjects()
        {
            foreach (GameObject obj in _objectsToAdd)
            {
                _objects.Add(obj);
                obj.OnDestroyAction += PrepareRemoveObject;
            }

            _objectsToAdd = new List<GameObject>();
        }
    }
}
