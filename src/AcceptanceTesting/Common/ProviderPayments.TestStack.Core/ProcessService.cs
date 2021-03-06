﻿using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Dapper;
using IlrGenerator;
using Newtonsoft.Json;
using ProviderPayments.TestStack.Core.Context;
using ProviderPayments.TestStack.Core.ExecutionStatus;
using ProviderPayments.TestStack.Core.Properties;
using ProviderPayments.TestStack.Core.Workflow.AccountsReferenceData;
using ProviderPayments.TestStack.Core.Workflow.CommitmentsReferenceData;
using ProviderPayments.TestStack.Core.Workflow.IlrSubmission;
using ProviderPayments.TestStack.Core.Workflow.RebuildDedsDatabase;
using ProviderPayments.TestStack.Core.Workflow.Summarisation;

namespace ProviderPayments.TestStack.Core
{
    public class ProcessService
    {
        private readonly ILogger _logger;

        public ProcessService(ILogger logger)
        {
            _logger = logger;
        }

        public void PrepareTablesNotOwnedByPayments(EnvironmentVariables environmentVariables = null)
        {
            var context = SetupExecutionEnvironment(null, environmentVariables ?? new EnvironmentVariables());
            var connectionString = context.Properties[KnownContextKeys.DedsDatabaseConnectionString];
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    var commands = Regex.Split(Resources.CopyValidLearnerRecordsTaskScript, @"GO\s*(\n|$|\r\n)", RegexOptions.IgnoreCase);
                    
                    foreach (var command in commands)
                    {
                        if (string.IsNullOrWhiteSpace(command))
                        {
                            continue;
                        }
                        try
                        {
                            connection.Execute(command);
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Error with command: {command}", e);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void RunIlrSubmission(IlrSubmission submission, EnvironmentVariables environmentVariables = null, StatusWatcherBase statusWatcher = null)
        {
            var context = SetupExecutionEnvironment(JsonConvert.SerializeObject(submission), environmentVariables ?? new EnvironmentVariables());
            context.DataLockEventsSource = "Submission";

            var workflow = new IlrSubmissionWorkflow(_logger);
            workflow.Execute(context, statusWatcher ?? new NullStatusWatcher());
        }
        public void RunIlrSubmission(byte[] ilrFileData, EnvironmentVariables environmentVariables = null, StatusWatcherBase statusWatcher = null)
        {
            var context = SetupExecutionEnvironment(Convert.ToBase64String(ilrFileData), environmentVariables ?? new EnvironmentVariables());
            context.DataLockEventsSource = "Submission";
            context.SubmissionIsIlrFile = true;

            var workflow = new IlrSubmissionWorkflow(_logger);
            workflow.Execute(context, statusWatcher ?? new NullStatusWatcher());
        }

        public void RunSummarisation(EnvironmentVariables environmentVariables = null, StatusWatcherBase statusWatcher = null)
        {
            var context = SetupExecutionEnvironment(string.Empty, environmentVariables ?? new EnvironmentVariables());
            context.DataLockEventsSource = "PeriodEnd";

            var workflow = new SummarisationWorkflow(_logger);
            workflow.Execute(context, statusWatcher ?? new NullStatusWatcher());
        }

        public void RunPrepareForEas(EnvironmentVariables environmentVariables = null,
            StatusWatcherBase statusWatcher = null)
        {
            var context = SetupExecutionEnvironment(string.Empty, environmentVariables ?? new EnvironmentVariables());
            context.DataLockEventsSource = "PeriodEnd";
            
            var workflow = new PrepareForEasWorkflow(_logger);
            workflow.Execute(context, statusWatcher ?? new NullStatusWatcher());
        }

        public void RunCleanupDeds(EnvironmentVariables environmentVariables = null,
            StatusWatcherBase statusWatcher = null)
        {
            var context = SetupExecutionEnvironment(string.Empty, environmentVariables ?? new EnvironmentVariables());
            context.DataLockEventsSource = "PeriodEnd";

            var workflow = new CleanupDedsWorkflow(_logger);
            workflow.Execute(context, statusWatcher ?? new NullStatusWatcher());
        }

        public void RebuildDedsDatabase(ComponentType componentType, EnvironmentVariables environmentVariables = null, StatusWatcherBase statusWatcher = null)
        {
            var context = SetupExecutionEnvironment(((int)componentType).ToString(), environmentVariables ?? new EnvironmentVariables());

            var workflow = new RebuildDedsDatabaseWorkflow(_logger);
            workflow.Execute(context, statusWatcher ?? new NullStatusWatcher());
        }

        public void RunAccountsReferenceData(EnvironmentVariables environmentVariables = null, StatusWatcherBase statusWatcher = null)
        {
            var context = SetupExecutionEnvironment(string.Empty, environmentVariables ?? new EnvironmentVariables());

            var workflow = new AccountsReferenceDataWorkflow(_logger);
            workflow.Execute(context, statusWatcher ?? new NullStatusWatcher());
        }

        public void RunCommitmentsReferenceData(EnvironmentVariables environmentVariables = null, StatusWatcherBase statusWatcher = null)
        {
            var context = SetupExecutionEnvironment(string.Empty, environmentVariables ?? new EnvironmentVariables());

            var workflow = new CommitmentsReferenceDataWorkflow(_logger);
            workflow.Execute(context, statusWatcher ?? new NullStatusWatcher());
        }

        private TestStackContext SetupExecutionEnvironment(string requestContent, EnvironmentVariables environmentVariables)
        {
            return new TestStackContext
            {
                TransientConnectionString = environmentVariables.TransientConnectionString,
                DedsDatabaseConnectionString = environmentVariables.DedsDatabaseConnectionString,
                LogLevel = environmentVariables.LogLevel,
                CurrentYear = environmentVariables.CurrentYear,
                OpaRulebaseYear =environmentVariables.OpaRulebaseYear,

                RequestContent = requestContent,
                WorkingDirectory = environmentVariables.WorkingDirectory,
                IlrFileDirectory = environmentVariables.IlrFileDirectory,
                CollectionPeriod = JsonConvert.SerializeObject(environmentVariables.CollectionPeriod),

                IlrAimRefLookups = JsonConvert.SerializeObject(environmentVariables.IlrAimRefLookups),

                CommitmentApiBaseUrl = environmentVariables.CommitmentApiBaseUrl,
                CommitmentApiClientToken = environmentVariables.CommitmentApiClientToken,

                AccountsApiBaseUrl = environmentVariables.AccountsApiBaseUrl,
                AccountsApiClientToken = environmentVariables.AccountsApiClientToken,
                AccountsApiClientId = environmentVariables.AccountsApiClientId,
                AccountsApiClientSecret = environmentVariables.AccountsApiClientSecret,
                AccountsApiIdentifierUri = environmentVariables.AccountsApiIdentifierUri,
                AccountsApiTenant = environmentVariables.AccountsApiTenant
            };
        }
    }
}
