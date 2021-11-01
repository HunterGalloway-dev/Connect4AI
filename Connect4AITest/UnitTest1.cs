using System;
using Xunit;
using ConnectFourAI;
using System.Collections.Generic;

namespace Connect4AITest
{
    public class UnitTest1
    {
        public enum FillType
        {
            Fill,
            Empty,
            Draw,
            Row,
            Column,
            DiagonalLR,
            DiagonalRL,
            EntireColumn
        }
        public static int[,] FillBoard(int startR, int startC, FillType fillType, int fill)
        {
            int[,] board = new int[Connect4AI.NumRows, Connect4AI.NumCols];
            int rInc = 0;
            int cInc = 0;

            switch(fillType)
            {
                case FillType.Draw:
                        board = new int[,]
                        {
                            { 1,2,1,2,1,2 },
                            { 2,1,2,1,2,1 },
                            { 1,2,1,2,1,2 },
                            { 1,2,1,2,1,2 },
                            { 2,2,2,1,2,2 },
                            { 2,1,2,1,2,1 },
                            { 1,2,1,2,1,2 }
                        };
                    break;
                case FillType.Fill:
                    for(int r = 0; r < Connect4AI.NumRows; r++)
                    {
                        for(int c = 0; c < Connect4AI.NumCols; c++)
                        {
                            board[r, c] = fill;
                        }
                    }
                    return board;
                case FillType.Empty:
                    return board;
                case FillType.EntireColumn:
                    for (int r = 0; r < Connect4AI.NumRows; r++)
                    {
                        board[r, startC] = fill;
                    }
                    return board;
                default:
                    // Move on to other types
                    break;
            }

            for(int i = 0; i < 4; i++)
            {
                switch(fillType)
                {
                    case FillType.Row:
                        cInc = i;
                        break;
                    case FillType.Column:
                        rInc = i;
                        break;
                    case FillType.DiagonalLR:
                        rInc = i;
                        cInc = i;
                        break;
                    case FillType.DiagonalRL:
                        rInc = i;
                        cInc = -i;
                        break;
                    default:
                        break;
                }
                board[startR + rInc, startC + cInc] = fill;
            }

            return board;
        }

        [Fact]
        public void MakeMoveTestEmpty()
        {
            int playerFill = (int)Connect4AI.BoardState.PlayerOne;
            int[,] expectedBoard = FillBoard(0, 0, FillType.Empty, 0);
            expectedBoard[Connect4AI.NumRows - 1, 0] = playerFill;

            int[,] board = FillBoard(0, 0, FillType.Empty, 0);
            Assert.Equal(Connect4AI.MakeMove(board, 0, playerFill), expectedBoard);
        }

        [Fact]
        public void MakeMoveAlmostFullTest()
        {
            int playerFill = (int)Connect4AI.BoardState.PlayerOne;
            int[,] expectedBoard = FillBoard(0, 0, FillType.Empty, 0);
            int[,] board = FillBoard(0, 0, FillType.Empty, 0);
            
            for(int r = 1; r < Connect4AI.NumRows; r++)
            {
                board[r, 0] = playerFill;
                expectedBoard[r, 0] = playerFill;
            }
            expectedBoard[0, 0] = playerFill;
            
            Assert.Equal(expectedBoard,Connect4AI.MakeMove(board, 0, playerFill));
        }

        [Fact]
        public void PossMovesTestEmptyboard()
        {
            List<int> ExpectedPossMoves = new List<int>() { 0, 1, 2, 3, 4, 5 };

            int[,] board = FillBoard(0, 0, FillType.Empty, 0);

            Assert.Equal(ExpectedPossMoves, Connect4AI.PossMoves(board));
        }
        [Fact]
        public void PossMovesWithOpenCol()
        {

            List<int> ExpectedPossMoves = new List<int>() { 0, 1, 2, 4, 5 };

            int[,] board = FillBoard(0, 3, FillType.EntireColumn, (int)Connect4AI.BoardState.PlayerOne);

            Assert.Equal(ExpectedPossMoves, Connect4AI.PossMoves(board));
        }

        [Fact]
        public void PossMovesWithOpenCols()
        {

            List<int> ExpectedPossMoves = new List<int>() { 0, 4, 5 };
            int[,] board = FillBoard(0, 0, FillType.Empty, 0);

            for (int r = 0; r < Connect4AI.NumRows; r++)
            {
                for(int c = 1; c < 4; c++ )
                {
                    board[r, c] = (int)Connect4AI.BoardState.PlayerOne;
                }
            }

            Assert.Equal(ExpectedPossMoves, Connect4AI.PossMoves(board));
        }

        [Fact]
        public void PossMovesFullBoard()
        {
            List<int> ExpectedPossMoves = new List<int>();

            int[,] board = FillBoard(0, 0, FillType.Fill, (int)Connect4AI.BoardState.PlayerOne);

            Assert.Equal(ExpectedPossMoves, Connect4AI.PossMoves(board));
        }

        [Fact]
        public void DrawWinTest()
        {
            int[,] board = FillBoard(0, 0, FillType.Draw, (int)Connect4AI.BoardState.PlayerOne);

            Assert.Equal(0, Connect4AI.WhoWon(board));
        }

        [Fact]
        public void RowWinTest()
        {
            int playerFill = (int)Connect4AI.BoardState.PlayerOne;
            int[,] board;
            bool flag = true;

            for(int r = 0; r < Connect4AI.NumRows; r++)
            {
                for(int c = 0; c < Connect4AI.NumCols - 3; c++)
                {
                    board = FillBoard(r, c, FillType.Row, playerFill);

                    if (Connect4AI.WhoWon(board) != playerFill)
                    {
                        Assert.True(false);
                    }
                }
            }
            Assert.True(flag);
        }

        [Fact]
        public void ColWinTest()
        {
            int playerFill = (int)Connect4AI.BoardState.PlayerOne;
            int[,] board;
            bool flag = true;

            for (int r = 0; r < Connect4AI.NumRows - 3; r++)
            {
                for (int c = 0; c < Connect4AI.NumCols; c++)
                {
                    board = FillBoard(r, c, FillType.Column, playerFill);

                    if (Connect4AI.WhoWon(board) != playerFill)
                    {
                        Assert.True(false);
                    }
                }
            }
            Assert.True(flag);
        }

        [Fact]
        public void DiagonaLRWinTest()
        {
            int playerFill = (int)Connect4AI.BoardState.PlayerOne;
            int[,] board;
            bool flag = true;

            for (int r = 0; r < Connect4AI.NumRows - 3; r++)
            {
                for (int c = 0; c < Connect4AI.NumCols - 3; c++)
                {
                    board = FillBoard(r, c, FillType.DiagonalLR, playerFill);

                    if (Connect4AI.WhoWon(board) != playerFill)
                    {
                        Assert.True(false);
                    }
                }
            }
            Assert.True(flag);
        }

        [Fact]
        public void DiagonRLTest()
        {
            int playerFill = (int)Connect4AI.BoardState.PlayerOne;
            int[,] board;
            bool flag = true;

            for (int r = 0; r < Connect4AI.NumRows - 3; r++)
            {
                for (int c = 3; c < Connect4AI.NumCols -3; c++)
                {
                    board = FillBoard(r, c, FillType.Row, playerFill);

                    if (Connect4AI.WhoWon(board) != playerFill)
                    {
                        Assert.True(false);
                    }
                }
            }
            Assert.True(flag);
        }
    }
}
