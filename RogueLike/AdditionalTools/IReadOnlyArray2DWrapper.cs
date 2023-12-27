using RogueLike.CustomMath;

namespace RogueLike.AdditionalTools
{
    internal interface IReadOnlyArray2DWrapper<ArrayType>
    {
        public ArrayType this[int x, int y] { get; }
        public Vector2Int ArraySize { get; }
        public int Count { get; }
    }
}
