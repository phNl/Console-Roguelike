using RogueLike.CustomMath;
using System.Collections;
using System.Drawing;

namespace RogueLike.AdditionalTools
{
    internal abstract class Array2DWrapper<ArrayType> : IReadOnlyArray2DWrapper<ArrayType>, IEnumerable<ArrayType>
    {
        private ArrayType[] _array;
        protected ArrayType[] Array => _array;

        private readonly Vector2Int _arraySize;
        public Vector2Int ArraySize => _arraySize;

        public int Count => _array.Length;

        public Array2DWrapper(Vector2Int arraySize)
        {
            _arraySize = arraySize;
            _array = new ArrayType[ArraySize.x * ArraySize.y];
        }

        public Array2DWrapper(Array2DWrapper<ArrayType> array2DWrapper)
        {
            _arraySize = array2DWrapper.ArraySize;
            _array = array2DWrapper.Array;
        }

        public ArrayType this[int x, int y]
        {
            get
            {
                return this[new Vector2Int(x, y)];
            }
            set
            {
                this[new Vector2Int(x, y)] = value;
            }
        }

        public ArrayType this[Vector2Int point]
        {
            get
            {
                if (!IsPointInBounds(point))
                {
                    ThrowOutOfBoundsException(point);
                }

                return Array[point.x + point.y * ArraySize.x];
            }
            set
            {
                if (!IsPointInBounds(point))
                {
                    ThrowOutOfBoundsException(point);
                }

                _array[point.x + point.y * ArraySize.x] = value;
            }
        }

        public void FillWith(ArrayType byteSymbol)
        {
            _array = Enumerable.Repeat(byteSymbol, ArraySize.x * ArraySize.y).ToArray();
        }

        public ArrayType[] GetAsArray()
        {
            var array = new ArrayType[Count];
            System.Array.Copy(Array, array, Count);
            return array;
        }

        public IEnumerator<ArrayType> GetEnumerator()
        {
            return ((IEnumerable<ArrayType>)Array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Array.GetEnumerator();
        }

        public bool IsPointInBounds(Vector2Int point)
        {
            return point.InRectangle(Vector2Int.Zero, ArraySize - 1);
        }

        private void ThrowOutOfBoundsException(Vector2Int point)
        {
            throw new Exception(
                $"Out of bounds in {this} (indexes: x = {point.x}, y = {point.y}) (arraySize: sizeX = {ArraySize.x}, sizeY = {ArraySize.y})"
            );
        }
    }
}
