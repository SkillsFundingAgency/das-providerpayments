using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers
{
    public static class TestDataHelper
    {
        public static ITestDataHelper Get(TestFixtureContext context)
        {
            if (context == TestFixtureContext.PeriodEnd)
            {
                return new PeriodEndTestDataHelper();
            }

            return new SubmissionTestDataHelper();
        }
    }

    public enum TestFixtureContext
    {
        PeriodEnd, Submission
    }
}
