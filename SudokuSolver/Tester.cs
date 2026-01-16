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
        private string[] input;
        private int inputAmount;
        private TimeSpan[] timeStampsCBT;
        private TimeSpan[] timeStampsFC;
        private TimeSpan[] timeStampsFCMCV;

        public void runTest()
        {
            readTestCases();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < inputAmount; i++)
            {
                long FCStart = Stopwatch.GetTimestamp();
                var solved = ForwardChecking.Solve(input[i]);
                timeStampsFC[i] = Stopwatch.GetElapsedTime(FCStart);
            }

            printTestRes();
        }

        private void readTestCases()
        {
            input = readAllFiles();
            inputAmount = this.input.Length;
        }

        public string[] readAllFiles()
        {
            string[] inputs = { "grid1.txt", "grid2.txt", "grid3.txt", "grid4.txt", "grid5.txt" };
            for(int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), inputs[i]);
            }
            return inputs;
        }

        public void printTestRes()
        {
            //
        }
    }
}
