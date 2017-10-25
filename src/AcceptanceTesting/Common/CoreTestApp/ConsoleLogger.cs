using System;
using ProviderPayments.TestStack.Core;

namespace CoreTestApp
{
    internal class ConsoleLogger : ILogger
    {
        private bool _canLogDebug = true;
        private bool _canLogInfo = true;
        private bool _canLogWarn = true;

        internal void SetLogLevel(string level)
        {
            switch (level.ToUpper())
            {
                case "INFO":
                    _canLogDebug = false;
                    _canLogInfo = true;
                    _canLogWarn = true;
                    break;
                case "WARN":
                    _canLogDebug = false;
                    _canLogInfo = false;
                    _canLogWarn = true;
                    break;
                case "ERROR":
                    _canLogDebug = false;
                    _canLogInfo = false;
                    _canLogWarn = false;
                    break;
                default:
                    _canLogDebug = true;
                    _canLogInfo = true;
                    _canLogWarn = true;
                    break;
            }
        }

        public void Debug(string message)
        {
            if (!_canLogDebug)
            {
                return;
            }

            Log($"[DEBUG] {message}", Console.ForegroundColor);
        }

        public void Info(string message)
        {
            if (!_canLogInfo)
            {
                return;
            }

            Log($"[INFO] {message}", Console.ForegroundColor);
        }

        public void Warn(string message)
        {
            if (!_canLogWarn)
            {
                return;
            }

            Log($"[WARN] {message}", ConsoleColor.Yellow);
        }

        public void Warn(Exception exception, string message)
        {
            if (!_canLogDebug)
            {
                return;
            }

            Log($"[WARN] {message} - {exception}", ConsoleColor.Yellow);
        }

        public void Error(Exception exception, string message)
        {
            Log($"[ERROR] {message} - {exception}", ConsoleColor.Red);
        }


        private void Log(string message, ConsoleColor color)
        {
            var resetColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            try
            {
                Console.WriteLine(message);
            }
            finally
            {
                Console.ForegroundColor = resetColor;
            }
        }
    }
}
