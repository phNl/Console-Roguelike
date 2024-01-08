namespace RogueLike.GameObjects.Characters.AI
{
    internal abstract class AIHandler
    {
        private readonly Enemy _enemy;
        protected Enemy Enemy => _enemy;

        public AIHandler(Enemy enemy)
        {
            _enemy = enemy;
        }
    }
}
