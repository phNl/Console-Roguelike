using RogueLike.CustomMath;
using RogueLike.Game;

namespace RogueLike
{
    internal class Program
    {
        /// <summary>
        /// Dont change it! Changes only inside the Quit() method.
        /// </summary>
        private static bool __isQuit = false;
        protected static bool IsQuit => __isQuit;

        static void Main(string[] args)
        {
            GameController.Initialize(new Vector2Int(120, 60));
            GameController.LoadLevel(new Level());

            while (!IsQuit)
            {
            }
        }

        public static void Quit()
        {
            __isQuit = true;
        }
    }
}