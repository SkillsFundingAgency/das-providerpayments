﻿using SFA.DAS.ProviderPayments.Calc.Common.Application;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery
{
    public class GetCurrentCollectionPeriodQueryResponse : Response
    {
        public CollectionPeriod Period { get; set; }
    }
}