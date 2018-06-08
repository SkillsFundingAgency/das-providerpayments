using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class DatalockComponentFactory : IDataLockComponentFactory
    {
        public IIShouldBeInTheDataLockComponent CreateDataLockComponent()
        {
            return new IShouldBeInTheDatalockComponent();            
        }
    }
}
