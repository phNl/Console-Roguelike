using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.GameObjects
{
    internal abstract class GameObject : IUpdate
    {
        public event Action<GameObject>? OnDestroyAction;
        public event Action<Vector2Int, Vector2Int>? PositionChanged;

        private Vector2Int __position = Vector2Int.Zero;
        public Vector2Int Position {
            get => __position;
            set
            {
                if (__position != value)
                {
                    var prevPos = __position;
                    __position = value;
                    PositionChanged?.Invoke(prevPos, __position);
                }
            }
        }

        private Collider _collider;
        public Collider Collider
        {
            get => _collider;
            protected set => _collider = value;
        }

        private RenderObject _renderObject;
        public RenderObject RenderObject
        {
            get => _renderObject;
            protected set => _renderObject = value;
        }

        private bool _isPreparedToDestroy = false;
        public bool IsPreparedToDestroy => _isPreparedToDestroy;

        public GameObject(RenderObject renderObject, Collider collider)
        {
            _renderObject = renderObject;
            _collider = collider;
        }

        public void Update(double deltaTime)
        {
            if (!IsPreparedToDestroy)
            {
                OnUpdate(deltaTime);
            }
        }

        public void Destroy()
        {
            _isPreparedToDestroy = true;
            OnDestroy();
            //OnDestroyAction?.Invoke(this);
        }

        protected virtual void OnUpdate(double deltaTime)
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}
