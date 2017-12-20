using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.Payments.DCFS.Extensions;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.TestContext;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers
{
    public class TestFixtureDataHelper
    {
        public static ITestFixtureDataHelper GetTestDataHelper(Enums.TestFixtureContext context)
        {
            if (context == Enums.TestFixtureContext.PeriodEnd)
            {
                return new PeriodEndTestFixtureDataHelper();
            }

            return new SubmissionTestFixtureDataHelper();
        }
       
    }
}
