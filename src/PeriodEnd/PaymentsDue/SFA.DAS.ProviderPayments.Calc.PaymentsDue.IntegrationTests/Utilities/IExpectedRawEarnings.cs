using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public interface IExpectedRawEarnings
    {
        List<RawEarning> RawEarnings { get; set; }
    }
}