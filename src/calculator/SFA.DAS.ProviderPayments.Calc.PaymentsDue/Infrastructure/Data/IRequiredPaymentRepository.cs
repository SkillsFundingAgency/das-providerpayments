using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data
{
    public interface IRequiredPaymentRepository
    {
        void AddRequiredPayments(RequiredPaymentEntity[] payments);

        RequiredPaymentEntity[] GetPreviousPayments(long ukprn, long uln, long? standardCode = null, int? programmeType = null, int? frameworkCode =null, int? pathwayCode = null);

       
       
    }
}