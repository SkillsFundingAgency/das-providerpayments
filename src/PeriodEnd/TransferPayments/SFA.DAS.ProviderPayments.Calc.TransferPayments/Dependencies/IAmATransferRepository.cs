using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies
{
    public interface IAmATransferRepository
    {
        IEnumerable<RequiredTransferPayment> RequiredTransferPayments();
        void SaveTransfers(List<TransferPaymentSet> transfers);
    }
}
