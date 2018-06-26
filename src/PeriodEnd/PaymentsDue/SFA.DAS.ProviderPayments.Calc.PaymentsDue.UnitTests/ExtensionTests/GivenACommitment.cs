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
        [Test, PaymentsDueAutoData]
        public void ThenTransferCommitmentInformationCopiesAccountId(
            Commitment testCommitment, FundingDue actual)
        {
            testCommitment.CopyCommitmentInformationTo(actual);

            actual.AccountId.Should().Be(testCommitment.AccountId);
        }

        [Test, PaymentsDueAutoData]
        public void ThenTransferCommitmentInformationCopiesCommitmentId(
            Commitment testCommitment, FundingDue actual)
        {
            testCommitment.CopyCommitmentInformationTo(actual);

            actual.CommitmentId.Should().Be(testCommitment.CommitmentId);
        }

        [Test, PaymentsDueAutoData]
        public void ThenTransferCommitmentInformationCopiesCommitmentVersionId(
            Commitment testCommitment, FundingDue actual)
        {
            testCommitment.CopyCommitmentInformationTo(actual);

            actual.CommitmentVersionId.Should().Be(testCommitment.CommitmentVersionId);
        }

        [Test, PaymentsDueAutoData]
        public void ThenTransferCommitmentInformationCopiesAccountVersionId(
            Commitment testCommitment, FundingDue actual)
        {
            testCommitment.CopyCommitmentInformationTo(actual);

            actual.AccountVersionId.Should().Be(testCommitment.AccountVersionId);
        }
    }
}
