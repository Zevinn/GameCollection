using System;
using System.Collections.Generic;
using System.Text;

namespace GameCollection
{
    class WordGuess
    {
        public enum LEVEL
        {
            Easy,
            Hard
        }

        public LEVEL level { get; private set; }
        private string[] Words;
        private List<char> Attempts;
        private int chances { get; set; }

        public void Run()
        {
            while(Lobby() != 2)
            {
                Initialize();
                Play();
            }
        }
        private int Lobby()
        {
            string menu = null;
            while (menu != "1")
            {
                Console.WriteLine("### WELCOME TO WORDGUESS GAME ###");
                Console.WriteLine("### 1. Start                  ###");
                Console.WriteLine("### 2. Exit                   ###");
                Console.Write("### -> ");
                menu = Console.ReadLine();
            }

            return Convert.ToInt32(menu);
        }
        private void Initialize()
        {
            Console.Clear();

            string lev = null;
            while (lev != "1" && lev != "2")
            {
                Console.WriteLine("### WELCOME TO WORDGUESS GAME ###");
                Console.WriteLine("### 1. Easy                   ###");
                Console.WriteLine("### 2. Hard                   ###");
                Console.Write("### -> ");
                lev = Console.ReadLine();
                Console.Clear();
            }
            
            switch(lev)
            {
                case "1":
                    level = LEVEL.Easy;
                    chances = 10;
                    break;
                case "2":
                    level = LEVEL.Hard;
                    chances = 15;
                    break;
            }
            SetWords();
        }
        #region WORDS SETTING: should be hidden at all times
        private void SetWords()
        {
            if (Words != null)
                Words = null;
            switch(level)
            {
                case LEVEL.Easy:
                    Words = new string[] 
                    {
                        "surge", "petty", "stride", "gasp", "verdict", 
                        "peer", "hostile", "flimsy", "analogy", "intrude"
                    };
                    break;
                case LEVEL.Hard:
                    Words = new string[]
                    {
                        "accommodate", "connotation", "triumph", "wholesome", "bothersome"
                    };
                    break;

            }
        }
        #endregion

        private void AttemptsHistory()
        {
            Console.WriteLine("### Attempts ###");
            if (Attempts.Count == 0)
            {
                Console.WriteLine("###   Your   ###");
                Console.WriteLine("### attempts ###");
                Console.WriteLine("###  appear  ###");
                Console.WriteLine("###   here   ###");
            }
            else
            {
                foreach (char c in Attempts)
                    Console.WriteLine($"###     {c}    ###");
            }

            Console.WriteLine("################");
        }

        private string Guess()
        {
            string input = null;

            
            while (input == null)
            {
                Console.WriteLine("######## GUESS THE WORD ########");
                try
                {
                    input = Console.ReadLine().Trim();
                    // how to catch numbers and other char?

                    if (input.Length != 1) // only a char allowed FOR NOW
                        throw new ArgumentException();
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine("# Only a single letter allowed #");
                    Console.ReadLine();
                    Console.Clear();
                    input = null;
                    continue;
                }
                catch (Exception e)
                {
                    // when not alphabet
                }
                
            }



            return input;
        }

        private void Play()
        {
            Console.Clear();

            List<int> completed = new List<int>(); // to store index of words that are already done
            Random ran = new Random();

            while (completed.Count != Words.Length)
            {
                int index = 0, round = 0;
                string input = null;
                Attempts = new List<char>();
                Console.Clear();
                index = ran.Next(0, Words.Length);
                if (completed.Contains(index)) continue; // if completed word, choose another word

                string word = null;
                for (int i = 0; i < Words[index].Length; i++)
                    word += "'\u25cf'";

                while (chances - round > 0)
                {
                    




                    input = Guess();
                    AttemptsHistory();
                    round++;
                }

                if(chances - round == 0)
                {
                    Console.WriteLine("Aww, you are out of your chances. ):");
                    Console.WriteLine($"The word was {Words[index]}");
                    Console.ReadLine();
                }
                else
                {
                    completed.Add(index);
                }

                if (completed.Count == Words.Length)
                {
                    Console.WriteLine("Congrats! You are done for the level!");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    string answer = null;
                    while (answer != "Q" && answer != "N")
                    {
                        Console.WriteLine("Continue?(Q: Quit to Lobby / N: Next word");
                        Console.Write(" -> ");
                        answer = Console.ReadLine();
                        answer = answer.ToUpper();
                        Console.Clear();
                    }
                    if (answer == "Q") return;
                }
                
            }
        }
    }
}

