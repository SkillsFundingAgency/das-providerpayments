﻿using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.ValidationErrorRepository
{
    public class WhenAddValidationErrorsCalledDuringAnIlrSubmission
    {
        private IDatalockRepository _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _validationErrorRepository = new DatalockRepository(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        [Test]
        public void ThenValidationErrorsAddedSuccessfully()
        {
            // Arrange
            var validationErrors = new[]
            {
                new ValidationErrorBuilder().Build(),
                new ValidationErrorBuilder().WithAimSeqNumber(2).WithRuleId(DataLockErrorCodes.MismatchingUln).Build(),

                new ValidationErrorBuilder().WithLearnRefNumber(string.Empty).Build(),
                new ValidationErrorBuilder().WithLearnRefNumber(null).Build(),

                new ValidationErrorBuilder().WithAimSeqNumber(0).Build(),

                new ValidationErrorBuilder().WithRuleId(string.Empty).Build(),
                new ValidationErrorBuilder().WithRuleId(null).Build(),

                new ValidationErrorBuilder().WithPriceEpisodeIdentifier("A").Build()
            };

            // Act
            _validationErrorRepository.WriteValidationErrors(validationErrors);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(8, errors.Length);
        }
    }
}
