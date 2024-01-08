using RogueLike.Render;

namespace RogueLike.Collision
{
    internal struct Collider
    {
        public static Collider Empty => new Collider();

        private readonly CollisionMap _collisionMap;
        public IReadOnlyCollisionMap CollisionMap => _collisionMap;

        private readonly bool _isTrigger;
        public bool IsTrigger => _isTrigger;

        /// <summary>
        /// Create collider
        /// </summary>
        /// <param name="collisionMap">CollisionMap of Collider</param>
        /// <param name="isTrigger">Is the collider a trigger?</param>
        public Collider(CollisionMap collisionMap, bool isTrigger)
        {
            _collisionMap = collisionMap;
            _isTrigger = isTrigger;
        }

        public Collider(RenderObject renderObject, bool isTrigger = false)
        {
            _collisionMap = Collision.CollisionMap.GetCollisionMapFromRenderPattern(renderObject.RenderPattern);
            _isTrigger = isTrigger;
        }

        /// <summary>
        /// Create solid (non-trigger) collider (isTrigger = false)
        /// </summary>
        /// <param name="collisionMap"></param>
        public Collider(CollisionMap collisionMap) : this(collisionMap, false)
        {
        }

        /// <summary>
        /// Create empty collider as trigger
        /// </summary>
        public Collider() : this(Collision.CollisionMap.Empty, true)
        {
        }
    }
}
