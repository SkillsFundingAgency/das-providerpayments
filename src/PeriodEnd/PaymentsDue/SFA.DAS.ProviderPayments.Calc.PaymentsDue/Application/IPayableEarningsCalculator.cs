using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public interface IPayableEarningsCalculator// dependent on data lock and maths english
    {
        List<PayableEarning> Calculate(long ukprn); //act1 raw earnings, 
    }
}