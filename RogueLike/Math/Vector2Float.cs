namespace RogueLike.CustomMath
{
    internal struct Vector2Float
    {
        public float x;
        public float y;

        public static Vector2Float Zero => new Vector2Float(0, 0);

        public Vector2Float(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2Float(float x) : this(x, x)
        {
        }

        public Vector2Float() : this(0, 0)
        {
        }

        public static implicit operator Vector2Float(Vector2Int vector2Int)
        {
            return new Vector2Float(vector2Int.x, vector2Int.y);
        }

        public static explicit operator Vector2Int(Vector2Float vector2Float)
        {
            return new Vector2Int((int)vector2Float.x, (int)vector2Float.y);
        }

        public static bool operator ==(Vector2Float left, Vector2Float right)
        {
            return (left.x == right.x)
                && (left.y == right.y);
        }

        public static bool operator !=(Vector2Float left, Vector2Float right)
        {
            return !(left == right);
        }

        public static Vector2Float operator -(Vector2Float value)
        {
            return new Vector2Float(-value.x, -value.y);
        }

        public static Vector2Float operator +(Vector2Float left, Vector2Float right)
        {
            return new Vector2Float(left.x + right.x, left.y + right.y);
        }

        public static Vector2Float operator +(Vector2Float left, int right)
        {
            return new Vector2Float(left.x + right, left.y + right);
        }

        public static Vector2Float operator +(int left, Vector2Float right)
        {
            return right + left;
        }

        public static Vector2Float operator -(Vector2Float left, Vector2Float right)
        {
            return new Vector2Float(left.x - right.x, left.y - right.y);
        }

        public static Vector2Float operator -(Vector2Float left, int right)
        {
            return new Vector2Float(left.x - right, left.y - right);
        }

        public static Vector2Float operator *(Vector2Float left, Vector2Float right)
        {
            return new Vector2Float(left.x * right.x, left.y * right.y);
        }

        public static Vector2Float operator *(Vector2Float left, int right)
        {
            return new Vector2Float(left.x * right, left.y * right);
        }

        public static Vector2Float operator *(int left, Vector2Float right)
        {
            return right * left;
        }

        public static Vector2Float operator /(Vector2Float left, Vector2Float right)
        {
            return new Vector2Float(left.x / right.x, left.y / right.y);
        }

        public static Vector2Float operator /(Vector2Float left, int right)
        {
            return new Vector2Float(left.x / right, left.y / right);
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Vector2Float other = (Vector2Float)obj;
                return this == other;
            }
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
