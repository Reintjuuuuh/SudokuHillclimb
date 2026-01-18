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
        private TimeSpan[] timeStampsCBT;
        private TimeSpan[] timeStampsFC;
        private TimeSpan[] timeStampsFCMCV;

        public void runTest()
        {
            inputFiles = new string[] { "grid1.txt", "grid2.txt", "grid3.txt", "grid4.txt", "grid5.txt" };
            inputAmount = inputFiles.Length;

            readTestCases();

            timeStampsCBT = new TimeSpan[inputAmount];
            timeStampsFC = new TimeSpan[inputAmount];
            timeStampsFCMCV = new TimeSpan[inputAmount];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < inputAmount; i++)
            {
                long CBTStart = Stopwatch.GetTimestamp();
                var CBTsolved = Backtracking.Solve(input[i]);
                timeStampsCBT[i] = Stopwatch.GetElapsedTime(CBTStart);

                long FCStart = Stopwatch.GetTimestamp();
                var FCsolved = ForwardChecking.Solve(input[i]);
                timeStampsFC[i] = Stopwatch.GetElapsedTime(FCStart);

                long FCMCVStart = Stopwatch.GetTimestamp();
                //var FCMCVsolved = ;
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
            for(int i = 0; i < inputAmount; i++)
            {
                inputs[i] = File.ReadAllText(basePath + inputFiles[i]);
            }
            return inputs;
        }

        private void printTestRes()
        {
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "Head", 
                inputFiles[0], inputFiles[1], inputFiles[2], inputFiles[3], inputFiles[4]);
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "CBT", 
                timeStampsCBT[0].Microseconds, timeStampsCBT[1].Microseconds, timeStampsCBT[2].Microseconds, 
                timeStampsCBT[3].Microseconds, timeStampsCBT[4].Microseconds);
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "FC",
                timeStampsFC[0].Microseconds, timeStampsFC[1].Microseconds, timeStampsFC[2].Microseconds,
                timeStampsFC[3].Microseconds, timeStampsFC[4].Microseconds);
            Console.WriteLine("{0,5} {1,10} {2,10} {3,10} {4,10} {5,10}", "FCMCV",
                timeStampsFCMCV[0].Microseconds, timeStampsFCMCV[1].Microseconds, timeStampsFCMCV[2].Microseconds,
                timeStampsFCMCV[3].Microseconds, timeStampsFCMCV[4].Microseconds);

            //Console.WriteLine("Head     " + combineHead(inputFiles));
            //Console.WriteLine("CBT      " + combineRes(timeStampsCBT));
            //Console.WriteLine("FC       " + combineRes(timeStampsFC));
            //Console.WriteLine("FCMCV    " + combineRes(timeStampsFCMCV));
        }

        private string combineHead(string[] inputHead)
        {
            string res = "";
            foreach (string head in inputHead)
            {
                res += head + "     ";
            }
            return res;
        }


        private string combineRes(TimeSpan[] timeStamps)
        {
            string res = "";
            foreach (TimeSpan timeStamp in timeStamps)
            {
                res += timeStamp.Microseconds.ToString() + "     ";
            }
            return res;
        }
    }
}
