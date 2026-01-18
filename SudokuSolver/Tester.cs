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

        public void runTest()
        {
            inputFiles = new string[] { "grid1.txt", "grid2.txt", "grid3.txt", "grid4.txt", "grid5.txt" };
            inputAmount = inputFiles.Length;

            readTestCases();

            timeStampsILS = new TimeSpan[inputAmount];
            timeStampsCBT = new TimeSpan[inputAmount];
            timeStampsFC = new TimeSpan[inputAmount];
            timeStampsFCMCV = new TimeSpan[inputAmount];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < inputAmount; i++)
            {
                //SudokuSolver ILS = new SudokuSolver();
                long ILSStart = Stopwatch.GetTimestamp();
                //ILS.Solve(input[i]);
                timeStampsILS[i] = Stopwatch.GetElapsedTime(ILSStart);
                
                long CBTStart = Stopwatch.GetTimestamp();
                var CBTSolved = Backtracking.Solve(input[i]);
                timeStampsCBT[i] = Stopwatch.GetElapsedTime(CBTStart);

                long FCStart = Stopwatch.GetTimestamp();
                var FCSolved = ForwardChecking.Solve(input[i]);
                timeStampsFC[i] = Stopwatch.GetElapsedTime(FCStart);

                long FCMCVStart = Stopwatch.GetTimestamp();
                //var FCMCVSolved = ;
                timeStampsFCMCV[i] = Stopwatch.GetElapsedTime(FCMCVStart);
            }

            printTestRes();
        }

        private void readTestCases()
        {
            input = readAllFiles();
        }

        private string[] readAllFiles()
        {
            string basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "input\\");
            string[] inputs = new string[inputAmount];
            for (int i = 0; i < inputAmount; i++)
            {
                inputs[i] = File.ReadAllText(basePath + inputFiles[i]);
            }
            return inputs;
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
                res[i + 1] = timeStamps[i].Microseconds + "ms";
            }
            return res;
        }
    }
}
