using System;
using System.Threading;
using ProviderPayments.TestStack.Application;
using ProviderPayments.TestStack.Engine;
using ProviderPayments.TestStack.Infrastructure.Data;
using ProviderPayments.TestStack.Infrastructure.Mapping;
using SFA.DAS.Messaging.AzureStorageQueue;

namespace ConsoleEngine
{
    class Program
    {
        private const string RequestQueueName = "teststackprocessrequests";

        static void Main(string[] args)
        {
            var queueConnectionString = "UseDevelopmentStorage=true;";
            var mapperConfiguration = AutoMapperConfiguration.Configure(null);
            var componentService = new ComponentService(new SqlServerComponentRepository(), new AutoMapperMapper(mapperConfiguration));

            var requestMonitor = new RequestMonitor(
                new AzureStorageQueueService(queueConnectionString, RequestQueueName),
                new RequestProcessor(componentService));
            var cancellationTokenSource = new CancellationTokenSource();
            var runTask = requestMonitor.RunAsync(cancellationTokenSource.Token);

            Console.WriteLine("Running. Press any key to exit...");
            Console.ReadKey();

            Console.WriteLine("Stopping");
            cancellationTokenSource.Cancel();
            try
            {
                runTask.Wait();
            }
            catch
            {
            }
            Console.WriteLine("Bye");
        }
    }
}
