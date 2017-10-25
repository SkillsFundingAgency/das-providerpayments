using System.Linq;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Logging;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Logging.LoggingConfigurationTests
{
    public class WhenConfiguringLogging
    {
        private readonly string _databaseSchema = "DataLock";
        private readonly string _connectionString = "Ilr.Transient.Connection.String";
        private readonly string _logLevel = "Debug";
        private readonly string _invalidLogLevel = "Debug1";

        [Test]
        public void ThenSqlServerTargetIsPresentAndCorrectlyConfigured()
        {
            // Arrange
            LoggingConfiguration.Configure(_connectionString, _logLevel, _databaseSchema);

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
            LoggingConfiguration.Configure(_connectionString, _logLevel, _databaseSchema);

            // Act
            var sqlServerRule = LogManager.Configuration.LoggingRules.FirstOrDefault(lr => lr.Targets.Count(t => t.Name == "sqlserver") == 1);

            // Assert
            Assert.IsNotNull(sqlServerRule);
            Assert.AreEqual(1, sqlServerRule.Levels.Count(lvl => lvl.Name == _logLevel));
        }

        [Test]
        public void ThenExpectingExceptionForInvalidLogLevel()
        {
            // Assert
            var ex = Assert.Throws<InvalidContextException>(() => LoggingConfiguration.Configure(_connectionString, _invalidLogLevel, _databaseSchema));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextPropertiesInvalidLogLevelMessage));
        }
    }
}
