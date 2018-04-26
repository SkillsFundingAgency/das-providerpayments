using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies
{
    public interface IAmAnAccountRepository
    {
        Account Account(long accountId);
    }
}
