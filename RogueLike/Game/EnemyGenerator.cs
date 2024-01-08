using RogueLike.Collision;
using RogueLike.GameObjects.Characters.AI;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.Game
{
    internal static class EnemyGenerator
    {
        public static RangeEnemy GenerateRangeEnemy()
        {
            RenderObject enemyRenderObject = new RenderObject(new RenderBuffer(new string[] { "R" }));
            RangeEnemy enemy = new RangeEnemy(
                renderObject: enemyRenderObject,
                collider: new Collider(enemyRenderObject),
                weapon: new RangeWeapon(1.5, 5, 5, 25),
                maxHealthValue: 100,
                attackRange: 15,
                moveCooldownInSeconds: 1,
                playerFindDepth: 35,
                minDistanceToPlayer: 7
                );

            return enemy;
        }

        public static MeleeEnemy GenerateMeleeEnemy()
        {
            RenderObject enemyRenderObject = new RenderObject(new RenderBuffer(new string[] { "M" }));
            MeleeEnemy enemy = new MeleeEnemy(
                renderObject: enemyRenderObject,
                collider: new Collider(enemyRenderObject),
                weapon: new RangeWeapon(0.5, 0, 1, 10),
                maxHealthValue: 200,
                moveCooldownInSeconds: 0.125,
                playerFindDepth: 30
                );

            return enemy;
        }
    }
}
