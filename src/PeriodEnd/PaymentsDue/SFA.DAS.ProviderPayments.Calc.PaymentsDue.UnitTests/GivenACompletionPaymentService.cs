using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
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
            CompletionPaymentService sut)
        {
            var result = sut.CreateCompletionPaymentEvidence(new List<PaymentEntity>(), new List<RawEarning>());

            result.State.Should().Be(CompletionPaymentEvidenceState.Checkable);
            result.TotalHistoricEmployerPayment.Should().Be(0);
            result.IlrEvidenceEmployerPayment.Should().Be(0);
        }

        [Test]
        [PaymentsDueInlineAutoData(0, CompletionPaymentEvidenceState.Checkable)]
        [PaymentsDueInlineAutoData(1, CompletionPaymentEvidenceState.ExemptRedundancy)]
        [PaymentsDueInlineAutoData(2, CompletionPaymentEvidenceState.ExemptOwnDelivery)]
        [PaymentsDueInlineAutoData(3, CompletionPaymentEvidenceState.ExemptOtherReason)]
        [PaymentsDueInlineAutoData(4, CompletionPaymentEvidenceState.ErrorOnIlr)]
        public void ThenItCreatesAValidCompletionPaymentEvidenceFromEarningsAndPaymentHistory(
            int pmrExempCode, 
            CompletionPaymentEvidenceState expectedState,
            List<RawEarning> rawEarnings,
            List<PaymentEntity> paymentHistory,
            CompletionPaymentService sut)
        {
            rawEarnings.ForEach(x =>
            {
                x.PriceEpisodeCumulativePmrs = 100;
                x.PriceEpisodeCompExemCode = pmrExempCode;
            });

            paymentHistory.ForEach(x =>
            {
                x.TransactionType = TransactionType.Learning;
                x.FundingSource = FundingSource.CoInvestedEmployer;
                x.Amount = 9;
            });

            var result = sut.CreateCompletionPaymentEvidence(paymentHistory, rawEarnings);

            result.State.Should().Be(expectedState);
            result.IlrEvidenceEmployerPayment.Should().Be(100);
            result.TotalHistoricEmployerPayment.Should().Be(27);
        }

        [Test, PaymentsDueInlineAutoData()]
        public void ThenItCreatesAnInvalidCompletionPaymentEvidenceFromEarningsAndPaymentHistory(
            List<RawEarning> rawEarnings,
            List<PaymentEntity> paymentHistory,
            CompletionPaymentService sut)
        {
            rawEarnings.ForEach(x =>
            {
                x.PriceEpisodeCumulativePmrs = 100;
            });

            var result = sut.CreateCompletionPaymentEvidence(paymentHistory, rawEarnings);

            result.State.Should().Be(CompletionPaymentEvidenceState.ErrorOnIlr);
            result.IlrEvidenceEmployerPayment.Should().Be(0);
            result.TotalHistoricEmployerPayment.Should().Be(0);
        }

        [Test, PaymentsDueInlineAutoData()]
        public void ThenItThrowsArgumentExceptionWhenNoPaymentHistory(
            CompletionPaymentService sut)
        {
            Action test = () => sut.CreateCompletionPaymentEvidence(null, new List<RawEarning>());

            test.ShouldThrow<ArgumentException>().And.Message.Should().ContainEquivalentOf("learnerHistoricalPayments");
        }

        [Test, PaymentsDueInlineAutoData()]
        public void ThenItThrowsArgumentExceptionWhenNoEarnings(
            CompletionPaymentService sut)
        {
            Action test = () => sut.CreateCompletionPaymentEvidence(new List<PaymentEntity>(), null);

            test.ShouldThrow<ArgumentException>().And.Message.Should().ContainEquivalentOf("learnerRawEarnings");
        }

    }
}