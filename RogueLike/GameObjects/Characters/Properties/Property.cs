using System.Numerics;

namespace RogueLike.GameObjects.Characters.Properties
{
    internal abstract class Property<T>
        where T : IComparable, IComparable<T>, IEquatable<T>, ISignedNumber<T>
    {
        
    }
}
