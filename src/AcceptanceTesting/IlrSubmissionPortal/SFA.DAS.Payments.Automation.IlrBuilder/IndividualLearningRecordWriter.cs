using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    public interface IIndividualLearningRecordWriter
    {
        void WriteDocument(IndividualLearningRecord ilr, Stream stream);
        string GetDocumentContent(IndividualLearningRecord ilr);
    }

    public class IndividualLearningRecordWriter : IIndividualLearningRecordWriter
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string TimeFormat = "HH:mm:ss";
        private const string DateTimeFormat = DateFormat + "T" + TimeFormat;
        private const string CurrencyFormat = "0";
        private string IlrNamespace = string.Empty;
        private string RulebaseYear = string.Empty;

        public IndividualLearningRecordWriter(string rulebaseYear)
        {
            RulebaseYear = rulebaseYear;
        }
        public void WriteDocument(IndividualLearningRecord ilr, Stream stream)
        {
            var xml = GetDocumentContent(ilr);
            var buffer = Encoding.UTF8.GetBytes(xml);
            stream.Write(buffer, 0, buffer.Length);
        }
        public string GetDocumentContent(IndividualLearningRecord ilr)
        {
            XDocument doc = null;
            XDocument baseDoc = null;

            if (RulebaseYear == "1617")
            {
                doc = XDocument.Parse(Properties.Resources.BaseIlr_1617);
                baseDoc = XDocument.Parse(Properties.Resources.BaseIlr_1617);
                IlrNamespace = "SFA/ILR/2016-17";
            }
            else
            {
                doc = XDocument.Parse(Properties.Resources.BaseIlr_1718);
                baseDoc = XDocument.Parse(Properties.Resources.BaseIlr_1718);
                IlrNamespace = "SFA/ILR/2017-18";
            }

            SetSystemIlrProperties(doc, ilr);
            SetProviderProperties(doc, ilr);

            DuplicateLearnerElementForLearners(doc, ilr.Learners.Count);
            for (var i = 0; i < ilr.Learners.Count; i++)
            {
                SetLearnerProperties(doc, ilr.Learners[i], i,baseDoc);
            }

            return doc.ToString(SaveOptions.None);
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
        private void SetSystemIlrProperties(XDocument doc, IndividualLearningRecord ilr)
        {
            var header = FindElement(doc, "Message", "Header");

            FindElement(header, "CollectionDetails", "Year").Value = ilr.AcademicYear;
            FindElement(header, "CollectionDetails", "FilePreparationDate").Value = ilr.PreparationDate.ToString(DateFormat);
            FindElement(header, "Source", "DateTime").Value = DateTime.Now.ToString(DateTimeFormat);
        }
        private void SetProviderProperties(XDocument doc, IndividualLearningRecord ilr)
        {
            FindElement(doc, "Message", "Header", "Source", "UKPRN").Value = ilr.Ukprn.ToString();
            FindElement(doc, "Message", "LearningProvider", "UKPRN").Value = ilr.Ukprn.ToString();
        }

        private void SetLearnerProperties(XDocument doc, Learner learner, int learnerIndex, XDocument baseDoc)

        {

            var learnerElement = FindElements(doc, "Message", "Learner")[learnerIndex];
            var deliveryElement = FindElement(learnerElement, "LearningDelivery");


            FindElement(learnerElement, "LearnRefNumber").Value = String.IsNullOrEmpty(learner.LearnerRefNumber) ? learnerIndex.ToString() : learner.LearnerRefNumber;
            FindElement(learnerElement, "ULN").Value = learner.Uln.ToString();
            if (learner.DateOfBirth != DateTime.MinValue)
            {
                FindElement(learnerElement, "DateOfBirth").Value = learner.DateOfBirth.ToString("yyyy-MM-dd");
            }

            if (learner.EmploymentStatuses != null && learner.EmploymentStatuses.Any())
            {
                var employmentStatusElement = FindElement(learnerElement, "LearnerEmploymentStatus");
                foreach (var employmentStatus in learner.EmploymentStatuses)
                {
                    var newEmploymentStatusElement = DuplicateElement(employmentStatusElement);

                    FindElement(newEmploymentStatusElement, "EmpStat").Value = employmentStatus.StatusCode.ToString();
                    FindElement(newEmploymentStatusElement, "EmpId").Value = employmentStatus.EmployerId.ToString();
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
            SetLearningDeliveryValues(delivery, deliveryElement, delivery.AimSequenceNumber == 0 ? 1 : delivery.AimSequenceNumber);

            for (var i = 1; i < learner.LearningDeliveries.Count; i++)
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
            FindElement(deliveryElement, "LearnAimRef").Value = delivery.AimRef;
            FindElement(deliveryElement, "AimType").Value = ((int)delivery.AimType).ToString();

            FindElement(deliveryElement, "LearnStartDate").Value = delivery.LearningStartDate.ToString(DateFormat);
            FindElement(deliveryElement, "LearnPlanEndDate").Value = delivery.PlannedLearningEndDate.ToString(DateFormat);

            FindElement(deliveryElement, "CompStatus").Value = ((int)delivery.CompletionStatus).ToString();
            FindElement(deliveryElement, "AimSeqNumber").Value = aimSeqNumber.ToString();

            if (delivery.ActualLearningEndDate.HasValue)
            {
                var actualEndDate = CreateElement("LearnActEndDate", delivery.ActualLearningEndDate.Value.ToString(DateFormat));
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

            var financialRecord = FindFinancialRecordElement(deliveryElement, "TrailblazerApprenticeshipFinancialRecord", "AppFinRecord");
            if (delivery.AimType == AimType.OnProgramme)
            {
                FindFinancialRecordElement(financialRecord, "TBFinDate", "AFinDate").Value = delivery.FinancialRecords[0].From.ToString(DateFormat);
                FindFinancialRecordElement(financialRecord, "TBFinAmount", "AFinAmount").Value = delivery.FinancialRecords[0].Amount.ToString(CurrencyFormat);
                FindFinancialRecordElement(financialRecord, "TBFinCode", "AFinCode").Value = delivery.FinancialRecords[0].Code.ToString();

                for (var x = 1; x < delivery.FinancialRecords.Count; x++)
                {
                    var newFinancialRecord = DuplicateElement(financialRecord);

                    FindFinancialRecordElement(newFinancialRecord, "TBFinDate", "AFinDate").Value = delivery.FinancialRecords[x].From.ToString(DateFormat);
                    FindFinancialRecordElement(newFinancialRecord, "TBFinAmount", "AFinAmount").Value = delivery.FinancialRecords[x].Amount.ToString(CurrencyFormat);
                    FindFinancialRecordElement(newFinancialRecord, "TBFinCode", "AFinCode").Value = delivery.FinancialRecords[x].Code.ToString();

                    financialRecord.AddAfterSelf(newFinancialRecord);
                }
            }
            else
            {
                financialRecord.Remove();
            }

            var famCodes = FindElements(deliveryElement, "LearningDeliveryFAM");

            var dasFam = famCodes.Single(x => FindElement(x, "LearnDelFAMType").Value == "ACT");

            var actFamRecords = delivery.FundingAndMonitoringCodes?.Where(x => x.Type == "ACT").ToArray();

            var hasFamRecords = actFamRecords != null && actFamRecords.Length > 0;
            if (hasFamRecords)
            {
                FindElement(dasFam, "LearnDelFAMCode").Value = actFamRecords[0].Code.ToString();
                FindElement(dasFam, "LearnDelFAMDateFrom").Value = actFamRecords[0].From.ToString(DateFormat);
                FindElement(dasFam, "LearnDelFAMDateTo").Value = GetFamToDate(actFamRecords[0].To, delivery.ActualLearningEndDate).ToString(DateFormat);

                for (var x = 1; x < actFamRecords.Length; x++)
                {
                    var newDasFam = DuplicateElement(dasFam);

                    FindElement(newDasFam, "LearnDelFAMCode").Value = actFamRecords[x].Code.ToString();
                    FindElement(newDasFam, "LearnDelFAMDateFrom").Value = actFamRecords[x].From.ToString(DateFormat);
                    FindElement(newDasFam, "LearnDelFAMDateTo").Value = GetFamToDate(actFamRecords[x].To, delivery.ActualLearningEndDate).ToString(DateFormat);

                    dasFam.AddAfterSelf(newDasFam);
                }
            }
            else
            {
                dasFam.Remove();
            }

            // other non act Fam records
            var nonActFamRecords = delivery.FundingAndMonitoringCodes?.Where(x => x.Type != "ACT").ToArray();

            if (nonActFamRecords != null && nonActFamRecords.Length > 0)
            {
                var learnerNonActFamElement = famCodes.Single(x => FindElement(x, "LearnDelFAMType").Value == "SOF");

                foreach (var famRecord in nonActFamRecords)
                {
                    var newNonActFamElement = DuplicateElement(learnerNonActFamElement);

                    FindElement(newNonActFamElement, "LearnDelFAMCode").Value = famRecord.Code.ToString();
                    FindElement(newNonActFamElement, "LearnDelFAMType").Value = famRecord.Type;

                    var famDateFrom = CreateElement("LearnDelFAMDateFrom", famRecord.From.ToString(DateFormat));
                    var famDateTo = CreateElement("LearnDelFAMDateTo", GetFamToDate(famRecord.To, delivery.ActualLearningEndDate).ToString(DateFormat));

                    FindElement(newNonActFamElement, "LearnDelFAMCode").AddAfterSelf(famDateTo);
                    FindElement(newNonActFamElement, "LearnDelFAMCode").AddAfterSelf(famDateFrom);

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
                element = element.Element(XName.Get(name, IlrNamespace));
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
                var el = element.Element(XName.Get(name, IlrNamespace));
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
                element = element.Element(XName.Get(elementChain[i], IlrNamespace));
            }

            return element.Elements(XName.Get(elementChain[elementChain.Length - 1], IlrNamespace)).ToArray();
        }
        private XElement CreateElement(string name, string value)
        {
            return new XElement(XName.Get(name, IlrNamespace), value);
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
