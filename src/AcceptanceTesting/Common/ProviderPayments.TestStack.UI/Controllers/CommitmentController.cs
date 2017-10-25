using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
using ProviderPayments.TestStack.Application;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Mapping;
using ProviderPayments.TestStack.UI.Models;

namespace ProviderPayments.TestStack.UI.Controllers
{
    public class CommitmentController : ControllerBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICommitmentService _commitmentService;
        private readonly IAccountService _accountService;
        private readonly IProviderService _providerService;
        private readonly ILearnerService _learnerService;
        private readonly ITrainingService _trainingService;
        private readonly IMapper _mapper;

        public CommitmentController(ICommitmentService commitmentService,
                                    IAccountService accountService,
                                    IProviderService providerService,
                                    ILearnerService learnerService,
                                    ITrainingService trainingService,
                                    IMapper mapper)
            : base(Logger)
        {
            _commitmentService = commitmentService;
            _accountService = accountService;
            _providerService = providerService;
            _learnerService = learnerService;
            _trainingService = trainingService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var commitments = await _commitmentService.GetAllCommitments();

            return View(commitments);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new CommitmentModel
            {
                Id = CreateNuid(),
                Version = 1,
                StartDate = new DateTime(2017, 4, 1),
                EndDate = new DateTime(2018, 5, 1),
                EffectiveFrom = new DateTime(2017, 4, 1)
            };

            await PopulateCommitmentModelReferenceData(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CommitmentModel model)
        {
            try
            {
                _logger.Info("Commitment creation requested");

                var commitment = _mapper.Map<Commitment>(model);
                var courseParts = model.SelectedCourse.Split('-');
                if (courseParts.Length == 3)
                {
                    commitment.Course = new Course
                    {
                        PathwayCode = int.Parse(courseParts[0]),
                        FrameworkCode = int.Parse(courseParts[1]),
                        ProgrammeType = int.Parse(courseParts[2])
                    };
                }
                else
                {
                    commitment.Course = new Course
                    {
                        StandardCode = long.Parse(courseParts[0])
                    };
                }


                _logger.Info($"Attempting to create commitment StandardCode={commitment.Course.StandardCode}, PathwayCode={commitment.Course.PathwayCode}, FrameworkCode={commitment.Course.FrameworkCode}, ProgrammeType={commitment.Course.ProgrammeType}");
                await _commitmentService.CreateCommitment(commitment);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            await PopulateCommitmentModelReferenceData(model);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long id, long version)
        {
            var commitment = await _commitmentService.GetCommitmentByPk(id, version);
            if (commitment == null)
            {
                return HttpNotFound();
            }

            var model = _mapper.Map<CommitmentModel>(commitment);
            await PopulateCommitmentModelReferenceData(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, CommitmentModel model)
        {
            try
            {
                _logger.Info($"Update commitment {id} requested");

                var commitment = ConvertPostedCommitmentModel(model);
                commitment.Id = id;

                _logger.Info($"Attempting to update commitment {id} StandardCode={commitment.Course.StandardCode}, PathwayCode={commitment.Course.PathwayCode}, FrameworkCode={commitment.Course.FrameworkCode}, ProgrammeType={commitment.Course.ProgrammeType}");
                await _commitmentService.UpdateCommitment(commitment);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            await PopulateCommitmentModelReferenceData(model);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long commitmentId, long versionId)
        {
            try
            {
                _logger.Info($"Delete commitment {commitmentId}, {versionId} requested");

                _logger.Info($"Attempting to delete commitment {commitmentId}, {versionId}");
                await _commitmentService.DeleteCommitment(commitmentId, versionId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }



        private string CreateNuid()
        {
            Random rnd = new Random();
            return  rnd.Next(int.MaxValue).ToString();
            
        }
        private async Task PopulateCommitmentModelReferenceData(CommitmentModel model)
        {
            var accountsTask = _accountService.GetAllAccounts();
            var providersTask = _providerService.GetAllProviders();
            var learnersTask = _learnerService.GetAllLearners();
            var standardsTask = _trainingService.GetAllStandards();
            var frameworksTask = _trainingService.GetAllFrameworks();

            await WhenAllSucceed(accountsTask, providersTask, learnersTask, standardsTask, frameworksTask);

            model.Accounts = accountsTask.Result.ToArray();
            model.Providers = providersTask.Result.ToArray();
            model.Learners = learnersTask.Result.ToArray();
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
        private Commitment ConvertPostedCommitmentModel(CommitmentModel model)
        {
            var commitment = _mapper.Map<Commitment>(model);
            var courseParts = model.SelectedCourse.Split('-');
            if (courseParts.Length == 3)
            {
                commitment.Course = new Course
                {
                    PathwayCode = int.Parse(courseParts[0]),
                    FrameworkCode = int.Parse(courseParts[1]),
                    ProgrammeType = int.Parse(courseParts[2])
                };
            }
            else
            {
                commitment.Course = new Course
                {
                    StandardCode = long.Parse(courseParts[0])
                };
            }
            return commitment;
        }
    }
}