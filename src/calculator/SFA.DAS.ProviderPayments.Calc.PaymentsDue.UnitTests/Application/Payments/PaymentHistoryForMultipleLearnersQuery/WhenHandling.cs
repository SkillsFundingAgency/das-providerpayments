using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Payments.PaymentHistoryForMultipleLearnersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.Testing.Shared;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.Payments.PaymentHistoryForMultipleLearnersQuery
{
    [TestFixture]
    public class WhenHandling
    {
        [Test, AutoMoqData]
        public void ThenItShouldReturnInvalidResponseOnException(
            [Frozen] Mock<IRequiredPaymentRepository> repository,
            PaymentHistoryForMultipleLearnersQueryHandler sut,
            PaymentHistoryForMultipleLearnersRequest request)
        {
            repository.Setup(x =>
                    x.GetPreviousPaymentsForMultipleLearners(It.IsAny<long>(), It.IsAny<IEnumerable<string>>()))
                .Throws<Exception>();

            var actual = sut.Handle(request);

            actual.IsValid.Should().BeFalse();
            actual.Exception.Should().NotBeNull();
        }

        [Test, AutoMoqData]
        public void ThenItShouldAlwaysReturnsANonNullValue(
            [Frozen] Mock<IRequiredPaymentRepository> repository,
            PaymentHistoryForMultipleLearnersQueryHandler sut,
            PaymentHistoryForMultipleLearnersRequest request)
        {
            repository.Setup(x =>
                    x.GetPreviousPaymentsForMultipleLearners(It.IsAny<long>(), It.IsAny<IEnumerable<string>>()))
                .Returns<PaymentHistoryForMultipleLearnersResponse>(null);

            var actual = sut.Handle(request);

            actual.IsValid.Should().BeTrue();
            actual.Items.Should().NotBeNull();
        }

        [Test, AutoMoqData]
        public void ThenItShouldTransformValues(
            [Frozen] Mock<IRequiredPaymentRepository> repository,
            PaymentHistoryForMultipleLearnersQueryHandler sut,
            PaymentHistoryForMultipleLearnersRequest request,
            List<HistoricalRequiredPaymentEntity> payments)
        {
            repository.Setup(x => x.GetPreviousPaymentsForMultipleLearners(request.Ukprn, request.LearnRefNumbers))
                .Returns(payments.ToArray);

            var actual = sut.Handle(request);

            actual.IsValid.Should().BeTrue();
            actual.Items.Length.Should().Be(3);
            actual.Items.Should().BeAssignableTo<RequiredPayment[]>();
            actual.Items[0].AccountId.Should().Be(payments[0].AccountId);
        }
    }
}
