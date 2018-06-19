using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes
{
    class SetupMatchingEarningsAndPastPayments : Attribute, IApplyToContext
    {
        private readonly int _apprenticeshipContractType;
        private readonly bool _datalockSuccess;
        private readonly decimal _onProgAmount;

        public SetupMatchingEarningsAndPastPayments(
            int apprenticeshipContractType,
            bool datalockSuccess = true,
            int onProgAmount = 500)
        {
            _apprenticeshipContractType = apprenticeshipContractType;
            _datalockSuccess = datalockSuccess;
            _onProgAmount = onProgAmount;
        }

        public void ApplyToContext(TestExecutionContext context)
        {
            var fixture = new Fixture();

            var priceEpisode1 = fixture.Create<string>();
            int programmeType = fixture.Create<int>();
            int standardCode = fixture.Create<int>();
            int pathwayCode = fixture.Create<int>();
            int frameworkCode = fixture.Create<int>();

            var mathsAndEnglishearnings = new List<RawEarningForMathsOrEnglish>();

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

                earnings[i].DeliveryMonth = (i + 1).DeliveryMonthFromPeriod();
                earnings[i].DeliveryYear = (i + 1).DeliveryYearFromPeriod();

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
                    pastPayments[i].AccountId = null;
                    pastPayments[i].AccountVersionId = null;
                    pastPayments[i].CommitmentId = null;
                    pastPayments[i].CommitmentVersionId = null;
                }
            }

            var transformedDatalocks = new HashSet<DatalockOutput>(datalocks.Select(x => new DatalockOutput(x)))
                .ToList();
            var earningsDictionary = new Dictionary<string, object>
            {
                {"MathsAndEnglishEarnings", mathsAndEnglishearnings},
                {"Earnings", earnings},
                {"PastPayments", pastPayments},
                {"Datalocks", transformedDatalocks},
                {"DatalockValidationErrors", datalockValidationErrors },
                {"Commitments", commitments },
            };

            context.CurrentTest.Properties.Add("EarningsDictionary", earningsDictionary);
        }
    }
}
