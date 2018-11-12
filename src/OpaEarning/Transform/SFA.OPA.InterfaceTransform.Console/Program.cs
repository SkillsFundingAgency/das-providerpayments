using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SFA.OPA.InterfaceTransform.Console
{
    class Program
    {
        private static string OutputResourcesLocation = @"SFA.DAS.CollectionEarnings.Calculator\Resources\{rulebaseVersion}";
        private static string OutputSqlDdlDeployLocation = @"Deploy\{rulebaseVersion}\sql\ddl";
        private static string OutputSqlDmlDeployLocation = @"Deploy\{rulebaseVersion}\sql\dml";
        private static string OutputCopyMappingsDeployLocation = @"Deploy\{rulebaseVersion}\copy mappings";

        static void Main(string[] args)
        {
            try
            {
                var arguments = Arguments.Parse(args);

                // check for help request
                if (arguments.ContainsKey("?") || arguments.ContainsKey("H") || arguments.ContainsKey("HELP"))
                {
                    ShowHelp();

                    Environment.Exit(1);
                }

                //var inputPath = @"D:\work\sfa\das-collectionearnings\src\opa earnings\Source\Interface\1718.01"; //arguments.GetValueOrDefault(string.Empty, "interface");
                //var destinationPath = @"D:\work\sfa\das-collectionearnings\src\opa earnings\Build"; //arguments.GetValueOrDefault(string.Empty, "destination");
                //var rulebaseVersion = "1718";//arguments.GetValueOrDefault("1617", "rulebaseVersion");


                // parse args
                var inputPath = arguments.GetValueOrDefault(string.Empty, "interface");
                var destinationPath = arguments.GetValueOrDefault(string.Empty, "destination");
                var rulebaseVersion =arguments.GetValueOrDefault("1617", "rulebaseVersion");

                OutputResourcesLocation = OutputResourcesLocation.ReplaceCaseInsensitive("{rulebaseVersion}", rulebaseVersion);
                OutputSqlDdlDeployLocation = OutputSqlDdlDeployLocation.ReplaceCaseInsensitive("{rulebaseVersion}", rulebaseVersion);
                OutputSqlDmlDeployLocation = OutputSqlDmlDeployLocation.ReplaceCaseInsensitive("{rulebaseVersion}", rulebaseVersion);
                OutputCopyMappingsDeployLocation = OutputCopyMappingsDeployLocation.ReplaceCaseInsensitive("{rulebaseVersion}", rulebaseVersion);

                CreateOutputFolders(destinationPath);

                TransformXmlConfiguration(inputPath, destinationPath,rulebaseVersion);
                TransformXslConfiguration(inputPath, destinationPath);
                TransformTransientRulebaseTables(inputPath, destinationPath);
                TransformTransientRulebaseStoredProcedures(inputPath, destinationPath,rulebaseVersion);
                TransformTransientRulebaseTablesIntoDedsTables(inputPath, destinationPath);
                TransformTransientRulebaseTablesIntoTransientViews(inputPath, destinationPath);
                TransformTransientReferenceTables(inputPath, destinationPath);
                TransformTransientRulebaseTablesCopyToDedsMappings(inputPath, destinationPath);
                TransformTransientRulebaseTablesToDedsCleanupScript(inputPath, destinationPath);

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("ERROR " + ex.FormatException());

                Environment.Exit(1);
            }
        }

        private static void CreateOutputFolders(string destinationPath)
        {
            if (!Directory.Exists(destinationPath + @"\" + OutputResourcesLocation))
            {
                Directory.CreateDirectory(destinationPath + @"\" + OutputResourcesLocation);
            }
            if (!Directory.Exists(destinationPath + @"\" + OutputSqlDdlDeployLocation))
            {
                Directory.CreateDirectory(destinationPath + @"\" + OutputSqlDdlDeployLocation);
            }
            if (!Directory.Exists(destinationPath + @"\" + OutputSqlDmlDeployLocation))
            {
                Directory.CreateDirectory(destinationPath + @"\" + OutputSqlDmlDeployLocation);
            }
            if (!Directory.Exists(destinationPath + @"\" + OutputCopyMappingsDeployLocation))
            {
                Directory.CreateDirectory(destinationPath + @"\" + OutputCopyMappingsDeployLocation);
            }
        }

        private static void ShowHelp()
        {
            var sb = new StringBuilder();

            sb.AppendLine("\nSFA.OPA.InterfaceTransform.Console command line help");
            sb.AppendLine("\nThis tool transforms the interface scripts for the OPA Apprenticeship Earnings rulebase so that the DAS automated tests can be executed.");
            sb.AppendLine("\nThe following command line arguments are supported (all case insensitive, any order):");
            sb.AppendLine("\n  /interface:<value>    - <value> is the path to the rulebase interface folder");
            sb.AppendLine("\n  /destination:<value>    - <value> is the path to the destination folder");
            sb.AppendLine("\nExample:");
            sb.AppendLine("\n  SFA.OPA.InterfaceTransform.Console /interface:\"c:\\work\\das\\das-collectionearnings\\src\\opa earnings\\Source\\Interface\\1617.10\" /destination:\"c:\\work\\das\\das-collectionearnings\\src\\opa earnings\\Build\"");

            System.Console.WriteLine(sb.ToString());
        }

        private static void TransformXmlConfiguration(string inputPath, string outputPath, string rulebaseVersion)
        {
            System.Console.WriteLine("Started transforming the xml configuration.");

            var xmlConfigFileName = $"AEC_OPAConfigILR_{rulebaseVersion}.xml";
            var zipFileName = $"{rulebaseVersion.Substring(0, 2)}_{rulebaseVersion.Substring(2)}";

            var doc = XDocument.Load($@"{inputPath}\{xmlConfigFileName}");

            // update RulebasePath attribute value
            var element = doc.Descendants("OPAMapping").Single(x => x.Attribute("RulebasePath") != null);

            element.SetAttributeValue("RulebasePath", @"${ComponentFolder}\Apprenticeships Earnings Calc " + zipFileName +".zip");

            // update PeriodStartDate attributes values
            var elements = doc.Descendants("Period").Where(x => x.Attribute("PeriodStartDate") != null);

            foreach (var elm in elements)
            {
                elm.SetAttributeValue("PeriodStartDate", "${YearOfCollectionStart}");
            }
            
            doc.Save($@"{outputPath}\{OutputResourcesLocation}\{xmlConfigFileName}");

            System.Console.WriteLine("Finished transforming the xml configuration.");
        }

        private static void TransformXslConfiguration(string inputPath, string outputPath)
        {
            System.Console.WriteLine("Started transforming the xsl configuration.");

            var xslConfigFileName = "ILR Apprenticeship Earnings Calc - Input_XSLT.xsl";

            File.Copy(
                $@"{inputPath}\{xslConfigFileName}",
                $@"{outputPath}\{OutputResourcesLocation}\{xslConfigFileName}",
                true);

            System.Console.WriteLine("Finished transforming the xsl configuration.");
        }

        private static void TransformTransientRulebaseTables(string inputPath, string outputPath)
        {
            System.Console.WriteLine("Started transforming the transient rulebase tables.");

            var inputFileName = "AEC_Tables.sql";
            var outputFileName = "OPA.Transient.Rulebase.ddl.tables.sql";

            var inputDdl = File.ReadAllLines($@"{inputPath}\{inputFileName}");

            var outputDdl = new List<string>();

            foreach (var line in inputDdl)
            {
                if (line.StartsWith("--"))
                {
                    continue;
                }

                if (line.StartsWith("go"))
                {
                    outputDdl.Add(line.ToUpper());
                    continue;
                }

                outputDdl.Add(line);
            }

            File.WriteAllLines($@"{outputPath}\{OutputSqlDdlDeployLocation}\{outputFileName}", outputDdl);

            System.Console.WriteLine("Finished transforming the transient rulebase tables.");
        }

        private static void TransformTransientRulebaseStoredProcedures(string inputPath, string outputPath,string rulebaseVersion)
        {
            System.Console.WriteLine("Started transforming the transient rulebase stored procedures.");

            var inpurFileName = "AEC_StoredProcedures.sql";
            var outputFileName = "OPA.Transient.Rulebase.ddl.sprocs.sql";

            var inputDdl = File.ReadAllLines($@"{inputPath}\{inpurFileName}");

            var outputDdl = new List<string>();

            foreach (var line in inputDdl)
            {
                if (line.StartsWith("--"))
                {
                    continue;
                }

                if (line.StartsWith("go"))
                {
                    outputDdl.Add(line.ToUpper());
                    continue;
                }

                outputDdl.Add(line.ReplaceCaseInsensitive(rulebaseVersion, "${YearOfCollection}"));
            }

            File.WriteAllLines($@"{outputPath}\{OutputSqlDdlDeployLocation}\{outputFileName}", outputDdl);

            System.Console.WriteLine("Finished transforming the transient rulebase stored procedures.");
        }

        private static void TransformTransientRulebaseTablesIntoDedsTables(string inputPath, string outputPath)
        {
            System.Console.WriteLine("Started transforming the transient rulebase tables into deds tables.");

            var inputFileName = "AEC_Tables.sql";
            var outputFileName = "OPA.Deds.Rulebase.ddl.tables.sql";

            var inputDdl = File.ReadAllLines($@"{inputPath}\{inputFileName}");

            var outputDdl = new List<string>();

            var afterCreateTable = false;
            var afterPrimaryKey = false;

            var tableName = string.Empty;

            foreach (var line in inputDdl)
            {
                if (line.StartsWith("--"))
                {
                    continue;
                }

                if (line.StartsWith("go"))
                {
                    outputDdl.Add(line.ToUpper());
                    continue;
                }

                if (line.Trim().StartsWith("if object_id", StringComparison.InvariantCultureIgnoreCase))
                {
                    tableName = line.Split('\'')[1];

                    outputDdl.Add(line);
                    continue;
                }

                if (line.Trim().StartsWith("create table", StringComparison.InvariantCultureIgnoreCase))
                {
                    outputDdl.Add(line);
                    if (!tableName.Equals("Rulebase.AEC_global", StringComparison.OrdinalIgnoreCase))
                    {
                        outputDdl.Add(" [Ukprn] bigint NOT NULL,");
                    }

                    afterCreateTable = true;
                    continue;
                }

                if (line.Trim().StartsWith("primary key", StringComparison.InvariantCultureIgnoreCase))
                {
                    afterPrimaryKey = true;
                }

                if (line.Trim().StartsWith("(") && afterPrimaryKey)
                {
                    outputDdl.Add(line + " [Ukprn] asc,");
                    afterPrimaryKey = false;
                    continue;
                }

                outputDdl.Add(line);
            }

            File.WriteAllLines($@"{outputPath}\{OutputSqlDdlDeployLocation}\{outputFileName}", outputDdl);

            System.Console.WriteLine("Finished transforming the transient rulebase tables into deds tables.");
        }

        private static readonly Regex FirstWordOnALineRegex = new Regex(@"\s*(\w*)\s", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        static string FirstWord(string input)
        {
            var matches = FirstWordOnALineRegex.Matches(input);
            if (matches.Count == 0)
            {
                return string.Empty;
            }

            return matches[0].Value.Replace("\t", "");
        }

        private static void TransformTransientRulebaseTablesIntoTransientViews(string inputPath, string outputPath)
        {
            System.Console.WriteLine("Started transforming the transient rulebase tables into transient views.");

            var inputFileName = "AEC_Tables.sql";
            var outputFileName = "OPA.Transient.Rulebase.ddl.views.sql";

            var inputDdl = File.ReadAllLines($@"{inputPath}\{inputFileName}");

            var outputDdl = new List<string>();

            var afterCreateTable = false;
            var afterPrimaryKey = false;
            var tableName = string.Empty;
            var viewName = string.Empty;

            foreach (var line in inputDdl)
            {
                if (line.StartsWith("--"))
                {
                    continue;
                }

                if (line.StartsWith("go"))
                {
                    outputDdl.Add(line.ToUpper());
                    continue;
                }

                if (line.Trim().StartsWith("if object_id", StringComparison.InvariantCultureIgnoreCase))
                {
                    tableName = line.Split('\'')[1];
                    viewName = tableName.ReplaceCaseInsensitive(".", ".vw_");

                    outputDdl.Add(line.ReplaceCaseInsensitive("'u'", "'v'").ReplaceCaseInsensitive(tableName, viewName));
                    continue;
                }

                if (line.Trim().StartsWith("drop table", StringComparison.InvariantCultureIgnoreCase))
                {
                    outputDdl.Add(line.ReplaceCaseInsensitive("drop table", "drop view").ReplaceCaseInsensitive(tableName, viewName));
                    continue;
                }

                if (line.Trim().StartsWith("create table", StringComparison.InvariantCultureIgnoreCase))
                {
                    outputDdl.Add(line.ReplaceCaseInsensitive("create table", "create view")
                        .ReplaceCaseInsensitive(tableName, viewName)
                        .ReplaceCaseInsensitive("(", ""));
                    afterCreateTable = true;

                    if (tableName.Equals("Rulebase.AEC_global", StringComparison.OrdinalIgnoreCase))
                    {
                        outputDdl.Add("AS SELECT '' AS [Nothing],");
                    }
                    else
                    {
                        outputDdl.Add("AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn],");
                    }

                    continue;
                }

                if (line.Trim().StartsWith("primary key", StringComparison.InvariantCultureIgnoreCase))
                {
                    afterPrimaryKey = true;
                    continue;
                }

                if (line.Trim().StartsWith(")") && afterPrimaryKey)
                {
                    afterPrimaryKey = false;
                    continue;
                }

                if (afterPrimaryKey)
                {
                    continue;
                }

                if (line.Trim().StartsWith(")") && afterCreateTable)
                {
                    var lastLine = outputDdl.Last().TrimEnd(',');
                    outputDdl.RemoveAt(outputDdl.Count - 1);
                    outputDdl.Add(lastLine);

                    outputDdl.Add("FROM " + tableName);
                    afterCreateTable = false;
                    continue;
                }

                var firstWord = FirstWord(line);
                if (!string.IsNullOrEmpty(firstWord) && afterCreateTable)
                {
                    outputDdl.Add($"{firstWord},");
                    continue;
                }
                
                outputDdl.Add(line);
            }

            // static view
            outputDdl.Add("IF EXISTS (SELECT [object_id] FROM sys.views WHERE [name] = 'vw_AEC_EarningHistory' and [schema_id] = SCHEMA_ID('Rulebase')) "
                          + "BEGIN "
                          + "DROP VIEW Rulebase.vw_AEC_EarningHistory "
                          + "END");
            outputDdl.Add("GO");
            outputDdl.Add("CREATE VIEW [Rulebase].[vw_AEC_EarningHistory] "
                          + "AS "
                          + "SELECT "
                          + "[AppIdentifierOutput] [AppIdentifier], "
                          + "[AppProgCompletedInTheYearOutput][AppProgCompletedInTheYearInput], "
                          + "(SELECT[Name] FROM[Reference].[CollectionPeriods]) AS[CollectionReturnCode], "
                          + "'${YearOfCollection}' AS[CollectionYear], "
                          + "[HistoricDaysInYearOutput] [DaysInYear], "
                          + "[HistoricFworkCodeOutput] [FworkCode], "
                          + "[HistoricEffectiveTNPStartDateOutput] [HistoricEffectiveTNPStartDateInput], "
                          + "[HistoricLearner1618AtStartOutput] [HistoricLearner1618StartInput], "
                          + "[HistoricTNP1Output] [HistoricTNP1Input], "
                          + "[HistoricTNP2Output] [HistoricTNP2Input], "
                          + "[HistoricTNP3Output] [HistoricTNP3Input], "
                          + "[HistoricTNP4Output] [HistoricTNP4Input], "
                          + "[HistoricTotal1618UpliftPaymentsInTheYear] [HistoricTotal1618UpliftPaymentsInTheYearInput], "
                          + "[HistoricVirtualTNP3EndofThisYearOutput] [HistoricVirtualTNP3EndOfTheYearInput], "
                          + "[HistoricVirtualTNP4EndofThisYearOutput] [HistoricVirtualTNP4EndOfTheYearInput], "
                          + "1 AS[LatestInYear], "
                          + "[LearnRefNumber], "
                          + "[HistoricProgrammeStartDateIgnorePathwayOutput] [ProgrammeStartDateIgnorePathway], "
                          + "[HistoricProgrammeStartDateMatchPathwayOutput] [ProgrammeStartDateMatchPathway], "
                          + "[HistoricProgTypeOutput] [ProgType], "
                          + "[HistoricPwayCodeOutput] [PwayCode], "
                          + "[HistoricSTDCodeOutput] [STDCode], "
                          + "[HistoricTotalProgAimPaymentsInTheYear] [TotalProgAimPaymentsInTheYear], "
                          + "(SELECT Ukprn FROM Valid.LearningProvider) [UKPRN], "
                          + "[HistoricULNOutput] [ULN], "
                          + "[HistoricUptoEndDateOutput] [UptoEndDate], "
                          + "0.00 [BalancingProgAimPaymentsInTheYear], "
                          + "0.00 [CompletionProgAimPaymentsInTheYear], "
                          + "0.00 [OnProgProgAimPaymentsInTheYear]"
                          + "FROM [Rulebase].[AEC_HistoricEarningOutput]");
            outputDdl.Add("GO");

            File.WriteAllLines($@"{outputPath}\{OutputSqlDdlDeployLocation}\{outputFileName}", outputDdl);

            System.Console.WriteLine("Finished transforming the transient rulebase tables into transient views.");
        }

        private static void TransformTransientReferenceTables(string inputPath, string outputPath)
        {
            System.Console.WriteLine("Started transforming the transient reference tables.");

            var inputFileName = "CreateReferenceDataTablesSQL.sql";
            var outputFileName = "OPA.Transient.Reference.ddl.tables.sql";

            var inputDdl = File.ReadAllLines($@"{inputPath}\Reference Data\{inputFileName}");

            var outputDdl = new List<string>();

            foreach (var line in inputDdl)
            {
                if (line.StartsWith("--"))
                {
                    continue;
                }

                if (line.StartsWith("go"))
                {
                    outputDdl.Add(line.ToUpper());
                    continue;
                }

                outputDdl.Add(line);
            }

            File.WriteAllLines($@"{outputPath}\{OutputSqlDdlDeployLocation}\{outputFileName}", outputDdl);

            System.Console.WriteLine("Finished transforming the transient reference tables.");
        }

        private static void TransformTransientRulebaseTablesCopyToDedsMappings(string inputPath, string outputPath)
        {
            System.Console.WriteLine("Started transforming the transient rulebase tables into copy to deds mapping xmls.");

            var inputFileName = "AEC_Tables.sql";
            var outputFileName = "DasEarningsCopyToDedsMapping.xml";

            var inputDdl = File.ReadAllLines($@"{inputPath}\{inputFileName}");

            var tableMappings = new List<TableMapping>();

            var afterCreateTable = false;
            var afterPrimaryKey = false;

            TableMapping tableMapping = null;

            foreach (var line in inputDdl)
            {
                if (line.StartsWith("--"))
                {
                    continue;
                }

                if (line.StartsWith("go"))
                {
                    continue;
                }

                if (line.Trim().StartsWith("if object_id", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (tableMapping != null)
                    {
                        tableMappings.Add(tableMapping);
                    }

                    var tableName = line.Split('\'')[1];
                    var viewName = tableName.ReplaceCaseInsensitive("].[", "].[vw_");

                    tableMapping = new TableMapping
                    {
                        Source = viewName,
                        Destination = tableName,
                        Ordinal = tableMappings.Count + 1,
                        Columns = tableName.Equals("[Rulebase].[AEC_global]", StringComparison.OrdinalIgnoreCase)
                            ? new List<string>()
                            : new List<string> { "[Ukprn]" }
                    };

                    continue;
                }

                if (line.Trim().StartsWith("drop table", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (line.Trim().StartsWith("create table", StringComparison.InvariantCultureIgnoreCase))
                {
                    afterCreateTable = true;
                    continue;
                }

                if (line.Trim().StartsWith("(") && afterCreateTable && !afterPrimaryKey)
                {
                    continue;
                }

                if (line.Trim().StartsWith("primary key", StringComparison.InvariantCultureIgnoreCase))
                {
                    afterPrimaryKey = true;
                    continue;
                }

                if (line.Trim().StartsWith(")") && afterPrimaryKey)
                {
                    afterPrimaryKey = false;
                    continue;
                }

                if (afterPrimaryKey)
                {
                    continue;
                }

                if (line.Trim().StartsWith(")") && afterCreateTable)
                {
                    afterCreateTable = false;
                    continue;
                }

                var idx = line.TrimStart().IndexOf("]", StringComparison.OrdinalIgnoreCase);
                if (idx != -1)
                {
                    tableMapping?.Columns.Add(line.TrimStart().Remove(idx + 1));
                }
            }

            if (tableMapping != null)
            {
                tableMappings.Add(tableMapping);
            }

            // static table mapping
            tableMappings.Add(
                new TableMapping
                {
                    Source = "[Rulebase].[vw_AEC_EarningHistory]",
                    Destination = "[dbo].[AEC_EarningHistory]",
                    Ordinal = tableMappings.Count + 1,
                    Columns = new List<string>
                    {
                        "AppIdentifier",
                        "AppProgCompletedInTheYearInput",
                        "CollectionReturnCode",
                        "CollectionYear",
                        "DaysInYear",
                        "FworkCode",
                        "HistoricEffectiveTNPStartDateInput",
                        "HistoricLearner1618StartInput",
                        "HistoricTNP1Input",
                        "HistoricTNP2Input",
                        "HistoricTNP3Input",
                        "HistoricTNP4Input",
                        "HistoricTotal1618UpliftPaymentsInTheYearInput",
                        "HistoricVirtualTNP3EndOfTheYearInput",
                        "HistoricVirtualTNP4EndOfTheYearInput",
                        "LatestInYear",
                        "LearnRefNumber",
                        "ProgrammeStartDateIgnorePathway",
                        "ProgrammeStartDateMatchPathway",
                        "ProgType",
                        "PwayCode",
                        "STDCode",
                        "TotalProgAimPaymentsInTheYear",
                        "UKPRN",
                        "ULN",
                        "UptoEndDate"
                    }
                });
            
            var outputDoc = new XDocument(
                new XElement("CopyCatConfig",
                    new XElement("SourceConnectionString", "SourceConnectionString"),
                    new XElement("DestinationConnectionString", "DestinationConnectionString"),
                    new XElement("SqlBulkCopySettings", 
                        new XElement("BatchSize", 2000),
                        new XElement("BulkCopyTimeout", 7200),
                        new XElement("EnableStreaming", true)),
                    new XElement("TableMappings", tableMappings.Select(tm => tm.Mapping))
                    )
                );

            if (!Directory.Exists($@"{outputPath}\{OutputCopyMappingsDeployLocation}"))
            {
                Directory.CreateDirectory($@"{outputPath}\{OutputCopyMappingsDeployLocation}");
            }

            outputDoc.Save($@"{outputPath}\{OutputCopyMappingsDeployLocation}\{outputFileName}");

            System.Console.WriteLine("Finished transforming the transient rulebase tables into copy to deds mapping xmls.");
        }

        private static void TransformTransientRulebaseTablesToDedsCleanupScript(string inputPath, string outputPath)
        {
            System.Console.WriteLine("Started transforming the transient rulebase tables into a deds cleanup script.");

            var inputFileName = "AEC_Tables.sql";
            var outputFileName = "Ilr.Earnings.Cleanup.Deds.DML.sql";

            var inputDdl = File.ReadAllLines($@"{inputPath}\{inputFileName}");

            var outputDml = new List<string>();

            foreach (var line in inputDdl)
            {
                if (line.Trim().StartsWith("if object_id", StringComparison.InvariantCultureIgnoreCase))
                {
                    var tableName = line.Split('\'')[1];

                    outputDml.Add("DELETE FROM ${ILR_Deds.FQ}." + tableName);
                    outputDml.Add("    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])");
                    outputDml.Add("GO");
                }
            }

            // add static part
            outputDml.Add("DELETE FROM ${ILR_Deds.FQ}.dbo.AEC_EarningHistory");
            outputDml.Add("    WHERE Ukprn IN (SELECT Ukprn FROM Rulebase.vw_AEC_EarningHistory)");
            outputDml.Add("        AND CollectionYear IN (SELECT CollectionYear FROM Rulebase.vw_AEC_EarningHistory)");
            outputDml.Add("        AND CollectionReturnCode IN (SELECT CollectionReturnCode FROM Rulebase.vw_AEC_EarningHistory)");
            outputDml.Add("GO");
            outputDml.Add("UPDATE ${ILR_Deds.FQ}.dbo.AEC_EarningHistory");
            outputDml.Add("        SET LatestInYear = 0");
            outputDml.Add("    WHERE CollectionYear IN (SELECT CollectionYear FROM Rulebase.vw_AEC_EarningHistory)");
            outputDml.Add("        AND Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])");
            outputDml.Add("GO");

            File.WriteAllLines($@"{outputPath}\{OutputSqlDmlDeployLocation}\{outputFileName}", outputDml);

            System.Console.WriteLine("Finished transforming the transient rulebase tables into a deds cleanup script.");
        }
    }
}
