namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface IDataLockComponentFactory
    {
        IIShouldBeInTheDataLockComponent CreateDataLockComponent();
    }
}