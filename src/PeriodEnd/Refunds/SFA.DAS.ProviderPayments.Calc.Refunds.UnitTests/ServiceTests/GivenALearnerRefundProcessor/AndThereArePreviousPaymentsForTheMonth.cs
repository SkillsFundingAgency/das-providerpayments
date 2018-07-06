using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities.TestHelpers;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.ServiceTests.GivenALearnerRefundProcessor
{
    [TestFixture]
    public class AndThereArePreviousPaymentsForTheMonth
    {
        [TestFixture]
        public class AndThePaymentsAreSuffientToCoverTheRefund
        {
            [Test]
            [CreateMatchingRefundsAndPayments]
            public void ThenThereAreRefundPaymentsForAllPastPayments(
                List<RequiredPaymentEntity> refunds, 
                List<HistoricalPaymentEntity> payments,
                LearnerRefundProcessor sut)
            {
                var actual = sut.ProcessRefundsForLearner(refunds, payments);

                actual.Should().HaveCount(9);
            }
        }

        [TestFixture]
        public class AndThePaymentsAreNotSufficientToCoverTheRefund
        {
            [Test]
            [CreateMatchingRefundsAndPayments(hasMatchingPastPayments: false)]
            public void AndThereAreNoPastPayments_ThenTheRefundsIsZero(
                List<RequiredPaymentEntity> refunds,
                List<HistoricalPaymentEntity> payments,
                LearnerRefundProcessor sut)
            {
                var actual = sut.ProcessRefundsForLearner(refunds, payments);

                actual.Sum(x => x.Amount).Should().Be(0);
            }

            [TestFixture]
            public class AndThereAreSomePayments
            {
                [Test, RefundsAutoData]
                public void ThenTheRefundAmountIsThePaymentAmount(
                    LearnerRefundProcessor sut)
                {
                    var data = RefundGenerator.Generate(
                        numberOfRefunds: 1, 
                        refundAmount: -500, 
                        paymentAmount: 100, 
                        numberOfPayments:3);
                    
                    var actual = sut.ProcessRefundsForLearner(data.Refunds, data.AssociatedPayments);

                    actual.Sum(x => x.Amount).Should().Be(-300);
                }

                [Test, RefundsAutoData]
                public void ThenRefundsForTwoPeriodsAreCorrect(
                    LearnerRefundProcessor sut)
                {
                    var data = RefundGenerator.Generate(numberOfRefunds: 4, paymentAmount: 200);

                    var refundOne = data.Refunds[0];
                    refundOne.DeliveryMonth = 12;
                    refundOne.AmountDue = -700;

                    var refundTwo = data.Refunds[1];
                    refundTwo.DeliveryMonth = 10;
                    refundTwo.AmountDue = -700;

                    data.AssociatedPayments[0].DeliveryMonth = 9;
                    data.AssociatedPayments[1].DeliveryMonth = 9;
                    data.AssociatedPayments[2].DeliveryMonth = 9;

                    data.AssociatedPayments[3].DeliveryMonth = 10;
                    data.AssociatedPayments[4].DeliveryMonth = 10;
                    data.AssociatedPayments[5].DeliveryMonth = 10;

                    data.AssociatedPayments[6].DeliveryMonth = 11;
                    data.AssociatedPayments[7].DeliveryMonth = 11;
                    data.AssociatedPayments[8].DeliveryMonth = 11;

                    data.AssociatedPayments[9].DeliveryMonth = 12;
                    data.AssociatedPayments[10].DeliveryMonth = 12;
                    data.AssociatedPayments[11].DeliveryMonth = 12;


                    var actual = sut.ProcessRefundsForLearner(data.Refunds.Take(2).ToList(), data.AssociatedPayments);

                    actual.Sum(x => x.Amount).Should().BeApproximately(-1400, 0.00005m);
                }
            }
        }

        [TestFixture]
        public class AndThereAreMultipleRefundsWithDifferentTransactionTypes
        {
            [Test, RefundsAutoData]
            public void AndPaymentsSufficient_ThenTheRefundPaymentsAreCorrect(
                LearnerRefundProcessor sut)
            {
                var data = RefundGenerator.Generate(numberOfRefunds: 2, paymentAmount: 500);

                var refundOne = data.Refunds[0];
                refundOne.TransactionType = TransactionType.Balancing;
                refundOne.AmountDue = -900;
                var refundTwo = data.Refunds[1];
                refundTwo.TransactionType = TransactionType.Balancing16To18FrameworkUplift;
                refundTwo.AmountDue = -1200;

                data.AssociatedPayments[0].TransactionType = TransactionType.Balancing;
                data.AssociatedPayments[1].TransactionType = TransactionType.Balancing;
                data.AssociatedPayments[2].TransactionType = TransactionType.Balancing;

                data.AssociatedPayments[3].TransactionType = TransactionType.Balancing16To18FrameworkUplift;
                data.AssociatedPayments[4].TransactionType = TransactionType.Balancing16To18FrameworkUplift;
                data.AssociatedPayments[5].TransactionType = TransactionType.Balancing16To18FrameworkUplift;

                var actual = sut.ProcessRefundsForLearner(data.Refunds, data.AssociatedPayments);

                actual.Where(x => x.TransactionType == TransactionType.Balancing).Sum(x => x.Amount).Should().BeApproximately(-900, 0.00005m);
                actual.Where(x => x.TransactionType == TransactionType.Balancing16To18FrameworkUplift).Sum(x => x.Amount).Should().BeApproximately(-1200, 0.00005m);
            }

            [Test, RefundsAutoData]
            public void AndPaymentsNotSufficient_ThenTheRefundPaymentsAreTheSameAsTheMatchingPreviousPayments(
                LearnerRefundProcessor sut)
            {
                var data = RefundGenerator.Generate(numberOfRefunds: 2, refundAmount: -900, paymentAmount: 200);

                var refundOne = data.Refunds[0];
                refundOne.TransactionType = TransactionType.Balancing;
                refundOne.AmountDue = -900;
                var refundTwo = data.Refunds[1];
                refundTwo.TransactionType = TransactionType.Balancing16To18FrameworkUplift;
                refundTwo.AmountDue = -1200;

                data.AssociatedPayments[0].TransactionType = TransactionType.Balancing;
                data.AssociatedPayments[1].TransactionType = TransactionType.Balancing;
                data.AssociatedPayments[2].TransactionType = TransactionType.Balancing;

                data.AssociatedPayments[3].TransactionType = TransactionType.Balancing16To18FrameworkUplift;
                data.AssociatedPayments[4].TransactionType = TransactionType.Balancing16To18FrameworkUplift;
                data.AssociatedPayments[5].TransactionType = TransactionType.Balancing16To18FrameworkUplift;
                data.AssociatedPayments[3].Amount = 100;
                data.AssociatedPayments[4].Amount = 100;
                data.AssociatedPayments[5].Amount = 100;

                var actual = sut.ProcessRefundsForLearner(data.Refunds, data.AssociatedPayments);

                actual.Where(x => x.TransactionType == TransactionType.Balancing).Sum(x => x.Amount).Should().BeApproximately(-600, 0.00005m);
                actual.Where(x => x.TransactionType == TransactionType.Balancing16To18FrameworkUplift).Sum(x => x.Amount).Should().BeApproximately(-300, 0.00005m);
            }
        }

        [TestFixture]
        public class AndThereAreMultipleRefundsWithDifferentContractTypes
        {
            [Test, RefundsAutoData]
            public void AndPaymentsSufficient_ThenTheRefundPaymentsAreCorrect(
                LearnerRefundProcessor sut)
            {
                var data = RefundGenerator.Generate(numberOfRefunds: 2, refundAmount: -900, paymentAmount: 500);

                var refundOne = data.Refunds[0];
                refundOne.ApprenticeshipContractType = 1;
                var refundTwo = data.Refunds[1];
                refundTwo.ApprenticeshipContractType = 2;
                refundTwo.AmountDue = -1200;

                data.AssociatedPayments[0].ApprenticeshipContractType = 1;
                data.AssociatedPayments[1].ApprenticeshipContractType = 1;
                data.AssociatedPayments[2].ApprenticeshipContractType = 1;

                data.AssociatedPayments[3].ApprenticeshipContractType = 2;
                data.AssociatedPayments[4].ApprenticeshipContractType = 2;
                data.AssociatedPayments[5].ApprenticeshipContractType = 2;

                var actual = sut.ProcessRefundsForLearner(data.Refunds, data.AssociatedPayments);

                actual.Where(x => x.RequiredPaymentId == refundOne.Id).Sum(x => x.Amount).Should().BeApproximately(-900, 0.00005m);
                actual.Where(x => x.RequiredPaymentId == refundTwo.Id).Sum(x => x.Amount).Should().BeApproximately(-1200, 0.00005m);
            }

            [Test, RefundsAutoData]
            public void AndPaymentsNotSufficient_ThenTheRefundPaymentsAreTheSameAsTheMatchingPreviousPayments(
                LearnerRefundProcessor sut)
            {
                var data = RefundGenerator.Generate(numberOfRefunds: 2, refundAmount: -900, paymentAmount: 200);

                var refundOne = data.Refunds[0];
                refundOne.ApprenticeshipContractType = 1;
                var refundTwo = data.Refunds[1];
                refundTwo.ApprenticeshipContractType = 2;
                refundTwo.AmountDue = -1200;

                data.AssociatedPayments[0].ApprenticeshipContractType = 1;
                data.AssociatedPayments[1].ApprenticeshipContractType = 1;
                data.AssociatedPayments[2].ApprenticeshipContractType = 1;

                data.AssociatedPayments[3].ApprenticeshipContractType = 2;
                data.AssociatedPayments[4].ApprenticeshipContractType = 2;
                data.AssociatedPayments[5].ApprenticeshipContractType = 2;
                data.AssociatedPayments[3].Amount = 100;
                data.AssociatedPayments[4].Amount = 100;
                data.AssociatedPayments[5].Amount = 100;

                var actual = sut.ProcessRefundsForLearner(data.Refunds, data.AssociatedPayments);

                actual.Where(x => x.RequiredPaymentId == refundOne.Id).Sum(x => x.Amount).Should().BeApproximately(-600, 0.00005m);
                actual.Where(x => x.RequiredPaymentId == refundTwo.Id).Sum(x => x.Amount).Should().BeApproximately(-300, 0.00005m);
            }
        }

        [TestFixture]
        public class AndThereAreMultipleRefundsWithDifferentAccounts
        {
            [Test, RefundsAutoData]
            public void AndPaymentsSufficient_ThenTheRefundPaymentsAreCorrect(
                LearnerRefundProcessor sut)
            {
                var data = RefundGenerator.Generate(numberOfRefunds: 2, refundAmount: -900, paymentAmount: 500);

                var refundOne = data.Refunds[0];
                refundOne.AccountId = 1000;
                var refundTwo = data.Refunds[1];
                refundTwo.AccountId = 2000;
                refundTwo.AmountDue = -1200;

                data.AssociatedPayments[0].AccountId = 1000;
                data.AssociatedPayments[1].AccountId = 1000;
                data.AssociatedPayments[2].AccountId = 1000;

                data.AssociatedPayments[3].AccountId = 2000;
                data.AssociatedPayments[4].AccountId = 2000;
                data.AssociatedPayments[5].AccountId = 2000;

                var actual = sut.ProcessRefundsForLearner(data.Refunds, data.AssociatedPayments);

                actual.Where(x => x.RequiredPaymentId == refundOne.Id).Sum(x => x.Amount).Should().BeApproximately(-900, 0.00005m);
                actual.Where(x => x.RequiredPaymentId == refundTwo.Id).Sum(x => x.Amount).Should().BeApproximately(-1200, 0.00005m);
            }

            [Test, RefundsAutoData]
            public void AndPaymentsNotSufficient_ThenTheRefundPaymentsAreTheSameAsTheMatchingPreviousPayments(
                LearnerRefundProcessor sut)
            {
                var data = RefundGenerator.Generate(numberOfRefunds: 2, refundAmount: -900, paymentAmount: 200);

                var refundOne = data.Refunds[0];
                refundOne.AccountId = 1000;
                var refundTwo = data.Refunds[1];
                refundTwo.AccountId = 2000;
                refundTwo.AmountDue = -1200;

                data.AssociatedPayments[0].AccountId = 1000;
                data.AssociatedPayments[1].AccountId = 1000;
                data.AssociatedPayments[2].AccountId = 1000;

                data.AssociatedPayments[3].AccountId = 2000;
                data.AssociatedPayments[4].AccountId = 2000;
                data.AssociatedPayments[5].AccountId = 2000;
                data.AssociatedPayments[3].Amount = 100;
                data.AssociatedPayments[4].Amount = 100;
                data.AssociatedPayments[5].Amount = 100;

                var actual = sut.ProcessRefundsForLearner(data.Refunds, data.AssociatedPayments);

                actual.Where(x => x.RequiredPaymentId == refundOne.Id).Sum(x => x.Amount).Should().BeApproximately(-600, 0.00005m);
                actual.Where(x => x.RequiredPaymentId == refundTwo.Id).Sum(x => x.Amount).Should().BeApproximately(-300, 0.00005m);
            }
        }
        
        [TestFixture]
        public class AndThereAreNetNegativeFundingSources
        {
            [Test]
            [CreateMatchingRefundsAndPayments(hasNegativeFundingSources: true)]
            public void ThenThereAreNoRefundsForPaymentsThatHaveNegativeFundingSources(
                List<RequiredPaymentEntity> refunds,
                List<HistoricalPaymentEntity> payments,
                LearnerRefundProcessor sut)
            {
                var actual = sut.ProcessRefundsForLearner(refunds, payments);

                actual.Should().HaveCount(0);
            }
        }

        [TestFixture]
        public class AndThereIsOneRefund
        {
            [TestFixture]
            public class AndThePastPaymentsForRefundMonthDoNotCoverRefund
            {
                [TestFixture]
                public class AndThePaymentsForThePreviousMonthCoverTheRefund
                {
                    [Test]
                    [CreateMatchingRefundsAndPayments(paymentAmount:200)]
                    public void ThenTheRefundPaymentAmountMatchesTheRefundAmount(
                        List<RequiredPaymentEntity> refunds,
                        List<HistoricalPaymentEntity> payments,
                        LearnerRefundProcessor sut)
                    {
                        var refund = refunds.Latest();
                        var actual = sut.ProcessRefundsForLearner(new List<RequiredPaymentEntity>{refund}, payments);

                        var expectedAmount = refund.AmountDue;
                        actual.Sum(x => x.Amount).Should().Be(expectedAmount);
                    }
                }
            }
        }

        [TestFixture]
        public class AndThereAreMultipleRefunds
        {
            [TestFixture]
            public class AndThereIsOneRefundForPeriodThreeWithoutEnoughPayments
            {
                [TestFixture]
                public class AndThereIsOneRefundForPeriodTwo
                {
                    [Test, RefundsAutoData]
                    public void ThenThePaymentsAreCorrect(
                        LearnerRefundProcessor sut)
                    {
                        var data = RefundGenerator.Generate(numberOfRefunds:2, paymentAmount:200);
                        var refundOne = data.Refunds[0];
                        refundOne.DeliveryMonth = 9;
                        refundOne.AmountDue = -700;
                        var refundTwo = data.Refunds[1];
                        refundTwo.AmountDue = -500;

                        data.AssociatedPayments[3].DeliveryMonth = 9;
                        data.AssociatedPayments[4].DeliveryMonth = 9;
                        data.AssociatedPayments[5].DeliveryMonth = 9;

                        var actual = sut.ProcessRefundsForLearner(data.Refunds, data.AssociatedPayments);

                        actual.Sum(x => x.Amount).Should().BeApproximately(-1200, 0.00005m);
                    }
                }
            }
        }
    }

    [TestFixture]
    public class AndThereAreNoPreviousPayments
    {
        [Test]
        [CreateMatchingRefundsAndPayments(hasMatchingPastPayments: false)]
        public void ThenThereAreNoRefunds(
            List<RequiredPaymentEntity> refunds,
            List<HistoricalPaymentEntity> payments,
            LearnerRefundProcessor sut)
        {
            var actual = sut.ProcessRefundsForLearner(refunds, payments);

            actual.Should().HaveCount(0);
        }
    }
}
