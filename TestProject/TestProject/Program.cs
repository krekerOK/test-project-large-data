using System;
using System.Diagnostics;
using TestProject.Utils;

namespace TestProject
{
    class Program
    {
        public static Stopwatch sw;
        static void Main(string[] args)
        {
            PrintMenu();
            var pressedKey = Console.ReadLine();
            while (pressedKey != "q")
            {
                switch (pressedKey)
                {
                    case "1": PerformanceTools.MeasureElapsedTime(FirstTask); break;
                    case "2":
                        {
                            //Console.WriteLine("Butch count:");
                            //var batchCount = int.Parse(Console.ReadLine());

                            //Console.WriteLine("Rows in file:");
                            //var rowsInFile = int.Parse(Console.ReadLine());

                            //sw = Stopwatch.StartNew();
                            //PerformanceTools.MeasureElapsedTime(() => SecondTask(batchCount, rowsInFile));

                            sw = Stopwatch.StartNew();
                            PerformanceTools.MeasureElapsedTime(() => SecondTask(100, 100000));

                            break;
                        };
                    case "3": PerformanceTools.MeasureElapsedTime(ThirdTask); break;
                    case "4": PerformanceTools.MeasureElapsedTime(FourthTask); break;
                    default: Console.WriteLine("Unknown commad, try one more time."); break;
                }
                PrintMenu();
                pressedKey = Console.ReadLine();
            }
        }

        private static void FirstTask()
        {
            var referenceDataGenerator = new ReferenceDataGenerator();
            referenceDataGenerator.GenerateReferenceData();
        }

        private static void SecondTask(int batchCount, int rowsInFile)
        {
            var transactionDataGenerator = new TransactionDataGenerator(
                batchCount, rowsInFile, @"C:\Data\TransactionData", SecondTaskHasBeenCompleted);
            transactionDataGenerator.GenerateTransactionData();
        }

        private static void SecondTaskHasBeenCompleted()
        {
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        private static void ThirdTask()
        {

        }

        private static void FourthTask()
        {

        }

        public static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Choose operation:");
            Console.WriteLine("Generate reference data, press '1'");
            Console.WriteLine("Generate transaction data, press '2'");
            Console.WriteLine("Quit, press 'q'");
        }
    }
}
