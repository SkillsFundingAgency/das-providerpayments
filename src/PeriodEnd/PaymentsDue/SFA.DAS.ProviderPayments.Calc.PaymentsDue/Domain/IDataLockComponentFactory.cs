namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface IDataLockComponentFactory
    {
        IIShouldBeInTheDatalockComponent CreateDataLockComponent();
    }
}