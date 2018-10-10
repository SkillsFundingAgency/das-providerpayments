using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.Calculator;

namespace RunOpaEarningsCalculatorTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new EarningsCalculatorContext
            {
                Properties = new Dictionary<string, string>
                {
                    { "TransientDatabaseConnectionString", "server=.;database=ProvPayTestStack_Transient;trusted_connection=true;" },
                    { "LogLevel", "Debug" },
                    { "YearOfCollection", "1617" }
                }
            };

            var task = new ApprenticeshipEarningsTask();

            task.Execute(context);
        }
    }
}
