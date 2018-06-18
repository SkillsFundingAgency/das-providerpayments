using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;
using Amor.DCFT.BinaryTask;
using CS.Common.XmlToSql.Implementation;
using CS.Common.XmlToSql.Interfaces;
using CS.Common.XmlToSql.Model;
using Dapper;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class ShredIlrTask : WorkflowTask
    {
        private readonly ILogger _logger;

        public ShredIlrTask(ILogger logger)
        {
            _logger = logger;
            Id = "ShredIlr";
            Description = "Shred ILR file";
        }

        internal override void Prepare(TestStackContext context)
        {
            var path = Path.Combine(context.WorkingDirectory, $"ILRTableMap_{context.OpaRulebaseYear}.xml");

            if (context.OpaRulebaseYear=="1617")
                File.WriteAllText(path, Properties.Resources.ILRTableMap_1617);
            else if(context.OpaRulebaseYear == "1718")
                File.WriteAllText(path, Properties.Resources.ILRTableMap_1718);
        }
        internal override void Execute(TestStackContext context)
        {
            var tableMap = LoadTableMap(context);
            var shredderSettings = GetSettings(context, tableMap);
            Shred(shredderSettings);

            AddExtraIlrDetails(context);
        }

        private TableMap LoadTableMap(TestStackContext context)
        {
            var path = Path.Combine(context.WorkingDirectory, $"ILRTableMap_{context.OpaRulebaseYear}.xml");
            return new TableMap(path);
        }
        private XmlToSqlExecutionSettings GetSettings(TestStackContext context, TableMap tableMap)
        {
            return new XmlToSqlExecutionSettings()
            {
                DestinationConnectionString = context.TransientConnectionString,
                PageSize = 10000,
                XmlFileName = context.IlrFilePath,
                TableMap = tableMap,
                SeedGenerator = new SeedGenerator()
            };
        }
        private void Shred(XmlToSqlExecutionSettings settings)
        {
            var shredder = new ShredXmlToSql();

            shredder.Execute(
                settings,
                new PreProcessingCallback(),
                new ProcessingContext(null, null, null, null, new Dictionary<string, string>(), null, null, null));
        }

        private void AddExtraIlrDetails(TestStackContext context)
        {
            using (var connection = new SqlConnection(context.TransientConnectionString))
            {
                connection.Execute("INSERT INTO dbo.FileDetails (UKPRN, Filename, SubmittedTime) SELECT UKPRN, @FileName, GETDATE() FROM Input.LearningProvider",
                    new { FileName = Path.GetFileName(context.IlrFilePath) });
            }
        }

        private class PreProcessingCallback : IPreProcessingCallback
        {
            public IEnumerable<XElement> Process(IEnumerable<XElement> elements, IProcessingContext context)
            {
                return elements;
            }
        }
    }
}
