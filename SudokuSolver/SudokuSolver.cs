using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
    static List<(Sudoku sudoku, (int row, int col) swap1, (int row, int col) swap2)> Solve(string input, int walkSize = 3, int walkTrigger = 4500, bool optimizedWalk = false)
        // For our optimized walk, the walkTrigger needs to be sufficiently high to not keep resetting the optimizedWalkSize.
    {
        Sudoku sudoku = new Sudoku(input);

        int totalMoves = 0;
        int bestScoreSeen = sudoku.score;
        var solveSteps = new List<(Sudoku sudoku, (int row, int col) swap1, (int row, int col) swap2)>();

        int movesSinceLastImprovement = 0;
        int plateauLevel = 0;
        int optimizedWalkSize = walkSize;

        while (sudoku.score != 0)
        {
            (Sudoku tempsudoku, var swapped1, var swapped2) = Algorithm.Iteration(sudoku);
            totalMoves++;

            if (tempsudoku.score <= bestScoreSeen)
            {
                sudoku = tempsudoku;
                solveSteps.Add((sudoku, swapped1, swapped2));
                if (sudoku.score < bestScoreSeen)
                {
                    movesSinceLastImprovement = 0;
                    bestScoreSeen = sudoku.score;
                }
            }
            else if (movesSinceLastImprovement > walkTrigger)
            {
                List<(Sudoku, (int row, int col), (int row, int col))> boardList;
                if (optimizedWalk)
                {
                    if (sudoku.score != plateauLevel)
                    {
                        plateauLevel = sudoku.score;
                        optimizedWalkSize = walkSize;
                    }
                    else
                    {
                        optimizedWalkSize++;
                    }
                    // debug; Console.WriteLine($"{sudoku.score}, {optimizedWalkSize}, {solveSteps.Count}");
                    boardList = Algorithm.RandomWalk(sudoku, optimizedWalkSize);
                }
                else
                {
                    boardList = Algorithm.RandomWalk(sudoku, walkSize);
                }
                


                (sudoku, _, _) = boardList[^1];
                foreach (var board in boardList)
                {
                    solveSteps.Add(board);
                }
                movesSinceLastImprovement = 0;

            }
            else
            {
                movesSinceLastImprovement++;
            }
        }

        return solveSteps;
    }
    public static void PrettyPrintSolution(List<(Sudoku sudoku, (int row, int col) swap1, (int row, int col) swap2)> solveSteps)
    {
        Console.WriteLine("Starting state:");
        (var firstBoard, _, _) = solveSteps[0];
        (var solvedBoard, _, _) = solveSteps[^1];
        var globalMask = firstBoard.mask;
        Console.WriteLine(globalMask);
        foreach (var step in solveSteps)
        {
            step.sudoku.PrettyPrint(globalMask, step.swap1, step.swap2);
        }

        Console.WriteLine("\n\nFinal state: ");
        solvedBoard.PrettyPrint(globalMask);
        Console.WriteLine($"\nTotal moves: {solveSteps.Count}");
    }

    static void Main(String[] args)
    {
        //string input = "0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0";
        //string input = "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0";
        //string input = Console.ReadLine();
        
        Console.WriteLine("What is the path to the sudoku?");
        string input = File.ReadAllText(Console.ReadLine());
        try
        {
            List <int> solveStepsCounter = new List<int>();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 10; i++)
            {                
                solveStepsCounter.Add(Solve(input).Count);
                
            }
            Console.WriteLine($"Average amount of moves: {solveStepsCounter.AsParallel().Sum() / solveStepsCounter.Count}. Average amount of time to solve (ms): {stopwatch.ElapsedMilliseconds / solveStepsCounter.Count}");




        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

}