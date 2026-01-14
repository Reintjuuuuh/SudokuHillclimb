using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Namespace
{
    internal class Backtracking
    {
        static bool validPlacement(Sudoku sudoku, int row, int col, int n)
        {
            for (int x = 0; x < 9; x++)
            {
                if (sudoku.grid[row, x] == n) { return false; }
            }
            for (int x = 0; x < 9; x++)
            {
                if (sudoku.grid[x, col] == n) { return false; }
            }

            // block check
            int blockIndex = (row / 3) * 3 + (col / 3);
            foreach (var elem in SudokuConstants.Blocks[blockIndex])
            {
                if (sudoku.grid[elem.row, elem.col] == n) { return false; }
            }

            return true;
        }

        static bool solveSudoku(Sudoku sudoku, int row, int col)
        {
            if (row == 8 && col == 9) { return true; } // Final row, final column

            if (col == 9) // Next layer
            {
                col = 0;
                row++;
            }

            if (sudoku.grid[row, col] != 0) // skip taken cells
            {
                return solveSudoku(sudoku, row, col + 1);
            }

            for (int n = 1; n <= 9; n++)
            {
                if (validPlacement(sudoku, row, col, n))
                {
                    sudoku.grid[row, col] = n;
                    if (solveSudoku(sudoku, row, col + 1)) {
                        return true; 
                    }
                    // recursively delete
                    sudoku.grid[row, col] = 0;
                }
            }
            return false;
        }

        public static Sudoku? Solve(string input)
        {
            Sudoku sudoku = new Sudoku(input);
            solveSudoku(sudoku, 0, 0);
            if (sudoku.CalculateFullScore() == 0) { return sudoku; }
            return null;
        }
    }
}
