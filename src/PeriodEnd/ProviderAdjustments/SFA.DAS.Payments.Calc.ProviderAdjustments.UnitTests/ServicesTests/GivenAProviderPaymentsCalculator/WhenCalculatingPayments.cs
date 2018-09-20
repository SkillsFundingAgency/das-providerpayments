using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Services;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.UnitTests.ServicesTests.GivenAProviderPaymentsCalculator
{
    public static class PaymentEntityTestExtensions
    {
        public static void ShouldContainPaymentMatchingEarning(this List<PaymentEntity> source, AdjustmentEntity earning)
        {
            source.Should().Contain(x => x.Ukprn == earning.Ukprn &&
                                         x.Amount == earning.Amount &&
                                         x.PaymentType == earning.PaymentType &&
                                         x.SubmissionId == earning.SubmissionId &&
                                         x.SubmissionCollectionPeriod == earning.SubmissionCollectionPeriod);
        }
    }

    [TestFixture]
    public class WhenCalculatingPayments
    {
        static void AssociatePaymentWithEarning(AdjustmentEntity payment, AdjustmentEntity earning)
        {
            payment.Ukprn = earning.Ukprn;
            payment.PaymentType = earning.PaymentType;
            payment.SubmissionCollectionPeriod = earning.SubmissionCollectionPeriod;
        }

        [TestFixture]
        public class WithNoPreviousPayments
        {
            [Test, AutoData]
            public void ThenTheTotalAmountPaidMatchesTheEarnings(
                ProviderAdjustmentsCalculator sut,
                List<AdjustmentEntity> testEarnings
                )
            {
                var actual = sut.CalculatePaymentsAndRefunds(new List<AdjustmentEntity>(), testEarnings);

                var expectedTotal = testEarnings.Sum(x => x.Amount);

                actual.Should().HaveCount(testEarnings.Count);
                actual.Sum(x => x.Amount).Should().Be(expectedTotal);
            }

            [Test, AutoData]
            public void ThenThereAreMatchingPayments(
                ProviderAdjustmentsCalculator sut,
                List<AdjustmentEntity> testEarnings
            )
            {
                var actual = sut.CalculatePaymentsAndRefunds(new List<AdjustmentEntity>(), testEarnings).ToList();

                actual.ShouldContainPaymentMatchingEarning(testEarnings[0]);
                actual.ShouldContainPaymentMatchingEarning(testEarnings[1]);
                actual.ShouldContainPaymentMatchingEarning(testEarnings[2]);
            }
        }

        [TestFixture]
        public class WithOneSetOfPreviousPayments
        {
            [Test, AutoData]
            public void ThenTheTotalAmountPaidMatchesTheEarnings(
                ProviderAdjustmentsCalculator sut,
                List<AdjustmentEntity> testEarnings,
                List<AdjustmentEntity> testPreviousPayments
            )
            {
                for (var i = 0; i < 3; i++)
                {
                    AssociatePaymentWithEarning(testPreviousPayments[i], testEarnings[i]);
                }

                var actual = sut.CalculatePaymentsAndRefunds(testPreviousPayments, testEarnings);

                var expectedTotal = testEarnings.Sum(x => x.Amount) - testPreviousPayments.Sum(x => x.Amount);

                actual.Should().HaveCount(testEarnings.Count);
                actual.Sum(x => x.Amount).Should().Be(expectedTotal);
            }

            [Test, AutoData]
            public void ThenThereAreMatchingPayments(
                ProviderAdjustmentsCalculator sut,
                List<AdjustmentEntity> testEarnings,
                List<AdjustmentEntity> testPreviousPayments
            )
            {
                for (var i = 0; i < 3; i++)
                {
                    AssociatePaymentWithEarning(testPreviousPayments[i], testEarnings[i]);
                }

                var actual = sut.CalculatePaymentsAndRefunds(testPreviousPayments, testEarnings).ToList();

                testEarnings[0].Amount -= testPreviousPayments[0].Amount;
                testEarnings[1].Amount -= testPreviousPayments[1].Amount;
                testEarnings[2].Amount -= testPreviousPayments[2].Amount;

                actual.ShouldContainPaymentMatchingEarning(testEarnings[0]);
                actual.ShouldContainPaymentMatchingEarning(testEarnings[1]);
                actual.ShouldContainPaymentMatchingEarning(testEarnings[2]);
            }

            [Test, AutoData]
            public void AndThereIsNoChangeThereAreNoPayments(
                ProviderAdjustmentsCalculator sut,
                List<AdjustmentEntity> testEarnings,
                List<AdjustmentEntity> testPreviousPayments
            )
            {
                for (var i = 0; i < 3; i++)
                {
                    AssociatePaymentWithEarning(testPreviousPayments[i], testEarnings[i]);
                    testPreviousPayments[i].SubmissionId = testEarnings[i].SubmissionId;
                    testPreviousPayments[i].Amount = testEarnings[i].Amount;
                }

                var actual = sut.CalculatePaymentsAndRefunds(testPreviousPayments, testEarnings);

                actual.Sum(x => x.Amount).Should().Be(0);
            }
        }

        [TestFixture]
        public class WithNoEarningsForPreviousPayments
        {
            [Test, AutoData]
            public void ThenTheTotalAmountPaidMatchesThePreviousPayments(
                ProviderAdjustmentsCalculator sut,
                List<AdjustmentEntity> testPreviousPayments
            )
            {
                var actual = sut.CalculatePaymentsAndRefunds(testPreviousPayments, new List<AdjustmentEntity>());

                var expectedTotal = -1 * testPreviousPayments.Sum(x => x.Amount);

                actual.Should().HaveCount(testPreviousPayments.Count);
                actual.Sum(x => x.Amount).Should().Be(expectedTotal);
            }

            [Test, AutoData]
            public void ThenThereAreOppositePayments(
                ProviderAdjustmentsCalculator sut,
                List<AdjustmentEntity> testPreviousPayments
            )
            {
                var actual = sut.CalculatePaymentsAndRefunds(testPreviousPayments, new List<AdjustmentEntity>()).ToList();

                testPreviousPayments[0].Amount *= -1;
                testPreviousPayments[1].Amount *= -1;
                testPreviousPayments[2].Amount *= -1;

                actual.ShouldContainPaymentMatchingEarning(testPreviousPayments[0]);
                actual.ShouldContainPaymentMatchingEarning(testPreviousPayments[1]);
                actual.ShouldContainPaymentMatchingEarning(testPreviousPayments[2]);
            }
        }
    }
}
