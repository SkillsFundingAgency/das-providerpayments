using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Tests.Infrastructure
{
    [TestFixture]
    public class GivenAPaymentRepository
    {
        private PaymentRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new PaymentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        public class AndSchemaIsRefunds : WhenCallingAddMany
        {
            [OneTimeSetUp]
            public void SetupForRefunds()
            {
                Setup(PaymentSchema.Refunds);
            }
        }

        // todo: AndSchemaIsLevyPayments

        // todo: AndSchemaIsCoInvestedPayments

        [TestFixture]
        public abstract class WhenCallingAddMany : GivenAPaymentRepository
        {
            private List<PaymentEntity> _expectedEntities;
            private List<PaymentEntity> _actualEntities;

            protected void Setup(PaymentSchema paymentSchema)
            {
                _expectedEntities = new Fixture()
                    .Build<PaymentEntity>()
                    .CreateMany()
                    .OrderBy(entity => entity.RequiredPaymentId)
                    .ToList();

                PaymentDataHelper.Truncate(paymentSchema);

                _sut.AddMany(_expectedEntities, paymentSchema);

                _actualEntities = PaymentDataHelper
                    .GetAll(paymentSchema)
                    .OrderBy(entity => entity.RequiredPaymentId)
                    .ToList();	
            }

            [Test]	
            public void ThenItSavesTheExpectedNumberOfEntities() =>
                _actualEntities.Count
                    .Should().Be(_expectedEntities.Count);

            [Test]	
            public void ThenItSetsDeliveryYear() =>
                _actualEntities[0].DeliveryYear
                    .Should().Be(_expectedEntities[0].DeliveryYear);

            [Test]
            public void ThenItSetsDeliveryMonth() =>
                _actualEntities[0].DeliveryMonth
                    .Should().Be(_expectedEntities[0].DeliveryMonth);

            [Test]
            public void ThenItSetsAmount() =>
                _actualEntities[0].Amount
                    .Should().BeApproximately(_expectedEntities[0].Amount, 0.00005m);

            [Test]
            public void ThenItSetsRequiredPaymentId() =>
                _actualEntities[0].RequiredPaymentId
                    .Should().Be(_expectedEntities[0].RequiredPaymentId);

            [Test]
            public void ThenItSetsCollectionPeriodName() =>
                _actualEntities[0].CollectionPeriodName
                    .Should().Be(_expectedEntities[0].CollectionPeriodName);

            [Test]
            public void ThenItSetsCollectionPeriodMonth() =>
                _actualEntities[0].CollectionPeriodMonth
                    .Should().Be(_expectedEntities[0].CollectionPeriodMonth);

            [Test]
            public void ThenItSetsCollectionPeriodYear() =>
                _actualEntities[0].CollectionPeriodYear
                    .Should().Be(_expectedEntities[0].CollectionPeriodYear);

            [Test]
            public void ThenItSetsFundingSource() =>
                _actualEntities[0].FundingSource
                    .Should().Be(_expectedEntities[0].FundingSource);

            [Test]
            public void ThenItSetsTransactionType() =>
                _actualEntities[0].TransactionType
                    .Should().Be(_expectedEntities[0].TransactionType);
        }
    }
}