using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.Render;

namespace RogueLike.GameObjects.Projectiles
{
    internal abstract class Projectile : GameObject, IMovable
    {
        private Vector2Int _direction;
        public Vector2Int Direction
        {
            get => _direction;
            private set
            {
                _direction = new Vector2Int(
                    Math.Clamp(value.x, -1, 1),
                    Math.Clamp(value.y, -1, 1)
                );
            }
        }

        private float _speed;
        /// <summary>
        /// How many times will the projectile move in a second
        /// </summary>
        public float Speed => _speed;

        private float _lifetimeInSeconds = 0;
        public float LifetimeInSeconds => _lifetimeInSeconds;

        private float _maxLifetimeInSeconds;
        public float MaxLifetimeInSeconds => _maxLifetimeInSeconds;

        public bool IsInfiniteLifetime => MaxLifetimeInSeconds < 0;

        private bool _isLaunched = false;
        public bool IsLaunched => _isLaunched;

        private float _downTime = 0;

        private GameObject? _owner;
        public GameObject? Owner => _owner;

        protected Projectile(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }

        protected Projectile(RenderObject renderObject) : base(renderObject)
        {
        }

        protected Projectile(Collider collider) : base(collider)
        {
        }

        protected Projectile()
        {
        }

        /// <summary></summary>
        /// <param name="maxLifeTimeInSeconds">Infinite life time if less than 0</param>
        public void Launch(Vector2Int direction, float speed, GameObject owner, float maxLifeTimeInSeconds = -1)
        {
            Direction = direction;
            _speed = speed;
            _maxLifetimeInSeconds = maxLifeTimeInSeconds;
            _owner = owner;

            _isLaunched = true;
        }

        public abstract void Move(Vector2Int direction);

        public override void Update()
        {
            if (GameController.GameLoop != null)
            {
                float deltaTime = GameController.GameLoop.DeltaTime;
                _downTime += deltaTime;

                if (!IsInfiniteLifetime)
                    _lifetimeInSeconds += deltaTime;
            }

            if (_downTime >= 1 / Speed)
            {
                _downTime = 0;
                Move(Direction);
            }
            
            if (!IsInfiniteLifetime && LifetimeInSeconds > MaxLifetimeInSeconds)
            {
                OnLifetimeEnd();
                Destroy();
            }
        }

        protected virtual void OnCollision()
        {
        }

        protected virtual void OnLifetimeEnd()
        {
        }
    }
}
