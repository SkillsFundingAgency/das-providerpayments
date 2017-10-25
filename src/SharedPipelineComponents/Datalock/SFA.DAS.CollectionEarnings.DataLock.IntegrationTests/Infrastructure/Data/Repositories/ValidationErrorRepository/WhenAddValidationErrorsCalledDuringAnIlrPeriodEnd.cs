using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.ValidationErrorRepository
{
    public class WhenAddValidationErrorsCalledDuringAnIlrPeriodEnd
    {
        private IValidationErrorRepository _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _validationErrorRepository = new DataLock.Infrastructure.Data.Repositories.ValidationErrorRepository(GlobalTestContext.Instance.PeriodEndConnectionString);
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

                new ValidationErrorBuilder().WithAimSeqNumber(null).Build(),

                new ValidationErrorBuilder().WithRuleId(string.Empty).Build(),
                new ValidationErrorBuilder().WithRuleId(null).Build()
            };

            // Act
            _validationErrorRepository.AddValidationErrors(validationErrors);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(7, errors.Length);
        }
    }
}