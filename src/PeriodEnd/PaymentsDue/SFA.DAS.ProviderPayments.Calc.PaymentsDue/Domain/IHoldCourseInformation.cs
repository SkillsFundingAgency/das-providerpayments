namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface IHoldCourseInformation
    {
        int StandardCode { get; }
        int ProgrammeType { get; }
        int PathwayCode { get; }
        int FrameworkCode { get; }
        int ApprenticeshipContractType { get; }
    }
}
