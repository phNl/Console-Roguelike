namespace RogueLike.CustomMath
{
    internal struct Vector2Int :
        IEquatable<Vector2Int>,
        IComparable<Vector2Int>
    {
        public int x;
        public int y;

        public static Vector2Int Zero => new Vector2Int(0, 0);
        public static Vector2Int Up => new Vector2Int(0, -1);
        public static Vector2Int Down => new Vector2Int(0, 1);
        public static Vector2Int Right => new Vector2Int(1, 0);
        public static Vector2Int Left => new Vector2Int(-1, 0);

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2Int(int x) : this(x, x)
        {
        }

        public Vector2Int() : this(0, 0)
        {
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Vector2Int other = (Vector2Int)obj;
                return this == other;
            }
        }

        public bool Equals(Vector2Int other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static implicit operator Vector2Int(Vector2Float vector2Float)
        {
            return new Vector2Int((int)vector2Float.x, (int)vector2Float.y);
        }

        public static explicit operator Vector2Float(Vector2Int vector2Int)
        {
            return new Vector2Float(vector2Int.x, vector2Int.y);
        }

        public static bool operator ==(Vector2Int left, Vector2Int right)
        {
            return (left.x == right.x) 
                && (left.y == right.y);
        }

        public static bool operator !=(Vector2Int left, Vector2Int right)
        {
            return !(left == right);
        }

        public static Vector2Int operator -(Vector2Int value)
        {
            return new Vector2Int(-value.x, -value.y);
        }

        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.x + right.x, left.y + right.y);
        }

        public static Vector2Int operator +(Vector2Int left, int right)
        {
            return new Vector2Int(left.x + right, left.y + right);
        }
        
        public static Vector2Int operator +(int left, Vector2Int right)
        {
            return right + left;
        }

        public static Vector2Int operator -(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.x - right.x, left.y - right.y);
        }

        public static Vector2Int operator -(Vector2Int left, int right)
        {
            return new Vector2Int(left.x - right, left.y - right);
        }

        public static Vector2Int operator *(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.x * right.x, left.y * right.y);
        }

        public static Vector2Int operator *(Vector2Int left, int right)
        {
            return new Vector2Int(left.x * right, left.y * right);
        }

        public static Vector2Int operator *(int left, Vector2Int right)
        {
            return right * left;
        }

        public static Vector2Int operator /(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.x / right.x, left.y / right.y);
        }

        public static Vector2Int operator /(Vector2Int left, int right)
        {
            return new Vector2Int(left.x / right, left.y / right);
        }

        public static bool operator <=(Vector2Int left, Vector2Int right)
        {
            return left.x <= right.x && left.y <= right.y;
        }

        public static bool operator <(Vector2Int left, Vector2Int right)
        {
            return left.x < right.x && left.y < right.y;
        }

        public static bool operator >=(Vector2Int left, Vector2Int right)
        {
            return !(left < right);
        }

        public static bool operator >(Vector2Int left, Vector2Int right)
        {
            return !(left <= right);
        }

        public int CompareTo(Vector2Int other)
        {
            if (this == other) return 0;
            if (this <= other) return -1;
            if (this >= other) return 1;

            throw new NotImplementedException();
        }

        public static Vector2Int Abs(Vector2Int vector)
        {
            return new Vector2Int(Math.Abs(vector.x), Math.Abs(vector.y));
        }

        public Vector2Int Abs()
        {
            return Abs(this);
        }

        public Vector2Int Clamp(int min, int max)
        {
            return new Vector2Int(
                Math.Clamp(x, min, max),
                Math.Clamp(y, min, max)
            );
        }

        public Vector2Int Clamp(int min, Vector2Int max)
        {
            return new Vector2Int(
                Math.Clamp(x, min, max.x),
                Math.Clamp(y, min, max.y)
            );
        }

        public Vector2Int Clamp(Vector2Int min, int max)
        {
            return new Vector2Int(
                Math.Clamp(x, min.x, max),
                Math.Clamp(y, min.y, max)
            );
        }

        public Vector2Int Clamp(Vector2Int min, Vector2Int max)
        {
            return new Vector2Int(
                Math.Clamp(x, min.x, max.x),
                Math.Clamp(y, min.y, max.x)
            );
        }

        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public bool InRectangle(Vector2Int minPoint, Vector2Int maxPoint)
        {
            if (minPoint >= maxPoint && minPoint != maxPoint)
                throw new Exception("FirstPoint cannot be greater than SecondPoint");

            return x >= minPoint.x && x <= maxPoint.x && y >= minPoint.y && y <= maxPoint.y;
        }

        public List<Vector2Int> GetNeighborsPositions(Vector2Int center)
        {
            return new List<Vector2Int>()
            {
                center + Up,
                center + Down,
                center + Left,
                center + Right,
            };
        }

        public List<Vector2Int> GetNeighborsPositions()
        {
            return GetNeighborsPositions(this);
        }
    }
}
