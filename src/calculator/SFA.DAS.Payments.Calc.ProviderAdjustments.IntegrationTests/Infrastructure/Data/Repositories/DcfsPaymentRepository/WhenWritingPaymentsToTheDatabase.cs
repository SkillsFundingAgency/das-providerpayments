using System;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Tools;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Infrastructure.Data.Repositories.DcfsPaymentRepository
{
    public class WhenWritingPaymentsToTheDatabase
    {
        private IPaymentRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new ProviderAdjustments.Infrastructure.Data.Repositories.DcfsPaymentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [Test]
        public void ThenItShouldWriteAllPayments()
        {
            // Arrange
            var ukprn = 10007459;
            var submissionId = Guid.NewGuid();
            var payments = new[]
            {
                new PaymentEntity
                {
                    Ukprn = ukprn,
                    SubmissionId = submissionId,
                    SubmissionCollectionPeriod = 1,
                    SubmissionAcademicYear = 1617,
                    PaymentType = 58,
                    PaymentTypeName = "Audit Adjustments: 16-18 Levy Apprenticeships - Provider",
                    Amount = 123.45m,
                    CollectionPeriodName = "1617-R09",
                    CollectionPeriodMonth = 4,
                    CollectionPeriodYear = 2017
                },
                new PaymentEntity
                {
                    Ukprn = ukprn,
                    SubmissionId = submissionId,
                    SubmissionCollectionPeriod = 2,
                    SubmissionAcademicYear = 1617,
                    PaymentType = 58,
                    PaymentTypeName = "Audit Adjustments: 16-18 Levy Apprenticeships - Provider",
                    Amount = 123.45m,
                    CollectionPeriodName = "1617-R09",
                    CollectionPeriodMonth = 4,
                    CollectionPeriodYear = 2017
                },
                new PaymentEntity
                {
                    Ukprn = ukprn,
                    SubmissionId = submissionId,
                    SubmissionCollectionPeriod = 3,
                    SubmissionAcademicYear = 1617,
                    PaymentType = 58,
                    PaymentTypeName = "Audit Adjustments: 16-18 Levy Apprenticeships - Provider",
                    Amount = 123.45m,
                    CollectionPeriodName = "1617-R09",
                    CollectionPeriodMonth = 4,
                    CollectionPeriodYear = 2017
                }
            };

            // Act
            _repository.AddProviderAdjustments(payments);

            // Assert
            var dbPayments = TestDataHelper.GetPaymentsForProvider(ukprn);

            Assert.IsNotNull(dbPayments);
            Assert.AreEqual(3, dbPayments.Length);
        }
    }
}