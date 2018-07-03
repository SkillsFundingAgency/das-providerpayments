using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class ExportIlrFileTask : WorkflowTask
    {
        private readonly ILogger _logger;

        public ExportIlrFileTask(ILogger logger)
        {
            _logger = logger;
            Id = "ExportILRFile";
            Description = "Export ILR to file";
        }

        internal override void Execute(TestStackContext context)
        {
            XDocument ilrDocument;

            if (context.SubmissionIsIlrFile)
            {
                using (var stream = new MemoryStream(Convert.FromBase64String(context.RequestContent)))
                {
                    ilrDocument = XDocument.Load(stream);
                }
            }
            else
            {
                var submission = GetSubmission(context);

                try
                {
                    ilrDocument = MakeIlrDocument(submission, context);
                }
                catch (InvalidOperationException e)
                {
                    // This happens when there are multiple providers and one provider
                    //  doesn't have an ilr for a period. Don't want to pollute the 
                    //  logs any further, so ignoring them here
                    if (e.Message.Equals("Sequence contains no elements"))
                    {
                        return;
                    }

                    throw;
                }
            }

            var path = SaveIlrDocument(ilrDocument, context);
            context.IlrFilePath = path;
        }


        private IlrGenerator.IlrSubmission GetSubmission(TestStackContext context)
        {
            return JsonConvert.DeserializeObject<IlrGenerator.IlrSubmission>(context.RequestContent);
        }
        private XDocument MakeIlrDocument(IlrGenerator.IlrSubmission submission, TestStackContext context)
        {
            var generatorSettings = new IlrGenerator.GeneratorSettings();
            generatorSettings.OpaRulebaseYear = context.OpaRulebaseYear;

            if (context.IlrAimRefLookups != null)
            {
                var lookups = JsonConvert.DeserializeObject<IlrAimRefLookup[]>(context.IlrAimRefLookups);
                if (lookups != null && lookups.Any())
                {
                    generatorSettings.AimRefLookups.AddRange(lookups.Select(x =>
                        new IlrGenerator.AimRefLookup
                        {
                            StandardCode = x.StandardCode,
                            ProgrammeType = x.ProgrammeType,
                            FrameworkCode = x.FrameworkCode,
                            PathwayCode = x.PathwayCode,
                            MathsAndEnglishLearnAimRef = x.MathsAndEnglishLearnAimRef,
                            ComponentLearnAimRef = x.ComponentLearnAimRef
                        }));
                }
            }
            var generator = new IlrGenerator.IlrBuilder(generatorSettings);
            return generator.MakeIlrDocument(submission);
        }
        private string SaveIlrDocument(XDocument ilrDocument, TestStackContext context)
        {
            _logger.Info("Preparing to save ILR File");
            var fileName = ExtractIlrFileNameFromDocument(ilrDocument,context.OpaRulebaseYear);
            _logger.Info($"File name is {fileName}");

            var dir = string.IsNullOrEmpty(context.IlrFileDirectory) ? context.WorkingDirectory : context.IlrFileDirectory;
            _logger.Info($"Directory is {dir}");
            var path = Path.Combine(dir, $"{fileName}.xml");
            ilrDocument.Save(path);
            _logger.Info($"ILR saved to {path}");
            return path;
        }
        private string ExtractIlrFileNameFromDocument(XDocument ilrDocument, string opeRulebaseYear)
        {
            // Check all elements exist
            var namespaceName = opeRulebaseYear == "1617" ? "SFA/ILR/2016-17" : "SFA/ILR/2017-18";
            var headerElement = ilrDocument?.Element(XName.Get("Message", namespaceName))?.Element(XName.Get("Header", namespaceName));
            if (headerElement == null)
            {
                throw new NullReferenceException("Invalid ILR! Missing Message\\Header element");
            }

            var sourceElement = headerElement.Element(XName.Get("Source", namespaceName));
            if (sourceElement == null)
            {
                throw new NullReferenceException("Invalid ILR! Missing Message\\Header\\Source element");
            }

            var ukprnElement = sourceElement.Element(XName.Get("UKPRN", namespaceName));
            if (ukprnElement == null)
            {
                throw new NullReferenceException("Invalid ILR! Missing Message\\Header\\Source\\UKPRN element");
            }

            var collectionDetailsElement = headerElement.Element(XName.Get("CollectionDetails", namespaceName));
            if (collectionDetailsElement == null)
            {
                throw new NullReferenceException("Invalid ILR! Missing Message\\Header\\CollectionDetails element");
            }

            var yearElement = collectionDetailsElement.Element(XName.Get("Year", namespaceName));
            if (yearElement == null)
            {
                throw new NullReferenceException("Invalid ILR! Missing Message\\Header\\CollectionDetails\\Year element");
            }

            var prepDateElement = collectionDetailsElement.Element(XName.Get("FilePreparationDate", namespaceName));
            if (prepDateElement == null)
            {
                throw new NullReferenceException("Invalid ILR! Missing Message\\Header\\CollectionDetails\\FilePreparationDate element");
            }

            // Build file name
            var ukprn = ukprnElement.Value;
            var academicYear = yearElement.Value;
            var preperationDate = prepDateElement.Value.Replace("-", "");
            return $"ILR-{ukprn}-{academicYear}-{preperationDate}-{DateTime.Now.ToString("HHmmss")}-01";
        }

    }
}
