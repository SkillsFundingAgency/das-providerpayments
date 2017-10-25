
using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery
{
    public class GetCurrentCollectionPeriodQueryResponse : Response
    {
         public CollectionPeriod Period { get; set; }
    }
}