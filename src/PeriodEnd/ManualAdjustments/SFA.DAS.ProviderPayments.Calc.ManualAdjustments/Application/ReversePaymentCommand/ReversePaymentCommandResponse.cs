using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.ReversePaymentCommand
{
    public class ReversePaymentCommandResponse : Response
    {
        public string RequiredPaymentIdForReversal { get; set; }
    }
}