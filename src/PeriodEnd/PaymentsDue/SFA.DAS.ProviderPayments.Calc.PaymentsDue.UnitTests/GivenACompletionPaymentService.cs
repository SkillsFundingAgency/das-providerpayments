using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenACompletionPaymentService
    {
        [Test, PaymentsDueAutoData]
        public void ThenItCreatesACompletionPaymentEvidenceWhenThereIsNoEarningsOrHistory(
            CheckEmployerPayments sut)
        {
            var result = sut.CreateCompletionPaymentEvidence(new List<LearnerSummaryPaymentEntity>(), new RawEarning());

            result.State.Should().Be(CompletionPaymentEvidenceState.Checkable);
            result.TotalHistoricEmployerPayment.Should().Be(0);
            result.IlrEvidenceEmployerPayment.Should().Be(0);
        }

        [Test]
        [PaymentsDueInlineAutoData(0, CompletionPaymentEvidenceState.Checkable)]
        [PaymentsDueInlineAutoData(1, CompletionPaymentEvidenceState.ExemptRedundancy)]
        [PaymentsDueInlineAutoData(2, CompletionPaymentEvidenceState.ExemptOwnDelivery)]
        [PaymentsDueInlineAutoData(3, CompletionPaymentEvidenceState.ExemptOtherReason)]
        public void ThenItCreatesAValidCompletionPaymentEvidenceFromEarningsAndPaymentHistory(
            int pmrExempCode, 
            CompletionPaymentEvidenceState expectedState,
            RawEarning rawEarning,
            List<LearnerSummaryPaymentEntity> paymentHistory,
            CheckEmployerPayments sut)
        {
            rawEarning.CumulativePmrs = 100;
            rawEarning.ExemptionCodeForCompletionHoldback = pmrExempCode;
            
            paymentHistory.ForEach(x =>
            {
                x.TransactionType = TransactionType.Learning;
                x.ApprenticeshipContractType = rawEarning.ApprenticeshipContractType;
                x.FrameworkCode = rawEarning.FrameworkCode;
                x.ProgrammeType = rawEarning.ProgrammeType;
                x.StandardCode = rawEarning.StandardCode;
                x.PathwayCode = rawEarning.PathwayCode;
                x.FundingLineType = rawEarning.FundingLineType;
                x.SfaContributionPercentage = rawEarning.SfaContributionPercentage;
                x.Amount = 9;
            });

            var result = sut.CreateCompletionPaymentEvidence(paymentHistory, rawEarning);

            result.State.Should().Be(expectedState);
            result.IlrEvidenceEmployerPayment.Should().Be(100);
            result.TotalHistoricEmployerPayment.Should().Be(27);
        }

        [Test, PaymentsDueInlineAutoData()]
        public void ThenItThrowsArgumentExceptionWhenNoPaymentHistory(
            CheckEmployerPayments sut)
        {
            Action test = () => sut.CreateCompletionPaymentEvidence(null, new RawEarning());

            test.ShouldThrow<ArgumentException>().And.Message.Should().ContainEquivalentOf("employerPayments");
        }

        [Test, PaymentsDueInlineAutoData()]
        public void ThenItThrowsArgumentExceptionWhenNoEarnings(
            CheckEmployerPayments sut)
        {
            Action test = () => sut.CreateCompletionPaymentEvidence(new List<LearnerSummaryPaymentEntity>(), null);

            test.ShouldThrow<ArgumentException>().And.Message.Should().ContainEquivalentOf("rawEarning");
        }
    }
}