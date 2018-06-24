using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Castle.Core.Logging;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes
{
    class SetupMatchingEarningsAndPastPayments : Attribute, IApplyToContext
    {
        private readonly int _apprenticeshipContractType;
        private readonly bool _datalockSuccess;
        private readonly decimal _onProgAmount;
        private readonly decimal _mathsEnglishAmount;
        private readonly string _academicYear;
        
        public SetupMatchingEarningsAndPastPayments(
            int apprenticeshipContractType,
            bool datalockSuccess = true,
            int onProgAmount = 500,
            string academicYear = "1718",
            int mathsEnglishAmount = 39)
        {
            _apprenticeshipContractType = apprenticeshipContractType;
            _datalockSuccess = datalockSuccess;
            _onProgAmount = onProgAmount;
            _academicYear = academicYear;
            _mathsEnglishAmount = mathsEnglishAmount;
        }

        public void ApplyToContext(TestExecutionContext context)
        {
            var fixture = new Fixture();

            var priceEpisode1 = fixture.Create<string>() + $"01/08/20{_academicYear.Substring(0, 2)}";
            int programmeType = fixture.Create<int>();
            int standardCode = fixture.Create<int>();
            int pathwayCode = fixture.Create<int>();
            int frameworkCode = fixture.Create<int>();

            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();

            var accountId = fixture.Create<long>();
            var commitmentId = fixture.Create<long>();

            var earnings = fixture.Build<RawEarning>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.ApprenticeshipContractType, _apprenticeshipContractType)
                .With(x => x.StandardCode, standardCode)
                .With(x => x.ProgrammeType, programmeType)
                .With(x => x.PathwayCode, pathwayCode)
                .With(x => x.FrameworkCode, frameworkCode)
                .CreateMany(12)
                .ToList();

            mathsAndEnglishEarnings.AddRange(fixture.Build<RawEarningForMathsOrEnglish>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.ApprenticeshipContractType, _apprenticeshipContractType)
                .With(x => x.StandardCode, standardCode)
                .With(x => x.ProgrammeType, programmeType)
                .With(x => x.PathwayCode, pathwayCode)
                .With(x => x.FrameworkCode, frameworkCode)
                .CreateMany(12));

            var pastPayments = fixture.Build<RequiredPaymentEntity>()
                    .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                    .With(x => x.ApprenticeshipContractType, _apprenticeshipContractType)
                    .With(x => x.StandardCode, standardCode)
                    .With(x => x.ProgrammeType, programmeType)
                    .With(x => x.PathwayCode, pathwayCode)
                    .With(x => x.FrameworkCode, frameworkCode)
                    .CreateMany(12)
                    .ToList();

            var datalocks = fixture.Build<DatalockOutputEntity>()
                .With(x => x.CommitmentId, commitmentId)
                .With(x => x.Payable, _datalockSuccess)
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.TransactionTypesFlag, 1)
                .CreateMany(12)
                .ToList();

            var commitments = fixture.Build<Commitment>()
                .With(x => x.CommitmentId, commitmentId)
                .With(x => x.AccountId, accountId)
                .CreateMany(1)
                .ToList();

            var datalockValidationErrors = fixture.Build<DatalockValidationError>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .CreateMany(1)
                .ToList();

            if (_datalockSuccess)
            {
                datalockValidationErrors.Clear();
            }

            for (var i = 0; i < 12; i++)
            {
                earnings[i].TransactionType01 = _onProgAmount;
                mathsAndEnglishEarnings[i].TransactionType13 = _mathsEnglishAmount;

                earnings[i].Period = i + 1;

                datalocks[i].Period = i + 1;

                earnings[i].TransactionType02 = 0m;
                earnings[i].TransactionType03 = 0m;
                earnings[i].TransactionType04 = 0m;
                earnings[i].TransactionType05 = 0m;
                earnings[i].TransactionType06 = 0m;
                earnings[i].TransactionType07 = 0m;
                earnings[i].TransactionType08 = 0m;
                earnings[i].TransactionType09 = 0m;
                earnings[i].TransactionType10 = 0m;
                earnings[i].TransactionType11 = 0m;
                earnings[i].TransactionType12 = 0m;
                earnings[i].TransactionType13 = 0m;
                earnings[i].TransactionType14 = 0m;
                earnings[i].TransactionType15 = 0m;

                mathsAndEnglishEarnings[i].TransactionType01 = 0m;
                mathsAndEnglishEarnings[i].TransactionType02 = 0m;
                mathsAndEnglishEarnings[i].TransactionType03 = 0m;
                mathsAndEnglishEarnings[i].TransactionType04 = 0m;
                mathsAndEnglishEarnings[i].TransactionType05 = 0m;
                mathsAndEnglishEarnings[i].TransactionType06 = 0m;
                mathsAndEnglishEarnings[i].TransactionType07 = 0m;
                mathsAndEnglishEarnings[i].TransactionType08 = 0m;
                mathsAndEnglishEarnings[i].TransactionType09 = 0m;
                mathsAndEnglishEarnings[i].TransactionType10 = 0m;
                mathsAndEnglishEarnings[i].TransactionType11 = 0m;
                mathsAndEnglishEarnings[i].TransactionType12 = 0m;
                mathsAndEnglishEarnings[i].TransactionType14 = 0m;
                mathsAndEnglishEarnings[i].TransactionType15 = 0m;

                earnings[i].DeliveryMonth = (i + 1).DeliveryMonthFromPeriod();
                earnings[i].DeliveryYear = (i + 1).DeliveryYearFromPeriod();

                mathsAndEnglishEarnings[i].DeliveryMonth = (i + 1).DeliveryMonthFromPeriod();
                mathsAndEnglishEarnings[i].DeliveryYear = (i + 1).DeliveryYearFromPeriod();

                pastPayments[i].DeliveryMonth = earnings[i].DeliveryMonth;
                pastPayments[i].DeliveryYear = earnings[i].DeliveryYear;
                pastPayments[i].AmountDue = earnings[i].TransactionType01;
                pastPayments[i].AccountId = accountId;
                pastPayments[i].CommitmentId = datalocks[0].CommitmentId;
                
                pastPayments[i].AimSeqNumber = earnings[i].AimSeqNumber;
                pastPayments[i].ApprenticeshipContractType = earnings[i].ApprenticeshipContractType;
                pastPayments[i].FundingLineType = earnings[i].FundingLineType;
                pastPayments[i].LearnAimRef = earnings[i].LearnAimRef;
                pastPayments[i].SfaContributionPercentage = earnings[i].SfaContributionPercentage;
                pastPayments[i].TransactionType = 1;
                pastPayments[i].UseLevyBalance = true;

                pastPayments[i].Period = earnings[i].Period;

                if (_apprenticeshipContractType == 2)
                {
                    pastPayments[i].AccountId = 0;
                    pastPayments[i].AccountVersionId = null;
                    pastPayments[i].CommitmentId = 0;
                    pastPayments[i].CommitmentVersionId = null;
                }
            }

            var validationService = new DatalockValidationService(NullLogger.Instance);
            var datalockOutput = validationService.ProcessDatalocks(datalocks, datalockValidationErrors, commitments);

            var earningsDictionary = new Dictionary<string, object>
            {
                {"MathsAndEnglishEarnings", mathsAndEnglishEarnings},
                {"Earnings", earnings},
                {"PastPayments", pastPayments},
                {"Datalocks", datalocks},
                {"DatalockValidationErrors", datalockValidationErrors},
                {"Commitments", commitments},
                {"DatalockOutput", datalockOutput},
            };

            context.CurrentTest.Properties.Add("EarningsDictionary", earningsDictionary);
        }
    }
}
