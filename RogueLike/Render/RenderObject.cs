using RogueLike.CustomMath;
using System.Text;

namespace RogueLike.Render
{
    internal struct RenderObject
    {
        private RenderBuffer _renderPattern;
        public IReadOnlyRenderBuffer RenderPattern => _renderPattern;

        private int _zLayer;
        public int ZLayer => _zLayer;

        /// <summary>
        /// Create render object
        /// </summary>
        /// <param name="renderPattern">Render pattern like image of object</param>
        /// <param name="zLayer">Depth of render object</param>
        public RenderObject(RenderBuffer renderPattern, int zLayer)
        {
            _renderPattern = renderPattern;
            _zLayer = zLayer;
        }

        /// <summary>
        /// Create render object on 0 z-layer
        /// </summary>
        /// <param name="renderPattern">Render pattern like image of object</param>
        public RenderObject(RenderBuffer renderPattern) : this(renderPattern, 0)
        {
        }

        /// <summary>
        /// Create an empty render object on 0 z-layer
        /// </summary>
        public RenderObject() : this(new RenderBuffer(Vector2Int.Zero), 0)
        {
        }

        public static RenderObject ChangeZLayer(RenderObject renderObject, int zlayer)
        {
            return new RenderObject(renderObject._renderPattern, zlayer);
        }

        public RenderObject ChangeZLayer(int zlayer)
        {
            return ChangeZLayer(this, zlayer);
        }
    }
}
