using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Render;

namespace RogueLike.GameObjects.Characters
{
    internal class Character : GameObject, IMovable
    {
        public Character()
        {
        }

        public Character(RenderObject renderObject) : base(renderObject)
        {
        }

        public Character(Collider collider) : base(collider)
        {
        }

        public Character(RenderObject renderObject, Collider collider) : base(renderObject, collider)
        {
        }

        public void Move(Vector2Int direction)
        {
            if (CollisionHandler.GetCollisionObjectsAtPoint(Position + direction).Count == 0)
                Position += direction;
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
