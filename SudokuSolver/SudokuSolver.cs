using Sudoku_Namespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


class SudokuSolver
{
    static List<(Sudoku sudoku, (int row, int col)? swap1, (int row, int col)? swap2)> Solve(string input, int walkSize = 7, int walkTrigger = 300, bool optimizedWalk = true)
    {
        Sudoku sudoku = new Sudoku(input);

        int totalMoves = 0;
        int bestScoreSeen = sudoku.score;
        var solveSteps = new List<(Sudoku sudoku, (int row, int col)? swap1, (int row, int col)? swap2)>();
        solveSteps.Add((sudoku, null, null));

        int movesSinceLastImprovement = 0;
        // optimizedWalk only
        int plateauLevel = 0;  // The level at which a local maximum is. if the code stays at the same plateauLevel for multiple randomwalks, the amount of steps (optimizedWalkSize) will increase.
        int optimizedWalkSize = walkSize;

        while (sudoku.score != 0)
        {
            (Sudoku tempsudoku, var swapped1, var swapped2) = Algorithm.Iteration(sudoku);
            totalMoves++;
            // Only add moves that improve or equal the current heuristic.
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
                if (optimizedWalk) // Dynamic walk size. If the algorithm stays on the same plateu, the amount of steps in the randomwalk will increase.
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
                    boardList = Algorithm.RandomWalk(sudoku, optimizedWalkSize);
                }
                else
                {
                    boardList = Algorithm.RandomWalk(sudoku, walkSize);
                }
                // In a randomwalk, all steps are added, no matter if the result is better or not.
                (sudoku, _, _) = boardList[^1];
                foreach (var board in boardList)
                {
                    solveSteps.Add(board);
                }
                movesSinceLastImprovement = 0;
                bestScoreSeen = sudoku.score;
            }
            else
            {
                movesSinceLastImprovement++;
            }
        }
        Console.WriteLine($"\nTotal moves: {solveSteps.Count}");
        (var firstBoard, _, _) = solveSteps[0];
        (var solvedBoard, _, _) = solveSteps[^1];
        var globalMask = firstBoard.mask;
        Console.WriteLine("\n\nFinal state: ");
        solvedBoard.PrettyPrint(globalMask);

        Console.WriteLine($"\nPrint solution steps? (y/n)");
        if (Console.ReadLine() == "y") PrettyPrintSolution(solveSteps);

        Console.WriteLine($"\nSolve another sudoku? (y/n)");
        if (Console.ReadLine() == "y") Main(new string[0] {});

        return solveSteps;
    }
    public static void PrettyPrintSolution(List<(Sudoku sudoku, (int row, int col)? swap1, (int row, int col)? swap2)> solveSteps)
    {
        // Pretty print for bonus points :)
        Console.WriteLine("\nStarting state:");
        (var firstBoard, _, _) = solveSteps[0];
        (var solvedBoard, _, _) = solveSteps[^1];
        var globalMask = firstBoard.mask;

        foreach (var step in solveSteps)
        {
            step.sudoku.PrettyPrint(globalMask, step.swap1, step.swap2);
        }

        Console.WriteLine("\n\nFinal state: ");
        solvedBoard.PrettyPrint(globalMask);
        Console.WriteLine($"\nTotal moves: {solveSteps.Count}");
    }

       
    public static void Main(String[] args)
    {
        string input = "0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0";
        Console.WriteLine("What is the path to the sudoku?");
        //string input = File.ReadAllText(Console.ReadLine());
        try
        {
            var solved = ForwardChecking.Solve(input);
            if (solved != null)
            { solved.PrettyPrint(solved.mask); }
               
                
            else
                Console.WriteLine("joever");
            //Solve(input, true);
            //PrettyPrintSolution(Solve(input));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Runt tests? (y/n)");
        if (Console.ReadLine() == "y")
        {
            Tester tester = new Tester();
            tester.runTest();
        }
    }
}