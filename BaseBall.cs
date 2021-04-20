using System;
using System.Collections.Generic;
using System.Text;

namespace GameCollection
{
    class BaseBall
    {
        public enum LEVEL
        {
            Easy = 3,
            Normal,
            Hard
        }
        public LEVEL level { get; private set; }
        public int[] AInumber { get; private set; }
        public List<Tuple<int[], string>> Mytry;

        public int Chances { get; private set; }

        public void Run()
        {
            while (this.Lobby() != 3) // 3 means EXIT the game
            {
                Initialize();
                Play();
            }
        }

        private int Lobby()
        {
            string menu = null;
            while(menu != "1" && menu != "3")
            {
                Console.WriteLine("### WELCOME TO BASEBALL GAME ###");
                Console.WriteLine("### 1. Start                 ###");
                Console.WriteLine("### 2. How To Play           ###");
                Console.WriteLine("### 3. Exit                  ###");
                Console.Write    ("### -> ");
                menu = Console.ReadLine();
                if (menu == "2") HowToPlay();
            }

            return Convert.ToInt32(menu);
        }
        private void HowToPlay()
        {
            Console.WriteLine("### WELCOME TO BASEBALL GAME ###");
            Console.WriteLine("This game is a number guessing game. The AI will have a set of numbers.");
            Console.WriteLine("A set of 3 numbers in Easy mode, 4 numbers in Normal mode, and 5 numbers in Hard mode");
            Console.WriteLine("you guess the exact number within the given chances according to the difficulty.");
            Console.WriteLine("Within the number of guessing chances, The AI will give you the result of your guess.");
            Console.WriteLine();
            Console.WriteLine("The result format is in 'strike and ball'.");
            Console.WriteLine("If you have the same number at the same spot of the answer set, You get 1 strike.");
            Console.WriteLine("If you have the same number but at a different spot of the answer set, You get 1 ball.");
            Console.WriteLine("If you have nothing matched, You get OUT.");
            Console.WriteLine();
            Console.WriteLine("Here's an example.");
            Console.WriteLine("Answer: 3961");
            Console.WriteLine("When you guess 1972,");
            Console.WriteLine("You have matched 9 at the same spot -> 1 strike");
            Console.WriteLine("You have matched 1 but at a different spot -> 1 ball");
            Console.WriteLine("2 & 7 you have matched are not anywhere -> nothing");
            Console.WriteLine("The result is 1S1B, which means 1 strike 1 ball");
            Console.WriteLine("When you guess 2754");
            Console.WriteLine("When you guess ,");
            Console.WriteLine("You have no number matching the answer number -> OUT");
            Console.WriteLine();
            Console.WriteLine("Finally, according to the results, when you guess the answer, you win.");
            Console.WriteLine("If not within the given chances, you lose.");
            Console.WriteLine();
            Console.WriteLine("Press Enter key to go back");
            Console.ReadLine();
            Console.Clear();
        }
        private void Initialize()
        {
            Console.Clear(); // Get ready for the Console to run the game

            string lev = null;
            while (lev != "1" && lev != "2" && lev != "3")
            {
                Console.WriteLine("### WELCOME TO BASEBALL GAME ###");
                Console.WriteLine("### 1. Easy                  ###");
                Console.WriteLine("### 2. Normal                ###");
                Console.WriteLine("### 3. Hard                  ###");
                Console.Write("### Select a level: ");
                lev = Console.ReadLine();
                Console.Clear();
            }

            if (lev == "1")
                level = LEVEL.Easy;
            else if (lev == "2")
                level = LEVEL.Normal;
            else
                level = LEVEL.Hard;

            switch(level)
            {
                case LEVEL.Easy:
                    Chances = 10;
                    break;
                case LEVEL.Normal:
                    Chances = 12;
                    break;
                case LEVEL.Hard:
                    Chances = 15;
                    break;
            }

            GenerateNumber();
        }

        private void Play()
        {
            Mytry = new List<Tuple<int[], string>>();

            while (Chances > 0)
            {
                int[] guess = new int[AInumber.Length];

                Console.Clear();
                GuessHistory();
                Console.WriteLine();

                guess = GuessInput();


                Mytry.Add(GetResultOfGuess(guess));

                if(Mytry[Mytry.Count-1].Item2 == "A")
                {
                    Console.Write("Correct! The answer was ");
                    foreach (int n in AInumber)
                        Console.Write(n);
                    Console.WriteLine();
                    Console.ReadLine();
                    break;
                }

                Chances--;
                if (Chances == 0)
                {
                    Console.Write("You are out of all chances ): The answer was ");
                    foreach (int n in AInumber)
                        Console.Write(n);
                    Console.WriteLine();
                    Console.ReadLine();
                }
            }
            Console.Clear();
        }

        private void GenerateNumber()
        {
            AInumber = new int[(int)level];

            Random rand = new Random();
            AInumber[0] = rand.Next(1, 10);
            int i = 1;
            while(i < AInumber.Length)
            {
                bool diff = false;
                int _nextNum = rand.Next(1, 10);
                for (int j = 0; j < i; j++)
                {
                    if (_nextNum == AInumber[j])
                    {
                        diff = true;
                        break;
                    }
                }
                if (diff) continue;
                
                AInumber[i++] = _nextNum;
                
            }
        }

        private int[] GuessInput()
        {
            int input = 0;
            int[] num = new int[AInumber.Length];
            while (input == 0)
            {
                Console.Write("Guess the number: ");
                try
                {
                    input = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString()); 
                    Console.WriteLine($"Only a set of {(int)level} numbers is valid.");
                    Console.ReadLine();
                    //Console.Clear();
                    continue;
                }

                if (input < Math.Pow(10, AInumber.Length - 1) || input >= Math.Pow(10, AInumber.Length)) // Math.Pow is to calcuate square of a number
                {
                    Console.WriteLine($"Must be a set of {(int)level} numbers.");
                    Console.ReadLine();
                    //Console.Clear();
                    input = 0;
                    continue;
                }

                num = ArrangeNumber(input);

                bool valid = true;
                for (int i = 0; i < num.Length; i++)
                {
                    for(int j = i+1; j < num.Length; j++)
                    {
                        if (num[i] == num[j]) valid = false;
                    }
                    if (!valid) break;
                }

                if (!valid)
                {
                    Console.WriteLine("The each number must be a different number.");
                    Console.ReadLine();
                    //Console.Clear();
                    num = null;
                    input = 0;
                    continue;
                }
            }

            return num;
        }

        private int[] ArrangeNumber(int input)
        {
            int[] num = new int[AInumber.Length];

            int divide = (int)Math.Pow(10, AInumber.Length-1);
            num[0] = input / divide;
            for(int i = 1; i < num.Length; i++)
            {
                divide /= 10;
                num[i] = input / divide % 10;
            }

            //num[0] = input / 10000 % 1;
            //num[1] = input / 1000 % 10;
            //num[2] = input / 100 % 10;
            //num[3] = input / 10 % 10;
            //num[4] = input / 1 % 10;

            //num[0] = input / 100 % 1;
            //num[1] = input / 10 % 10;
            //num[2] = input / 1 % 10; // 제일 앞자리만 divide % 1, 그 외 모두 divide*10씩 커지는 값으로 나누고 %10

            return num;
        }
        private Tuple<int[], string> GetResultOfGuess(int[] guess)
        {
            int strike = 0, ball = 0;

            for(int i = 0; i < AInumber.Length; i++)
            {
                if (AInumber[i] == guess[i]) strike++;
                for(int j = 0; j < guess.Length; j++)
                {
                    if (i == j) continue;
                    if (AInumber[i] == guess[j]) ball++;
                }
            }
            if (strike == 4)
                return Tuple.Create(guess, "A");
            else if (strike == 0 && ball == 0)
                return Tuple.Create(guess, "OUT ");
            else
                return Tuple.Create(guess, $"{strike}S{ball}B");
        }

        private void GuessHistory()
        {
            
            Console.WriteLine("# # # Guess History # # #");
            if(Mytry.Count == 0)
                Console.WriteLine("#   Results show here   #");
            else
            {
                Console.WriteLine("# == Number | Result == #");
                //Mytry.ForEach(guess => Console.WriteLine($"#     {guess.Item1}      {guess.Item2}     #"));

                for (int i = 0; i < Mytry.Count; i++)
                {
                    Console.Write("      ");
                    int[] guess = Mytry[i].Item1;
                    
                    foreach (int n in guess) 
                        Console.Write(n);
                    Console.WriteLine($"     {Mytry[i].Item2}      ");
                }
            }

            Console.WriteLine("# # # # # # # # # # # # #");
        }
    }
}
