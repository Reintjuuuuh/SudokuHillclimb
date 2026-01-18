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
        private TimeSpan[] timeStampILS;
        private TimeSpan[] timeStampsCBT;
        private TimeSpan[] timeStampsFC;
        private TimeSpan[] timeStampsFCMCV;

        public void runTest()
        {
            inputFiles = new string[] { "grid1.txt", "grid2.txt", "grid3.txt", "grid4.txt", "grid5.txt" };
            inputAmount = inputFiles.Length;

            readTestCases();

            timeStampILS = new TimeSpan[inputAmount];
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
                timeStampILS[i] = Stopwatch.GetElapsedTime(ILSStart);
                
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
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "Head",
                inputFiles[0], inputFiles[1], inputFiles[2], inputFiles[3], inputFiles[4]);
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "ILS",
                timeStampILS[0].Microseconds + "ms", timeStampILS[1].Microseconds + "ms", 
                timeStampILS[2].Microseconds + "ms", timeStampILS[3].Microseconds + "ms", 
                timeStampILS[4].Microseconds + "ms");
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "CBT",
                timeStampsCBT[0].Microseconds + "ms", timeStampsCBT[1].Microseconds + "ms", 
                timeStampsCBT[2].Microseconds + "ms", timeStampsCBT[3].Microseconds + "ms", 
                timeStampsCBT[4].Microseconds + "ms");
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "FC",
                timeStampsFC[0].Microseconds + "ms", timeStampsFC[1].Microseconds + "ms", 
                timeStampsFC[2].Microseconds + "ms", timeStampsFC[3].Microseconds + "ms", 
                timeStampsFC[4].Microseconds + "ms");
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "FCMCV",
                timeStampsFCMCV[0].Microseconds + "ms", timeStampsFCMCV[1].Microseconds + "ms", 
                timeStampsFCMCV[2].Microseconds + "ms", timeStampsFCMCV[3].Microseconds + "ms", 
                timeStampsFCMCV[4].Microseconds + "ms");
        }
    }
}
