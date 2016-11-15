using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.Tools
{
    internal static class ContextMother
    {
        internal static Mock<IExternalContext> CreateContext()
        {
            var context = new Mock<IExternalContext>();

            context.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "DbConnection"},
                    {ContextPropertyKeys.LogLevel, "DEBUG"},
                    {ContextPropertyKeys.YearOfCollection, "1617"}
                });

            return context;
        }
    }
}
