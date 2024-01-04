using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.GameObjects.Characters
{
    internal abstract class Character : GameObject, IMovable
    {
        /// <summary>
        /// Invoke when the character moves in a direction
        /// </summary>
        public event Action<Vector2Int>? OnMove;

        public Character(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }

        public Character(RenderObject renderObject) : base(renderObject)
        {
        }

        public Character(Collider collider) : base(collider)
        {
        }

        public Character()
        {
        }

        public void Move(Vector2Int direction)
        {
            Position += direction;

            if (CollisionHandler.HasCollisionWithAny(this, CollisionMode.Only_Colliders))
            {
                Position -= direction;
            }
            else
            {
                OnMove?.Invoke(direction);
            }
        }

        public void MoveUp()
        {
            Move(Vector2Int.Up);
        }

        public void MoveDown()
        {
            Move(Vector2Int.Down);
        }

        public void MoveLeft()
        {
            Move(Vector2Int.Left);
        }

        public void MoveRight()
        {
            Move(Vector2Int.Right);
        }
    }
}
