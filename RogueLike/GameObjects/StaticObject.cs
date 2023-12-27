using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.GameObjects
{
    internal class StaticObject : GameObject
    {
        public StaticObject()
        {
        }

        public StaticObject(RenderObject renderObject) : base(renderObject)
        {
        }

        public StaticObject(Collider collider) : base(collider)
        {
        }

        public StaticObject(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }
    }
}
