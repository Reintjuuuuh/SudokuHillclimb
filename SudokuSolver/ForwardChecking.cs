using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku_Namespace
{
    internal class ForwardChecking
    {
        private struct Change
        {
            public int Row;
            public int Col;
            public int Val;
        }

        public static Sudoku? Solve(string input, bool MCV = false)
        {
            Sudoku sudoku = new Sudoku(input);

            // init
            List<int>[,] domains = new List<int>[9, 9];
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (sudoku.grid[r, c] == 0)
                    {
                        domains[r, c] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    }
                    else
                    {
                        // fixed, size of domain is 1
                        domains[r, c] = new List<int> { sudoku.grid[r, c] };
                    }
                }
            }

            // node consistency
            if (!MakeNodeConsistent(sudoku, domains)) { return null; } // null -> invalid puzzle

            // goofy recursive stuff
            if (SolveSudoku(sudoku, domains, 0, 0, MCV)) { return sudoku; }

            return null; 
        }

        private static bool MakeNodeConsistent(Sudoku sudoku, List<int>[,] domains)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    // Set cell, remove the arcs from peers
                    if (sudoku.grid[r, c] != 0)
                    {
                        int val = sudoku.grid[r, c];
                        // No need to track changes
                        if (!RemoveValueFromPeers(sudoku, domains, r, c, val, null))
                        {
                            return false; // conflict (empty domain)
                        }
                    }
                }
            }
            return true;
        }

        private static bool SolveSudoku(Sudoku sudoku, List<int>[,] domains, int row, int col, bool MCV)
        {
            if (row == 9) { return true; } // Final row, final column

            //get next row and column to solve
            (int nextRow, int nextCol) = GetNextCell(sudoku, domains, row, col, MCV);

            if (sudoku.grid[row, col] != 0)  // skip taken cells
            {
                return SolveSudoku(sudoku, domains, nextRow, nextCol, MCV);
            }


            // current domain to loop over without getting modified
            List<int> currentDomain = new List<int>(domains[row, col]);

            foreach (int val in currentDomain)
            {
                // assign value, create local history
                sudoku.grid[row, col] = val;
                List<Change> history = new List<Change>();

                if (RemoveValueFromPeers(sudoku, domains, row, col, val, history))
                {
                    if (SolveSudoku(sudoku, domains, nextRow, nextCol, MCV))
                    {
                        return true;
                    }
                }

                // backtrack
                sudoku.grid[row, col] = 0;
                
                foreach (var change in history) // restore changes to neighbours
                {
                    domains[change.Row, change.Col].Add(change.Val);
                    domains[change.Row, change.Col].Sort();
                }
            }
            return false;
        }

        private static (int, int) GetNextCell(Sudoku sudoku, List<int>[,] domains, int row, int col, bool MCV)
        {
            int nextRow = -1;
            int nextCol = -1;
            int smallestSize = int.MaxValue;
            if (!MCV) //if most constrained variable is not turned on, we return the next row/column
            {
                // "next" for the forward checking part
                nextRow = row;
                nextCol = col + 1;
                if (nextCol == 9)
                {
                    nextRow = row + 1;
                    nextCol = 0;
                }
            }
            else
            {
                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        if (sudoku.grid[r, c] == 0) //only care about unfilled cells
                        {
                            int size = domains[r, c].Count;

                            if (size < smallestSize)
                            {
                                smallestSize = size;
                                nextRow = r; nextCol = c;

                                if (smallestSize <= 1) goto Found; //optimization; impossible to beat
                            }
                        }
                    }
                }
            }

            if (smallestSize == int.MaxValue) nextRow = 9; //if no possible domain is found then we return nextrow 9, indicating the algorithm can stop

            Found:
                return (nextRow, nextCol);
        }

        private static bool RemoveValueFromPeers(Sudoku sudoku, List<int>[,] domains, int row, int col, int val, List<Change> history)
        {
            for (int x = 0; x < 9; x++)
            {
                if (x != col && sudoku.grid[row, x] == 0) // Only check unassigned cells
                {
                    if (domains[row, x].Contains(val))
                    {
                        domains[row, x].Remove(val);
                        history?.Add(new Change { Row = row, Col = x, Val = val });
                        if (domains[row, x].Count == 0) { return false; } // domain empty
                    }
                }

                if (x != row && sudoku.grid[x, col] == 0) // Only check unassigned cells
                {
                    if (domains[x, col].Contains(val))
                    {
                        domains[x, col].Remove(val);
                        history?.Add(new Change { Row = x, Col = col, Val = val });
                        if (domains[x, col].Count == 0) { return false; } // domain empty
                    }
                }
            }


            // block check
            int blockIndex = (row / 3) * 3 + (col / 3);
            foreach (var elem in SudokuConstants.Blocks[blockIndex])
            {
                if ((sudoku.grid[elem.row, elem.col] == 0) && (elem.row != row || elem.col != col)) { // skip the row and column we checked above
                    if (domains[elem.row, elem.col].Contains(val))
                    {
                        domains[elem.row, elem.col].Remove(val);
                        history?.Add(new Change { Row = elem.row, Col = elem.col, Val = val });
                        if (domains[elem.row, elem.col].Count == 0) { return false; } // domain empty
                    }
                }
            }

            return true;
        }
    }
}