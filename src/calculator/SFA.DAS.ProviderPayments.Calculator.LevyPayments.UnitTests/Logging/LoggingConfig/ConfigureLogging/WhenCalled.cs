using System.Linq;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Exceptions;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Logging.LoggingConfig.ConfigureLogging
{
    public class WhenCalled
    {
        private readonly string _connectionString = "Ilr.Transient.Connection.String";
        private readonly string _logLevel = "Debug";
        private readonly string _invalidLogLevel = "Debug1";

        [Test]
        public void ThenSqlServerTargetIsPresentAndCorrectlyConfigured()
        {
            // Arrange
            LevyPayments.Logging.LoggingConfig.ConfigureLogging(_connectionString, _logLevel);

            // Act
            var sqlServerTarget = (DatabaseTarget)LogManager.Configuration.FindTargetByName("sqlserver");

            // Assert
            Assert.IsNotNull(sqlServerTarget);
            Assert.AreEqual(_connectionString, ((SimpleLayout)sqlServerTarget.ConnectionString).OriginalText);
        }

        [Test]
        public void ThenSqlServerRuleIsPresentAndCorrectlyConfigured()
        {
            // Arrange
            LevyPayments.Logging.LoggingConfig.ConfigureLogging(_connectionString, _logLevel);

            // Act
            var sqlServerRule = LogManager.Configuration.LoggingRules.FirstOrDefault(lr => lr.Targets.Count(t => t.Name == "sqlserver") == 1);

            // Assert
            Assert.IsNotNull(sqlServerRule);
            Assert.AreEqual(1, sqlServerRule.Levels.Count(lvl => lvl.Name == _logLevel));
        }

        [Test]
        public void ThenExpectingExceptionForInvalidLogLevelProvided()
        {
            // Arrange
            LevyPayments.Logging.LoggingConfig.ConfigureLogging(_connectionString, _logLevel);

            // Assert
            var ex = Assert.Throws<LevyPaymentsInvalidContextException>(() => LevyPayments.Logging.LoggingConfig.ConfigureLogging(_connectionString, _invalidLogLevel));
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsExceptionMessages.ContextPropertiesInvalidLogLevel));
        }
    }
}
