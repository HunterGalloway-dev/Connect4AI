using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourAI
{
    interface IConectFourAI
    {
        /// <summary>
        /// Returns an integer which describes the win condtiion
        /// </summary>
        /// <param name="board">Board representing the connect 4 game board</param>
        /// <returns>-1 if no win condition, 0 for draw, 1 for player 1, 2 for player 2</returns>
        public int WhoWon(int[,] board);
        /// <summary>
        /// Makes the move for depicted player by "dropping" the players piece in the selected move spot
        /// </summary>
        /// <param name="board">Board representing the connect 4 board</param>
        /// <param name="move">Selected column that depicts the move</param>
        /// <param name="player">Player which to make the move for</param>
        /// <returns>A copy of the orginal board with the move made</returns>
        public int[,] MakeMove(int[,] board, int move, int player);
        /// <summary>
        /// Swaps the player to the opposite player, player 1 will be swaped to 2, player 2 will be swapped 2
        /// </summary>
        /// <param name="player">Current player</param>
        /// <returns>Opposite of current player</returns>
        int SwitchPlayer(int player);
        /// <summary>
        /// generates all possible moves based on the current board, 0 will be column 1, while dropping in columb 6 will be 5
        /// </summary>
        /// <param name="board">Board representing the connect 4 board</param>
        /// <returns>Array of moves</returns>
        List<int> PossMoves(int[,] board);
        /// <summary>
        /// Determines if the selected column in hte board is full
        /// </summary>
        /// <param name="board"></param>
        /// <param name="col"></param>
        /// <returns>Whether that column is full</returns>
        bool ColumnFull(int[,] board, int col);
    }
}
