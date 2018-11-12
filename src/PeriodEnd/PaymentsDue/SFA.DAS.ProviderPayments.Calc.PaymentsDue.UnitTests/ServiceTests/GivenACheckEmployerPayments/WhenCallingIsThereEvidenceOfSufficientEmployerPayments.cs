using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ServiceTests.GivenACheckEmployerPayments
{
    [TestFixture]
    public class WhenCallingIsThereEvidenceOfSufficientEmployerPayments
    {
        [Test, PaymentsDueInlineAutoData()]
        public void ThenItThrowsArgumentExceptionWhenNoPaymentHistory(
            CheckEmployerPayments sut)
        {
            Action test = () => sut.EvidenceOfSufficientEmployerPayments(null, new RawEarning());

            test.Should().Throw<ArgumentException>().And.Message.Should().ContainEquivalentOf("employerPayments");
        }

        [Test, PaymentsDueInlineAutoData()]
        public void ThenItThrowsArgumentExceptionWhenNoEarnings(
            CheckEmployerPayments sut)
        {
            Action test = () => sut.EvidenceOfSufficientEmployerPayments(new List<LearnerSummaryPaymentEntity>(), null);

            test.Should().Throw<ArgumentException>().And.Message.Should().ContainEquivalentOf("rawEarning");
        }

        [TestFixture]
        public class AndThereIsAnExemptionCode
        {
            [Test, PaymentsDueAutoData]
            public void ThenItReturnsTrue(
                RawEarning testEarning,
                List<LearnerSummaryPaymentEntity> testPayments,
                CheckEmployerPayments sut)
            {
                foreach (var learnerSummaryPaymentEntity in testPayments)
                {
                    testEarning.CopyPaymentMetadataTo(learnerSummaryPaymentEntity);
                    learnerSummaryPaymentEntity.Amount = 0;
                }

                var actual = sut.EvidenceOfSufficientEmployerPayments(testPayments, testEarning);

                actual.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndThereIsNoExemptionCode
        {
            [TestFixture]
            public class AndTheEvidenceIsMoreThanThePayments
            {
                [Test, PaymentsDueAutoData]
                public void ThenItReturnsTrue(
                    RawEarning testEarning,
                    List<LearnerSummaryPaymentEntity> testPayments,
                    CheckEmployerPayments sut)
                {
                    testEarning.ExemptionCodeForCompletionHoldback = 0;

                    foreach (var learnerSummaryPaymentEntity in testPayments)
                    {
                        testEarning.CopyPaymentMetadataTo(learnerSummaryPaymentEntity);
                        learnerSummaryPaymentEntity.Amount = testEarning.CumulativePmrs;
                    }

                    var actual = sut.EvidenceOfSufficientEmployerPayments(testPayments, testEarning);

                    actual.Should().BeTrue();
                }
            }

            [TestFixture]
            public class AndTheEvidenceIsLessThanThePayments
            {
                [Test, PaymentsDueAutoData]
                public void ThenItReturnsFalse(
                    RawEarning testEarning,
                    List<LearnerSummaryPaymentEntity> testPayments,
                    CheckEmployerPayments sut)
                {
                    testEarning.ExemptionCodeForCompletionHoldback = 0;

                    foreach (var learnerSummaryPaymentEntity in testPayments)
                    {
                        testEarning.CopyPaymentMetadataTo(learnerSummaryPaymentEntity);
                        learnerSummaryPaymentEntity.Amount = testEarning.CumulativePmrs / 4;
                    }

                    var actual = sut.EvidenceOfSufficientEmployerPayments(testPayments, testEarning);

                    actual.Should().BeTrue();
                }
            }

            [TestFixture]
            public class AndTheEvidenceIsExactlyThePayments
            {
                [Test, PaymentsDueAutoData]
                public void ThenItReturnsTrue(
                    RawEarning testEarning,
                    List<LearnerSummaryPaymentEntity> testPayments,
                    CheckEmployerPayments sut)
                {
                    var exactlyOneThird = testEarning.CumulativePmrs;
                    testEarning.CumulativePmrs *= 3;

                    testEarning.ExemptionCodeForCompletionHoldback = 0;

                    foreach (var learnerSummaryPaymentEntity in testPayments)
                    {
                        testEarning.CopyPaymentMetadataTo(learnerSummaryPaymentEntity);
                        learnerSummaryPaymentEntity.Amount = exactlyOneThird;
                    }

                    var actual = sut.EvidenceOfSufficientEmployerPayments(testPayments, testEarning);

                    actual.Should().BeTrue();
                }
            }
        }
    }
}
