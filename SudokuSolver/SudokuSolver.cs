using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


class SudokuSolver
{
    static List<(Sudoku sudoku, (int row, int col)? swap1, (int row, int col)? swap2)> Solve(string input, int walkSize = 1, int walkTrigger = 400, bool optimizedWalk = true)
        // For our optimized walk, the walkTrigger needs to be sufficiently high to not keep resetting the optimizedWalkSize.
    {
        Sudoku sudoku = new Sudoku(input);

        int totalMoves = 0;
        int bestScoreSeen = sudoku.score;
        var solveSteps = new List<(Sudoku sudoku, (int row, int col)? swap1, (int row, int col)? swap2)>();
        solveSteps.Add((sudoku, null, null));

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
                bestScoreSeen = sudoku.score;
            }
            else
            {
                movesSinceLastImprovement++;
            }
        }

        return solveSteps;
    }
    public static void PrettyPrintSolution(List<(Sudoku sudoku, (int row, int col)? swap1, (int row, int col)? swap2)> solveSteps)
    {
        Console.WriteLine("Starting state:");
        (var firstBoard, _, _) = solveSteps[0];
        (var solvedBoard, _, _) = solveSteps[^1];
        var globalMask = firstBoard.mask;

        //Console.WriteLine(globalMask);
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
        
        //Console.WriteLine("What is the path to the sudoku?");
        //string input = File.ReadAllText(Console.ReadLine());
        //string input = "0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0";
        string input = "0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0";
        try
        {
            PrettyPrintSolution(Solve(input));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}