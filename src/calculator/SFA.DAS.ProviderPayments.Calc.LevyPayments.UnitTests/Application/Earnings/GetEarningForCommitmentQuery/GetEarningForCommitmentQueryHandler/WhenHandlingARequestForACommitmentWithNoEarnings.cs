using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.Earnings.GetEarningForCommitmentQuery.GetEarningForCommitmentQueryHandler
{
    public class WhenHandlingARequestForACommitmentWithNoEarnings
    {
        private const string CommitmentId = "Commitment1";

        private GetEarningForCommitmentQueryRequest _request;
        private Mock<IEarningRepository> _earningRepository;
        private LevyPayments.Application.Earnings.GetEarningForCommitmentQuery.GetEarningForCommitmentQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetEarningForCommitmentQueryRequest
            {
                CommitmentId = CommitmentId
            };

            _earningRepository = new Mock<IEarningRepository>();
            _earningRepository.Setup(r => r.GetEarningForCommitment(CommitmentId))
                .Returns<EarningEntity>(null);

            _handler = new LevyPayments.Application.Earnings.GetEarningForCommitmentQuery.GetEarningForCommitmentQueryHandler(_earningRepository.Object);
        }

        [Test]
        public void ThenItShouldReturnAnInstanceOfGetEarningForCommitmentQueryResponse()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void ThenItShouldNotReturnAnEarning()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNull(actual.Earning);
        }
    }
}
