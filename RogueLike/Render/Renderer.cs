using RogueLike.ConsoleExtenders;
using RogueLike.CustomMath;
using RogueLike.GameObjects;
using RogueLike.Render;

namespace RogueLike.RenderTools
{
    internal class Renderer
    {
        private readonly Vector2Int _bufferSize;
        public Vector2Int BufferSize => _bufferSize;

        private SortedDictionary<int, RenderBuffer> _layerBuffers = new SortedDictionary<int, RenderBuffer>();

        public Renderer(Vector2Int screenSize, string fontName, short fontSize)
        {
            ConsoleHelper.SetCurrentFont(fontName, fontSize);
            Console.CursorVisible = false;

            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(1, 1);
                Console.SetBufferSize(screenSize.x, screenSize.y);
                Console.SetWindowSize(
                    Math.Clamp(screenSize.x, 0, Console.LargestWindowWidth),
                    Math.Clamp(screenSize.y + 1, 0, Console.LargestWindowHeight));
            }
            else
            {
                new Exception("This program is only available for Windows.");
            }

            _bufferSize = screenSize;
            ClearAllBuffers();
        }

        public void ClearAllBuffers()
        {
            foreach (var buffer in _layerBuffers.Values)
            {
                buffer.FillWith(RenderBuffer.NullSymbol);
            }
        }

        public void AddObject(GameObject gameObject)
        {
            CreateLayerIfNotExists(gameObject.RenderObject.ZLayer);

            _layerBuffers[gameObject.RenderObject.ZLayer].InsertPattern(gameObject.RenderObject.RenderPattern, gameObject.Position);
        }

        public void Draw()
        {
            var renderBuffer = GetLayersUnion();

            Console.SetCursorPosition(0, 0);
            using (Stream stdout = Console.OpenStandardOutput(BufferSize.x * BufferSize.y))
            {
                stdout.Write(renderBuffer.GetAsArray(), 0, renderBuffer.Count);
            }
        }

        private RenderBuffer GetLayersUnion()
        {
            RenderBuffer layersUnion = new RenderBuffer(BufferSize);
            layersUnion.FillWith(RenderBuffer.NullSymbol);

            for (int y = 0; y < BufferSize.y; y++)
            {
                for (int x = 0; x < BufferSize.x; x++)
                {
                    foreach (int layerKey in _layerBuffers.Keys.OrderByDescending(x => x))
                    {
                        if (_layerBuffers[layerKey][x, y] != RenderBuffer.NullSymbol)
                        {
                            layersUnion[x, y] = _layerBuffers[layerKey][x, y];
                            break;
                        }
                    }
                }
            }

            return layersUnion;
        }

        private void CreateLayerIfNotExists(int layer)
        {
            if (_layerBuffers.ContainsKey(layer))
                return;

            _layerBuffers[layer] = new RenderBuffer(BufferSize);
        }
    }
}
