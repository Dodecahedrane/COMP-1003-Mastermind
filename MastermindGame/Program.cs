using System;

namespace COMP1003_Mastermind_Console_Game
{
    class GameParameters
    {
        public int Positions { get; set; }  //position to guess
        public int Numbers { get; set; }  //numbers (aka colors) to guess  
        public int[] CodeToGuess { get; set; }      //The code the user has to guess
        public bool Debug { get; set; }             //does user want the debug info

        public void SetUpGame(string error = null)
        {
            bool validParams = false;
            while (!validParams)
            {
                //prints error if user input is invalid and method is recursivly called
                if (error != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please Try Again.");
                    Console.WriteLine("Error: ");
                    Console.WriteLine(error);
                    Console.WriteLine(Environment.NewLine);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("How many positions do you want to guess? Any positive numerical value.");
                string possStr = Console.ReadLine();
                Console.WriteLine("How many numbers do you want to guess? Between 1 and 9.");
                string numStr = Console.ReadLine();
                try
                {
                    Positions = Int32.Parse(possStr);
                    
                }
                catch (FormatException)
                {
                    SetUpGame("'Positions To Guess' must be a numerical value");
                }
                try
                {
                    Numbers = Int32.Parse(numStr);
                }
                catch (FormatException)
                {
                    SetUpGame("'Numbers To Guess' must be a numerical value");
                }
                if (Numbers < 1 | Numbers > 9)
                {
                    SetUpGame("'Numbers To Guess' must be between 1 and 9");
                }
                if (Positions < 1)
                {
                    SetUpGame("'Possitions To Guess' must be above 0");
                }
                validParams = true; //if it gets this far then params are valid, break out of loop
                Console.WriteLine("Do you want debug mode? Y/N");
                string debugInput = Console.ReadLine();
                if(debugInput.ToLower() == "y")
                {
                    Debug = true;
                }
                else
                {
                    //if user puts anything other than Y then assume false.
                    Debug = false;
                }
            }

            //set up random code for user to guess
            CodeToGuess = GenerateRandomCodeToGuess();
        }

        private int[] GenerateRandomCodeToGuess()
        {
            int[] code = new int[Positions];
            Random rand = new Random();
            int[] CodeToGuess = new int[Positions];
            for(int i = 0; i < Positions; i++)
            {
                code[i] = rand.Next(1,Numbers);
            }
            return code;
        }
    }

    class HistoryStack
    {
        public int Turn { get; set; }

        public void HistorySetUp(int Positions)
        {
            Turn = 1;
        }

        public void PushToStack()
        {

        }

        public void ClearStack()
        {

        }

        public void PrintHistory()
        {
            
        }
    }

    class PlayerGuess
    {
        int[] Guess { get; set; }   //the players last guess


        public void SetUpPlayerGuess(int positions)
        {
            Guess = new int[positions];
        }

        public void SetGuess(int[] guess)
        {
            Guess = guess;
        }

        public bool IsGuessValid(int[] currentCode)
        {
            if(Guess == currentCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }




    class Program
    {
        private static GameParameters gp = new GameParameters();
        private static HistoryStack hs = new HistoryStack();
        private static PlayerGuess pg = new PlayerGuess();



        static void GetPlayerGuess()
        {

        }

        static string ReturnCode(int[] code, int len)
        {
            string s = "";
            for (int i = 0; i < len; i++)
            {
                s = s + code[i];
            }
            return s;
        }
        
        static void PrintHistory()
        {

        }

        static void RunGame(string error = null)
        {
            Console.Clear();

            if(error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error, please try again");
                Console.WriteLine(error);
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine($"You Are On Turn {hs.Turn}");

            //print history
            //ask for guess
            //validate and check guess
            //add to history

        }


        static void Main(string[] args)
        {
            Console.WriteLine("COMP 1003 Mastermind");
            gp.SetUpGame();
            hs.HistorySetUp(gp.Positions);
            pg.SetUpPlayerGuess(gp.Positions);

            if (gp.Debug)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Debug is: {gp.Debug}");
                Console.WriteLine($"Positions are: {gp.Positions}");
                Console.WriteLine($"Numbers are: {gp.Numbers}");
                Console.WriteLine($"Code To Guess is: {ReturnCode(gp.CodeToGuess, gp.Positions)}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("Click To Start Game");
            Console.ReadLine();


            while (!pg.IsGuessValid(gp.CodeToGuess))
            {
                RunGame();

            }
            if (pg.IsGuessValid(gp.CodeToGuess))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You Have Won!!!");
                Console.WriteLine($"The Code Was: {ReturnCode(gp.CodeToGuess, gp.Positions)}");
                PrintHistory();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
