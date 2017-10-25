using SFA.DAS.Payments.Automation.WebUI.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.IO.Compression;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs;
using SFA.DAS.Payments.Automation.WebUI.ViewModels;

namespace SFA.DAS.Payments.Automation.WebUI.Controllers
{
    [Authorize]
    public class IlrBuilderController : Controller
    {
        private const string DateFormat = "yyyyMMdd";
        private const string TimeFormat = "HHmmss";
        private const string DateTimeFormat = DateFormat + "T" + TimeFormat;

        private IIlrBuilderService _builderService;
        private IUlnService _ulnService;
        public IlrBuilderController(IIlrBuilderService builderService, IUlnService ulnService)
        {
            _builderService = builderService;
            _ulnService = ulnService;

        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IlrSubmissionModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.Specs.StartsWith("Feature:"))
                {
                    model.Specs = "Feature: Default Feature for testing \n" + model.Specs;
                }

                var result = _builderService.BuildIlrWithRefenceData(new IlrBuilderRequest
                {
                    Gherkin = model.Specs,
                    Ukprn = model.Ukprn,
                    AcademicYear = model.AcademicYear,
                    ShiftToMonth = model.ShiftMonth,
                    ShiftToYear = model.ShiftYear

                });
                if (!result.IsSuccess)
                {
                    var message = GetBuilderResultErrorMessage(result.Exception);
                    ModelState.AddModelError("Specs", message);
                    return View(model);
                }

                var zippedFiles = GetZippedFiles(result, model.Ukprn, model.AcademicYear);
                return File(zippedFiles, "application/zip", $"IlrSubmission{DateTime.Now.ToString(DateTimeFormat)}.zip");
            }
            return View(model);
        }


        private string GetBuilderResultErrorMessage(Exception exception)
        {
            var invalidSpecException = exception as InvalidSpecificationsException;
            if (invalidSpecException == null)
            {
                return exception.Message;
            }

            return invalidSpecException.InnerExceptions
                                        .Select(e => $"[{e.RuleId}]({(e.SpecificationName.Length > 50 ? e.SpecificationName.Substring(0, 47) + "..." : e.SpecificationName)}) : {e.Description}")
                                        .Aggregate((x, y) => $"{x}\n{y}");
        }
        private byte[] GetZippedFiles(IlrBuilderResponse response, long ukprn, string academicYear)
        {
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    zip.AddEntry(response.IlrContent, $"ILR-{ukprn}-{academicYear}-{DateTime.Now.ToString(DateFormat)}-{DateTime.Now.ToString(TimeFormat)}-01.xml");
                    if (response.CommitmentsContent != null)
                    {
                        zip.AddEntry(response.CommitmentsContent, $"commitments_{DateTime.Now.ToString(DateTimeFormat)}.sql");
                    }
                    if (response.AccountsContent != null)
                    {
                        zip.AddEntry(response.AccountsContent, $"accounts_{DateTime.Now.ToString(DateTimeFormat)}.sql");
                    }
                    if (response.UsedUlnCSV != null)
                    {
                        zip.AddEntry(response.UsedUlnCSV, $"UsedULNs_{DateTime.Now.ToString(DateTimeFormat)}.csv");
                    }
                    if (response.CommitmentsBulkCsv != null)
                    {
                        zip.AddEntry(response.CommitmentsBulkCsv, $"CommitmentsBulkCSV_{DateTime.Now.ToString(DateTimeFormat)}.csv");
                    }
                }
                return zipStream.ToArray();
            }
        }
    }
}