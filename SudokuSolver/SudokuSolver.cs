using System;

//Parser leest grid in en maakt een 2d array met 0'en en hardcoded items. een mask bepaald door 0 en 1 welke we mogen verplaatsen
//Het grid wordt willekeurig ingevuld op alle 0 plekken met missende getallen binnen het 3x3 blok.
//search algoritme -> kies 1 van de 9 blokken,
//Maak een lijst  met nieuwe sudoku objecten waarin
//kies de beste door op elk in de lijst de evaluatiefunctie uit te voeren.

//Parsing en invullen tot sudoku object
//Algoritme
//Evaluatiefunctie

class SudokuSolver
{
    static void Main(String[] args)
    {
        Console.WriteLine("Please input the sudoku grid: ");
        
        try
        {
            //Sudoku sudoku = new Sudoku(Console.ReadLine());
            Sudoku sudoku = new Sudoku("0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0");
            Mask globalMask = new Mask(sudoku); //Make a global mask because we will be altering the sudoku state so we need to remember the original sudoku mask.

            sudoku.PrettyPrint(globalMask);
            sudoku.PrettyPrintBlock(globalMask, SudokuConstants.BottomMiddle);

            //foreach (var TopLeftIndices in SudokuConstants.Blocks[SudokuConstants.TopLeft])
            //{
            //    Console.WriteLine(sudoku.grid[TopLeftIndices.row, TopLeftIndices.col]);
            //}
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}