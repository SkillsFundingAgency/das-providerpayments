using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Dapper;
using NLog;
using OPAWrapperLIB;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.CollectionEarnings.Calculator
{
    public class ApprenticeshipEarningsProcessor
    {
        private const string XslFileName = "ILR Apprenticeship Earnings Calc - Input_XSLT.xsl";
        private string RulebaseFileName = "Apprenticeships Earnings Calc {OpaRulebaseYear}.zip";
        private string OpaConfigFilePath = "AEC_OPAConfigILR_{OpaRulebaseYear}.xml";

        private readonly ILogger _logger;

        private string _tempDirectory;

        public ApprenticeshipEarningsProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public virtual void Process(ContextWrapper context)
        {
            _logger.Info("Started the OPA Apprenticeship Earnings Processor.");

            _tempDirectory = CreateTemporaryDirectory();

            _logger.Info("Started deploying the rulebase artifacts.");
            DeployRulebaseArtifacts(context);
            _logger.Info("Finished deploying the rulebase artifacts.");

            _logger.Info("Started preparing the database.");
            PrepareDatabase(context);
            _logger.Info("Finished preparing the database");

            _logger.Info("Started running the OPA rulebase.");
            RunRulebase(context);
            _logger.Info("Finished running the OPA rulebase.");

            _logger.Info("Started post rulebase execution activities.");
            PostExecution(context);
            _logger.Info("Finished post rulebase execution activities.");

            _logger.Info("Started cleanup.");
            Cleanup();
            _logger.Info("Finished cleanup.");

            _logger.Info("Finished the OPA Apprenticeship Earnings Processor.");
        }

        private string CreateTemporaryDirectory()
        {
            var path = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), DateTime.UtcNow.ToString("HHmmssfffff"));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private void DeployRulebaseArtifacts(ContextWrapper context)
        {
            var yearOfCollectionStart = GetYearOfCollectionStart(context);
            var yearOfRulebase= GetOpaRulebaseYear(context);
            var executablesDirectory = GetExecutablesDirectory(context);

            RulebaseFileName = RulebaseFileName.Replace("{OpaRulebaseYear}", $"{yearOfRulebase.Substring(0, 2)}_{yearOfRulebase.Substring(2)}");

            var xslPath = $@"{_tempDirectory}\{XslFileName}";
            var rulebasePath = $@"{_tempDirectory}\{RulebaseFileName}";

            
            File.WriteAllText(xslPath, File.ReadAllText(executablesDirectory + @"\Resources\" + XslFileName));
            File.WriteAllBytes(rulebasePath, File.ReadAllBytes(executablesDirectory + @"\Resources\" +  RulebaseFileName));
        }

        private void PrepareDatabase(ContextWrapper context)
        {
            var connectionString = context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute("EXEC [Rulebase].[AEC_Insert_Cases]");
            }
        }

        private void ExecuteSqlScript(string sql, SqlConnection connection)
        {
            var commands = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                connection.Execute(command);
            }
        }

        private void RunRulebase(ContextWrapper context)
        {
            var yearOfCollectionStart = GetYearOfCollectionStart(context);
            var executablesDirectory = GetExecutablesDirectory(context);


            var opaXslPath = $@"{_tempDirectory}\{XslFileName}";

            var opaXmlConfig = new XmlDocument();

            OpaConfigFilePath = OpaConfigFilePath.Replace("{OpaRulebaseYear}", GetOpaRulebaseYear(context));

            opaXmlConfig.LoadXml(
                File.ReadAllText( executablesDirectory + @"\Resources\" + OpaConfigFilePath) 
                    .Replace("${ComponentFolder}", _tempDirectory)
                    .Replace("${YearOfCollectionStart}", yearOfCollectionStart)
            );

            var opaConnectionStrings = GetOpaConnectionStrings(context);

            var opaWrapper = new OPAWrapperLib(opaXmlConfig);

            opaWrapper.SetLoggingLevel(LoggingType.Error);
            opaWrapper.NumberOfThreads = 8;
            opaWrapper.XslFile = opaXslPath;

            if (context.GetPropertyValue(ContextPropertyKeys.LogLevel) == "Debug")
            {
                opaWrapper.DumpXdsFileName = "LearnRefNumber@Learner";
                opaWrapper.DumpXdsPath = GetXdsDumpPath();
            }

            foreach (var connection in opaConnectionStrings)
            {
                opaWrapper.AddConString(connection.Key, connection.Value);
            }

            opaWrapper.Run();
        }

        private Dictionary<string, string> GetOpaConnectionStrings(ContextWrapper context)
        {
            var connectionString = context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString);

            return new Dictionary<string, string>
            {
                { "ILR", connectionString },
                { "LoggingDB", connectionString }
            };
        }

        private string GetYearOfCollectionStart(ContextWrapper context)
        {
            var yearOfCollection = context.GetPropertyValue("YearOfCollection");

            return $"20{yearOfCollection.Substring(0, 2)}-08-01";
        }


        private string GetOpaRulebaseYear(ContextWrapper context)
        {
            return context.GetPropertyValue("OpaRulebaseYear");
        }

        private string GetExecutablesDirectory(ContextWrapper context)
        {
            return context.GetPropertyValue("ExecutablesDirectory");
        }

        private string GetXdsDumpPath()
        {
            var path = @"C:\temp\xds";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private void PostExecution(ContextWrapper context)
        {
            var connectionString = context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString);

            using (var connection = new SqlConnection(connectionString))
            {
                _logger.Info("PostExecution:AEC_PivotTemporals_ApprenticeshipPriceEpisode");
                connection.Execute("EXEC [Rulebase].[AEC_PivotTemporals_ApprenticeshipPriceEpisode]", commandTimeout: 90);
                _logger.Info("PostExecution:AEC_PivotTemporals_LearningDelivery");
                connection.Execute("EXEC [Rulebase].[AEC_PivotTemporals_LearningDelivery]", commandTimeout: 90);
            }
        }

        private void Cleanup()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }
    }
}