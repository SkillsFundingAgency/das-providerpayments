using System;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.DCFS.Logging
{
    public class LoggingConfiguration
    {
        public static void Configure(string connectionString, string logLevel, string schema)
        {
            var config = new NLog.Config.LoggingConfiguration();
            var sqlServerTarget = new DatabaseTarget("sqlserver");

            sqlServerTarget.ConnectionString = connectionString;

            sqlServerTarget.CommandText = $@"INSERT INTO [{schema}].[TaskLog] (
                                        Level, Logger, Message, Exception
                                    ) VALUES (
                                        @level, @logger, @message, @exception
                                    )";

            sqlServerTarget.Parameters.Add(new DatabaseParameterInfo("@level", new SimpleLayout("${level}")));
            sqlServerTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new SimpleLayout("${logger}")));
            sqlServerTarget.Parameters.Add(new DatabaseParameterInfo("@message", new SimpleLayout("${message}")));
            sqlServerTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new SimpleLayout("${exception:tostring}")));

            config.AddTarget("sqlserver", sqlServerTarget);
            config.LoggingRules.Add(new LoggingRule("*", GetLogLevel(logLevel), sqlServerTarget));

            LogManager.Configuration = config;
            LogManager.ThrowExceptions = true;
        }

        private static LogLevel GetLogLevel(string logLevel)
        {
            try
            {
                return LogLevel.FromString(logLevel);
            }
            catch (Exception ex)
            {
                throw new InvalidContextException(InvalidContextException.ContextPropertiesInvalidLogLevelMessage, ex);
            }
        }
    }
}
