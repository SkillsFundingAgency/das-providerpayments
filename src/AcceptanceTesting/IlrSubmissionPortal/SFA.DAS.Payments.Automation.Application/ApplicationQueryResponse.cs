namespace SFA.DAS.Payments.Automation.Application
{
    public abstract class ApplicationQueryResponse<T> : ApplicationResponse
    {
        public T[] Results { get; set; }
    }
}