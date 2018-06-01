using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public interface IProviderProcessor
    {
        void Process(ProviderEntity provider);
    }
}