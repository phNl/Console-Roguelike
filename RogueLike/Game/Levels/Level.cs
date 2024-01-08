using RogueLike.GameObjects;

namespace RogueLike.Game.Levels
{
    internal abstract class Level
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
            }

            _objectsToRemove.Clear();
        }

        public void AddPreparedObjects()
        {
            foreach (GameObject obj in _objectsToAdd)
            {
                AddObjectImmediately(obj);
            }

            _objectsToAdd.Clear();
        }

        public void AddObjectImmediately(GameObject obj)
        {
            _objects.Add(obj);
        }
    }
}
