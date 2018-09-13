using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using ClosedXML.Excel;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader
{
    static class TestData
    {
        public static TestDataParameters LoadFrom(string filename)
        {
            var result = new TestDataParameters();

            var fixture = new Fixture();
            result.LearnRefNumber = fixture.Create<string>().Substring(0, 12);
            result.Ukprn = fixture.Create<long>();
            result.Uln = fixture.Create<long>();

            var document = new XLWorkbook(Path.Combine(TestContext.CurrentContext.TestDirectory, "ScenarioData", $"{filename}.xlsx"));

            var pastPaymentSheet = document.Worksheet("PastPayments");
            var range = pastPaymentSheet.RowsUsed();
            foreach (var xlRow in range.Skip(1))
            {
                var pastPayment = new RequiredPayment();
                pastPayment.CommitmentId = xlRow.Cell(1).IsEmpty() || xlRow.Cell(1).Value.Equals("NULL") ? 0 : xlRow.Cell(1).GetValue<long>();
                pastPayment.CommitmentVersionId = xlRow.Cell(2).GetValue<string>();
                pastPayment.AccountId = xlRow.Cell(3).IsEmpty() || xlRow.Cell(3).Value.Equals("NULL") ? 0 : xlRow.Cell(3).GetValue<long>();
                pastPayment.DeliveryMonth = xlRow.Cell(5).GetValue<int>();
                pastPayment.DeliveryYear = xlRow.Cell(6).GetValue<int>();
                pastPayment.CollectionPeriodName = xlRow.Cell(7).GetValue<string>();
                pastPayment.TransactionType = xlRow.Cell(8).GetValue<int>();
                pastPayment.AmountDue = xlRow.Cell(9).GetValue<decimal>();
                pastPayment.StandardCode = xlRow.Cell(10).IsEmpty() || xlRow.Cell(10).Value.Equals("NULL") ? 0 : xlRow.Cell(10).GetValue<int>();
                pastPayment.ProgrammeType = xlRow.Cell(11).IsEmpty() || xlRow.Cell(11).Value.Equals("NULL") ? 0 : xlRow.Cell(11).GetValue<int>();
                pastPayment.FrameworkCode = xlRow.Cell(12).IsEmpty() || xlRow.Cell(12).Value.Equals("NULL") ? 0 : xlRow.Cell(12).GetValue<int>();
                pastPayment.PathwayCode = xlRow.Cell(13).IsEmpty() || xlRow.Cell(13).Value.Equals("NULL") ? 0 : xlRow.Cell(13).GetValue<int>();
                pastPayment.PriceEpisodeIdentifier = xlRow.Cell(14).GetValue<string>();
                pastPayment.LearnAimRef = xlRow.Cell(15).GetValue<string>();
                pastPayment.LearningStartDate = xlRow.Cell(16).GetDateTime();
                pastPayment.ApprenticeshipContractType = xlRow.Cell(17).GetValue<int>();
                pastPayment.SfaContributionPercentage = xlRow.Cell(18).GetValue<decimal>();
                pastPayment.FundingLineType = xlRow.Cell(19).GetValue<string>();

                pastPayment.Ukprn = result.Ukprn;
                pastPayment.Uln = result.Uln;
                pastPayment.LearnRefNumber = result.LearnRefNumber;

                result.PastPayments.Add(pastPayment);
            }

            var rawEarningsSheet = document.Worksheet("RawEarnings");
            range = rawEarningsSheet.RowsUsed();
            foreach (var xlRow in range.Skip(1))
            {
                var earning = new RawEarning();
                
                earning.PriceEpisodeIdentifier = xlRow.Cell(1).GetValue<string>();
                earning.EpisodeStartDate = xlRow.Cell(2).GetDateTime();
                earning.EpisodeEffectiveTnpStartDate = xlRow.Cell(3).GetDateTime();
                earning.Period = xlRow.Cell(4).GetValue<int>();
                earning.ProgrammeType = xlRow.Cell(5).IsEmpty() || xlRow.Cell(5).Value.Equals("NULL") ? 0 : xlRow.Cell(5).GetValue<int>();
                earning.FrameworkCode = xlRow.Cell(6).IsEmpty() || xlRow.Cell(6).Value.Equals("NULL") ? 0 : xlRow.Cell(6).GetValue<int>();
                earning.PathwayCode = xlRow.Cell(7).IsEmpty() || xlRow.Cell(7).Value.Equals("NULL") ? 0 : xlRow.Cell(7).GetValue<int>();
                earning.StandardCode = xlRow.Cell(8).IsEmpty() || xlRow.Cell(8).Value.Equals("NULL") ? 0 : xlRow.Cell(8).GetValue<int>();
                earning.SfaContributionPercentage = xlRow.Cell(9).GetValue<decimal>();
                earning.FundingLineType = xlRow.Cell(10).GetValue<string>();
                earning.LearnAimRef = xlRow.Cell(11).GetValue<string>();
                earning.LearningStartDate = xlRow.Cell(12).GetDateTime();
                earning.TransactionType01 = xlRow.Cell(13).GetValue<decimal>();
                earning.TransactionType02 = xlRow.Cell(14).GetValue<decimal>();
                earning.TransactionType03 = xlRow.Cell(15).GetValue<decimal>();
                earning.TransactionType04 = xlRow.Cell(16).GetValue<decimal>();
                earning.TransactionType05 = xlRow.Cell(17).GetValue<decimal>();
                earning.TransactionType06 = xlRow.Cell(18).GetValue<decimal>();
                earning.TransactionType07 = xlRow.Cell(19).GetValue<decimal>();
                earning.TransactionType08 = xlRow.Cell(20).GetValue<decimal>();
                earning.TransactionType09 = xlRow.Cell(21).GetValue<decimal>();
                earning.TransactionType10 = xlRow.Cell(22).GetValue<decimal>();
                earning.TransactionType11 = xlRow.Cell(23).GetValue<decimal>();
                earning.TransactionType12 = xlRow.Cell(24).GetValue<decimal>();
                earning.TransactionType13 = xlRow.Cell(25).Value.Equals("NULL") ? 0 : xlRow.Cell(25).GetValue<decimal>();
                earning.TransactionType14 = xlRow.Cell(26).Value.Equals("NULL") ? 0 : xlRow.Cell(26).GetValue<decimal>();
                earning.TransactionType15 = xlRow.Cell(27).GetValue<decimal>();
                earning.ApprenticeshipContractType = xlRow.Cell(28).GetValue<int>();

                earning.Ukprn = result.Ukprn;
                earning.Uln = result.Uln;
                earning.LearnRefNumber = result.LearnRefNumber;

                result.RawEarnings.Add(earning);
            }

            var rawEarningsMathsEnglishSheet = document.Worksheet("RawEarningsMathsAndEnglish");
            range = rawEarningsMathsEnglishSheet.RowsUsed();
            foreach (var xlRow in range.Skip(1))
            {
                var earning = new RawEarningForMathsOrEnglish();

                earning.PriceEpisodeIdentifier = xlRow.Cell(1).GetValue<string>();
                earning.EpisodeStartDate = xlRow.Cell(2).Value.Equals("NULL") ? (DateTime?)null : xlRow.Cell(2).GetDateTime();
                earning.EpisodeEffectiveTnpStartDate = xlRow.Cell(3).Value.Equals("NULL") ? (DateTime?)null : xlRow.Cell(3).GetDateTime();
                earning.Period = xlRow.Cell(4).GetValue<int>();
                earning.ProgrammeType = xlRow.Cell(5).IsEmpty() || xlRow.Cell(5).Value.Equals("NULL") ? 0 : xlRow.Cell(5).GetValue<int>();
                earning.FrameworkCode = xlRow.Cell(6).IsEmpty() || xlRow.Cell(6).Value.Equals("NULL") ? 0 : xlRow.Cell(6).GetValue<int>();
                earning.PathwayCode = xlRow.Cell(7).IsEmpty() || xlRow.Cell(7).Value.Equals("NULL") ? 0 : xlRow.Cell(7).GetValue<int>();
                earning.StandardCode = xlRow.Cell(8).IsEmpty() || xlRow.Cell(8).Value.Equals("NULL") ? 0 : xlRow.Cell(8).GetValue<int>();
                earning.SfaContributionPercentage = xlRow.Cell(9).GetValue<decimal>();
                earning.FundingLineType = xlRow.Cell(10).GetValue<string>();
                earning.LearnAimRef = xlRow.Cell(11).GetValue<string>();
                earning.LearningStartDate = xlRow.Cell(12).GetDateTime();
                earning.TransactionType01 = xlRow.Cell(13).Value.Equals("NULL") ? 0 : xlRow.Cell(13).GetValue<decimal>();
                earning.TransactionType02 = xlRow.Cell(14).Value.Equals("NULL") ? 0 : xlRow.Cell(14).GetValue<decimal>();
                earning.TransactionType03 = xlRow.Cell(15).Value.Equals("NULL") ? 0 : xlRow.Cell(15).GetValue<decimal>();
                earning.TransactionType04 = xlRow.Cell(16).Value.Equals("NULL") ? 0 : xlRow.Cell(16).GetValue<decimal>();
                earning.TransactionType05 = xlRow.Cell(17).Value.Equals("NULL") ? 0 : xlRow.Cell(17).GetValue<decimal>();
                earning.TransactionType06 = xlRow.Cell(18).Value.Equals("NULL") ? 0 : xlRow.Cell(18).GetValue<decimal>();
                earning.TransactionType07 = xlRow.Cell(19).Value.Equals("NULL") ? 0 : xlRow.Cell(19).GetValue<decimal>();
                earning.TransactionType08 = xlRow.Cell(20).Value.Equals("NULL") ? 0 : xlRow.Cell(20).GetValue<decimal>();
                earning.TransactionType09 = xlRow.Cell(21).Value.Equals("NULL") ? 0 : xlRow.Cell(21).GetValue<decimal>();
                earning.TransactionType10 = xlRow.Cell(22).Value.Equals("NULL") ? 0 : xlRow.Cell(22).GetValue<decimal>();
                earning.TransactionType11 = xlRow.Cell(23).Value.Equals("NULL") ? 0 : xlRow.Cell(23).GetValue<decimal>();
                earning.TransactionType12 = xlRow.Cell(24).Value.Equals("NULL") ? 0 : xlRow.Cell(24).GetValue<decimal>();
                earning.TransactionType13 = xlRow.Cell(25).Value.Equals("NULL") ? 0 : xlRow.Cell(25).GetValue<decimal>();
                earning.TransactionType14 = xlRow.Cell(26).Value.Equals("NULL") ? 0 : xlRow.Cell(26).GetValue<decimal>();
                earning.TransactionType15 = xlRow.Cell(27).GetValue<decimal>();
                earning.ApprenticeshipContractType = xlRow.Cell(28).GetValue<int>();

                earning.Ukprn = result.Ukprn;
                earning.Uln = result.Uln;
                earning.LearnRefNumber = result.LearnRefNumber;

                result.RawEarningsForMathsOrEnglish.Add(earning);
            }

            var datalockSheet = document.Worksheet("Datalocks");
            range = datalockSheet.RowsUsed();
            foreach (var xlRow in range.Skip(1))
            {
                var datalock = new DatalockOutputEntity
                {
                    PriceEpisodeIdentifier = xlRow.Cell(1).GetValue<string>(),
                    CommitmentId = xlRow.Cell(2).GetValue<long>(),
                    VersionId = xlRow.Cell(3).GetValue<string>(),
                    Period = xlRow.Cell(4).GetValue<int>(),
                    Payable = xlRow.Cell(5).GetValue<int>() == 1,
                    TransactionTypesFlag = xlRow.Cell(6).IsEmpty() || xlRow.Cell(6).Value.Equals("NULL") ? 1 : xlRow.Cell(6).GetValue<int>(),
                    Ukprn = result.Ukprn,
                    LearnRefNumber = result.LearnRefNumber,
                };
                result.DatalockOutputs.Add(datalock);
            }

            var datalockValidationErrorSheet = document.Worksheet("DatalockValidationErrors");
            range = datalockValidationErrorSheet.RowsUsed();
            foreach (var xlRow in range.Skip(1))
            {
                var validationError = new DatalockValidationError
                {
                    PriceEpisodeIdentifier = xlRow.Cell(1).GetValue<string>(),
                    RuleId = xlRow.Cell(2).GetValue<string>(),
                    Ukprn = result.Ukprn,
                    LearnRefNumber = result.LearnRefNumber,
                };
                result.DatalockValidationErrors.Add(validationError);
            }

            var commitmentsSheet = document.Worksheet("Commitments");
            range = commitmentsSheet.RowsUsed();
            foreach (var xlRow in range.Skip(1))
            {
                var commitment = new Commitment
                {
                    CommitmentId = xlRow.Cell(1).GetValue<long>(),
                    CommitmentVersionId = xlRow.Cell(2).GetValue<string>(),
                    AccountId = xlRow.Cell(3).GetValue<long>(),
                    StartDate = xlRow.Cell(4).GetDateTime(),
                    EndDate = xlRow.Cell(5).GetDateTime(),
                    AgreedCost = xlRow.Cell(6).GetValue<decimal>(),
                    StandardCode = xlRow.Cell(7).IsEmpty() || xlRow.Cell(7).Value.Equals("NULL") ? 0 : xlRow.Cell(7).GetValue<int>(),
                    ProgrammeType = xlRow.Cell(8).IsEmpty() || xlRow.Cell(8).Value.Equals("NULL") ? 0 : xlRow.Cell(8).GetValue<int>(),
                    FrameworkCode = xlRow.Cell(9).IsEmpty() || xlRow.Cell(9).Value.Equals("NULL") ? 0 : xlRow.Cell(9).GetValue<int>(),
                    PathwayCode = xlRow.Cell(10).IsEmpty() || xlRow.Cell(10).Value.Equals("NULL") ? 0 : xlRow.Cell(10).GetValue<int>(),
                    PaymentStatus = xlRow.Cell(11).GetValue<int>(),
                    Priority = xlRow.Cell(12).GetValue<int>(),
                    EffectiveFrom = xlRow.Cell(13).GetDateTime(),
                    EffectiveTo = xlRow.Cell(14).IsEmpty() || xlRow.Cell(14).Value.Equals("NULL") ? (DateTime?)null : xlRow.Cell(14).GetDateTime(),
                    TransferSendingEmployerAccountId = xlRow.Cell(15).Value.Equals("NULL") ? 0 : xlRow.Cell(15).GetValue<long>(),
                    TransferApprovalDate = xlRow.Cell(16).IsEmpty() || xlRow.Cell(16).Value.Equals("NULL") ? (DateTime?)null : xlRow.Cell(16).GetDateTime(),
                    Uln = result.Uln,
                    Ukprn = result.Ukprn,
                };
                result.Commitments.Add(commitment);
            }

            IXLWorksheet worksheet;
            if (document.TryGetWorksheet("Payments", out worksheet))
            {
                range = worksheet.RowsUsed();
                foreach (var xlRow in range.Skip(1))
                {
                    var payment = new RequiredPayment
                    {
                        DeliveryYear = xlRow.Cell(1).GetValue<int>(),
                        DeliveryMonth = xlRow.Cell(2).GetValue<int>(),
                        AmountDue = xlRow.Cell(3).GetValue<decimal>(),
                        TransactionType = xlRow.Cell(4).GetValue<int>(),
                        PriceEpisodeIdentifier = xlRow.Cell(5).GetValue<string>(),
                        Ukprn = result.Ukprn,
                        Uln = result.Uln,
                        LearnRefNumber = result.LearnRefNumber,
                    };
                    result.Payments.Add(payment);
                }
            }
            
            result.RawEarnings.ForEach(x => x.DeliveryMonth = x.Period.DeliveryMonthFromPeriod());
            result.RawEarnings.ForEach(x => x.DeliveryYear = x.Period.DeliveryYearFromPeriod());

            result.RawEarningsForMathsOrEnglish.ForEach(x => x.DeliveryMonth = x.Period.DeliveryMonthFromPeriod());
            result.RawEarningsForMathsOrEnglish.ForEach(x => x.DeliveryYear = x.Period.DeliveryYearFromPeriod());

            return result;
        }
    }

    class TestDataParameters
    {
        public string LearnRefNumber { get; set; }
        public long Uln { get; set; }
        public long Ukprn { get; set; }

        public List<DatalockOutputEntity> DatalockOutputs { get; set; } = new List<DatalockOutputEntity>();
        public List<Commitment> Commitments { get; set; } = new List<Commitment>();
        public List<RawEarning> RawEarnings { get; set; } = new List<RawEarning>();
        public List<RawEarningForMathsOrEnglish> RawEarningsForMathsOrEnglish { get; set; } = new List<RawEarningForMathsOrEnglish>();
        public List<RequiredPayment> PastPayments { get; set; } = new List<RequiredPayment>();
        public List<RequiredPayment> Payments { get; set; } = new List<RequiredPayment>();
        public List<DatalockValidationError> DatalockValidationErrors { get; set; } = new List<DatalockValidationError>();
    }
}
