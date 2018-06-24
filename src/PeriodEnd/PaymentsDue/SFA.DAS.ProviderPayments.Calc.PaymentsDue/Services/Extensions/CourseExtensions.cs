using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions
{
    public static class CourseExtensions
    {
        public static bool HasMatchingCourseInformationWith(this IHoldCourseInformation left, IHoldCourseInformation right)
        {
            return left.StandardCode == right.StandardCode &&
                   left.ProgrammeType == right.ProgrammeType &&
                   left.PathwayCode == right.PathwayCode &&
                   left.FrameworkCode == right.FrameworkCode &&
                   left.ApprenticeshipContractType == right.ApprenticeshipContractType;
        }
    }
}
