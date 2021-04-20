using System;

namespace GameCollection
{
    class GameLauncher
    {
        static void Main(string[] args)
        {
            // public void Launch() {
            BaseBall baseball = new BaseBall();
            baseball.Run();
            //}
            WordGuess wordguess = new WordGuess();
            wordguess.Run();
        }
    }
}
