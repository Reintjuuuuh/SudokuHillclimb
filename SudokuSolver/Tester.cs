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
        private string[] inputFiles;
        private string[] input;
        private int inputAmount;
        private TimeSpan[] timeStampsILS;
        private TimeSpan[] timeStampsCBT;
        private TimeSpan[] timeStampsFC;
        private TimeSpan[] timeStampsFCMCV;

        public void runTest(string[] fileNames)
        {
            // fileNames are the .txt that are read from the file "input"
            inputFiles = fileNames;
            inputAmount = inputFiles.Length;

            input = readGivenFiles();

            timeStampsILS = new TimeSpan[inputAmount];
            timeStampsCBT = new TimeSpan[inputAmount];
            timeStampsFC = new TimeSpan[inputAmount];
            timeStampsFCMCV = new TimeSpan[inputAmount];

            // solve each sudoku, with each algorithm
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < inputAmount; i++)
            {
                // Test: Iterated Local Search
                //SudokuSolver ILS = new SudokuSolver();
                long ILSStart = Stopwatch.GetTimestamp();
                //ILS.Solve(input[i]);
                timeStampsILS[i] = Stopwatch.GetElapsedTime(ILSStart);

                // Test: Chronological Backtracking
                long CBTStart = Stopwatch.GetTimestamp();
                var CBTSolved = Backtracking.Solve(input[i]);
                timeStampsCBT[i] = Stopwatch.GetElapsedTime(CBTStart);

                // Test: Forward Checking
                long FCStart = Stopwatch.GetTimestamp();
                var FCSolved = ForwardChecking.Solve(input[i]);
                timeStampsFC[i] = Stopwatch.GetElapsedTime(FCStart);

                // Test: Forward Checking most-constrained-variable (MCV) heuristiek
                long FCMCVStart = Stopwatch.GetTimestamp();
                var FCMCVSolved = ForwardChecking.Solve(input[i], true); ;
                timeStampsFCMCV[i] = Stopwatch.GetElapsedTime(FCMCVStart);
            }

            printTestRes();
        }

        private string[] readGivenFiles()
        {
            string basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "input\\");
            string[] res = new string[inputAmount];
            for (int i = 0; i < inputAmount; i++)
            {
                res[i] = File.ReadAllText(basePath + inputFiles[i]);
            }
            return res;
        }

        private void printTestRes()
        {
            string printFormat = "{0,10} ";
            for (int i = 0; i < inputAmount; i++)
            {
                printFormat += $"{{{i+1},15}} ";
            }

            Console.WriteLine(printFormat, combineHead(inputFiles));
            Console.WriteLine(printFormat, combineRes("ILS", timeStampsILS));
            Console.WriteLine(printFormat, combineRes("CBT", timeStampsCBT));
            Console.WriteLine(printFormat, combineRes("FC", timeStampsFC));
            Console.WriteLine(printFormat, combineRes("FCMCV", timeStampsFCMCV));
        }

        private string[] combineHead(string[] heads)
        {
            string[] res = new string[inputAmount + 1];
            res[0] = "Algorithm";
            for (int i = 0;i < inputAmount; i++)
            {
                res[i + 1] = heads[i];
            }
            return res;
        }

        private string[] combineRes(string name, TimeSpan[] timeStamps)
        {
            string[] res = new string[inputAmount + 1];
            res[0] = name;
            for (int i = 0; i < inputAmount; i++)
            {
                res[i + 1] = timeStamps[i].Microseconds + "μs";
            }
            return res;
        }
    }
}
