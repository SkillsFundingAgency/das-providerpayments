using System.Linq;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.DCFS.UnitTests.Logging.LoggingConfiguration.Configure
{
    public class WhenConfiguring
    {
        private readonly string _connectionString = "Ilr.Transient.Connection.String";
        private readonly string _logLevel = "Debug";
        private readonly string _invalidLogLevel = "Debug1";
        private readonly string _databaseSchema = "Schema";

        [Test]
        public void ThenSqlServerTargetIsPresentAndCorrectlyConfigured()
        {
            // Arrange
            DCFS.Logging.LoggingConfiguration.Configure(_connectionString, _logLevel, _databaseSchema);

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
            DCFS.Logging.LoggingConfiguration.Configure(_connectionString, _logLevel, _databaseSchema);

            // Act
            var sqlServerRule = LogManager.Configuration.LoggingRules.FirstOrDefault(lr => lr.Targets.Count(t => t.Name == "sqlserver") == 1);

            // Assert
            Assert.IsNotNull(sqlServerRule);
            Assert.AreEqual(1, sqlServerRule.Levels.Count(lvl => lvl.Name == _logLevel));
        }

        [Test]
        public void ThenExpectingExceptionForInvalidLogLevelProvided()
        {
            // Assert
            var ex = Assert.Throws<InvalidContextException>(() => DCFS.Logging.LoggingConfiguration.Configure(_connectionString, _invalidLogLevel, _databaseSchema));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextPropertiesInvalidLogLevelMessage));
        }
    }
}
