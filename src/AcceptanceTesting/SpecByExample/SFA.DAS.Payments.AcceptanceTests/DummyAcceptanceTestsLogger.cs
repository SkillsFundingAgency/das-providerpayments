using System;
using ProviderPayments.TestStack.Core;

namespace SFA.DAS.Payments.AcceptanceTests
{
    internal class DummyAcceptanceTestsLogger : ILogger
    {
        public void Debug(string message) { }

        public void Info(string message) { }

        public void Warn(string message) { }

        public void Warn(Exception exception, string message) { }

        public void Error(Exception exception, string message) { }
    }
}