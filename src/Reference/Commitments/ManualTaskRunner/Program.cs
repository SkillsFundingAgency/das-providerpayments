using System;
using System.Collections.Generic;
using System.Text;
using CS.Common.External.Interfaces;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Commitments;

namespace ManualTaskRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Base Url: ");
            var baseUrl = Console.ReadLine();

            Console.Write("Token (can be split over multiple lines. use blank line to end): ");

            string line;
            var token = new StringBuilder();
            while ((line = Console.ReadLine()).Length > 0)
            {
                token.Append(line);
            }

            Console.Write("Transient connection string (leave blank for local): ");
            var transientConnectionString = Console.ReadLine();
            if (string.IsNullOrEmpty(transientConnectionString))
            {
                transientConnectionString = "server=.;database=ReferenceCommitments_Transient;trusted_connection=true;";
            }

            try
            {
                var context = new TextContext(transientConnectionString, baseUrl, token.ToString());
                var task = new ImportCommitmentsTask();

                task.Execute(context);

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }

    internal class TextContext : IExternalContext
    {
        public TextContext(string transientConnectionString, string baseUrl, string token)
        {
            Properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, transientConnectionString},
                { ContextPropertyKeys.LogLevel, "DEBUG"},
                { "DAS.Payments.Commitments.BaseUrl", baseUrl},
                { "DAS.Payments.Commitments.ClientToken", token}
            };
        }

        public IDictionary<string, string> Properties { get; set; }
    }
}
