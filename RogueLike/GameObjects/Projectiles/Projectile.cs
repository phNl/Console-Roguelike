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

        private double _speed;
        /// <summary>
        /// How many times will the projectile move in a second
        /// </summary>
        public double Speed => _speed;

        private double _lifetimeInSeconds = 0;
        public double LifetimeInSeconds => _lifetimeInSeconds;

        private double _maxLifetimeInSeconds;
        public double MaxLifetimeInSeconds => _maxLifetimeInSeconds;

        public bool IsInfiniteLifetime => MaxLifetimeInSeconds < 0;

        private bool _isLaunched = false;
        public bool IsLaunched => _isLaunched;

        private double _stayTime = 0;

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

            OnLaunch();
            _isLaunched = true;
        }

        public abstract void Move(Vector2Int direction);

        public override sealed void Update(double deltaTime)
        {
            if (IsLaunched)
            {
                if (GameController.GameLoop != null)
                {
                    _stayTime += deltaTime;

                    if (!IsInfiniteLifetime)
                        _lifetimeInSeconds += deltaTime;
                }

                if (_stayTime >= 1 / Speed)
                {
                    _stayTime = 0;
                    Move(Direction);
                }
            
                if (!IsInfiniteLifetime && LifetimeInSeconds > MaxLifetimeInSeconds)
                {
                    OnLifetimeEnd();
                    Destroy();
                }
            }

            OnUpdate();
        }

        protected virtual void OnLaunch()
        {
        }

        protected virtual void OnCollision()
        {
        }

        protected virtual void OnLifetimeEnd()
        {
        }

        protected virtual void OnUpdate()
        {
        }
    }
}
