using RogueLike.CustomMath;

namespace RogueLike.AdditionalTools
{
    internal interface IReadOnlyArray2DWrapper<ArrayType> : IEnumerable<ArrayType>
    {
        public ArrayType this[int x, int y] { get; }
        public ArrayType this[Vector2Int point] { get; }
        public Vector2Int ArraySize { get; }
        public int Count { get; }

        public ArrayType[] GetAsArray();
        public bool IsPointInBounds(Vector2Int point);
    }
}
