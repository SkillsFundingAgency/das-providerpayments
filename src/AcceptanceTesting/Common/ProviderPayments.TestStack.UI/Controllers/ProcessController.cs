using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using ProviderPayments.TestStack.Application;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Mapping;
using ProviderPayments.TestStack.UI.Models;

namespace ProviderPayments.TestStack.UI.Controllers
{
    public class ProcessController : ControllerBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IProcessService _processService;
        private readonly IProviderService _providerService;
        private readonly ITrainingService _trainingService;
        private readonly IMapper _mapper;

        public ProcessController(IProcessService processService,
                                 IProviderService providerService,
                                 ITrainingService trainingService,
                                 IMapper mapper)
            : base(Logger)
        {
            _processService = processService;
            _providerService = providerService;
            _trainingService = trainingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> SubmitIlr()
        {
            var model = new IlrSubmissionModel
            {
                YearOfCollection = "1617"
            };

            await PopulateIlrSubmissionModelReferenceData(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitIlr(string button, IlrSubmissionModel ilrModel, IlrLearnerModel learnerModel)
        {
            if (button == "add")
            {
                AddLearnerToSubmission(learnerModel, ilrModel);

                await PopulateIlrSubmissionModelReferenceData(ilrModel);

                return View(ilrModel);
            }
            if (button.StartsWith("remove-"))
            {
                var idx = int.Parse(button.Split('-')[1]);

                ilrModel.Learners?.RemoveAt(idx);

                await PopulateIlrSubmissionModelReferenceData(ilrModel);

                return View(ilrModel);
            }


            AddLearnerToSubmission(learnerModel, ilrModel);

            var submission = _mapper.Map<IlrSubmission>(ilrModel);

            var submissionId = await _processService.SubmitIlr(submission);
            _logger.Info($"Submitted ILR with id {submissionId}");

            return RedirectToAction("SubmissionStatus", new { id = submissionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadIlr(HttpPostedFileBase ilrFile, string yearOfCollection)
        {
            var file = await ilrFile.InputStream.ReadAllBytesAsync();

            var submissionId = await _processService.UploadIlr(file, yearOfCollection);
            _logger.Info($"Submitted ILR (via upload) with id {submissionId}");

            return RedirectToAction("SubmissionStatus", new { id = submissionId });
        }

        [HttpGet]
        public ActionResult RunSummarisation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RunSummarisation(RunSummarisationModel model)
        {
            var collectionPeriod = (CollectionPeriod)model;
            collectionPeriod.CollectionOpen = 1;
            PopulateCollectionDataFromAcademicYear(collectionPeriod, model.YearOfCollection);

            var runId = await _processService.RunSummarisation(collectionPeriod);
            _logger.Info($"Requested summariation run with id {runId}");

            return RedirectToAction("SubmissionStatus", new { id = runId });
        }


        [HttpGet]
        public async Task<ActionResult> SubmissionStatus(string id)
        {
            var status = await _processService.GetProcessStatus(id);
            var model = new SubmissionStatusModel
            {
                Status = status
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult RunAccountsReferenceData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RunAccountsReferenceData(RunAccountsReferenceDataModel model)
        {
            var runId = await _processService.RunAccountsReferenceData();
            _logger.Info($"Requested accounts reference data run with id {runId}");

            return RedirectToAction("SubmissionStatus", new { id = runId });
        }

        [HttpGet]
        public ActionResult RunCommitmentsReferenceData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RunCommitmentsReferenceData(RunCommitmentsReferenceDataModel model)
        {
            var runId = await _processService.RunCommitmentsReferenceData();
            _logger.Info($"Requested commitments reference data run with id {runId}");

            return RedirectToAction("SubmissionStatus", new { id = runId });
        }

        private void PopulateCollectionDataFromAcademicYear(CollectionPeriod collectionPeriod, string year)
        {
            var startYear = 2000 + int.Parse(year.Substring(0, 2));
            var endYear = 2000 + int.Parse(year.Substring(2, 2));

            switch (collectionPeriod.Period)
            {
                case "R01":
                    collectionPeriod.PeriodId = 1;
                    collectionPeriod.CalendarMonth = 8;
                    collectionPeriod.CalendarYear = startYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{startYear}08";
                    break;
                case "R02":
                    collectionPeriod.PeriodId = 2;
                    collectionPeriod.CalendarMonth = 9;
                    collectionPeriod.CalendarYear = startYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{startYear}09";
                    break;
                case "R03":
                    collectionPeriod.PeriodId = 3;
                    collectionPeriod.CalendarMonth = 10;
                    collectionPeriod.CalendarYear = startYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{startYear}10";
                    break;
                case "R04":
                    collectionPeriod.PeriodId = 4;
                    collectionPeriod.CalendarMonth = 11;
                    collectionPeriod.CalendarYear = startYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{startYear}11";
                    break;
                case "R05":
                    collectionPeriod.PeriodId = 5;
                    collectionPeriod.CalendarMonth = 12;
                    collectionPeriod.CalendarYear = startYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{startYear}12";
                    break;
                case "R06":
                    collectionPeriod.PeriodId = 6;
                    collectionPeriod.CalendarMonth = 1;
                    collectionPeriod.CalendarYear = endYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{endYear}01";
                    break;
                case "R07":
                    collectionPeriod.PeriodId = 7;
                    collectionPeriod.CalendarMonth = 2;
                    collectionPeriod.CalendarYear = endYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{endYear}02";
                    break;
                case "R08":
                    collectionPeriod.PeriodId = 8;
                    collectionPeriod.CalendarMonth = 3;
                    collectionPeriod.CalendarYear = endYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{endYear}03";
                    break;
                case "R09":
                    collectionPeriod.PeriodId = 9;
                    collectionPeriod.CalendarMonth = 4;
                    collectionPeriod.CalendarYear = endYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{endYear}04";
                    break;
                case "R10":
                    collectionPeriod.PeriodId = 10;
                    collectionPeriod.CalendarMonth = 5;
                    collectionPeriod.CalendarYear = endYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{endYear}05";
                    break;
                case "R11":
                    collectionPeriod.PeriodId = 11;
                    collectionPeriod.CalendarMonth = 6;
                    collectionPeriod.CalendarYear = endYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{endYear}06";
                    break;
                case "R12":
                    collectionPeriod.PeriodId = 12;
                    collectionPeriod.CalendarMonth = 7;
                    collectionPeriod.CalendarYear = endYear;
                    collectionPeriod.ActualsSchemaPeriod = $"{endYear}07";
                    break;
            }
        }
        private void AddLearnerToSubmission(IlrLearnerModel model, IlrSubmissionModel ilrModel)
        {
            if (model.Uln != 0 && model.ActualStartDate > DateTime.MinValue && model.PlannedEndDate > DateTime.MinValue)
            {
                var learners = ilrModel.Learners ?? new List<IlrLearnerModel>();
                learners.Add(model);

                ilrModel.Learners = learners;
            }
        }

        private async Task PopulateIlrSubmissionModelReferenceData(IlrSubmissionModel model)
        {
            var providersTask = _providerService.GetAllProviders();
            var standardsTask = _trainingService.GetAllStandards();
            var frameworksTask = _trainingService.GetAllFrameworks();

            await WhenAllSucceed(standardsTask, frameworksTask);

            model.Providers = providersTask.Result.ToArray();
            model.Standards = standardsTask.Result.ToArray();
            model.Frameworks = frameworksTask.Result.ToArray();
        }
        private async Task WhenAllSucceed(params Task[] tasks)
        {
            await Task.WhenAll(tasks);

            var exceptions = new List<Exception>();
            foreach (var task in tasks)
            {
                if (task.Exception != null)
                {
                    exceptions.Add(task.Exception);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}