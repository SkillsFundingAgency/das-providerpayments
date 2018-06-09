namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public interface ILearner
    {
        LearnerProcessResults CalculatePaymentsDue();
    }
}