﻿using System;
using System.Data.SqlClient;
using Dapper;
using ProviderPayments.TestStack.Core;
using ProviderPayments.TestStack.Core.ExecutionStatus;
using ProviderPayments.TestStack.Core.Workflow;

namespace SFA.DAS.Payments.AcceptanceTests.ExecutionManagers
{
    internal static class PreparationManager
    {
        internal static void PrepareDatabasesForTestRun()
        {
            PrepareDatabaseForAt();
            PrepareDatabaseForAllComponents();
            //TestEnvironment.ProcessService.PrepareTablesNotOwnedByPayments(TestEnvironment.Variables);
        }

        internal static void PrepareDatabasesForScenario()
        {
            DdlManager.ResetHasRanFlags();

            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                connection.Execute("TRUNCATE TABLE Adjustments.ManualAdjustments");

                connection.Execute("TRUNCATE TABLE AT.ReferenceData");

                connection.Execute("TRUNCATE TABLE DataLock.DataLockEventCommitmentVersions");
                connection.Execute("TRUNCATE TABLE DataLock.DataLockEventErrors");
                connection.Execute("TRUNCATE TABLE DataLock.DataLockEventPeriods");
                connection.Execute("TRUNCATE TABLE DataLock.DataLockEvents");

                connection.Execute("TRUNCATE TABLE DataLock.PriceEpisodeMatch");
                connection.Execute("TRUNCATE TABLE DataLock.PriceEpisodePeriodMatch");
                connection.Execute("TRUNCATE TABLE DataLock.ValidationError");
                connection.Execute("TRUNCATE TABLE Datalock.ValidationErrorByPeriod");

                connection.Execute("DELETE FROM dbo.AEC_EarningHistory");
                connection.Execute("DELETE FROM dbo.Collection_Period_Mapping");
                connection.Execute("DELETE FROM dbo.DasAccounts");
                connection.Execute("DELETE FROM dbo.DasCommitments");
                connection.Execute("DELETE FROM dbo.DasCommitmentsHistory");
                connection.Execute("DELETE FROM dbo.EAS_Submission");
                connection.Execute("DELETE FROM dbo.EAS_Submission_Values");
                connection.Execute("DELETE FROM dbo.FileDetails");
                //connection.Execute("DELETE FROM dbo.Payment_Types");

                connection.Execute("TRUNCATE TABLE Payments.Payments");
                connection.Execute("TRUNCATE TABLE Payments.Periods");
                connection.Execute("TRUNCATE TABLE PaymentsDue.Earnings");
                connection.Execute("TRUNCATE TABLE PaymentsDue.NonPayableEarnings");
                connection.Execute("TRUNCATE TABLE PaymentsDue.RequiredPayments");

                connection.Execute("TRUNCATE TABLE ProviderAdjustments.Payments");

                connection.Execute("TRUNCATE TABLE Rulebase.AEC_ApprenticeshipPriceEpisode");
                connection.Execute("TRUNCATE TABLE Rulebase.AEC_ApprenticeshipPriceEpisode_Period");
                connection.Execute("TRUNCATE TABLE Rulebase.AEC_ApprenticeshipPriceEpisode_PeriodisedValues");

                connection.Execute("TRUNCATE TABLE Rulebase.AEC_Cases");
                connection.Execute("TRUNCATE TABLE Rulebase.AEC_global");
                connection.Execute("TRUNCATE TABLE Rulebase.AEC_HistoricEarningOutput");

                connection.Execute("TRUNCATE TABLE Rulebase.AEC_LearningDelivery");
                connection.Execute("TRUNCATE TABLE Rulebase.AEC_LearningDelivery_Period");
                connection.Execute("TRUNCATE TABLE Rulebase.AEC_LearningDelivery_PeriodisedTextValues");
                connection.Execute("TRUNCATE TABLE Rulebase.AEC_LearningDelivery_PeriodisedValues");

                connection.Execute("TRUNCATE TABLE Submissions.LastSeenVersion");
                connection.Execute("TRUNCATE TABLE Submissions.SubmissionEvents");

                connection.Execute("TRUNCATE TABLE TransferPayments.AccountTransfers");


                connection.Execute("TRUNCATE TABLE Valid.Learner");
                connection.Execute("TRUNCATE TABLE Valid.LearningDelivery");
                connection.Execute("TRUNCATE TABLE Valid.LearningDeliveryFAM");
                connection.Execute("TRUNCATE TABLE Valid.LearningProvider");

                if (TestEnvironment.Variables.OpaRulebaseYear == "1617")
                {
                    connection.Execute("TRUNCATE TABLE Valid.TrailblazerApprenticeshipFinancialRecord");
                }
                else
                {
                    connection.Execute("TRUNCATE TABLE Valid.AppFinRecord");
                }
            }
        }

        private static void PrepareDatabaseForAt()
        {
            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                connection.ExecuteScript(Properties.Resources.ddl_AT_deds_tables);
            }
        }

        private static void PrepareDatabaseForAllComponents()
        {
            var watcher = new RebuildStatusWatcher();

            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.ReferenceCommitments, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.ReferenceAccounts, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.DataLockSubmission, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.DataLockPeriodEnd, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.EarningsCalculator, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.LevyCalculator, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.PaymentsDue, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.Refunds, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.CoInvestedPayments, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.PeriodEndScripts, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.DataLockEvents, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.SubmissionEvents, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.ManualAdjustments, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.ProviderAdjustments, TestEnvironment.Variables, watcher);
            PrepareDatabaseForComponent(TestEnvironment.ProcessService, ComponentType.TransferPayments, TestEnvironment.Variables, watcher);

            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                connection.ExecuteScript(Properties.Resources.PeriodEnd_Deds_ProviderAdjustments_ddl_tables);
                connection.ExecuteScript(Properties.Resources.EAS_Deds_PaymentTypes_dml);
            }
        }

        private static void PrepareDatabaseForComponent(ProcessService processService, ComponentType componentType, EnvironmentVariables environmentVariables, RebuildStatusWatcher watcher)
        {
            processService.RebuildDedsDatabase(componentType, environmentVariables, watcher);
            if (watcher.LastError != null)
            {
                throw new Exception($"Error rebuilding deds for {componentType} - {watcher.LastError.Message}", watcher.LastError);
            }
        }
        
        private class RebuildStatusWatcher : StatusWatcherBase
        {
            public Exception LastError { get; private set; }

            public override void ExecutionStarted(TaskDescriptor[] tasks)
            {
                TestEnvironment.Logger.Info("Started execution of rebuild");
                LastError = null;
            }
            public override void TaskStarted(string taskId)
            {
                TestEnvironment.Logger.Info($"Task {taskId} started");
            }
            public override void TaskCompleted(string taskId, Exception error)
            {
                if (error != null)
                {
                    TestEnvironment.Logger.Error(error, $"Task {taskId} failed");
                    if (LastError == null)
                    {
                        LastError = error;
                    }
                }
                else
                {
                    TestEnvironment.Logger.Info($"Task {taskId} succeeded");
                }
            }

            public override void ExecutionCompleted(Exception error)
            {
                if (error != null)
                {
                    TestEnvironment.Logger.Error(error, "Execution of rebuild failed");
                    if (LastError == null)
                    {
                        LastError = error;
                    }
                }
                else
                {
                    TestEnvironment.Logger.Info("Execution of rebuild succeeded");
                }
            }
        }
    }
}
