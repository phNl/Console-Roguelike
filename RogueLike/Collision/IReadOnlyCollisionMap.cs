using RogueLike.AdditionalTools;
using RogueLike.GameObjects;

namespace RogueLike.Collision
{
    internal interface IReadOnlyCollisionMap : IReadOnlyArray2DWrapper<List<GameObject>>
    {
    }
}
