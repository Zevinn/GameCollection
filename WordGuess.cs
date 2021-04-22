using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        private List<string> Attempts;
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
            Console.WriteLine("\t\t### Attempts ###");
            if (Attempts.Count == 0)
            {
                Console.WriteLine("\t\t###   Your   ###");
                Console.WriteLine("\t\t### attempts ###");
                Console.WriteLine("\t\t###  appear  ###");
                Console.WriteLine("\t\t###   here   ###");
            }
            else
            {
                foreach (string s in Attempts)
                    Console.WriteLine($"\t\t\t{s}");
            }

            Console.WriteLine("\t\t################");
            Console.WriteLine();
        }

        private bool Guess(int index)
        {
            // Problems :
            // 1) when correcting as matching each letter -> end the game as player wins
            // but, can't as it's hard to track whether the char array is all revealed or not.
            // if it was string, I could have.
            char[] word = new char[Words[index].Length];
            for (int i = 0; i < Words[index].Length; i++)
                word[i] = '\u25cf';

            string input = null;
            int round = 0;
            
            while (chances - round > 0)
            {
                Console.WriteLine($"Spare Attempts: {chances - round}");
                AttemptsHistory();
                Console.WriteLine("######## GUESS THE WORD ########");
                Console.Write("[ ");
                foreach (char c in word)
                    Console.Write(c);
                Console.WriteLine(" ]");
                Console.Write("-> ");
                try
                {
                    input = Console.ReadLine().Trim();
                    Regex regex = new Regex(@"^[a-zA-Z]$"); // only single alphabetic character
                    bool valid = regex.IsMatch(input);
                    if (input.Length == word.Length)
                    {
                        if (Words[index].CompareTo(input) == 0) // input.Length == word.Length
                        {
                            Console.WriteLine($"Correct! the word is [ {input} ]");
                            Console.ReadLine();

                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Beep! wrong, try again.");
                            Console.ReadLine();
                            Attempts.Add(input);
                            round++;
                        }
                    }
                    else if (regex.IsMatch(input))
                    {
                        if (Attempts.Contains(input))
                        {
                            Console.WriteLine("You've already tried the letter {0}!", input);
                            Console.ReadLine();
                            Console.Clear();
                            continue;
                        }

                        if (Words[index].Contains(input))
                        {
                            int startIndex = 0;
                            while (Words[index].IndexOf(input, startIndex) != -1)
                            {
                                int toReveal = Words[index].IndexOf(input, startIndex);
                                word[toReveal] = input.ToUpper().ToCharArray()[0]; // this MUST be one-sized char array as checked above
                                startIndex = toReveal + 1;
                            }
                        }
                        Attempts.Add(input);
                        round++;
                    }
                    else
                        throw new ArgumentException();
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine("Only single alphabetic letter is allowed");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                if (chances - round == 0)
                {
                    Console.WriteLine("Aww, you are out of your chances. ):");
                    Console.WriteLine($"The word was {Words[index].ToUpper()}");
                    Console.ReadLine();
                }
                Console.Clear();
            }
            return false;
        }

        private void Play()
        {
            Console.Clear();

            List<int> completed = new List<int>(); // to store index of words that are already done
            Random ran = new Random();

            while (completed.Count != Words.Length)
            {
                int index = 0;
                //string input = null;
                Attempts = new List<string>();
                Console.Clear();
                index = ran.Next(0, Words.Length);
                if (completed.Contains(index)) continue; // if completed word, choose another word

                if (Guess(index)) // player correct will return true, otherwise false
                {
                    completed.Add(index);
                }

                if (completed.Count == Words.Length)
                {
                    Console.WriteLine("Congrats! You have completed the level! Challenge the next level!");
                    Console.ReadLine();
                    Console.Clear();
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

