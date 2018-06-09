namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public interface IDataLockComponentFactory
    {
        IIShouldBeInTheDataLockComponent CreateDataLockComponent();
    }
}