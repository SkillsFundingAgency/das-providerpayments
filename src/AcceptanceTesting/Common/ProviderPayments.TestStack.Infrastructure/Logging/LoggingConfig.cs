using System.Linq;
using Microsoft.Azure;
using NLog;
using NLog.Targets;

namespace ProviderPayments.TestStack.Infrastructure.Logging
{
    public static class LoggingConfig
    {
        public static void ConfigureLogging()
        {
            SetConnectionString();

            SetLoggingLevel();

            LogManager.ReconfigExistingLoggers();
        }

        private static void SetLoggingLevel()
        {
            var loggingLevels = LogLevel.AllLoggingLevels.ToList();
            var minLevel = GetLogLevelFromConfigurationManager();
            var levelIndex = loggingLevels.IndexOf(minLevel);

            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                for (var i = 0; i < loggingLevels.Count; i++)
                {
                    var level = loggingLevels[i];
                    var hasLevel = rule.IsLoggingEnabledForLevel(level);
                    if (i < levelIndex && hasLevel)
                    {
                        rule.DisableLoggingForLevel(level);
                    }
                    else if (i >= levelIndex && !hasLevel)
                    {
                        rule.EnableLoggingForLevel(level);
                    }
                }
            }
        }

        private static void SetConnectionString()
        {
            LogManager.Configuration.FindTargetByName<DatabaseTarget>("Db").ConnectionString
                = CloudConfigurationManager.GetSetting("TransientConnectionString");
        }

        private static LogLevel GetLogLevelFromConfigurationManager()
        {
            var settingValue = CloudConfigurationManager.GetSetting("LogLevel");
            return LogLevel.FromString(settingValue);
        }
    }
}
