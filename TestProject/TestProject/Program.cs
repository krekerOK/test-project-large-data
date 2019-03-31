using System;
using TestProject.Services;
using TestProject.Utils;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintMenu(false);
            RunProgramMenu();
        }

        private static void RunProgramMenu()
        {
            var pressedKey = Console.ReadLine();
            while (pressedKey != "q")
            {
                switch (pressedKey)
                {
                    case "1": PerformanceTools.MeasureElapsedTime(FirstTask); break;
                    case "2":
                        {
                            Console.WriteLine("Butch count:");
                            var batchCount = int.Parse(Console.ReadLine());

                            Console.WriteLine("Rows in file:");
                            var rowsInFile = int.Parse(Console.ReadLine());

                            PerformanceTools.MeasureElapsedTime(() => SecondTask(batchCount, rowsInFile));

                            break;
                        };
                    case "3": PerformanceTools.MeasureElapsedTime(ThirdTask); break;
                    case "4":
                        {
                            Console.WriteLine("Skip:");
                            var skip = int.Parse(Console.ReadLine());

                            Console.WriteLine("Take:");
                            var take = int.Parse(Console.ReadLine());
                            PerformanceTools.MeasureElapsedTime(() => FourthTask(skip, take));

                            break;
                        }
                    default: Console.WriteLine("Unknown commad, try one more time."); break;
                }
                PrintMenu(true);
                pressedKey = Console.ReadLine();
            }
        }

        private static void FirstTask()
        {
            var referenceDataService = new ReferenceDataService();
            referenceDataService.Generate();
        }

        private static void SecondTask(int batchCount, int rowsInFile)
        {
            var transactionDataService = new TransactionDataService();
            transactionDataService.Generate(batchCount, rowsInFile);
        }

        private static void ThirdTask()
        {
            var transactionDataService = new TransactionDataService();
            transactionDataService.Import();
        }

        private static void FourthTask(int skip, int take)
        {
            var transactionDataService = new TransactionDataService();
            var transactionalDataItems = transactionDataService.FetchTransactionalData(skip, take);

            Console.WriteLine();
            Console.WriteLine("Fetched transactional data items:");
            foreach (var item in transactionalDataItems)
            {
                Console.WriteLine($"{item.Id}\t{item.ReferenceDataId}\t{item.DataValue}");
            }
            Console.WriteLine();
        }

        public static void PrintMenu(bool indent)
        {
            if (indent)
            {
                Console.WriteLine();
            }

            Console.WriteLine("Choose operation:");
            Console.WriteLine("Generate reference data, press '1'");
            Console.WriteLine("Generate transaction data, press '2'");
            Console.WriteLine("Import transaction data, press '3'");
            Console.WriteLine("Get transactional data, press '4'");
            Console.WriteLine("Quit, press 'q'");
        }
    }
}
