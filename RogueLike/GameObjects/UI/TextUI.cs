using RogueLike.Render;

namespace RogueLike.GameObjects.UI
{
    internal class TextUI : UIElement
    {
        private string __text;
        public string Text
        {
            get => __text;
            set
            {
                if (__text != value)
                {
                    __text = GetRightLengthString(value);
                    RenderObject = GetRenderObjectFromText(__text);
                }
            }
        }

        private readonly int _textLength;
        public int TextLength => _textLength;

        public TextUI(string text, int textLength) : base(GetRenderObjectFromText(GetRightLengthString(text, textLength)))
        {
            __text = text;
            _textLength = textLength;
        }

        public TextUI(string text) : this(text, text.Length)
        {
        }

        public static RenderObject GetRenderObjectFromText(string text)
        {
            RenderBuffer renderBuffer = new RenderBuffer(new string[] { text });
            return new RenderObject(renderBuffer);
        }

        private static string GetRightLengthString(string text, int textLength)
        {
            if (textLength > text.Length)
            {
                return text;
            }
            else
            {
                return text.Substring(0, textLength);
            }
        }

        private string GetRightLengthString(string text)
        {
            return GetRightLengthString(text, TextLength);
        }
    }
}
