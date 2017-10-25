using System;
using System.Linq;
using System.Xml.Linq;

namespace IlrGenerator
{
    public class IlrBuilder
    {
        private readonly GeneratorSettings _settings;
        private const string DateFormat = "yyyy-MM-dd";
        private const string TimeFormat = "HH:mm:ss";
        private const string DateTimeFormat = DateFormat + "T" + TimeFormat;
        private const string CurrencyFormat = "0";
        private string _ilrNamespace = string.Empty;

        public IlrBuilder(GeneratorSettings settings)
        {
            _settings = settings;
        }

        public IlrBuilder() : this(new GeneratorSettings())
        {
        }

        public XDocument MakeIlrDocument(IlrSubmission submission)
        {
            XDocument doc;
            XDocument baseDoc;

            if (_settings.OpaRulebaseYear == "1617")
            {
                doc = XDocument.Parse(Properties.Resources.BaseIlr_1617);
                baseDoc = XDocument.Parse(Properties.Resources.BaseIlr_1617);
                _ilrNamespace = "SFA/ILR/2016-17";
            }
            else
            {
                doc = XDocument.Parse(Properties.Resources.BaseIlr_1718);
                baseDoc = XDocument.Parse(Properties.Resources.BaseIlr_1718);
                _ilrNamespace = "SFA/ILR/2017-18";
            }

            SetSystemIlrProperties(doc, submission);
            SetProviderProperties(doc, submission);

            DuplicateLearnerElementForLearners(doc, submission.Learners.Length);
            for (var i = 0; i < submission.Learners.Length; i++)
            {
                SetLearnerProperties(doc, submission.Learners[i], i,baseDoc);
            }

            return doc;
        }
        
        private void DuplicateLearnerElementForLearners(XDocument doc, int numberOfLearners)
        {
            var learnerElem = FindElement(doc, "Message", "Learner");
            for (var i = 0; i < numberOfLearners - 1; i++)
            {
                var dupliate = DuplicateElement(learnerElem);
                learnerElem.AddAfterSelf(dupliate);
            }
        }

        private void SetSystemIlrProperties(XDocument doc, IlrSubmission submission)
        {
            var header = FindElement(doc, "Message", "Header");

            FindElement(header, "CollectionDetails", "Year").Value = submission.AcademicYear;
            FindElement(header, "CollectionDetails", "FilePreparationDate").Value = submission.PreperationDate.ToString(DateFormat);
            FindElement(header, "Source", "DateTime").Value = DateTime.Now.ToString(DateTimeFormat);
        }

        private void SetProviderProperties(XDocument doc, IlrSubmission submission)
        {
            FindElement(doc, "Message", "Header", "Source", "UKPRN").Value = submission.Ukprn.ToString();
            FindElement(doc, "Message", "LearningProvider", "UKPRN").Value = submission.Ukprn.ToString();
        }

        private void SetLearnerProperties(XDocument doc, Learner learner, int learnerIndex, XDocument baseDoc)
        {

            var learnerElement = FindElements(doc, "Message", "Learner")[learnerIndex];
            var deliveryElement = FindElement(learnerElement, "LearningDelivery");


            FindElement(learnerElement, "LearnRefNumber").Value = String.IsNullOrEmpty(learner.LearnRefNumber) ? learnerIndex.ToString() : learner.LearnRefNumber;
            FindElement(learnerElement, "ULN").Value = learner.Uln.ToString();
            if (learner.DateOfBirth != DateTime.MinValue)
            {
                FindElement(learnerElement, "DateOfBirth").Value = learner.DateOfBirth.ToString("yyyy-MM-dd");
            }

            if (learner.EmploymentStatuses != null)
            {
                var employmentStatusElement = FindElement(learnerElement, "LearnerEmploymentStatus");
                foreach (var employmentStatus in learner.EmploymentStatuses)
                {
                    var newEmploymentStatusElement = DuplicateElement(employmentStatusElement);

                    FindElement(newEmploymentStatusElement, "EmpStat").Value = employmentStatus.StatusCode.ToString();
                    if (employmentStatus.EmployerId > 0)
                    {
                        FindElement(newEmploymentStatusElement, "EmpId").Value = employmentStatus.EmployerId.ToString();
                    }
                    else
                    {
                        var elementEmpId = FindElement(newEmploymentStatusElement, "EmpId");
                        elementEmpId.Remove();
                    }

                    FindElement(newEmploymentStatusElement, "DateEmpStatApp").Value = employmentStatus.DateFrom.ToString("yyyy-MM-dd");

                    if (employmentStatus.EmploymentStatusMonitoring != null)
                    {
                        var monitoringElement = FindElement(newEmploymentStatusElement, "EmploymentStatusMonitoring");
                        if (monitoringElement != null)
                        {
                            var newEmploymentStatusMonitorng = DuplicateElement(monitoringElement);

                            FindElement(newEmploymentStatusMonitorng, "ESMType").Value = employmentStatus.EmploymentStatusMonitoring.Type;
                            FindElement(newEmploymentStatusMonitorng, "ESMCode").Value = employmentStatus.EmploymentStatusMonitoring.Code.ToString();

                            newEmploymentStatusElement.Add(newEmploymentStatusMonitorng);
                        }
                    }
                    deliveryElement.AddBeforeSelf(newEmploymentStatusElement);
                }
                employmentStatusElement.RemoveAll();
                employmentStatusElement.Remove();
            }

            var delivery = learner.LearningDeliveries.First();

            //set the learning delivery values for first learning delivery
            SetLearningDeliveryValues(delivery, deliveryElement, delivery.AimSequenceNumber ==0 ? 1 : delivery.AimSequenceNumber );

            if (learner.LearningDeliveries.Length == 1)
            {
                var updatedDeliveries = new LearningDelivery[2];
                updatedDeliveries[0] = learner.LearningDeliveries[0];
                updatedDeliveries[1] = new LearningDelivery
                {
                    Type = AimType.Component,
                    StandardCode = learner.LearningDeliveries[0].StandardCode,
                    FrameworkCode = learner.LearningDeliveries[0].FrameworkCode,
                    ProgrammeType = learner.LearningDeliveries[0].ProgrammeType,
                    PathwayCode = learner.LearningDeliveries[0].PathwayCode,
                    ActualStartDate = learner.LearningDeliveries[0].ActualStartDate,
                    PlannedEndDate = learner.LearningDeliveries[0].PlannedEndDate,
                    ActualEndDate = learner.LearningDeliveries[0].ActualEndDate,
                    FamRecords = learner.LearningDeliveries[0].FamRecords.Where(x => x.FamType != "ACT").ToArray(),
                    ActFamCodeValue = learner.LearningDeliveries[0].ActFamCodeValue,
                    CompletionStatus = learner.LearningDeliveries[0].CompletionStatus
                };
                learner.LearningDeliveries = updatedDeliveries;
            }

            for (var i = 1; i < learner.LearningDeliveries.Length; i++)
            {
                
                var baseLearnerElement = FindElements(baseDoc, "Message", "Learner")[0];

                var newDeliveryElement = DuplicateElement(FindElement(baseLearnerElement, "LearningDelivery"));

                var aimSequence = learner.LearningDeliveries[i].AimSequenceNumber == 0 ? i + 1 : learner.LearningDeliveries[i].AimSequenceNumber;
                SetLearningDeliveryValues(learner.LearningDeliveries[i], newDeliveryElement, aimSequence);
                deliveryElement.AddAfterSelf(newDeliveryElement);
            }
        }

        private void SetLearningDeliveryValues(LearningDelivery delivery, XElement deliveryElement, int aimSeqNumber)
        {
            if (delivery.Type == AimType.MathsOrEnglish || delivery.Type == AimType.Component)
            {
                if (string.IsNullOrEmpty(delivery.LearnAimRef))
                {
                    var aimRefLookup = _settings.AimRefLookups?.SingleOrDefault(x => x.StandardCode == delivery.StandardCode
                                                                             && x.ProgrammeType == delivery.ProgrammeType
                                                                             && x.FrameworkCode == delivery.FrameworkCode
                                                                             && x.PathwayCode == delivery.PathwayCode);
                    if (aimRefLookup != null)
                    {
                        FindElement(deliveryElement, "LearnAimRef").Value = delivery.Type == AimType.MathsOrEnglish ? aimRefLookup.MathsAndEnglishLearnAimRef : aimRefLookup.ComponentLearnAimRef;
                    }
                    else if (delivery.StandardCode > 0)
                    {
                        FindElement(deliveryElement, "LearnAimRef").Value = delivery.Type == AimType.MathsOrEnglish ? _settings.DefaultStandardMathsAndEnglishLearnAimRef : _settings.DefaultStandardComponentLearnAimRef;
                    }
                    else
                    {
                        FindElement(deliveryElement, "LearnAimRef").Value = delivery.Type == AimType.MathsOrEnglish ? _settings.DefaultFrameworkMathsAndEnglishLearnAimRef : _settings.DefaultFrameworkComponentLearnAimRef;
                    }
                }
                else
                {
                    FindElement(deliveryElement, "LearnAimRef").Value = delivery.LearnAimRef;
                }

                FindElement(deliveryElement, "AimType").Value = "3";
            }

            FindElement(deliveryElement, "LearnStartDate").Value = delivery.ActualStartDate.ToString(DateFormat);
            FindElement(deliveryElement, "LearnPlanEndDate").Value = delivery.PlannedEndDate.ToString(DateFormat);

            FindElement(deliveryElement, "CompStatus").Value = ((int)delivery.CompletionStatus).ToString();
            FindElement(deliveryElement, "AimSeqNumber").Value = aimSeqNumber.ToString();

            if (delivery.ActualEndDate.HasValue)
            {
                var actualEndDate = CreateElement("LearnActEndDate", delivery.ActualEndDate.Value.ToString(DateFormat));
                var outcome = CreateElement("Outcome", delivery.CompletionStatus == CompletionStatus.Transferred ? "3" : "8");

                FindElement(deliveryElement, "CompStatus").AddAfterSelf(actualEndDate);
                actualEndDate.AddAfterSelf(outcome);

                if (delivery.CompletionStatus == CompletionStatus.Transferred)
                {
                    var withdrawReason = CreateElement("WithdrawReason", "2");
                    actualEndDate.AddAfterSelf(withdrawReason);
                }
            }

            var fundModel = FindElement(deliveryElement, "FundModel");
            if (delivery.StandardCode > 0)
            {
                var programme = CreateElement("ProgType", "25");
                var standard = CreateElement("StdCode", delivery.StandardCode.ToString());

                fundModel.AddAfterSelf(programme);
                programme.AddAfterSelf(standard);
            }
            else
            {
                var programme = CreateElement("ProgType", delivery.ProgrammeType.ToString());
                var framework = CreateElement("FworkCode", delivery.FrameworkCode.ToString());
                var pathway = CreateElement("PwayCode", delivery.PathwayCode.ToString());

                fundModel.AddAfterSelf(programme);
                programme.AddAfterSelf(framework);
                framework.AddAfterSelf(pathway);
            }

            if (delivery.LearningAdjustmentForPriorLearning > -1)
            {
                deliveryElement.Add(CreateElement("PriorLearnFundAdj", delivery.LearningAdjustmentForPriorLearning.ToString()));
            }

            if (delivery.OtherFundingAdjustments > -1)
            {
                deliveryElement.Add(CreateElement("OtherFundAdj", delivery.OtherFundingAdjustments.ToString()));
            }

            var financialRecord = FindFinancialRecordElement(deliveryElement, "TrailblazerApprenticeshipFinancialRecord", "AppFinRecord");
            if (delivery.Type == AimType.Programme)
            {
                if (delivery.FinancialRecords == null)
                {
                    FindFinancialRecordElement(financialRecord, "TBFinDate", "AFinDate").Value = delivery.ActualStartDate.ToString(DateFormat);
                    FindFinancialRecordElement(financialRecord, "TBFinAmount", "AFinAmount").Value = delivery.TrainingCost.ToString(CurrencyFormat);

                    if (delivery.StandardCode > 0)
                    {
                        var epFinancialRecord = DuplicateElement(financialRecord);
                        FindFinancialRecordElement(epFinancialRecord, "TBFinDate", "AFinDate").Value = delivery.ActualStartDate.ToString(DateFormat);
                        FindFinancialRecordElement(epFinancialRecord, "TBFinAmount", "AFinAmount").Value =
                            delivery.EndpointAssesmentCost.ToString(CurrencyFormat);
                        FindFinancialRecordElement(epFinancialRecord, "TBFinCode", "AFinCode").Value = "2";
                        financialRecord.AddAfterSelf(epFinancialRecord);
                    }
                }
                else
                {
                    FindFinancialRecordElement(financialRecord, "TBFinDate", "AFinDate").Value =
                        delivery.FinancialRecords[0].Date.ToString(DateFormat);
                    FindFinancialRecordElement(financialRecord, "TBFinAmount", "AFinAmount").Value =
                        delivery.FinancialRecords[0].Amount.ToString(CurrencyFormat);
                    FindFinancialRecordElement(financialRecord, "TBFinCode", "AFinCode").Value = delivery.FinancialRecords[0].Code.ToString();

                    for (var x = 1; x < delivery.FinancialRecords.Length; x++)
                    {
                        var newFinancialRecord = DuplicateElement(financialRecord);

                        FindFinancialRecordElement(newFinancialRecord, "TBFinDate", "AFinDate").Value =
                            delivery.FinancialRecords[x].Date.ToString(DateFormat);
                        FindFinancialRecordElement(newFinancialRecord, "TBFinAmount", "AFinAmount").Value =
                            delivery.FinancialRecords[x].Amount.ToString(CurrencyFormat);
                        FindFinancialRecordElement(newFinancialRecord, "TBFinCode", "AFinCode").Value =
                            delivery.FinancialRecords[x].Code.ToString();

                        financialRecord.AddAfterSelf(newFinancialRecord);
                    }
                }
            }
            else
            {
                financialRecord.Remove();
            }

            var famCodes = FindElements(deliveryElement, "LearningDeliveryFAM");

            var dasFam = famCodes.Single(x => FindElement(x, "LearnDelFAMType").Value == "ACT");

            var actFamRecords = delivery.FamRecords?.Where(x => x.FamType == "ACT").ToArray();

            var hasFamRecords = actFamRecords != null && actFamRecords.Length > 0;
            if (hasFamRecords)
            {
                FindElement(dasFam, "LearnDelFAMCode").Value = actFamRecords[0].Code;
                FindElement(dasFam, "LearnDelFAMDateFrom").Value = actFamRecords[0].From.ToString(DateFormat);
                FindElement(dasFam, "LearnDelFAMDateTo").Value = GetFamToDate(actFamRecords[0].To, delivery.ActualEndDate).ToString(DateFormat);

                for (var x = 1; x < actFamRecords.Length; x++)
                {
                    var newDasFam = DuplicateElement(dasFam);

                    FindElement(newDasFam, "LearnDelFAMCode").Value = actFamRecords[x].Code;
                    FindElement(newDasFam, "LearnDelFAMDateFrom").Value = actFamRecords[x].From.ToString(DateFormat);
                    FindElement(newDasFam, "LearnDelFAMDateTo").Value = GetFamToDate(actFamRecords[x].To, delivery.ActualEndDate).ToString(DateFormat);

                    dasFam.AddAfterSelf(newDasFam);
                }
            }
            else if (delivery.ActFamCodeValue > 0)
            {
                FindElement(dasFam, "LearnDelFAMCode").Value = delivery.ActFamCodeValue.ToString();
                FindElement(dasFam, "LearnDelFAMDateFrom").Value = delivery.ActualStartDate.ToString(DateFormat);
                FindElement(dasFam, "LearnDelFAMDateTo").Value = delivery.ActualEndDate?.ToString(DateFormat) ?? delivery.PlannedEndDate.ToString(DateFormat);
            }
            else
            {
                dasFam.Remove();
            }

            // other non act Fam records
            var nonActFamRecords = delivery.FamRecords?.Where(x => x.FamType != "ACT").ToArray();

            if (nonActFamRecords != null && nonActFamRecords.Length > 0)
            {
                var learnerNonActFamElement = famCodes.Single(x => FindElement(x, "LearnDelFAMType").Value == "SOF");

                foreach (var famRecord in nonActFamRecords)
                {
                    var newNonActFamElement = DuplicateElement(learnerNonActFamElement);

                    FindElement(newNonActFamElement, "LearnDelFAMCode").Value = famRecord.Code;
                    FindElement(newNonActFamElement, "LearnDelFAMType").Value = famRecord.FamType;

                    if (!famRecord.FamType.Equals("RES", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var famDateFrom = CreateElement("LearnDelFAMDateFrom", famRecord.From.ToString(DateFormat));
                        var famDateTo = CreateElement("LearnDelFAMDateTo", GetFamToDate(famRecord.To, delivery.ActualEndDate).ToString(DateFormat));

                        FindElement(newNonActFamElement, "LearnDelFAMCode").AddAfterSelf(famDateTo);
                        FindElement(newNonActFamElement, "LearnDelFAMCode").AddAfterSelf(famDateFrom);
                    }

                    learnerNonActFamElement.AddAfterSelf(newNonActFamElement);
                }
            }
        }
        
        private DateTime GetFamToDate(DateTime famToDate, DateTime? actualEndDate)
        {
            if (!actualEndDate.HasValue)
            {
                return famToDate;
            }
            return famToDate > actualEndDate.Value ? actualEndDate.Value : famToDate;
        }
        
        private XElement FindElement(XContainer parent, params string[] elementChain)
        {
            if (elementChain.Length == 0)
                throw new ArgumentNullException(nameof(elementChain));

            var element = parent;
            foreach (var name in elementChain)
            {
                element = element.Element(XName.Get(name, _ilrNamespace));
            }
            return (XElement)element;
        }
        
        private XElement FindFinancialRecordElement(XContainer parent, params string[] elementChain)
        {
            if (elementChain.Length == 0)
                throw new ArgumentNullException(nameof(elementChain));

            var element = parent;
            foreach (var name in elementChain)
            {
                var el = element.Element(XName.Get(name, _ilrNamespace));
                if (el != null)
                    element = el;
            }
            return (XElement)element;
        }
        
        private XElement[] FindElements(XContainer parent, params string[] elementChain)
        {
            if (elementChain.Length == 0)
                throw new ArgumentNullException(nameof(elementChain));

            var element = parent;
            for (var i = 0; i < elementChain.Length - 1; i++)
            {
                element = element.Element(XName.Get(elementChain[i], _ilrNamespace));
            }

            return element.Elements(XName.Get(elementChain[elementChain.Length - 1], _ilrNamespace)).ToArray();
        }

        private XElement CreateElement(string name, string value)
        {
            return new XElement(XName.Get(name, _ilrNamespace), value);
        }

        private XElement DuplicateElement(XElement element)
        {
            var duplicate = new XElement(element.Name);

            foreach (var attribute in element.Attributes())
            {
                duplicate.Add(new XAttribute(attribute.Name, attribute.Value));
            }

            if (element.HasElements)
            {
                foreach (var subElement in element.Elements())
                {
                    duplicate.Add(DuplicateElement(subElement));
                }
            }
            else if (!element.IsEmpty)
            {
                duplicate.Value = element.Value;
            }

            return duplicate;
        }
    }
}
