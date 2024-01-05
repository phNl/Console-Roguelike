using RogueLike.Collision;
using RogueLike.Render;

namespace RogueLike.GameObjects
{
    internal class StaticObject : GameObject
    {
        public StaticObject(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }
    }
}
