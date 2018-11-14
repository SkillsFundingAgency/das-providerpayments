using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IlrGenerator;
using Newtonsoft.Json;
using ProviderPayments.TestStack.Core;
using ProviderPayments.TestStack.Core.Domain;
using ProviderPayments.TestStack.Core.ExecutionStatus;

namespace CoreTestApp
{
    class Program
    {
        enum UserAction
        {
            SetEnvironmentVariables = 1,
            RebuildDeds = 2,
            CleanDedsDatabase = 3,
            RunIlrSubmission = 4,
            RunPeriodEnd = 5,
            ImportAccountData = 6,
            ImportCommitmentData = 7
        }

        private static ConsoleColor _defaultColor;
        private static string _dataPath;
        private static EnvironmentVariables _environmentVariables;
        private static ProcessStatusWatcher _statusWatcher;
        private static ConsoleLogger _logger;

        private static void Main(string[] args)
        {
            _defaultColor = Console.ForegroundColor;
            _logger = new ConsoleLogger();

            WriteLine("Loading");
            WriteLine();
            _dataPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "data");
            _environmentVariables = LoadEnvironmentVariables();
            _logger.SetLogLevel(_environmentVariables.LogLevel);

            OutputEnvironmentVariables();

            _statusWatcher = new ProcessStatusWatcher(ExecutionStarted, TaskStarted, TaskCompleted, ExecutionCompleted);

            Run(args);
        }

        private static void Run(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    switch (args[0].ToLower())
                    {
                        case "-rebuilddeds":
                            if (args.Length == 1)
                            {
                                WriteLine("Invalid command line parameters - missing component types", ConsoleColor.Red);
                                break;
                            }

                            var componentTypes = args[1].Split(',').Select(x => int.Parse(x.Trim())).ToArray();
                            foreach (var componentType in componentTypes)
                            {
                                RebuildDedsDatabase((ComponentType)componentType);
                            }
                            break;
                        default:
                            WriteLine("Invalid command line parameters", ConsoleColor.Red);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(ex.ToString(), ConsoleColor.Red);
                }
                return;
            }

            RunTillExit();
        }
        private static void RunTillExit()
        {
            int action;
            while ((action = GetAction()) != 0)
            {
                switch ((UserAction)action)
                {
                    case UserAction.SetEnvironmentVariables:
                        SetEnvironmentVariables();
                        break;
                    case UserAction.RebuildDeds:
                        RebuildDedsDatabase();
                        break;
                    case UserAction.CleanDedsDatabase:
                        CleanDedsDatabase();
                        break;
                    case UserAction.RunIlrSubmission:
                        RunIlrSubmissionProcess();
                        break;
                    case UserAction.RunPeriodEnd:
                        RunSummarisationProcess();
                        break;
                    case UserAction.ImportAccountData:
                        RunAccountsReferenceData();
                        break;
                    case UserAction.ImportCommitmentData:
                        RunCommitmentsReferenceData();
                        break;
                    default:
                        return;
                }
            }
        }
        private static int GetAction()
        {
            var selection = -1;
            while (selection < 0)
            {
                WriteLine("What would you like to do?");
                WriteLine($"   {(int)UserAction.SetEnvironmentVariables}. Set environment variables");
                WriteLine($"   {(int)UserAction.RebuildDeds}. Rebuild DEDS database");
                WriteLine($"   {(int)UserAction.CleanDedsDatabase}. Clean DEDS database");
                WriteLine($"   {(int)UserAction.RunIlrSubmission}. Run ILR submission process");
                WriteLine($"   {(int)UserAction.RunPeriodEnd}. Run period end process");
                WriteLine($"   {(int)UserAction.ImportAccountData}. Run accounts reference data process");
                WriteLine($"   {(int)UserAction.ImportCommitmentData}. Run commitments reference data process");
                WriteLine($"   0. Exit");
                Write("Selection: ");

                var input = ReadLine();
                if (int.TryParse(input, out selection) && selection >= 0 && selection <= 7)
                {
                    WriteLine();
                    break;
                }

                WriteLine("Invalid selection!", ConsoleColor.Red);
                selection = -1;
            }
            return selection;
        }

        private static void SetEnvironmentVariables()
        {
            WriteLine("Enter one update per line, in the format NAME:VALUE");
            WriteLine("Enter empty line to finish editing");

            string line;
            while (!string.IsNullOrEmpty((line = Console.ReadLine())))
            {
                var index = line.IndexOf(':');
                if (index < 0)
                {
                    WriteLine("Invalid format!", ConsoleColor.Red);
                    continue;
                }

                var key = line.Substring(0, index);
                var value = line.Substring(index + 1);
                switch (key.ToLower())
                {
                    case "transientconnectionstring":
                        _environmentVariables.TransientConnectionString = value;
                        break;
                    case "dedsdatabaseconnectionstring":
                        _environmentVariables.DedsDatabaseConnectionString = value;
                        break;
                    case "currentyear":
                        _environmentVariables.CurrentYear = value;
                        break;
                    case "workingdirectory":
                        _environmentVariables.WorkingDirectory = value;
                        break;
                    case "opencollection":
                        _environmentVariables.CollectionPeriod = ParseCollectionPeriod(value);
                        break;
                    default:
                        WriteLine("Invalid key", ConsoleColor.Red);
                        break;
                }
            }

            SaveEnvironmentVariables();
            WriteLine("Saved", ConsoleColor.Green);
            WriteLine();

            OutputEnvironmentVariables();
        }
        private static CollectionPeriod ParseCollectionPeriod(string input)
        {
            var match = Regex.Match(input, "^([0-9]{4})\\-(R[0-9]{2})$");
            if (!match.Success)
            {
                return _environmentVariables.CollectionPeriod;
            }

            var periodNumber = int.Parse(match.Groups[2].Value.Substring(1));
            var month = 7 + periodNumber;
            var year = int.Parse(match.Groups[1].Value.Substring(0, 2)) + 2000;
            if (month > 12)
            {
                month -= 12;
                year++;
            }

            return new CollectionPeriod
            {
                PeriodId = periodNumber,
                Period = match.Groups[2].Value,
                CalendarMonth = month,
                CalendarYear = year,
                CollectionOpen = 1,
                ActualsSchemaPeriod = $"{year}{month:00}"
            };
        }
        private static EnvironmentVariables LoadEnvironmentVariables()
        {
            var path = Path.Combine(_dataPath, "environment-variables.json");
            if (!File.Exists(path))
            {
                return new EnvironmentVariables();
            }

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<EnvironmentVariables>(json);
        }
        private static void SaveEnvironmentVariables()
        {
            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }
            var path = Path.Combine(_dataPath, "environment-variables.json");
            var json = JsonConvert.SerializeObject(_environmentVariables);
            File.WriteAllText(path, json);

            _logger.SetLogLevel(_environmentVariables.LogLevel);
        }
        private static void OutputEnvironmentVariables()
        {
            WriteLine("ENVIRONMENT VARIABLES");
            WriteLine("------------------------------");
            WriteLine($"   TransientConnectionString = {_environmentVariables.TransientConnectionString}");
            WriteLine($"   DedsDatabaseConnectionString = {_environmentVariables.DedsDatabaseConnectionString}");
            WriteLine($"   CurrentYear = {_environmentVariables.CurrentYear}");
            WriteLine($"   WorkingDirectory = {_environmentVariables.WorkingDirectory}");
            WriteLine($"   OpenCollection = {_environmentVariables.CollectionPeriod.Period}-{_environmentVariables.CollectionPeriod.CalendarMonth:00}/{_environmentVariables.CollectionPeriod.CalendarYear}");
            WriteLine();
        }

        private static void RebuildDedsDatabase(ComponentType componentType = ComponentType.AllComponents)
        {
            try
            {
                var processService = new ProcessService(_logger);

                if ((int)componentType == 99)
                {
                    var values = Enum.GetValues(typeof(ComponentType));
                    
                    foreach (ComponentType value in values)
                    {
                        if (value == ComponentType.AllComponents)
                        {
                            continue;
                        }
                        WriteLine($"Rebuilding DEDS for {value}");
                        processService.RebuildDedsDatabase(value, _environmentVariables, _statusWatcher);
                    }
                }
                else
                {
                    processService.RebuildDedsDatabase(componentType, _environmentVariables, _statusWatcher);
                }
            }
            catch (Exception ex)
            {
                WriteLine("Error!", ConsoleColor.Red);
                WriteLine(ex.ToString(), ConsoleColor.Red);
            }
            WriteLine();
        }
        private static ComponentType GetComponentType()
        {
            var selection = -1;
            while (selection < 0)
            {
                WriteLine("What component would you like to rebuild?");
                WriteLine("   1. Data Lock");
                WriteLine("   2. Levy Calculator");
                WriteLine("   3. Earnings Calculator");
                WriteLine("   4. PaymentsDue Calculator");
                WriteLine("   5. CoInvested Payments Calculator");
                WriteLine("   10. Data Lock Events");
                WriteLine("   11. Submission Events");
                WriteLine("   99. All components");
                WriteLine("   0. Cancel. Back to main menu");
                Write("Selection: ");

                var input = ReadLine();
                if (int.TryParse(input, out selection) && (selection == 99 || (selection >= 0 && selection <= 11)))
                {
                    WriteLine();
                    break;
                }

                WriteLine("Invalid selection!", ConsoleColor.Red);
                selection = -1;
            }

            return (ComponentType)selection;
        }

        private static void CleanDedsDatabase()
        {
            var tablesToExclude = new List<string>()
            {
                "AT.Logs",
                "AT.ReferenceData",
                "AT.TestRuns"
            };

            Write("Would you like to clean commitment reference data (Y/N)? ");
            if (!Console.ReadLine().Trim().Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                tablesToExclude.Add("dbo.DasCommitments");
                tablesToExclude.Add("dbo.EventStreamPointer");
                tablesToExclude.Add("dbo.DasCommitmentsHistory");
            }

            Write("Would you like to clean account reference data (Y/N)? ");
            if (!Console.ReadLine().Trim().Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                tablesToExclude.Add("dbo.DasAccounts");
                tablesToExclude.Add("dbo.DasAccountsAudit");
            }

            using (var connection = new SqlConnection(_environmentVariables.DedsDatabaseConnectionString))
            {
                connection.Open();
                try
                {
                    // Read all table names
                    var allTableNames = new List<string>();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT [TABLE_SCHEMA] + '.' + [TABLE_NAME] FullTableName FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_TYPE] = 'BASE TABLE' ORDER BY [TABLE_SCHEMA], [TABLE_NAME]";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allTableNames.Add(reader.GetString(0));
                            }
                        }
                    }

                    // Truncate tables
                    using (var command = connection.CreateCommand())
                    {
                        foreach (var tableName in allTableNames)
                        {
                            if (!tablesToExclude.Any(x => x.Equals(tableName, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                command.CommandText = $"TRUNCATE TABLE {tableName}";
                                command.ExecuteNonQuery();

                                WriteLine($"Cleared {tableName}");
                            }
                            else
                            {
                                WriteLine($"Skipped {tableName}");
                            }
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            WriteLine("Successfully cleaned DEDs", ConsoleColor.Green);
            WriteLine();
        }

        private static void RunIlrSubmissionProcess()
        {
            try
            {
                //var submission = GetIlrSubmission();
                var data = GetIlrFileData();
                if (data == null)
                {
                    WriteLine("Aborted", ConsoleColor.Yellow);
                    return;
                }

                var processService = new ProcessService(_logger);
                processService.RunIlrSubmission(data, _environmentVariables, _statusWatcher);
            }
            catch (Exception ex)
            {
                WriteLine("Error!", ConsoleColor.Red);
                WriteLine(ex.ToString(), ConsoleColor.Red);
            }
            WriteLine();
        }
        private static byte[] GetIlrFileData()
        {
            while (true)
            {
                Console.Write("ILR file path: ");
                var path = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(path))
                {
                    return null;
                }

                if (File.Exists(path))
                {
                    return File.ReadAllBytes(path);
                }

                WriteLine("File not found!", ConsoleColor.Red);
                WriteLine();
            }
        }
        private static IlrSubmission GetIlrSubmission()
        {
            return new IlrSubmission
            {
                Ukprn = 10000,
                AcademicYear = "1718",
                PreperationDate = new DateTime(2017, 9, 30),
                Learners = new[]
                {
                    new Learner
                    {
                        Uln = 11000,
                        LearningDeliveries = new[]
                        {
                            new LearningDelivery
                            {
                                Type = AimType.Programme,
                                StandardCode = 23,
                                ActualStartDate = new DateTime(2017, 9, 1),
                                PlannedEndDate = new DateTime(2018, 9, 8),
                                FamRecords = new[]
                                {
                                    new LearningDeliveryFamRecord
                                    {
                                        Code = "1",
                                        FamType = "ACT",
                                        From = new DateTime(2017, 9, 1),
                                        To = new DateTime(2018, 9, 8)
                                    }
                                },
                                FinancialRecords = new[]
                                {
                                    new FinancialRecord
                                    {
                                        Code = 1,
                                        Type = "TNP",
                                        Amount = 12000,
                                        Date = new DateTime(2017, 9, 1)
                                    },

                                    new FinancialRecord
                                    {
                                        Code = 2,
                                        Type = "TNP",
                                        Amount = 3000,
                                        Date = new DateTime(2017, 9, 1)
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private static void RunSummarisationProcess()
        {
            try
            {
                var processService = new ProcessService(_logger);
                processService.RunSummarisation(_environmentVariables, _statusWatcher);
            }
            catch (Exception ex)
            {
                WriteLine("Error!", ConsoleColor.Red);
                WriteLine(ex.ToString(), ConsoleColor.Red);
            }
            WriteLine();
        }

        private static void RunAccountsReferenceData()
        {
            try
            {
                //_environmentVariables.TransientConnectionString = "server=.;database=ProvPayTestStack_Transient;trusted_connection=true;";
                //_environmentVariables.DedsDatabaseConnectionString = "server=.;database=ProvPayTestStack_Deds;trusted_connection=true;";

                var processService = new ProcessService(_logger);
                processService.RunAccountsReferenceData(_environmentVariables, _statusWatcher);
            }
            catch (Exception ex)
            {
                WriteLine("Error!", ConsoleColor.Red);
                WriteLine(ex.ToString(), ConsoleColor.Red);
            }
            WriteLine();
        }

        private static void RunCommitmentsReferenceData()
        {
            try
            {
                //_environmentVariables.TransientConnectionString = "server=.;database=ProvPayTestStack_Transient;trusted_connection=true;";
                //_environmentVariables.DedsDatabaseConnectionString = "server=.;database=ProvPayTestStack_Deds;trusted_connection=true;";

                var processService = new ProcessService(_logger);
                processService.RunCommitmentsReferenceData(_environmentVariables, _statusWatcher);
            }
            catch (Exception ex)
            {
                WriteLine("Error!", ConsoleColor.Red);
                WriteLine(ex.ToString(), ConsoleColor.Red);
            }
            WriteLine();
        }

        private static void ExecutionStarted(TaskDescriptor[] tasks)
        {
            WriteLine("Execution started with the following tasks");
            foreach (var task in tasks)
            {
                WriteLine($"   {task.Id} - {task.Description}");
            }
        }
        private static void TaskStarted(string taskId)
        {
            WriteLine($"Starting task {taskId}");
        }
        private static void TaskCompleted(string taskId, Exception error)
        {
            if (error != null)
            {
                WriteLine($"Task {taskId} failed", ConsoleColor.Red);
                WriteLine(error.ToString(), ConsoleColor.Red);
                return;
            }

            WriteLine($"Task {taskId} completed successfully");
        }
        private static void ExecutionCompleted(Exception error)
        {
            if (error != null)
            {
                WriteLine("Execution failed", ConsoleColor.Red);
                WriteLine(error.ToString(), ConsoleColor.Red);
                return;
            }

            WriteLine($"Execution completed successfully", ConsoleColor.Green);
        }

        private static void WriteLine()
        {
            WriteLine(string.Empty, _defaultColor);
        }
        private static void WriteLine(string line)
        {
            WriteLine(line, _defaultColor);
        }
        private static void WriteLine(string line, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(line);
            Console.ForegroundColor = _defaultColor;
        }
        private static void Write(string text)
        {
            Write(text, _defaultColor);
        }
        private static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = _defaultColor;
        }
        private static string ReadLine(string defaultValue = "")
        {
            var input = Console.ReadLine();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }

        private class ProcessStatusWatcher : StatusWatcherBase
        {
            private readonly Action<TaskDescriptor[]> _executionStarted;
            private readonly Action<string> _taskStarted;
            private readonly Action<string, Exception> _taskCompleted;
            private readonly Action<Exception> _executionCompleted;

            public ProcessStatusWatcher(Action<TaskDescriptor[]> executionStarted = null, Action<string> taskStarted = null,
                Action<string, Exception> taskCompleted = null, Action<Exception> executionCompleted = null)
            {
                _executionStarted = executionStarted;
                _taskStarted = taskStarted;
                _taskCompleted = taskCompleted;
                _executionCompleted = executionCompleted;
            }

            public override void ExecutionStarted(TaskDescriptor[] tasks)
            {
                if (_executionStarted == null)
                {
                    return;
                }

                _executionStarted.Invoke(tasks);
            }
            public override void TaskStarted(string taskId)
            {
                if (_taskStarted == null)
                {
                    return;
                }

                _taskStarted.Invoke(taskId);
            }
            public override void TaskCompleted(string taskId, Exception error)
            {
                if (_taskCompleted == null)
                {
                    return;
                }

                _taskCompleted.Invoke(taskId, error);
            }
            public override void ExecutionCompleted(Exception error)
            {
                if (_executionCompleted == null)
                {
                    return;
                }

                _executionCompleted.Invoke(error);
            }
        }
    }
}
