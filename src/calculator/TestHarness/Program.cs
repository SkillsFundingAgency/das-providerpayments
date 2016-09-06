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
                        Console.WriteLine("Context reset");
                        break;
                    case 2:
                        ExecuteLevyTask(context);
                        break;
                    default:
                        exit = true;
                        break;
                }
            }
        }

        private static int GetRequestedAction()
        {
            while (true)
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("   1. Reset context");
                Console.WriteLine("   2. Execute levy task");
                Console.WriteLine("   0. Exit");
                Console.Write("Selection: ");

                var input = Console.ReadLine()?.Trim();
                int selection;
                if (int.TryParse(input, out selection) && selection >= 0 && selection <= 2)
                {
                    return selection;
                }
                Console.WriteLine("Invalid input! Please enter a number between 0 and 1 (inclusive)");
                Console.WriteLine();
            }
        }
        private static void ExecuteLevyTask(IExternalContext context)
        {
            try
            {
                var task = new LevyPaymentsTask();

                Console.WriteLine("Starting task...");
                task.Execute(context);

                Console.WriteLine("Task completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR : " + ex.Message);
            }
        }
    }
}
