using RogueLike.AdditionalTools;
using RogueLike.CustomMath;
using System.Text;

namespace RogueLike.Render
{
    internal class RenderBuffer : Array2DWrapper<byte>, IReadOnlyRenderBuffer
    {
        public static byte NullSymbol => 0;
        public static Encoding RenderBufferEncoding => Encoding.Unicode;

        public RenderBuffer(Vector2Int bufferSize) : base(bufferSize)
        {
            FillWith(NullSymbol);
        }

        public RenderBuffer(string[] stringPattern) : base(new Vector2Int(GetMaxStringSize(stringPattern), stringPattern.Length))
        {
            FillWith(NullSymbol);
            InsertPattern(stringPattern, Vector2Int.Zero);
        }

        public RenderBuffer(string[] stringPattern, char nullSymbolReplacer) : base(new Vector2Int(GetMaxStringSize(stringPattern), stringPattern.Length))
        {
            FillWith(NullSymbol);
            InsertPattern(stringPattern, nullSymbolReplacer, Vector2Int.Zero);
        }

        public static byte GetByteFromChar(char symbol)
        {
            return RenderBufferEncoding.GetBytes($"{symbol}")[0];
        }

        public static char GetCharFromByte(byte number)
        {
            return RenderBufferEncoding.GetString(new byte[1] { number })[0];
        }

        public void FillWith(char symbol)
        {
            FillWith(GetByteFromChar(symbol));
        }

        public static int GetMaxStringSize(string[] stringArray)
        {
            int size = 0;

            foreach (string str in stringArray)
            {
                if (str.Length > size)
                    size = str.Length;
            }

            return size;
        }

        public override string ToString()
        {
            string str = "";
            for (int y = 0; y < ArraySize.y; y++)
            {
                for (int x = 0; x < ArraySize.x; x++)
                {
                    str += GetCharFromByte(this[x, y]);
                }

                str += "\n";
            }

            return str;
        }

        public void InsertPattern(string[] stringPattern, Vector2Int position)
        {
            InsertPattern(position, (y) => stringPattern[y].Length, stringPattern.Length, (xy) => GetByteFromChar(stringPattern[xy.y][xy.x]));
        }

        public void InsertPattern(string[] stringPattern, char nullSymbolReplacer, Vector2Int position)
        {
            for (int i = 0; i < stringPattern.Length; i++)
            {
                stringPattern[i] = stringPattern[i].Replace(nullSymbolReplacer, GetCharFromByte(NullSymbol));
            }

            InsertPattern(position, (y) => stringPattern[y].Length, stringPattern.Length, (xy) => GetByteFromChar(stringPattern[xy.y][xy.x]));
        }

        public void InsertPattern(byte[][] bytePattern, Vector2Int position)
        {
            InsertPattern(position, (y) => bytePattern[y].Length, bytePattern.Length, (xy) => bytePattern[xy.y][xy.x]);
        }

        public void InsertPattern(IReadOnlyRenderBuffer pattern, Vector2Int position)
        {
            InsertPattern(position, (y) => pattern.ArraySize.x, pattern.ArraySize.y, (xy) => pattern[xy]);
        }

        public void InsertPattern(RenderBuffer pattern)
        {
            InsertPattern(pattern, Vector2Int.Zero);
        }

        protected void InsertPattern(Vector2Int position, Func<int, int> patternSizeX, int patternSizeY, Func<Vector2Int, byte> GetElementOfPattern)
        {
            if (position.y >= ArraySize.y || position.x >= ArraySize.x)
                return;

            for (int y = Math.Clamp(position.y, 0, ArraySize.y - 1);
                y < Math.Clamp(position.y + patternSizeY, position.y, ArraySize.y);
                y++)
            {
                for (int x = Math.Clamp(position.x, 0, ArraySize.x - 1);
                    x < Math.Clamp(position.x + patternSizeX(y), position.x, ArraySize.x);
                    x++)
                {
                    this[x, y] = GetElementOfPattern(new Vector2Int(x - position.x, y - position.y));
                }
            }
        }
    }
}
