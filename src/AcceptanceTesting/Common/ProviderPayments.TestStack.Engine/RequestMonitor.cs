using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain;
using SFA.DAS.Messaging;

namespace ProviderPayments.TestStack.Engine
{
    public class RequestMonitor
    {
        private readonly IPollingMessageReceiver _messageReceiver;
        private readonly IRequestProcessor _requestProcessor;

        public RequestMonitor(IPollingMessageReceiver messageReceiver, IRequestProcessor requestProcessor)
        {
            _messageReceiver = messageReceiver;
            _requestProcessor = requestProcessor;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = await _messageReceiver.ReceiveAsAsync<ProcessRequest>().ConfigureAwait(false);
                if (message != null)
                {
                    try
                    {
                        _requestProcessor.ProcessRequest(message.Content.ProcessType, message.Content.Content);
                    }
                    catch (Exception ex)
                    {
                        //TODO: Log
                        Trace.TraceError("RequestMonitor: " + ex.Message);
                    }

                    await message.CompleteAsync().ConfigureAwait(false);
                }
                else
                {
                    await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}
