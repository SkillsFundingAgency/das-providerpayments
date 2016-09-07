using System;
using CS.Common.External.Interfaces;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new TestHarnessContext();
            var exit = false;

            while (!exit)
            {
                var requestedAction = GetRequestedAction();
                switch (requestedAction)
                {
                    case 1:
                        context = new TestHarnessContext();
                        WriteLine("Context reset");
                        break;
                    case 2:
                        ExecuteLevyTask(context);
                        break;
                    default:
                        exit = true;
                        break;
                }

                WriteLine();
            }
        }

        private static int GetRequestedAction()
        {
            while (true)
            {
                WriteLine("What would you like to do?");
                WriteLine("   1. Reset context");
                WriteLine("   2. Execute levy task");
                WriteLine("   0. Exit");
                Write("Selection: ");

                var input = Console.ReadLine()?.Trim();
                int selection;
                if (int.TryParse(input, out selection) && selection >= 0 && selection <= 2)
                {
                    return selection;
                }
                WriteErrorLine("Invalid input! Please enter a number between 0 and 1 (inclusive)");
                WriteLine();
            }
        }
        private static void ExecuteLevyTask(IExternalContext context)
        {
            try
            {
                var task = new LevyPaymentsTask();

                WriteLine("Starting task...");
                task.Execute(context);

                WriteLine("Task completed successfully");
            }
            catch (Exception ex)
            {
                WriteErrorLine("ERROR: " + ex.Message);
            }
        }


        private static void WriteLine(string line = "")
        {
            Console.WriteLine(line);
        }
        private static void WriteErrorLine(string line = "")
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine(line);
            Console.ForegroundColor = color;
        }
        private static void Write(string value)
        {
            Console.Write(value);
        }
    }
}
