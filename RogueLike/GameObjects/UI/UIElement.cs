using RogueLike.Collision;
using RogueLike.Render;

namespace RogueLike.GameObjects.UI
{
    internal abstract class UIElement : GameObject
    {
        public UIElement(RenderObject renderObject) : base(renderObject, Collider.Empty)
        {
        }

        protected void ChangeRenderObject(RenderObject renderObject)
        {
            RenderObject = renderObject;
        }
    }
}
