using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.Game;
using RogueLike.GameObjects.Characters;
using RogueLike.GameObjects.Characters.Properties;
using RogueLike.Render;

namespace RogueLike.GameObjects
{
    internal class LevelExitDoor : StaticObject
    {
        public event Action? OnPlayerTriggered;

        private bool _isTriggered = false;

        private int _healValue;

        public LevelExitDoor(RenderObject renderObject, Collider collider, int healValue) : base(renderObject, collider)
        {
            _healValue = healValue;

            UpdateSubscriptionToPlayer(null, GameController.Player);
            GameController.OnPlayerChanged += UpdateSubscriptionToPlayer;
        }

        private void CheckCollision(Vector2Int direction)
        {
            if (GameController.Player != null && !_isTriggered)
            {
                var collisionPointsWithPlayer = CollisionHandler.GetCollisionPointsBetweenObjects(this, GameController.Player);
                if (collisionPointsWithPlayer.Count > 0)
                {
                    OnPlayerTriggered?.Invoke();
                    _isTriggered = true;
                    (GameController.Player as IHealable)?.Heal(_healValue);
                    GameController.GenerateAndLoadInGameLevel();
                }
            }
        }

        private void UpdateSubscriptionToPlayer(Player? oldPlayer, Player? newPlayer)
        {
            if (oldPlayer != null)
            {
                oldPlayer.OnMove -= CheckCollision;
            }

            if (newPlayer != null)
            {
                newPlayer.OnMove += CheckCollision;
            }
        }

        protected override void OnDestroy()
        {
            if (GameController.Player != null)
            {
                UpdateSubscriptionToPlayer(GameController.Player, null);
            }

            GameController.OnPlayerChanged -= UpdateSubscriptionToPlayer;
        }
    }
}
