using System;
using System.IO;
using System.Linq;
using Ionic.Zip;
using Microsoft.Azure;
using Newtonsoft.Json;
using NLog;
using ProviderPayments.TestStack.Application;
using ProviderPayments.TestStack.Domain;

namespace ProviderPayments.TestStack.Engine
{
    public interface IRequestProcessor
    {
        void ProcessRequest(ProcessType type, string content);
    }

    public class RequestProcessor : IRequestProcessor
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IComponentService _componentService;

        public RequestProcessor(IComponentService componentService)
        {
            _componentService = componentService;
        }

        public void ProcessRequest(ProcessType type, string content)
        {
            var tempDirectory = CreateTemporaryDirectory();

            try
            {
                ExtractRequiredComponents(type, tempDirectory);

                Execute(type, content, tempDirectory);
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDirectory, true);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, $"Failed to delete temp directory {tempDirectory} - {ex.Message}");
                }
            }
        }
        
        private string CreateTemporaryDirectory()
        {
            var path = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), DateTime.UtcNow.ToString("HHmmssfffff"));
            Logger.Debug($"Temp directory is {path}");
            if (!Directory.Exists(path))
            {
                Logger.Debug($"Creating temp directory {path}");
                Directory.CreateDirectory(path);
            }
            return path;
        }

        private void ExtractRequiredComponents(ProcessType processType, string tempDirectory)
        {
            Logger.Debug($"Extracting components for {processType}");

            var extractDirectory = Path.Combine(tempDirectory, "components");
          
            if (!Directory.Exists(extractDirectory))
            {
                Logger.Debug($"Creating component directory {extractDirectory}");
                Directory.CreateDirectory(extractDirectory);
            }

            switch (processType)
            {
                case ProcessType.SubmitIlr:
                case ProcessType.UploadIlr:
                    ExtractComponent(ComponentType.DataLockSubmission, extractDirectory);
                    ExtractComponent(ComponentType.EarningsCalculator, extractDirectory);
                    break;
                case ProcessType.RunSummarisation:
                    ExtractComponent(ComponentType.DataLockPeriodEnd, extractDirectory);
                    ExtractComponent(ComponentType.PaymentsDue, extractDirectory);
                    ExtractComponent(ComponentType.LevyCalculator, extractDirectory);
                    ExtractComponent(ComponentType.CoInvestedPaymentsCalculator, extractDirectory);
                    ExtractComponent(ComponentType.PeriodEndScripts, extractDirectory);
                    break;
                case ProcessType.RebuildDedsDatabase:
                    //TODO: Maybe just do the component requested
                    ExtractComponent(ComponentType.DataLockSubmission, extractDirectory);
                    ExtractComponent(ComponentType.EarningsCalculator, extractDirectory);
                    ExtractComponent(ComponentType.PaymentsDue, extractDirectory);
                    ExtractComponent(ComponentType.LevyCalculator, extractDirectory);
                    ExtractComponent(ComponentType.CoInvestedPaymentsCalculator, extractDirectory);
                    ExtractComponent(ComponentType.ReferenceAccounts, extractDirectory);
                    ExtractComponent(ComponentType.ReferenceCommitments, extractDirectory);
                    ExtractComponent(ComponentType.DataLockPeriodEnd, extractDirectory);
                    ExtractComponent(ComponentType.PeriodEndScripts, extractDirectory);
                    break;
                case ProcessType.RunAccountsReferenceData:
                    ExtractComponent(ComponentType.ReferenceAccounts, extractDirectory);
                    break;
                case ProcessType.RunCommitmentsReferenceData:
                    ExtractComponent(ComponentType.ReferenceCommitments, extractDirectory);
                    break;
            }
        }

        private void ExtractComponent(ComponentType componentType, string extractDirectory)
        {
            Logger.Debug($"Extracting files for component {componentType} to {extractDirectory}");

            var component = _componentService.GetComponent(componentType).Result;

            if (component == null)
            {
                return;
            }

            using (var stream = new MemoryStream(component.Data))
            using (var zipFile = ZipFile.Read(stream))
            {
                zipFile.ExtractAll(extractDirectory);
            }
        }

        private void Execute(ProcessType processType, string content, string tempDirectory)
        {
            var processService = new Core.ProcessService(new CoreLoggerNLogAdapter(Logger));
            var environmentVariables = GetEnvironmentVariables(tempDirectory);

            if (processType == ProcessType.SubmitIlr)
            {
                var id = GetIlrSubmissionId(content, processType);
                Logger.Info($"Executing request {id} of type {processType}");

                var submission = GetSubmission(content);

                environmentVariables.CurrentYear = GetYearOfCollection(content, processType);

                var statusWatcher = new RequestStatusWatcher(id);
                processService.RunIlrSubmission(submission, environmentVariables, statusWatcher);
            }
            else if (processType == ProcessType.UploadIlr)
            {
                var id = GetIlrSubmissionId(content, processType);
                Logger.Info($"Executing request {id} of type {processType}");

                var ilrFile = GetIlrFileData(content);
                environmentVariables.CurrentYear = GetYearOfCollection(content, processType);

                var statusWatcher = new RequestStatusWatcher(id);
                processService.RunIlrSubmission(ilrFile, environmentVariables, statusWatcher);
            }
            else if (processType == ProcessType.RunSummarisation)
            {
                var id = GetSummarisationRunId(content);
                Logger.Info($"Executing request {id} of type {processType}");

                environmentVariables.CollectionPeriod = GetCollectionPeriod(content);
                environmentVariables.CurrentYear = GetYearOfCollection(content, processType);

                var statusWatcher = new RequestStatusWatcher(id);
                processService.RunSummarisation(environmentVariables, statusWatcher);
            }
            else if (processType == ProcessType.RebuildDedsDatabase)
            {
                var id = GetDedsRebuildId(content);
                Logger.Info($"Executing request {id} of type {processType}");

                var componentType = GetComponentType(content);

                var statusWatcher = new RequestStatusWatcher(id);
                processService.RebuildDedsDatabase(componentType, environmentVariables, statusWatcher);
            }
            else if (processType == ProcessType.RunAccountsReferenceData)
            {
                var id = GetRunReferenceDataComponentRequestId(content);
                Logger.Info($"Executing request {id} of type {processType}");

                var statusWatcher = new RequestStatusWatcher(id);
                processService.RunAccountsReferenceData(environmentVariables, statusWatcher);
            }
            else if (processType == ProcessType.RunCommitmentsReferenceData)
            {
                var id = GetRunReferenceDataComponentRequestId(content);
                Logger.Info($"Executing request {id} of type {processType}");

                var statusWatcher = new RequestStatusWatcher(id);
                processService.RunCommitmentsReferenceData(environmentVariables, statusWatcher);
            }
        }

        private Core.EnvironmentVariables GetEnvironmentVariables(string tempDirectory)
        {
            return new Core.EnvironmentVariables
            {
                TransientConnectionString = CloudConfigurationManager.GetSetting("TransientConnectionString"),
                DedsDatabaseConnectionString = CloudConfigurationManager.GetSetting("DedsConnectionString"),
                LogLevel = CloudConfigurationManager.GetSetting("LogLevel"),
                WorkingDirectory = tempDirectory,
                CommitmentApiBaseUrl = CloudConfigurationManager.GetSetting("CommitmentApiBaseUrl"),
                CommitmentApiClientToken = CloudConfigurationManager.GetSetting("CommitmentApiClientToken"),
                AccountsApiBaseUrl = CloudConfigurationManager.GetSetting("AccountsApiBaseUrl"),
                AccountsApiClientToken = CloudConfigurationManager.GetSetting("AccountsApiClientToken"),
                AccountsApiClientId = CloudConfigurationManager.GetSetting("AccountsApiClientId"),
                AccountsApiClientSecret = CloudConfigurationManager.GetSetting("AccountsApiClientSecret"),
                AccountsApiIdentifierUri = CloudConfigurationManager.GetSetting("AccountsApiIdentifierUri"),
                AccountsApiTenant = CloudConfigurationManager.GetSetting("AccountsApiTenant")
            };
        }

        private IlrGenerator.IlrSubmission GetSubmission(string content)
        {
            var innerSubmission = JsonConvert.DeserializeObject<IlrSubmission>(content);

            return new IlrGenerator.IlrSubmission
            {
                Ukprn = innerSubmission.Ukprn,
                Learners = innerSubmission.Learners.Select(l =>
                    new IlrGenerator.Learner
                    {
                        Uln = l.Uln,
                        LearningDeliveries = new[]
                        {
                            new IlrGenerator.LearningDelivery
                            {
                                StandardCode = l.StandardCode,
                                FrameworkCode = l.FrameworkCode,
                                ProgrammeType = l.ProgrammeType,
                                PathwayCode = l.PathwayCode,
                                ActualStartDate = l.ActualStartDate,
                                PlannedEndDate = l.PlannedEndDate,
                                ActualEndDate =
                                    l.ActualEndDate < new DateTime(2016, 1, 1) ? null : (DateTime?) l.ActualEndDate,
                                TrainingCost = l.TrainingCost,
                                EndpointAssesmentCost = l.EndpointAssementCost,
                                ActFamCodeValue = l.FamCode
                            }
                        }
                    }).ToArray()
            };
        }

        private byte[] GetIlrFileData(string content)
        {
            var uploadRequest = JsonConvert.DeserializeObject<UploadIlrRequest>(content);
            return uploadRequest.Data;
        }

        private Core.Domain.CollectionPeriod GetCollectionPeriod(string content)
        {
            var period = JsonConvert.DeserializeObject<CollectionPeriod>(content);
            return new Core.Domain.CollectionPeriod
            {
                PeriodId = period.PeriodId,
                Period = period.Period,
                CalendarMonth = period.CalendarMonth,
                CalendarYear = period.CalendarYear,
                CollectionOpen = period.CollectionOpen,
                ActualsSchemaPeriod = period.ActualsSchemaPeriod
            };
        }

        private string GetYearOfCollection(string content, ProcessType processType)
        {
            if (processType == ProcessType.UploadIlr)
            {
                var uploadRequest = JsonConvert.DeserializeObject<UploadIlrRequest>(content);
                return uploadRequest.YearOfCollection;
            }

            if (processType == ProcessType.RunSummarisation)
            {
                var period = JsonConvert.DeserializeObject<CollectionPeriod>(content);
                return period.YearOfCollection;
            }

            var innerSubmission = JsonConvert.DeserializeObject<IlrSubmission>(content);
            return innerSubmission.YearOfCollection;
        }

        private Core.ComponentType GetComponentType(string content)
        {
            var request = JsonConvert.DeserializeObject<RebuildDedsForComponentRequest>(content);
            return (Core.ComponentType)((int)request.ComponentType);
        }

        private string GetIlrSubmissionId(string content, ProcessType processType)
        {
            if (processType == ProcessType.UploadIlr)
            {
                var uploadRequest = JsonConvert.DeserializeObject<UploadIlrRequest>(content);
                return uploadRequest.Id;
            }

            var innerSubmission = JsonConvert.DeserializeObject<IlrSubmission>(content);
            return innerSubmission.Id;
        }

        private string GetSummarisationRunId(string content)
        {
            var summarisationCollectionPeriod = JsonConvert.DeserializeObject<CollectionPeriod>(content);
            return summarisationCollectionPeriod.Id;
        }

        private string GetDedsRebuildId(string content)
        {
            var request = JsonConvert.DeserializeObject<RebuildDedsForComponentRequest>(content);
            return request.Id;
        }

        private string GetRunReferenceDataComponentRequestId(string content)
        {
            var request = JsonConvert.DeserializeObject<RunReferenceDataComponentRequest>(content);

            return request.Id;
        }
    }
}
