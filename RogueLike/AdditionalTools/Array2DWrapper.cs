using RogueLike.CustomMath;

namespace RogueLike.AdditionalTools
{
    internal abstract class Array2DWrapper<ArrayType> : IReadOnlyArray2DWrapper<ArrayType>
    {
        private ArrayType[] _array;
        protected ArrayType[] Array => _array;

        private Vector2Int _arraySize;
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
                CheckIfOutOfBounds(new Vector2Int(x, y));
                return Array[x + y * ArraySize.x];
            }
            set
            {
                CheckIfOutOfBounds(new Vector2Int(x, y));
                Array[x + y * ArraySize.x] = value;
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

        private bool CheckIfOutOfBounds(Vector2Int coord)
        {
            if (coord.x >= ArraySize.x || coord.y >= ArraySize.y || coord.x < 0 || coord.y < 0)
            {
                throw new Exception($"Out of bounds in {this} " +
                    $"(indexes: x = {coord.x}, y = {coord.y}) " +
                    $"(arraySize: sizeX = {ArraySize.x}, sizeY = {ArraySize.y})");
            }
            else
            {
                return false;
            }
        }
    }
}
