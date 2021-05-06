using System;

namespace COMP1003_Mastermind_Console_Game
{
    class GameParameters
    {
        public int Positions { get; set; }  //position to guess
        public int Numbers { get; set; }  //numbers (aka colors) to guess  
        public int[] CodeToGuess { get; set; }      //The code the user has to guess
        public bool Debug { get; set; }             //does user want the debug info
        public int[] NumbersInCodeNoRepetes { get; set; }       //Array holds every number in CodeToGuess, with no repetes, IntMaxValue for empty index that should be ignored


        private string numStr;
        private string possStr;
        private string debugInput;

        public void SetUpGame()
        {
            bool validParams = false;
            while (!validParams)
            {
                SetParameters();    //asks user for game parameters
                string error = ValidateParameters();

                if(error == null)
                {
                    validParams = true;
                }
                else
                {
                    PrintError(error);
                }
            }

            //set up random code for user to guess
            CodeToGuess = GenerateRandomCodeToGuess();

            //Set up NumberInCodeNoRepetes
            GetNumbersInCode(CodeToGuess);
        }

        private void SetParameters()
        {
            Console.WriteLine("How many positions do you want to guess? Any positive numerical value.");
            possStr = Console.ReadLine();
            Console.WriteLine("How many numbers do you want to guess? Between 1 and 9.");
            numStr = Console.ReadLine();
            Console.WriteLine("Do you want debug mode? Y/N");
            debugInput = Console.ReadLine();
        }

        private string ValidateParameters()
        {
            try
            {
                Positions = Int32.Parse(possStr);
            }
            catch (FormatException)
            {
                return "'Positions To Guess' must be a numerical value";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            try
            {
                Numbers = Int32.Parse(numStr);
            }
            catch (FormatException)
            {
                return "'Numbers To Guess' must be a numerical value";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            if (Numbers < 1 | Numbers > 9)
            {
                return "'Numbers To Guess' must be between 1 and 9";
            }
            if (Positions < 1)
            {
                return "'Possitions To Guess' must be above 0";
            }


            if (debugInput.ToLower() == "y")
            {
                Debug = true;
            }
            else
            {
                //if user puts anything other than Y then assume false.
                Debug = false;
            }


            return null; //if null then all input is valid
        }

        private void PrintError(string errorToPrint)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please Try Again.");
            Console.WriteLine($"Error: {errorToPrint}");
            Console.WriteLine(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
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

        private void GetNumbersInCode(int[] code)
        {
            NumbersInCodeNoRepetes = new int[Positions];

            for(int i =0; i < Positions; i++)
            {
                if(!IsNumberInArray(NumbersInCodeNoRepetes, Positions, code[i]))
                {
                    //number not in NumbersInCodeNoRepetes array
                    //Add to array
                    NumbersInCodeNoRepetes[i] = code[i];
                }
                else
                {
                    NumbersInCodeNoRepetes[i] = int.MaxValue;
                }
            }
        }

        private bool IsNumberInArray(int[] arr, int arrLen, int num)
        {
            for(int i = 0; i < arrLen; i++)
            {
                if(arr[i] == num)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class HistoryStack
    {
        public class TurnModel
        {
            public int[] Guess { get; set; }
            public string GuessValidity { get; set; }
        }

        public int Turn { get; set; }
        public TurnModel[] Stack { get; set;}

        public void HistorySetUp(int Positions)
        {
            Turn = 1;
            Stack = new TurnModel[1];
        }

        public void PushToStack(TurnModel turn)
        {
            Stack[Turn] = turn;
            IncrementStackByOne();
            Turn++;
        }

        public void ClearStack()
        {

        }

        private void IncrementStackByOne()
        {
            //ERROR OutOfRange Exception
            //make Stack property one index larger, while keeping all contents the same
            TurnModel[] tempStack = Stack;
            Stack = new TurnModel[Turn + 1];
            for (int i = 0; i < Turn; i++)
            {
                Stack[i] = tempStack[i];
            }
        }
    }

    class PlayerGuess
    {
        public int[] Guess { get; set; }   //the players last guess
        private int Positions { get; set; }

        public void SetUpPlayerGuess(int positions)
        {
            Guess = new int[positions];
            Positions = positions;
        }

        public bool IsGuessValid(int[] currentCode)
        {
            string guessStr = "";
            string currentCodeStr = "";

            for (int i = 0; i < Positions; i++ )
            {
                try
                {
                    guessStr = guessStr + Guess[i].ToString();
                    currentCodeStr = currentCodeStr + currentCode[i].ToString();
                }
                catch
                {
                    return false;
                }
            }

            if (guessStr == currentCodeStr)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string HowCorrectIsTheGuess(int[] guessCode, int[] correctCode, int[] numbersInCodeNoRepetes, int indexs)
        {
            int correctNumbers = 0;
            int correctPositions = 0;
            //Returns:
            //3/6 Are the correct numbers, 1/6 Are in the correct possition

            //check how many numbers are in the code correctly
            for(int i = 0; i < indexs; i++)
            {
                for (int j = 0; j < indexs; j++)
                {
                    
                    if(numbersInCodeNoRepetes[i] == guessCode[j])
                    {
                        correctNumbers++;
                    }
                }
            }

            //check how many positions and numbers are correct
            for (int i = 0; i < indexs; i++)
            {
                for (int j = 0; j < indexs; j++)
                {

                    if (numbersInCodeNoRepetes[i] == correctCode[j])
                    {
                        correctPositions++;
                    }
                }
            }


            string returnString = $"{correctNumbers}/{indexs} Are the correct numbers, {correctPositions}/{indexs} Are in the correct possition";

            return returnString;
        }
    }




    class Program
    {
        private static GameParameters gp = new GameParameters();
        private static HistoryStack hs = new HistoryStack();
        private static PlayerGuess pg = new PlayerGuess();


        static string ReturnCode(int[] code, int len)
        {
            string s = "";
            for (int i = 0; i < len; i++)
            {
                s = s + code[i];
            }
            return s;
        }
        
        //returns length of string inputted
        static int StringLength(string strToCheck)
        {
            int i = 0;
            while (true)
            {
                try
                {
                    _ = strToCheck[i];
                }
                catch (IndexOutOfRangeException)
                {
                    return i;
                }
                i++;
            }
        }

        static int[] GetPlayersGuess()
        {
            //validate guess
            Console.WriteLine("Please Now Make A Guess...");
            string guess = Console.ReadLine();
            int[] guessArr = new int[gp.Positions];
            if(StringLength(guess) != gp.Positions)
            {
                return null;
            }
            else
            {
                for(int i = 0; i < gp.Positions; i++)
                {
                    try
                    {
                        guessArr[i] = Int32.Parse(guess[i].ToString());
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return guessArr;
        }

        static void PrintHistory()
        {
            //TODO History Print
            Console.WriteLine("Here are your previous guesses:");
        }

        static void PrintDebug()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Debug is: {gp.Debug}");
            Console.WriteLine($"Positions are: {gp.Positions}");
            Console.WriteLine($"Numbers are: {gp.Numbers}");
            Console.WriteLine($"Code To Guess is: {ReturnCode(gp.CodeToGuess, gp.Positions)}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void RunGame(string error = null)
        {
            Console.Clear();
            if (gp.Debug)
            {
                PrintDebug();
            }
            if (error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error, please try again");
                Console.WriteLine(error);
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine($"You Are On Turn {hs.Turn}");

            //print history
            PrintHistory();

            //ask for guess
            pg.Guess = GetPlayersGuess();
            while(pg.Guess == null)
            {
                string example = "";
                for(int i = 1; i <= gp.Positions; i++) { example = example + i.ToString(); }    //generates example of required input that is the correct length
                Console.WriteLine($"That is not a valid guess. Must be all numbers, with no spaces. Such as '{example}'. Must be the same length as {gp.Positions}");
                pg.Guess = GetPlayersGuess();
            }

            //show guess validity
            string guessValidity = pg.HowCorrectIsTheGuess(pg.Guess, gp.CodeToGuess, gp.NumbersInCodeNoRepetes, gp.Positions);
            Console.WriteLine(guessValidity);

            //add to history
            HistoryStack.TurnModel turn = new HistoryStack.TurnModel();
            turn.Guess = pg.Guess;
            turn.GuessValidity = guessValidity;    //TODO add how valid is guess
            hs.PushToStack(turn);
        }


        static void Main(string[] args)
        {
            Console.WriteLine("COMP 1003 Mastermind");
            gp.SetUpGame();
            hs.HistorySetUp(gp.Positions);
            pg.SetUpPlayerGuess(gp.Positions);

            if (gp.Debug)
            {
                PrintDebug();
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
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Unknown Error Has Occured");
                Console.ReadLine();
            }
        }
    }
}
