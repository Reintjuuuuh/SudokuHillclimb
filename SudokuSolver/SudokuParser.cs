using System;

public static class SudokuParser
{
    public static int[,] ParseSudokuGrid(string input)
    {
        string[] inputArray = input.Split(" ");

        if (inputArray.Length != 81) { throw new Exception("Input does not have 81 entries. Try adding zeroes for empty spaces."); }

        int[,] result = new int[9, 9];

        for (int i = 0; i < 81; i++)
        {
            int row = i / 9;
            int column = i % 9;

            result[row, column] = int.Parse(inputArray[i]);
        }

        return result;
    }

    public static int[,] ParseSudokuMask(Sudoku sudoku)
    {
        int[,] result = new int[9, 9];

        for (int row = 0; row < 9; row++) //for every row
        {
            for (int col = 0; col < 9; col++) //for every column
            {
                if (sudoku.grid[row, col] == 0) //check if the number is zero.
                {
                    result[row, col] = 0; //if this is the case, we can mask with a zero. This represents the fact we can swap this indice later on in the program.
                }
                else
                {
                    result[row, col] = 1;
                }
            }
        }

        return result;
    }
}
