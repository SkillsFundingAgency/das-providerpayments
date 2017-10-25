namespace SFA.DAS.Payments.Automation.Infrastructure.Data
{
    public interface IReferenceDataRepository
    {
        long GetNextUkprn();
        long GetNextUln(string scenarioName, string learnRefNumber,long ukprn);


    }
}
