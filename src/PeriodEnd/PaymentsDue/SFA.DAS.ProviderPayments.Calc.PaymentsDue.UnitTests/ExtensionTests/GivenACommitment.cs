using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ExtensionTests
{
    [TestFixture]
    public class GivenACommitment
    {
        [Theory, PaymentsDueAutoData]
        public void ThenTransferCommitmentInformationCopiesAccountId(
            Commitment testCommitment, FundingDue actual)
        {
            testCommitment.TransferCommitmentInformationTo(actual);

            actual.AccountId.Should().Be(testCommitment.AccountId);
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTransferCommitmentInformationCopiesCommitmentId(
            Commitment testCommitment, FundingDue actual)
        {
            testCommitment.TransferCommitmentInformationTo(actual);

            actual.CommitmentId.Should().Be(testCommitment.CommitmentId);
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTransferCommitmentInformationCopiesCommitmentVersionId(
            Commitment testCommitment, FundingDue actual)
        {
            testCommitment.TransferCommitmentInformationTo(actual);

            actual.CommitmentVersionId.Should().Be(testCommitment.CommitmentVersionId);
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTransferCommitmentInformationCopiesAccountVersionId(
            Commitment testCommitment, FundingDue actual)
        {
            testCommitment.TransferCommitmentInformationTo(actual);

            actual.AccountVersionId.Should().Be(testCommitment.AccountVersionId);
        }
    }
}
