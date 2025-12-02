using System;
using System.Collections.Generic;

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
        //string input = "0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0";
        //string input = "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0";
        //string input = Console.ReadLine();
        string input = "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0";
        int walkSize = 3;

        try
        {
            Sudoku sudoku = new Sudoku(input);
            Mask globalMask = sudoku.mask; //Make a global mask because we will be altering the sudoku state so we need to remember the original sudoku mask.

            int totalMoves = 0;
            int bestScoreSeen = sudoku.score;
            var solveSteps = new List<(Sudoku sudoku, (int row, int col) swap1, (int row, int col) swap2)>();

            Console.WriteLine("Starting state: ");
            sudoku.PrettyPrint(globalMask);
            int movesSinceLastImprovement = 0;

            while (sudoku.score != 0)
            {
                (sudoku, var swapped1, var swapped2) = Algorithm.Iteration(sudoku, globalMask);
                //sudoku.PrettyPrint(globalMask, swapped1, swapped2);
                totalMoves++;
                movesSinceLastImprovement++;

                if (sudoku.score < bestScoreSeen)
                {
                    movesSinceLastImprovement = 0;
                    bestScoreSeen = sudoku.score;
                    solveSteps.Add((sudoku, swapped1, swapped2));
                }
                if (movesSinceLastImprovement > 20)
                {
                    movesSinceLastImprovement = 0;
                    var boardList = Algorithm.RandomWalk(sudoku, globalMask, walkSize);
                    (sudoku, _, _) = boardList[^1];
                    foreach (var board in boardList)
                    {
                        solveSteps.Add(board);
                    }
                }
            }
                
            foreach (var step in solveSteps)
            {
                step.sudoku.PrettyPrint(globalMask, step.swap1, step.swap2);
            }

            Console.WriteLine("\n\nFinal state: ");
            sudoku.PrettyPrint(globalMask);
            Console.WriteLine($"\nTotal moves: {totalMoves}");
                
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
}