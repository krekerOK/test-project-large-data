using System;
using TestProject.Utils;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintMenu();
            var pressedKey = Console.ReadLine();
            while (pressedKey != "q")
            {
                switch (pressedKey)
                {
                    case "1": PerformanceTools.MeasureElapsedTime(FirstTask); break;
                    case "2": PerformanceTools.MeasureElapsedTime(SecondTask); break;
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

        private static void SecondTask()
        {

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
            Console.WriteLine("Genereta reference data, press '1'");
            Console.WriteLine("Quit, press 'q'");
        }
    }
}
