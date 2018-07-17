using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.StructureMap.Infrastructure;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories;
using SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.Attributes;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenAPaymentsHistoryRepository
    {
        private HistoricalPaymentsRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new HistoricalPaymentsRepository(new DcConfiguration(new ContextWrapper(new RefundsTestContext())));
        }

        [TestFixture, SetupNoPaymentsHistory]
        public class AndThereAreNoHistoricalPaymentsForProvider : GivenAPaymentsHistoryRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreNoHistoricalPaymentsForProvider
            {
                [Test]
                public void ThenItReturnsAnEmptyList()
                {
                    var result = _sut.GetAllForProvider(RefundsTestContext.Ukprn);
                    result.Should().BeEmpty();
                }
            }
        }

        [TestFixture, SetupPaymentsHistory]
        public class AndThereAreSomeHistoricalPaymentsForProvider : GivenAPaymentsHistoryRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreSomeHistoricalPaymentsForProvider
            {
                private List<HistoricalPaymentEntity> _actualHistoricalPayments;
                private List<HistoricalPaymentEntity> _expectedHistoricalPayments;

                [SetUp]
                public new void Setup()
                {
                    _actualHistoricalPayments = _sut.GetAllForProvider(RefundsTestContext.Ukprn).ToList();

                    _expectedHistoricalPayments = RefundsTestContext.PaymentsHistory
                        .Where(payment => payment.Ukprn == RefundsTestContext.Ukprn).ToList();
                }

                [Test]
                public void ThenItRetrievesExpectedCount()
                {
                    if (_expectedHistoricalPayments.Count < 1)
                        Assert.Fail("Not enough historical payments to test");

                    _actualHistoricalPayments.Count.Should().Be(_expectedHistoricalPayments.Count);
                }

                [Test]
                public void ThenPaymentIdIsSetCorrectly() =>
                    _actualHistoricalPayments[0].PaymentId.Should().Be(_expectedHistoricalPayments[0].PaymentId);

                [Test]
                public void ThenRequiredPaymentIdIsSetCorrectly() =>
                    _actualHistoricalPayments[0].RequiredPaymentId.Should().Be(_expectedHistoricalPayments[0].RequiredPaymentId);

                [Test]
                public void ThenDeliveryMonthIsSetCorrectly() =>
                    _actualHistoricalPayments[0].DeliveryMonth.Should().Be(_expectedHistoricalPayments[0].DeliveryMonth);

                [Test]
                public void ThenDeliveryYearIsSetCorrectly() =>
                    _actualHistoricalPayments[0].DeliveryYear.Should().Be(_expectedHistoricalPayments[0].DeliveryYear);

                [Test]
                public void ThenCollectionPeriodNameIsSetCorrectly() =>
                    _actualHistoricalPayments[0].CollectionPeriodName.Should().Be(_expectedHistoricalPayments[0].CollectionPeriodName);

                [Test]
                public void ThenCollectionPeriodMonthIsSetCorrectly() =>
                    _actualHistoricalPayments[0].CollectionPeriodMonth.Should().Be(_expectedHistoricalPayments[0].CollectionPeriodMonth);

                [Test]
                public void ThenCollectionPeriodYearIsSetCorrectly() =>
                    _actualHistoricalPayments[0].CollectionPeriodYear.Should().Be(_expectedHistoricalPayments[0].CollectionPeriodYear);

                [Test]
                public void ThenFundingSourceIsSetCorrectly() =>
                    _actualHistoricalPayments[0].FundingSource.Should().Be(_expectedHistoricalPayments[0].FundingSource);

                [Test]
                public void ThenTransactionTypeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].TransactionType.Should().Be(_expectedHistoricalPayments[0].TransactionType);

                [Test]
                public void ThenAmountIsSetCorrectly() =>
                    _actualHistoricalPayments[0].Amount.Should().Be(_expectedHistoricalPayments[0].Amount);

                [Test]
                public void ThenApprenticeshipContractTypeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].ApprenticeshipContractType.Should().Be(_expectedHistoricalPayments[0].ApprenticeshipContractType);

                [Test]
                public void ThenUkprnIsSetCorrectly() =>
                    _actualHistoricalPayments[0].Ukprn.Should().Be(_expectedHistoricalPayments[0].Ukprn);

                [Test]
                public void ThenAccountIdIsSetCorrectly() =>
                    _actualHistoricalPayments[0].AccountId.Should().Be(_expectedHistoricalPayments[0].AccountId);

                [Test]
                public void ThenLearnRefNumberIsSetCorrectly() =>
                    _actualHistoricalPayments[0].LearnRefNumber.Should().Be(_expectedHistoricalPayments[0].LearnRefNumber);

                [Test]
                public void ThenFundingLineTypeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].FundingLineType.Should().Be(_expectedHistoricalPayments[0].FundingLineType);
            }
        }
    }
}