using RogueLike.CustomMath;
using RogueLike.Game;

namespace RogueLike
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Level testLevel = new Level();

            GameController.Initialize(new Vector2Int(120, 60));
            GameController.LoadLevel(testLevel);

            SpinWait.SpinUntil(() => false);
        }
    }
}