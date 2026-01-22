using Sudoku_Namespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;


class SudokuSolver
{
    public static void Main(String[] args)
    {
        List<String> fileNames = new List<string>();
        Console.WriteLine("Do you want to run all tests, or select tests? (a/s)");
        string input = Console.ReadLine().ToLower();
        if (input == "a")
        {
            foreach (string file in Directory.EnumerateFiles("..\\..\\..\\input\\", "*.txt"))
                fileNames.Add(file);
        }
        else if (input == "s")
        {
            Console.WriteLine("Give one-by-one the txt-file names placed 'input' to be solved. Add an empty line when done.");

            while (true)
            {
                var x = Console.ReadLine();
                if (x.Length == 0) { break; }
                fileNames.Add("..\\..\\..\\input\\" + x);
            }   
        }

        // Initially solve and print solution.
        foreach (var fileName in fileNames)
        {
            string contents = File.ReadAllText(fileName);
            var solved = ForwardChecking.Solve(contents);
            if (solved != null)
            {
                Console.WriteLine();
                Console.WriteLine($"{Path.GetFileName(fileName)}: "); // get truncated file name
                solved.PrettyPrint(solved.mask);
            }
            else
                Console.WriteLine("Can't be solved.");
        }

        // Run and print test results.
        Tester tester = new Tester();
        tester.runTest(fileNames);

        Console.WriteLine("Go again? (y/n)");
        if (Console.ReadLine() == "y")
            Main(new string[0] { });
    }

    public List<(Sudoku sudoku, (int row, int col)? swap1, (int row, int col)? swap2)>? Solve(string input, int walkSize = 7, int walkTrigger = 300, bool optimizedWalk = true, bool interactive = false, Stopwatch? stopwatch
 = null, int timeout = 0)
    {
        if (stopwatch != null) { stopwatch.Start(); }
        Sudoku sudoku = new Sudoku(input, true);

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
            if (stopwatch != null && stopwatch.ElapsedMilliseconds > timeout) { 
                return null; 
            }
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
        if (interactive)
        {
            Console.WriteLine($"\nTotal moves: {solveSteps.Count}");
            (var firstBoard, _, _) = solveSteps[0];
            (var solvedBoard, _, _) = solveSteps[^1];
            var globalMask = firstBoard.mask;
            Console.WriteLine("\n\nFinal state: ");
            solvedBoard.PrettyPrint(globalMask);

            Console.WriteLine($"\nPrint solution steps? (y/n)");
            if (Console.ReadLine() == "y") PrettyPrintSolution(solveSteps);

            Console.WriteLine($"\nSolve another sudoku? (y/n)");
            if (Console.ReadLine() == "y") Main(new string[0] { });
        }
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
}