using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests
{
    [TestFixture]
    public class GivenAProviderRefundProcessor
    {
        [TestFixture]
        public class WhenCallingProcess
        {
            [Test, RefundsAutoData]
            public void ThenItGetsLearnersForTheProvider(
                ProviderEntity provider,
                [Frozen] Mock<ILearnerBuilder> learnerBuilder,
                ProviderRefundsProcessor sut)
            {
                sut.Process(provider);
                learnerBuilder.Verify(builder => builder.CreateLearnersForProvider(provider.Ukprn), Times.Once);
            }

            [Test, RefundsAutoData]
            public void ThenItProcessesEachLearner(
                ProviderEntity provider,
                List<LearnerData> learners,
                [Frozen] Mock<ILearnerBuilder> learnerBuilder,
                [Frozen] Mock<ILearnerProcessor> learnerProcessor,
                [Frozen] Mock<ISummariseAccountBalances> summariseAccountBalances,
                ProviderRefundsProcessor sut,
                List<PaymentEntity>[] refunds
            )
            {
                learnerBuilder.Setup(builder => builder.CreateLearnersForProvider(provider.Ukprn))
                    .Returns(learners);

                sut.Process(provider);

                for (var i = 0; i < learners.Count; i++)
                {
                    var learnerRefunds = refunds[i];
                    var learner = learners[i];
                    learnerProcessor.Verify(x => x.Process(learner), Times.Once);
                }
            }

            [Test, RefundsAutoData]
            public void ThenItSavesAllRefundsForProviderAndReturnsAllRefunds(
                ProviderEntity provider,
                List<LearnerData> learners,
                [Frozen] Mock<ILearnerBuilder> learnerBuilder,
                [Frozen] Mock<ILearnerProcessor> learnerProcessor,
                [Frozen] Mock<IPaymentRepository> refundPaymentRepository,
                ProviderRefundsProcessor sut,
                List<PaymentEntity> refunds
            )
            {
                learnerBuilder.Setup(builder => builder.CreateLearnersForProvider(provider.Ukprn))
                    .Returns(learners);

                learnerProcessor.Setup(x => x.Process(It.IsAny<LearnerData>())).Returns(refunds);

                var result = sut.Process(provider);

                refundPaymentRepository.Verify(
                    x => x.AddMany(
                        It.Is<List<PaymentEntity>>(p => p.Count() == learners.Count() * refunds.Count())),
                    Times.Once);

                result.Count().Should().Be(learners.Count() * refunds.Count());
            }

        } 
    }
}