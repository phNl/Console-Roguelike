using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.GameObjects
{
    internal interface IReadOnlyGameObject
    {
        public Vector2Int Position { get; }

        public Collider Collider { get; }

        public RenderObject RenderObject { get; }
    }
}
