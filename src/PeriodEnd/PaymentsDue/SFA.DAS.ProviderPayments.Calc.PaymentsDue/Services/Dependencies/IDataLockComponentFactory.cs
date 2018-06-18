namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface IDataLockComponentFactory
    {
        IIShouldBeInTheDataLockComponent CreateDataLockComponent();
    }
}