using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.ValidationErrorRepository
{
    public class WhenAddValidationErrorCalledDuringAnIlrSubmission
    {
        private IValidationErrorRepository _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _validationErrorRepository = new DataLock.Infrastructure.Data.Repositories.ValidationErrorRepository(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        [Test]
        public void ThenValidationErrorAddedSuccessfully()
        {
            // Arrange
            var validationError = new ValidationErrorBuilder().Build();

            // Act
            _validationErrorRepository.AddValidationError(validationError);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
        }
    }
}
