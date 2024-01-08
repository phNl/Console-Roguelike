using RogueLike.Collision;
using RogueLike.CustomMath;
using RogueLike.GameObjects;
using RogueLike.GameObjects.Characters.AI;
using RogueLike.GameObjects.UI;
using RogueLike.Render;
using RogueLike.Weapons;

namespace RogueLike.Game.Levels
{
    internal class LevelsGenerator
    {
        public static MazeLevel GenerateInGameLevel()
        {
            MazeLevel level = new MazeLevel();

            // Level exit trigger
            RenderObject levelExitDoorRenderObject = new RenderObject(new RenderBuffer(new string[] { "E" }));
            Collider levelExitDoorCollider = new Collider(levelExitDoorRenderObject, true);
            LevelExitDoor levelExitDoor = new LevelExitDoor(levelExitDoorRenderObject, levelExitDoorCollider);
            levelExitDoor.Position = new Vector2Int(20, 20);
            level.PrepareAddObject(levelExitDoor);

            // Test Range Enemy
            var rangeEnemy = EnemyGenerator.GenerateRangeEnemy();
            rangeEnemy.Position = new Vector2Int(25, 10);
            level.PrepareAddObject(rangeEnemy);

            // Test Melee Enemy
            var meleeEnemy = EnemyGenerator.GenerateMeleeEnemy();
            meleeEnemy.Position = new Vector2Int(10, 25);
            level.PrepareAddObject(meleeEnemy);

            level.AddPreparedObjects();
            return level;
        }

        public static DeathScreenLevel GenerateDeathScreenLevel()
        {
            DeathScreenLevel level = new DeathScreenLevel();

            TextUI deathTextUI = new TextUI("YOU DIED");
            deathTextUI.Position = GameController.LevelSize / 2 - new Vector2Int(deathTextUI.TextLength / 2, GameController.LevelSize.y / 3);
            level.PrepareAddObject(deathTextUI);

            TextUI quitTextUI = new TextUI("QUIT");
            quitTextUI.Position = GameController.LevelSize / new Vector2Int(3, 3) - new Vector2Int(quitTextUI.TextLength / 2, 0);
            level.PrepareAddObject(quitTextUI);

            TextUI quitButtonTextUI = new TextUI("[Esc]");
            quitButtonTextUI.Position = quitTextUI.Position + new Vector2Int(0, 1);
            level.PrepareAddObject(quitButtonTextUI);

            TextUI continueTextUI = new TextUI("CONTINUE");
            continueTextUI.Position = GameController.LevelSize / new Vector2Int(3, 3) * new Vector2Int(2, 1) - new Vector2Int(continueTextUI.TextLength / 2, 0);
            level.PrepareAddObject(continueTextUI);

            TextUI continueButtonTextUI = new TextUI("[Enter]");
            continueButtonTextUI.Position = continueTextUI.Position + new Vector2Int(0, 1);
            level.PrepareAddObject(continueButtonTextUI);

            level.AddPreparedObjects();
            return level;
        }
    }
}
