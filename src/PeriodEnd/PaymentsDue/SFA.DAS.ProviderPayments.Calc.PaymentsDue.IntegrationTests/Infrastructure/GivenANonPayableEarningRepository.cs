using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenANonPayableEarningRepository
    {
        private NonPayableEarningRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new NonPayableEarningRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupNoNonPayableEarnings]
        public class AndThereAreNoNonPayableEarnings : GivenANonPayableEarningRepository
        {
            [TestFixture]
            public class WhenCallingAddMany : AndThereAreNoNonPayableEarnings
            {
                [Test, AutoData, Ignore("for now")]
                public void ThenItReturnsAnEmptyList(List<NonPayableEarningEntity> nonPayableEarnings)
                {
                    Setup();

                    _sut.AddMany(nonPayableEarnings);

                    NonPayableEarningsDataHelper.GetAll().Count.Should().Be(nonPayableEarnings.Count);
                }
            }
        }
    }
}