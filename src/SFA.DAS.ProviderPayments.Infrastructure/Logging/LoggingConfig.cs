using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using NLog;

namespace SFA.DAS.ProdiverPayments.Infrastructure.Logging
{
    public static class LoggingConfig
    {
        public static void ConfigureLogging()
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

            LogManager.ReconfigExistingLoggers();
        }

        private static LogLevel GetLogLevelFromConfigurationManager()
        {
            var settingValue = CloudConfigurationManager.GetSetting("LogLevel");
            return LogLevel.FromString(settingValue);
        }
    }
}
