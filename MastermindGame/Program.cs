using System;

namespace COMP1003_Mastermind_Console_Game
{
    class GameParameters
    {
        public int n { get; set; }  //position to guess
        public int m { get; set; }  //numbers (aka colors) to guess  

    }

    class HistoryStack
    {


        public void PushToStack()
        {

        }

        public void ClearStack()
        {

        }

        public GamePlays ReturnHistory()
        {
            throw new NotImplementedException();
        }
    }

    class GamePlay
    {

    }

    class GamePlays
    {

    }

    class Program
    {
        GameParameters gp = new GameParameters();
        HistoryStack hs = new HistoryStack();

        static void SetGameParameters()
        {
            bool validParams = false;
            while (!validParams)
            {
                Console.WriteLine("How many positions do you want to guess?");
                string possStr = Console.ReadLine();
                Console.WriteLine("How many numbers do you want to guess?");
                string colorsStr = Console.ReadLine();

            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("COMP 1003 Mastermind");

        }
    }
}
