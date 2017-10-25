namespace SFA.DAS.Payments.Automation.Application
{
    public abstract class ApplicationScalarResponse<T> : ApplicationResponse
    {
        public T Value { get; set; }

    }
}
