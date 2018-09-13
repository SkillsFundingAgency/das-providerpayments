using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ExtensionTests
{
    [TestFixture]
    public class GivenAnEntityThatHoldsCourseInformation
    {
        [Test, PaymentsDueAutoData]
        public void ThenHasMatchingCourseInformationReturnsTrueWhenCourseInformationIsTheSame(
            FundingDue lhs, FundingDue rhs)
        {
            rhs.PathwayCode = lhs.PathwayCode;
            rhs.StandardCode = lhs.StandardCode;
            rhs.FrameworkCode = lhs.FrameworkCode;
            rhs.ProgrammeType = lhs.ProgrammeType;
            rhs.ApprenticeshipContractType = lhs.ApprenticeshipContractType;

            var actual = lhs.HasMatchingCourseInformationWith(rhs);

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasMatchingCourseInformationReturnsFalseWhenPathwayCodeIsDifferent(
            FundingDue lhs, FundingDue rhs)
        {
            rhs.PathwayCode = lhs.PathwayCode + 2;
            rhs.StandardCode = lhs.StandardCode;
            rhs.FrameworkCode = lhs.FrameworkCode;
            rhs.ProgrammeType = lhs.ProgrammeType;
            rhs.ApprenticeshipContractType = lhs.ApprenticeshipContractType;

            var actual = lhs.HasMatchingCourseInformationWith(rhs);

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasMatchingCourseInformationReturnsFalseWhenFrameworkCodeIsDifferent(
            FundingDue lhs, FundingDue rhs)
        {
            rhs.PathwayCode = lhs.PathwayCode;
            rhs.StandardCode = lhs.StandardCode;
            rhs.FrameworkCode = lhs.FrameworkCode + 2;
            rhs.ProgrammeType = lhs.ProgrammeType;
            rhs.ApprenticeshipContractType = lhs.ApprenticeshipContractType;

            var actual = lhs.HasMatchingCourseInformationWith(rhs);

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasMatchingCourseInformationReturnsFalseWhenStandardCodeIsDifferent(
            FundingDue lhs, FundingDue rhs)
        {
            rhs.PathwayCode = lhs.PathwayCode;
            rhs.StandardCode = lhs.StandardCode + 2;
            rhs.FrameworkCode = lhs.FrameworkCode;
            rhs.ProgrammeType = lhs.ProgrammeType;
            rhs.ApprenticeshipContractType = lhs.ApprenticeshipContractType;

            var actual = lhs.HasMatchingCourseInformationWith(rhs);

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasMatchingCourseInformationReturnsFalseWhenProgrammeTypeIsDifferent(
            FundingDue lhs, FundingDue rhs)
        {
            rhs.PathwayCode = lhs.PathwayCode;
            rhs.StandardCode = lhs.StandardCode;
            rhs.FrameworkCode = lhs.FrameworkCode;
            rhs.ProgrammeType = lhs.ProgrammeType + 2;
            rhs.ApprenticeshipContractType = lhs.ApprenticeshipContractType;

            var actual = lhs.HasMatchingCourseInformationWith(rhs);

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasMatchingCourseInformationReturnsFalseWhenActIsDifferent(
            FundingDue lhs, FundingDue rhs)
        {
            rhs.PathwayCode = lhs.PathwayCode;
            rhs.StandardCode = lhs.StandardCode;
            rhs.FrameworkCode = lhs.FrameworkCode;
            rhs.ProgrammeType = lhs.ProgrammeType;
            rhs.ApprenticeshipContractType = lhs.ApprenticeshipContractType + 1;

            var actual = lhs.HasMatchingCourseInformationWith(rhs);

            actual.Should().BeFalse();
        }
    }
}
