using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions.Common;
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
    public class GivenAProviderProcessor
    {
        [TestFixture]
        public class WhenCallingProcess
        {
            [Test, RefundsAutoData]
            public void ThenItGetsLearnersForTheProvider(
                ProviderEntity provider,
                [Frozen] Mock<ILearnerBuilder> learnerBuilder,
                ProviderProcessor sut)
            {
                sut.Process(provider);
                learnerBuilder.Verify(builder => builder.CreateLearnersForProvider(provider.Ukprn), Times.Once);
            }

            [Test, RefundsAutoData]
            public void ThenItProcessesEachLearnerAddingTheLearnerRefundsToAccountBalance(
                ProviderEntity provider,
                List<LearnerData> learners,
                [Frozen] Mock<ILearnerBuilder> learnerBuilder,
                [Frozen] Mock<ILearnerProcessor> learnerProcessor,
                [Frozen] Mock<ISummariseAccountBalances> summariseAccountBalances,
                ProviderProcessor sut,
                List<PaymentEntity>[] refunds
            )
            {
                learnerBuilder.Setup(builder => builder.CreateLearnersForProvider(provider.Ukprn))
                    .Returns(learners);

                for (var i = 0; i < learners.Count; i++)
                {
                    var learnerRefunds = refunds[i];
                    var learner = learners[i];
                    learnerProcessor.Setup(x => x.Process(learner)).Returns(learnerRefunds);
                }

                sut.Process(provider);

                for (var i = 0; i < learners.Count; i++)
                {
                    var learnerRefunds = refunds[i];
                    var learner = learners[i];
                    learnerProcessor.Verify(x => x.Process(learner), Times.Once);
                    summariseAccountBalances.Verify(x => x.IncrementAccountLevyBalance(learnerRefunds), Times.Once);
                }
            }

            [Test, RefundsAutoData]
            public void ThenItSavesAllRefundsForProvider(
                ProviderEntity provider,
                List<LearnerData> learners,
                [Frozen] Mock<ILearnerBuilder> learnerBuilder,
                [Frozen] Mock<ILearnerProcessor> learnerProcessor,
                [Frozen] Mock<IPaymentRepository> refundPaymentRepository,
                ProviderProcessor sut,
                List<PaymentEntity> refunds
            )
            {
                learnerBuilder.Setup(builder => builder.CreateLearnersForProvider(provider.Ukprn))
                    .Returns(learners);

                learnerProcessor.Setup(x => x.Process(It.IsAny<LearnerData>())).Returns(refunds);

                sut.Process(provider);

                refundPaymentRepository.Verify(
                    x => x.AddMany(
                        It.Is<List<PaymentEntity>>(p => p.Count() == learners.Count() * refunds.Count())),
                    Times.Once);
            }

            [Test, RefundsAutoData]
            public void ThenItReturnsTheLevyCreditsByAccount(
                ProviderEntity provider,
                [Frozen] Mock<ISummariseAccountBalances> summariseAccountBalances,
                ProviderProcessor sut,
                List<AccountLevyCredit> levyCredits)

            {
                summariseAccountBalances.Setup(x => x.AsList()).Returns(levyCredits);            

                var result = sut.Process(provider);

                result.IsSameOrEqualTo(levyCredits);
            }
        } 
    }
}