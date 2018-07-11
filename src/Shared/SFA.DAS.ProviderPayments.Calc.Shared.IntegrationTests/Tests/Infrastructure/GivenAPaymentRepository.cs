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

        [TestFixture]
        public class WhenCallingAddMany : GivenAPaymentRepository
        {
            private List<PaymentEntity> _expectedEntities;	
            private List<PaymentEntity> _actualEntities;

            [OneTimeSetUp]
            public void Setup()
            {
                _expectedEntities = new Fixture()
                    .Build<PaymentEntity>()
                    .CreateMany()
                    .ToList();

                PaymentDataHelper.Truncate(PaymentSchema.Refunds);

                _sut.AddMany(_expectedEntities, PaymentSchema.Refunds);

                _actualEntities = PaymentDataHelper.GetAll(PaymentSchema.Refunds).ToList();	
            }

            [Test]	
            public void ThenItSavesTheExpectedNumberOfEntities() =>
                _actualEntities.Count
                    .Should().Be(_expectedEntities.Count);
        }
    }
}