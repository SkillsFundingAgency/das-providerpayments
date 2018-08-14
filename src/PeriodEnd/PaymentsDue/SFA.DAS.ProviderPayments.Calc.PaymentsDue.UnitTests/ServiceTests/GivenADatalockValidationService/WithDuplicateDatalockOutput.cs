﻿using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ServiceTests.GivenADatalockValidationService
{
    [TestFixture]
    public class WithDuplicateDatalockOutput
    {
        private static List<DatalockOutputEntity> RawDatalocks { get; set; }
        private static List<Commitment> Commitments { get; set; }

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            var commitmentId = fixture.Create<long>();

            RawDatalocks = fixture.Build<DatalockOutputEntity>()
                .With(x => x.CommitmentId, commitmentId)
                .With(x => x.Payable, true)
                .CreateMany(10)
                .ToList();

            Commitments = fixture.Build<Commitment>()
                .With(x => x.CommitmentId, commitmentId)
                .CreateMany(1)
                .ToList();
        }

        [Test, PaymentsDueAutoData]
        public void ThenThereShouldBeNoDuplicateOutput(DatalockValidationService sut)
        {
            RawDatalocks[1] = RawDatalocks[0];
            RawDatalocks[2] = RawDatalocks[0];
            RawDatalocks[3] = RawDatalocks[0];

            var actual = sut.ProcessDatalocks(RawDatalocks, new List<DatalockValidationError>(), Commitments);

            actual.Should().HaveCount(7);
        }
    }
}