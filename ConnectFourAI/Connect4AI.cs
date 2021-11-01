using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourAI
{
    public class Connect4AI
    {
        public static int NumRows = 6;
        public static int NumCols = 7;

        public static int Depth { get; private set; } = 9;

        public enum BoardState
        {
            Empty = 0,
            PlayerOne = 1,
            PlayerTwo = 2
        }

        public static int BestMove(int[,] board, int curPlayer, int myPlayer)
        {
            List<int> possMoves = PossMoves(board);
            if(possMoves.Count == 0)
            {
                return -1;
            }

            int bestMove = possMoves[0];
            int maxScore = MinMax(board, bestMove, curPlayer, myPlayer, Depth),tmpScore;

            Parallel.ForEach(possMoves, move =>
            {
                if ((tmpScore = MinMax(board, move, curPlayer, myPlayer, Depth)) > maxScore)
                {
                    bestMove = move;
                    maxScore = tmpScore;
                }
            });

            return bestMove;
        }

        public static int MinMaxV2(int[,] board, int move, int curPlayer, int mainPlayer)
        {
            board = MakeMove(board, move, curPlayer);
            int[,] tempBoard;
            Queue<int[,]> queue = new Queue<int[,]>();
            queue.Enqueue(board);
            int sum = 0;
            int i = 0;
            int maxIterations = 10;

            while(queue.Count > 0 && i < maxIterations)
            {
                tempBoard = queue.Dequeue();
                int whoWon = WhoWon(tempBoard);
                if (whoWon != -1)
                {
                    if (whoWon == 0)
                    {
                        sum += 0;
                    }
                    else if (whoWon == mainPlayer)
                    {
                        sum += 10;
                    }
                    else if (whoWon == SwitchPlayer(mainPlayer))
                    {
                        sum -= 100;
                    }
                } else
                {
                    List<int> possMoves = PossMoves(tempBoard);
                    foreach(int possMove in possMoves)
                    {
                        queue.Enqueue(MakeMove(tempBoard, possMove, SwitchPlayer(curPlayer)));
                    }
                }
                i++;
            }

            return sum;
        }

        public static int MinMax(int[,] board, int move, int curPlayer, int mainPlayer,int depth)
        {
            if(depth == 0)
            {
                return 0;
            }
            board = MakeMove(board, move, curPlayer);
            //DisplayBoard(board);
            int whoWon = WhoWon(board);

            if(whoWon != -1)
            {
                if(whoWon == 0)
                {
                    return 0;
                } else if(whoWon == mainPlayer)
                {
                    return 100;
                } else if(whoWon == SwitchPlayer(mainPlayer))
                {
                    return -10;
                }
            }

            int sum = -1;
            List<int> possMoves = PossMoves(board);

            foreach (int possMove in possMoves)
            {
                sum += MinMax(board, possMove, SwitchPlayer(curPlayer), mainPlayer, depth - 1);
            }

            return sum;
        }

        public static bool ColumnFull(int[,] board, int col)
        {
            for(int r = 0; r < NumRows; r++)
            {
                if (board[r, col] == (int)BoardState.Empty)
                {
                    return false;
                }
            }

            return true;
        }

        public static int[,] MakeMove(int[,] board, int move, int player)
        {
            int[,] boardCopy = new int[NumRows, NumCols];
            Array.Copy(board, boardCopy, NumRows * NumCols);

            if(board[0,move] != ((int)BoardState.Empty))
            {
                throw new Exception();
            }

            int startR = 0;
            int prevR = startR;

            while(startR < NumRows && boardCopy[startR,move] == (int)BoardState.Empty)
            {
                prevR = startR;
                startR++;
            }

            boardCopy[prevR, move] = player;
            return boardCopy;
        }

        public static List<int> PossMoves(int[,] board)
        {
            List<int> possMoves = new List<int>();
            for(int c = 0; c < NumCols; c++)
            {
                if(!ColumnFull(board,c))
                {
                    possMoves.Add(c);
                }
            }

            return possMoves;
        }

        public static int SwitchPlayer(int player)
        {
            return (BoardState)player == BoardState.PlayerOne ? (int)BoardState.PlayerTwo : (int)BoardState.PlayerOne;
        }
        /// <summary>
        /// Returns an integer which describes the win condtiion
        /// </summary>
        /// <param name="board">Board representing the connect 4 game board</param>
        /// <returns>-1 if no win condition, 0 for draw, 1 for player 1, 2 for player 2</returns>
        public static int WhoWon(int[,] board)
        {
            int cnt = 0, r, c, i;
            bool full = true;

            for(r = 0; r < NumRows; r++)
            {
                for(c = 0; c < NumCols; c++)
                {
                    if (board[r, c] == (int)BoardState.Empty)
                    {
                        full = false;
                    }
                    cnt = 0;
                    if (c < NumCols - 3 && board[r, c] != (int)BoardState.Empty)
                    {
                        for (i = 1; i < 4; i++)
                        {
                            if (board[r, c] == board[r, c + i])
                            {
                                cnt++;
                            }
                        }

                        if (cnt == 3)
                        {
                            return board[r, c];
                        }
                        else
                        {
                            cnt = 0;
                        }
                    }

                    cnt = 0;
                    if (r < NumRows - 3 && board[r, c] != (int)BoardState.Empty)
                    {
                        for (i = 1; i < 4; i++)
                        {
                            if (board[r, c] == board[r + i, c])
                            {
                                cnt++;
                            }
                        }

                        if (cnt == 3)
                        {
                            return board[r, c];
                        }
                        else
                        {
                            cnt = 0;
                        }
                    }

                    cnt = 0;
                    if ((r < NumRows - 3 && c < NumCols - 3) && board[r, c] != (int)BoardState.Empty)
                    {
                        for (i = 1; i < 4; i++)
                        {
                            if (board[r, c] == board[r + i, c + i])
                            {
                                cnt++;
                            }
                        }

                        if (cnt == 3)
                        {
                            return board[r, c];
                        }
                        else
                        {
                            cnt = 0;
                        }
                    }

                    cnt = 0;
                    if (c >= 3 && r < NumRows - 3 && board[r, c] != (int)BoardState.Empty)
                    {
                        for (i = 1; i < 4; i++)
                        {
                            if (board[r, c] == board[r + i, c - i])
                            {
                                cnt++;
                            }
                        }

                        if (cnt == 3)
                        {
                            return board[r, c];
                        }
                        else
                        {
                            cnt = 0;
                        }
                    }
                }
            }

            if(full)
            {
                return 0;
            }

            return -1;
        }

        public static void DisplayBoard(int[,] board)
        {
            Console.Clear();
            for(int r = 0; r < Connect4AI.NumRows; r++)
            {
                for(int c = 0; c < Connect4AI.NumCols; c++)
                {
                    Console.Write(board[r, c]);
                }
                Console.WriteLine();
            }
        }

        /*
         * public static int WhoWon(int[,] board)
        {
            int cnt = 0, r, c, i;

            for(r = 0; r < NumRows; r++)
            {
                for(c = 0; c < NumCols; c++)
                {

                }
            }

            // Horizontal win conditions
            for(r = 0; r < NumRows; r++)
            {
                for(c = 0; c < NumCols-3; c++)
                {
                    cnt = 0;
                    if(c < NumCols - 3 && board[r,c] != (int)BoardState.Empty)
                    {
                        for (i = 1; i < 4; i++)
                        {
                            if (board[r, c] == board[r, c + i])
                            {
                                cnt++;
                            }
                        }

                        if(cnt == 3)
                        {
                            return board[r, c];
                        } else
                        {
                            cnt = 0;
                        }
                    }
                }
            }

            cnt = 0;
            // Column win conditions
            for (r = 0; r < NumRows-3; r++)
            {
                for (c = 0; c < NumCols; c++)
                {
                    cnt = 0;
                    if (r < NumRows-3 && board[r, c] != (int)BoardState.Empty)
                    {
                        for (i = 1; i < 4; i++)
                        {
                            if (board[r, c] == board[r+i, c ])
                            {
                                cnt++;
                            }
                        }

                        if (cnt == 3)
                        {
                            return board[r, c];
                        } else
                        {
                            cnt = 0;
                        }
                    }
                }
            }

            cnt = 0;
            // Top lef to bottom right diagonal win condition
            for (r = 0; r < NumRows -3; r++)
            {
                for (c = 0; c < NumCols - 3; c++)
                {
                    cnt = 0;
                    if ((r < NumRows - 3 && c < NumCols - 3) && board[r, c] != (int)BoardState.Empty)
                    {
                        for (i = 1; i < 4; i++)
                        {
                            if (board[r, c] == board[r + i, c + i])
                            {
                                cnt++;
                            }
                        }

                        if (cnt == 3)
                        {
                            return board[r, c];
                        } else
                        {
                            cnt = 0;
                        }
                    }
                }
            }

            cnt = 0;
            // Top right to bottom left win condition
            for (r = 0; r < NumRows - 3; r++)
            {
                for (c = 3; c < NumCols; c++)
                {
                    cnt = 0;
                    if (r < NumRows -3 && board[r, c] != (int)BoardState.Empty)
                    {
                        for (i = 1; i < 4; i++)
                        {
                            if (board[r, c] == board[r, c - i])
                            {
                                cnt++;
                            }
                        }

                        if (cnt == 3)
                        {
                            return board[r, c];
                        }
                        else
                        {
                            cnt = 0;
                        }
                    }
                }
            }

            cnt = 0;
            //Check Draw
            c = 0;
            while (c < Connect4AI.NumCols)
            {
                if (Connect4AI.ColumnFull(board, c))
                {
                    cnt++;
                }
                c++;
            }

            if (cnt == Connect4AI.NumRows - 1)
            {
                return 0;
            }

            return -1;
        }
         */
    }
}
