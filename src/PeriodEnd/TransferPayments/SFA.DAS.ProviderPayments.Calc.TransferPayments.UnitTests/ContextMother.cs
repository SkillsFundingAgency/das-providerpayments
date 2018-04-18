using System.Collections.Generic;
using System.Configuration;
using CS.Common.External.Interfaces;
using Moq;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests
{
    internal static class ContextMother
    {
        internal static Mock<IExternalContext> CreateContext()
        {
            var context = new Mock<IExternalContext>();

            context.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, ConfigurationManager.AppSettings["TransientConnectionString"]},
                    {ContextPropertyKeys.LogLevel, "DEBUG"}
                });

            return context;
        }
    }
}
