using System;

public class Sudoku
{
    public int[,] grid = new int[9, 9]; //2d array in the form of row column. So index [3,8] is the fourth row and nineth column.

    public Sudoku(string input)
    {
        this.grid = SudokuParser.ParseSudokuGrid(input);
    }
    public Sudoku(int[,] grid) //needed for creating new sudoku objects after swapping two indices.
    {
        this.grid = grid;
    }

    public void FillRandom()
    {

    }

    public void PrettyPrint(Mask mask)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;

        Console.WriteLine("\n +-------+-------+-------+ ");
        for (int row = 0; row < 9; row++)
        {
            Console.Write(" | ");
            for (int col = 0; col < 9; col++)
            {
                if (mask.mask[row, col] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                
                Console.Write($"{grid[row, col].ToString()} ");

                Console.ForegroundColor = ConsoleColor.Black;

                if ((col + 1) % 3 == 0) Console.Write("| "); 
            }
            Console.WriteLine();

            if ((row + 1) % 3 == 0) Console.WriteLine(" +-------+-------+-------+ ");
        }

        Console.ResetColor();
    }

    public void PrettyPrintBlock(Mask mask, int blockNumber)
    {
        var indices = SudokuConstants.Blocks[blockNumber];

        Console.WriteLine($"\nPrinting {SudokuConstants.BlockNames[blockNumber]}.");

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;

        Console.WriteLine("+-------+");
        for (int i = 0; i < indices.Length; i++)
        {
            if ((i + 3) % 3 == 0) Console.Write("| ");

            if (mask.mask[indices[i].row, indices[i].col] == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

            Console.Write($"{grid[indices[i].row, indices[i].col].ToString()} ");

            Console.ForegroundColor = ConsoleColor.Black;

            if ((i + 1) % 3 == 0) Console.WriteLine("|");
        }
        Console.WriteLine("+-------+");

        Console.ResetColor();
    }
}

public static class SudokuConstants
{
    //Explanation for how this works: it is an array which stores the indices of all 9 3x3 blocks in the sudoku. So for example, SudokuConstants.Blocks[SudokuConstants.TopLeft] would return [(0,0), (0,1), (0,2),  (1,0), (1,1), (1,2),  (2,0), (2,1), (2,2)]
    public static readonly (int row, int col)[][] Blocks = new (int row, int col)[][] //This cannot be a const because an array is an object so it cant be frozen at compile-time. Thats why static readonly
    {
        //TopLeft 3x3
        new (int, int)[] { (0,0), (0,1), (0,2),
                           (1,0), (1,1), (1,2),
                           (2,0), (2,1), (2,2) },

        //TopMiddle 3x3
        new (int, int)[] { (0,3), (0,4), (0,5),
                           (1,3), (1,4), (1,5),
                           (2,3), (2,4), (2,5) },

        //TopRight 3x3
        new (int, int)[] { (0,6), (0,7), (0,8),
                           (1,6), (1,7), (1,8),
                           (2,6), (2,7), (2,8) },

        //MiddleLeft 3x3
        new (int, int)[] { (3,0), (3,1), (3,2),
                           (4,0), (4,1), (4,2),
                           (5,0), (5,1), (5,2) },

        //Center 3x3
        new (int, int)[] { (3,3), (3,4), (3,5),
                           (4,3), (4,4), (4,5),
                           (5,3), (5,4), (5,5) },

        //MiddleRight 3x3
        new (int, int)[] { (3,6), (3,7), (3,8),
                           (4,6), (4,7), (4,8),
                           (5,6), (5,7), (5,8) },

        //BottomLeft 3x3
        new (int, int)[] { (6,0), (6,1), (6,2),
                           (7,0), (7,1), (7,2),
                           (8,0), (8,1), (8,2) },

        //BottomMiddle 3x3
        new (int, int)[] { (6,3), (6,4), (6,5),
                           (7,3), (7,4), (7,5),
                           (8,3), (8,4), (8,5) },

        //BottomRight 3x3
        new (int, int)[] { (6,6), (6,7), (6,8),
                           (7,6), (7,7), (7,8),
                           (8,6), (8,7), (8,8) }
    };

    public const int TopLeft = 0; //for better readability. This is so you can say SudokuConstants.Blocks[TopLeft] instead of SudokuConstants.Blocks.1
    public const int TopMiddle = 1;
    public const int TopRight = 2;
    public const int MiddleLeft = 3;
    public const int Center = 4;
    public const int MiddleRight = 5;
    public const int BottomLeft = 6;
    public const int BottomMiddle = 7;
    public const int BottomRight = 8;

    public static readonly string[] BlockNames = //so it can also go the other way. So you say SudokuConstants.BlockNames[0] and it returns TopLeft
    {
        "TopLeft", "TopMiddle", "TopRight",
        "MiddleLeft", "Center", "MiddleRight",
        "BottomLeft", "BottomMiddle", "BottomRight"
    };
}