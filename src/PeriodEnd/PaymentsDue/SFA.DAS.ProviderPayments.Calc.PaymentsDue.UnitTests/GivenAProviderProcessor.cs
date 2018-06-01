using System.Collections.Generic;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenAProviderProcessor
    {
        [Test, PaymentsDueAutoData]
        public void ThenItGetsLearnersForTheProvider(
            ProviderEntity provider,
            [Frozen] Mock<IProviderLearnersBuilder> mockLearnersBuilder,
            ProviderProcessor sut)
        {
            sut.Process(provider);

            mockLearnersBuilder.Verify(builder => builder.Build(provider.Ukprn), Times.Once);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItDoesSomethingForEachLearner(//todo:name
            ProviderEntity provider,
            Dictionary<string, Learner> learners,
            [Frozen] Mock<IProviderLearnersBuilder> mockLearnersBuilder,
            ProviderProcessor sut)
        {
            mockLearnersBuilder
                .Setup(builder => builder.Build(It.IsAny<long>()))
                .Returns(learners);

            sut.Process(provider);

            foreach (var learner in learners)
            {
                // todo: assert something is done
            }
        }
    }
}