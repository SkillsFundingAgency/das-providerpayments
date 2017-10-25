using System;

namespace ProviderPayments.TestStack.Core
{
    public interface ILogger
    {
        void Debug(string message);

        void Info(string message);

        void Warn(string message);
        void Warn(Exception exception, string message);

        void Error(Exception exception, string message);
    }
}
