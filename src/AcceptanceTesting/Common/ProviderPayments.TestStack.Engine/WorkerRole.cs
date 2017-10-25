using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using ProviderPayments.TestStack.Application;
using ProviderPayments.TestStack.Infrastructure.Data;
using ProviderPayments.TestStack.Infrastructure.Logging;
using ProviderPayments.TestStack.Infrastructure.Mapping;
using SFA.DAS.Messaging.AzureStorageQueue;

namespace ProviderPayments.TestStack.Engine
{
    public class WorkerRole : RoleEntryPoint
    {
        private const string RequestQueueName = "teststackprocessrequests";

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private RequestMonitor _requestMonitor;

        public override void Run()
        {
            Trace.TraceInformation("ProviderPayments.TestStack.Engine is running");

            try
            {
                _requestMonitor.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();


            LoggingConfig.ConfigureLogging();

            var queueConnectionString = CloudConfigurationManager.GetSetting("RequestQueueConnectionString");
            var mapperConfiguration = AutoMapperConfiguration.Configure(null);
            var componentService = new ComponentService(new SqlServerComponentRepository(), new AutoMapperMapper(mapperConfiguration));
            _requestMonitor = new RequestMonitor(
                new AzureStorageQueueService(queueConnectionString, RequestQueueName),
                new RequestProcessor(componentService));

            Trace.TraceInformation("ProviderPayments.TestStack.Engine has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("ProviderPayments.TestStack.Engine is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("ProviderPayments.TestStack.Engine has stopped");
        }
    }
}
