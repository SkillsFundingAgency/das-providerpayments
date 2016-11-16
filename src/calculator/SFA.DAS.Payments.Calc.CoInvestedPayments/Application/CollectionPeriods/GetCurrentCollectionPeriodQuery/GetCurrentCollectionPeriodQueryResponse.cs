using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery
{
    public class GetCurrentCollectionPeriodQueryResponse : Response
    {
         public CollectionPeriod Period { get; set; }
    }
}