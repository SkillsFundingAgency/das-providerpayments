using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenAProviderProcessor
    {
        [Test, PaymentsDueAutoData]
        public void WhenCallingProcess_ThenItGetsLearnersForTheProvider(
            ProviderEntity provider,
            [Frozen] Mock<IProviderLearnersBuilder> mockLearnersBuilder,
            ProviderProcessor sut)
        {
            sut.Process(provider);

            mockLearnersBuilder.Verify(builder => builder.Build(provider.Ukprn), Times.Once);
        }

        [Test, PaymentsDueAutoData]
        public void WhenCallingProcess_ThenItSavesNonPayableEarningsForEachLearner(
            ProviderEntity provider,
            Dictionary<string, Learner> learners,
            [Frozen] Mock<IProviderLearnersBuilder> mockLearnersBuilder,
            [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
            ProviderProcessor sut)
        {
            var nonPayableEarnings = new List<List<NonPayableEarningEntity>>();

            mockLearnersBuilder
                .Setup(builder => builder.Build(It.IsAny<long>()))
                .Returns(learners);

            mockNonPayableEarningsRepository
                .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarningEntity>>()))
                .Callback<List<NonPayableEarningEntity>>(entities => nonPayableEarnings.Add(entities));

            sut.Process(provider);

            using (var e = learners.GetEnumerator())
            {
                for (var i = 0; i < learners.Count; i++)
                {
                    e.MoveNext();
                    e.Current.Value.NonPayableEarnings.ShouldAllBeEquivalentTo(nonPayableEarnings[i]);
                }
            }
        }

        [Test, PaymentsDueAutoData]
        public void WhenCallingProcess_ThenItSavesPaymentsForEachLearner(
            ProviderEntity provider,
            Dictionary<string, Learner> learners,
            [Frozen] Mock<IProviderLearnersBuilder> mockLearnersBuilder,
            [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
            ProviderProcessor sut)
        {
            var actualPayments = new List<RequiredPaymentEntity[]>();

            mockLearnersBuilder
                .Setup(builder => builder.Build(It.IsAny<long>()))
                .Returns(learners);

            mockRequiredPaymentsRepository
                .Setup(repository => repository.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                .Callback<RequiredPaymentEntity[]>(entities => actualPayments.Add(entities));

            sut.Process(provider);

            using (var e = learners.GetEnumerator())
            {
                for (var i = 0; i < learners.Count; i++)
                {
                    e.MoveNext();
                    e.Current.Value.RequiredPayments.ShouldAllBeEquivalentTo(actualPayments[i]);
                }
            }
        }
    }
}