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

            //keeps asking for parameters until user gives valid parameters
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
            //checks position parameter input
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

            //checks numbers (colors) parameters input
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

            //checks the paramets are in the correct ranges
            if (Numbers < 1 | Numbers > 9)
            {
                return "'Numbers To Guess' must be between 1 and 9";
            }
            if (Positions < 1)
            {
                return "'Possitions To Guess' must be above 0";
            }

            //sets up debug
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
            for(int i = 0; i < Positions; i++)
            {
                code[i] = rand.Next(1,Numbers);
            }
            return code;
        }

        private void GetNumbersInCode(int[] code)
        {
            //creates an array that contains all the numbers in the code within no repets, insets int.MaxValue in output where a number has been repeted
            //eg [1, 6, 4, 6, 2, 1] becomes [1, 6, 4, Int.MaxValue, 2, Int.MaxValue]

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

        //returns true if the number is in the array, false if not
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
        //Model for each turn, stores the code and the GuessValidity (eg 2/4 numbers correct and 1/4 positions correct)
        public class TurnModel
        {
            public int[] Guess { get; set; }
            public string GuessValidity { get; set; }
        }

        public int Turn { get; set; }
        public TurnModel[] Stack { get; set;}

        public void HistorySetUp()
        {
            Turn = 1;
            Stack = new TurnModel[1];
        }

        public void PushToStack(TurnModel turn)
        {
            //make Stack property one index larger, while keeping all contents the same
            TurnModel[] tempStack = Stack;
            Stack = new TurnModel[Turn + 1];
            for (int i = 0; i < Turn; i++)
            {
                Stack[i] = tempStack[i];
            }

            Stack[Turn-1] = turn;
            Turn++;
        }
    }

    class PlayerGuess
    {
        public int[] Guess { get; set; }   //the players last guess
        private int Positions { get; set; } //length of Guess array

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
                    guessStr += Guess[i].ToString();
                    currentCodeStr += currentCode[i].ToString();
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

            //ERROR
            //check how many positions and numbers are correct
            for (int i = 0; i < indexs; i++)
            {
                

                if (correctCode[i] == guessCode[i])
                {
                    correctPositions++;
                }
                
            }


            string returnString = $"{correctNumbers}/{indexs} Are the correct numbers, {correctPositions}/{indexs} Are in the correct position";

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
                s += code[i];
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
            //Do Not Print History On First Turn
            //Because there is no history to print
            if(hs.Turn > 1)
            {
                Console.WriteLine("Here are your previous guesses:");
                for (int i = 0; i < hs.Turn -1; i++)
                {
                    string guess = "";
                    for (int j = 0; j <= gp.Positions-1; j++) { guess += hs.Stack[i].Guess[j]; }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Guess:  {guess} Validity: {hs.Stack[i].GuessValidity}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
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

        static void RunGame()
        {
            Console.Clear();

            //Excluded From Structogram
            if (gp.Debug) { PrintDebug(); }

            Console.WriteLine($"You Are On Turn {hs.Turn}");

            //print history
            PrintHistory();

            //ask for guess
            pg.Guess = GetPlayersGuess();
            while(pg.Guess == null)
            {
                string example = "";
                for(int i = 1; i <= gp.Positions; i++) { example += i.ToString(); }    //generates example of required input that is the correct length (excluded from structogram)
                Console.WriteLine($"That is not a valid guess. Must be all numbers, with no spaces. Such as '{example}'. Must be {gp.Positions} long.");
                pg.Guess = GetPlayersGuess();
            }

            //show guess validity
            string guessValidity = pg.HowCorrectIsTheGuess(pg.Guess, gp.CodeToGuess, gp.NumbersInCodeNoRepetes, gp.Positions);
            Console.WriteLine(guessValidity);

            //add to history
            HistoryStack.TurnModel turn = new HistoryStack.TurnModel
            {
                Guess = pg.Guess,
                GuessValidity = guessValidity
            };
            hs.PushToStack(turn);
        }

        static void SetUpGame()
        {
            Console.Clear();
            Console.WriteLine("COMP 1003 Mastermind");
            Console.WriteLine("Please Set Up Your Game");
            gp.SetUpGame();
            hs.HistorySetUp();
            pg.SetUpPlayerGuess(gp.Positions);

        }

        static void Main(string[] args)
        {
            while (true)
            {
                
                SetUpGame();

                //excluded from structogram
                if (gp.Debug) { PrintDebug(); }
                

                Console.WriteLine("Click To Start Game");
                Console.ReadLine();


                while (!pg.IsGuessValid(gp.CodeToGuess))
                {
                    RunGame();
                }
                if (pg.IsGuessValid(gp.CodeToGuess))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("You Have Won!!!");
                    Console.WriteLine($"The Code Was: {ReturnCode(gp.CodeToGuess, gp.Positions)}");
                    PrintHistory();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press 'g' To Play Again");
                    Console.WriteLine("Press any other key to quit");
                    
                    char s = Console.ReadKey().KeyChar;
                    if(s == 'g')
                    {
                        SetUpGame();
                    }
                    else
                    {
                        Console.WriteLine("You Have Quit The Game");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Unknown Error Has Occured");
                    Console.ReadLine();
                }
            }
            
        }
    }
}