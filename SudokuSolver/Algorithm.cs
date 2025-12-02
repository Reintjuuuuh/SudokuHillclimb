using System;
using System.Collections.Generic;
using System.Linq;

public static class Algorithm
{
    private static readonly Random rng = new Random();
    //Get random block, perform one iteration which returns a list of all possible swap states, and pick the best one

    // More efficient for randomWalk, maybe remove later
    public static (Sudoku sudoku, (int row, int col) swap1, (int row, int col) swap2) getPossibleBoard(Sudoku sudoku)
    {
        List<(int row, int col)> swapableIndices;

        do
        {
            var indices = SudokuConstants.Blocks[rng.Next(0, 9)];
            swapableIndices = indices.Where(index => sudoku.mask.mask[index.row, index.col] == 0).ToList();
        }
        while (swapableIndices.Count < 2);
        var index1 = swapableIndices[rng.Next(swapableIndices.Count)];
        var index2 = swapableIndices[rng.Next(swapableIndices.Count)];

        Sudoku newState = Swap(sudoku, index1, index2);
        return (newState, index1, index2);

    }

    public static List<(Sudoku sudoku, (int row, int col) swap1, (int row, int col) swap2)> getPossibleBoards(Sudoku sudoku)
    {
        //Get random block. Filter out indices within the mask
        //Make a list for new sudokus
        //Perform all possible swaps within the block.
        List<(int row, int col)> swapableIndices;

        do
        {
            var indices = SudokuConstants.Blocks[rng.Next(0, 9)];
            swapableIndices = indices.Where(index => sudoku.mask.mask[index.row, index.col] == 0).ToList();
        }
        while (swapableIndices.Count < 2);

        var nextSudokus = new List<(Sudoku sudoku, (int row, int col) swap1, (int row, int col) swap2)>();
        //nextSudokus.Add((sudoku, (0, 0), (0, 0)));

        for (int i = 0; i < swapableIndices.Count; i++)
        {
            for (int j = i + 1; j < swapableIndices.Count; j++)
            {
                var index1 = swapableIndices[i];
                var index2 = swapableIndices[j];

                Sudoku newState = Swap(sudoku, index1, index2);
                nextSudokus.Add((newState, index1, index2));
            }
        }
        return nextSudokus;
    }
    
    public static List<(Sudoku, (int row, int col), (int row, int col))> RandomWalk(Sudoku sudoku, int length)
    {
        
        var path = new List<(Sudoku, (int row, int col), (int row, int col))>();
        List<(Sudoku, (int row, int col), (int row, int col))> nextSudokus;
        for (int i = 0; i < length; i++) {
            path.Add(getPossibleBoard(sudoku));
        }
        return path;
    }

    public static (Sudoku, (int row, int col), (int row, int col)) Iteration(Sudoku sudoku)
    {
        var nextSudokus = getPossibleBoards(sudoku);
        
        var minScore = nextSudokus.Min(s => s.sudoku.score);
        var lowestScored = nextSudokus.Where(s => s.sudoku.score == minScore).ToList();

        return lowestScored[rng.Next(lowestScored.Count)];

    }

    public static Sudoku Swap(Sudoku sudoku, (int row, int col) field1, (int row, int col) field2)
    {
        int[,] resultGrid = (int[,])sudoku.grid.Clone();
        int resultScore = sudoku.score - GetPenalty(sudoku.grid, field1, field2);

        int tempVal                         = resultGrid[field1.row, field1.col];
        resultGrid[field1.row, field1.col]  = resultGrid[field2.row, field2.col];
        resultGrid[field2.row, field2.col]  = tempVal;

        resultScore += GetPenalty(resultGrid, field1, field2);

        return new Sudoku(sudoku.mask, resultGrid, resultScore);
    }

    public static int GetPenalty(int[,] grid, (int row, int col) field1, (int row, int col) field2)
    {
        int penalty = 0;

        penalty += CalculateRowPenalty(grid, field1.row);
        if (field1.row != field2.row) penalty += CalculateRowPenalty(grid, field2.row);

        penalty += CalculateColumnPenalty(grid, field1.col);
        if (field1.col != field2.col) penalty += CalculateColumnPenalty(grid, field2.col);

        return penalty;
    }
    public static int CalculateRowPenalty(int[,] grid, int row)
    {
        HashSet<int> numbers = new HashSet<int>();

        for (int col = 0; col < 9; col++)
        {
            numbers.Add(grid[row, col]);
        }

        return (9 - numbers.Count);
    }
    public static int CalculateColumnPenalty(int[,] grid, int col)
    {
        HashSet<int> numbers = new HashSet<int>();

        for (int row = 0; row < 9; row++)
        {
            numbers.Add(grid[row, col]);
        }

        return (9 - numbers.Count);
    }
}
