using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes
{
    class SetupMatchingEarningsAndPastPayments : Attribute, IApplyToContext
    {
        private readonly int _apprenticeshipContractType;
        private readonly List<int> _datalockSuccess;
        private readonly decimal _onProgAmount;

        public SetupMatchingEarningsAndPastPayments(
            int apprenticeshipContractType, 
            bool datalockSuccess = true,
            int onProgAmount = 500)
        {
            _apprenticeshipContractType = apprenticeshipContractType;
            if (datalockSuccess)
            {
                _datalockSuccess = Enumerable.Range(1, 12).ToList();
            }
            else
            {
                _datalockSuccess = new List<int>();
            }
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

            var datalocks = fixture.Build<PriceEpisode>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.PayablePeriods, _datalockSuccess)
                .CreateMany(1)
                .ToList();

            for (var i = 0; i < 12; i++)
            {
                earnings[i].TransactionType01 = _onProgAmount;
                earnings[i].TransactionType02 = 0;
                earnings[i].TransactionType03 = 0;
                earnings[i].TransactionType04 = 0;
                earnings[i].TransactionType05 = 0;
                earnings[i].TransactionType06 = 0;
                earnings[i].TransactionType07 = 0;
                earnings[i].TransactionType08 = 0;
                earnings[i].TransactionType09 = 0;
                earnings[i].TransactionType10 = 0;
                earnings[i].TransactionType11 = 0;
                earnings[i].TransactionType12 = 0;
                earnings[i].TransactionType13 = 0;
                earnings[i].TransactionType14 = 0;
                earnings[i].TransactionType15 = 0;

                earnings[i].Period = i + 1;

                pastPayments[i].DeliveryMonth = earnings[i].DeliveryMonth;
                pastPayments[i].DeliveryYear = earnings[i].DeliveryYear;
                pastPayments[i].AmountDue = earnings[i].TransactionType01;
                pastPayments[i].AccountId = datalocks[0].AccountId;
                pastPayments[i].AccountVersionId = datalocks[0].AccountVersionId;
                pastPayments[i].CommitmentId = datalocks[0].CommitmentId;
                pastPayments[i].CommitmentVersionId = datalocks[0].CommitmentVersionId;

                pastPayments[i].AimSeqNumber = earnings[i].AimSeqNumber;
                pastPayments[i].ApprenticeshipContractType = earnings[i].ApprenticeshipContractType;
                pastPayments[i].FundingLineType = earnings[i].FundingLineType;
                pastPayments[i].LearnAimRef = earnings[i].LearnAimRef;
                pastPayments[i].SfaContributionPercentage = earnings[i].SfaContributionPercentage;
                pastPayments[i].TransactionType = 1;
                pastPayments[i].UseLevyBalance = datalocks[0].PayablePeriods.Contains(i);

                pastPayments[i].Period = earnings[i].Period;

                if (_apprenticeshipContractType == 2)
                {
                    pastPayments[i].AccountId = null;
                    pastPayments[i].AccountVersionId = null;
                    pastPayments[i].CommitmentId = null;
                    pastPayments[i].CommitmentVersionId = null;
                }
            }

            var earningsDictionary = new Dictionary<string, object>
            {
                {"MathsAndEnglishEarnings", mathsAndEnglishearnings},
                {"Earnings", earnings},
                {"PastPayments", pastPayments},
                {"Datalocks", datalocks}
            };

            context.CurrentTest.Properties.Add("EarningsDictionary", earningsDictionary);
        }
    }
}
