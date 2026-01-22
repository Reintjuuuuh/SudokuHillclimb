using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Namespace
{
    internal class Tester
    {
        private List<TimeSpan> timeStampsILS = new();
        private List<TimeSpan> timeStampsCBT = new();
        private List<TimeSpan> timeStampsFC = new();
        private List<TimeSpan> timeStampsFCMCV = new();

        public void runTest(List<String> fileNames)
        {

            // solve each sudoku, with each algorithm
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (string fileName in fileNames)
            {
                var content = File.ReadAllText(fileName);
                //Console.WriteLine(input[i]);
                // Test: Iterated Local Search
                //SudokuSolver ILS = new SudokuSolver();
                long ILSStart = Stopwatch.GetTimestamp();
                //ILS.Solve(input[i]);
                timeStampsILS.Add(Stopwatch.GetElapsedTime(ILSStart));

                // Test: Chronological Backtracking
                long CBTStart = Stopwatch.GetTimestamp();
                var CBTSolved = Backtracking.Solve(content);
                timeStampsCBT.Add(Stopwatch.GetElapsedTime(CBTStart));

                // Test: Forward Checking
                long FCStart = Stopwatch.GetTimestamp();
                var FCSolved = ForwardChecking.Solve(content);
                timeStampsFC.Add(Stopwatch.GetElapsedTime(FCStart));

                // Test: Forward Checking most-constrained-variable (MCV) heuristics
                long FCMCVStart = Stopwatch.GetTimestamp();
                var FCMCVSolved = ForwardChecking.Solve(content, true);
                timeStampsFCMCV.Add(Stopwatch.GetElapsedTime(FCMCVStart));
            }

            printTestRes(fileNames);
        }
        private void printTestRes(List<string> inputFiles)
        {
            int w = Console.WindowWidth;
            int filesPerBlock = (w - 10) / 15;
            for (int i = 0; i < inputFiles.Count; i += filesPerBlock)
            {
                int count = Math.Min(filesPerBlock, inputFiles.Count - i);
                var fileBlock = inputFiles.Skip(i).Take(count).ToList();
                Console.WriteLine(); // extra newline

                // Header
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("{0,-10}", "");
                foreach (var file in fileBlock)
                {
                    Console.Write("{0,15}", Path.GetFileName(file)); // remove the ../../../
                }
                Console.WriteLine();
                Console.ResetColor();

                // Rows
                PrintRowBlock("CBT", timeStampsCBT, i, count, ConsoleColor.Red);
                PrintRowBlock("FC", timeStampsFC, i, count, ConsoleColor.Yellow);
                PrintRowBlock("FCMCV", timeStampsFCMCV, i, count, ConsoleColor.Green);
            }
        }

        private void PrintRowBlock(string name, List<TimeSpan> times, int startIndex, int count, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write("{0,-10}", name);

            for (int i = startIndex; i < startIndex + count; i++)
            {
                Console.Write("{0,15}", $"{times[i].TotalMicroseconds:F0} µs");
            }

            Console.WriteLine();
            Console.ResetColor();
        }

    }
}
