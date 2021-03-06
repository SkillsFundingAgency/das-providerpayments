﻿using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.Datalocks
{
    public class SetupNoDatalocksAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            DatalockDataHelper.Truncate();

            SharedTestContext.DataLockPriceEpisodePeriodMatches = new List<DatalockOutputEntity>();

            base.BeforeTest(test);
        }
    }
}