using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConnectFourAI
{
    class Program
    {
        static void Main(string[] args)
        {

            //BenchMarkAI();
            PlayGame();
        }

        public static void BenchMarkAI()
        {
            int[,] board;
            board = new int[,]
                        {
                            { 0,0,0,0,0,0,0 },
                            { 0,0,0,0,0,0,0 },
                            { 0,0,0,0,2,0,0 },
                            { 0,0,1,2,1,0,0 },
                            { 0,0,2,1,1,0,0 },
                            { 2,2,1,1,1,2,0 }
                        };
            


            Stopwatch watch = new Stopwatch();
            long time = 0;
            double iterations = 1.0;
            for (int i = 0; i < iterations; i++)
            {
                watch.Reset();
                watch.Start();
                Console.WriteLine(Connect4AI.BestMove(board, (int)Connect4AI.BoardState.PlayerOne, (int)Connect4AI.BoardState.PlayerTwo));
                watch.Stop();
                time += (watch.ElapsedMilliseconds);
            }
            Console.WriteLine(time / iterations / 1000.0 + " Seconds");
        }

        public static void PlayGame()
        {
            int[,] board = new int[Connect4AI.NumRows, Connect4AI.NumCols];
            int curPlayer = (int)Connect4AI.BoardState.PlayerOne;
            int computer = (int)Connect4AI.BoardState.PlayerTwo;
            int move;
            int whoWon;
            while((whoWon = Connect4AI.WhoWon(board)) == -1)
            {
                Connect4AI.DisplayBoard(board);
                Console.Write($"Player {curPlayer} Make Your Move: ");

                if(curPlayer == computer)
                {
                    Console.WriteLine("\nCalculating Optimal Move...");
                    move = Connect4AI.BestMove(board, Connect4AI.SwitchPlayer(curPlayer), computer);         
                } else
                {
                    move = Int32.Parse(Console.ReadLine());
                }
                board = Connect4AI.MakeMove(board, move, curPlayer);
                curPlayer = Connect4AI.SwitchPlayer(curPlayer);
            }

            Console.WriteLine(whoWon);
        }
    }
}
