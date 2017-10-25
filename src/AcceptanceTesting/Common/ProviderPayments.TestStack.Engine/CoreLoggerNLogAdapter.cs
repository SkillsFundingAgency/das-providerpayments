using System;

namespace ProviderPayments.TestStack.Engine
{
    internal class CoreLoggerNLogAdapter : Core.ILogger
    {
        private readonly NLog.ILogger _nlogLogger;

        public CoreLoggerNLogAdapter(NLog.ILogger nlogLogger)
        {
            _nlogLogger = nlogLogger;
        }

        public void Debug(string message)
        {
            _nlogLogger.Debug(message);
        }

        public void Info(string message)
        {
            _nlogLogger.Info(message);
        }

        public void Warn(string message)
        {
            _nlogLogger.Warn(message);
        }

        public void Warn(Exception exception, string message)
        {
            _nlogLogger.Warn(exception, message);
        }

        public void Error(Exception exception, string message)
        {
            _nlogLogger.Error(exception, message);
        }
    }
}
