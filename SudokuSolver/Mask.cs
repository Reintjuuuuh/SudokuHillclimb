using System;

public class Mask
{
    public int[,] mask = new int[9, 9]; //Mask where a 1 represents this number can never be swapped.

    public Mask(Sudoku sudoku)
    {
        this.mask = SudokuParser.ParseSudokuMask(sudoku);
    }
}