using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Context.ContextWrapper.ContextWrapper
{
    public class WhenCalledWithValidContext
    {
        public void ThenExpectingInstance()
        {
            // Arrange
            var context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {"key", "value"}
                }
            };

            // Act
            var contextWrapper = new Common.Context.ContextWrapper(context);

            // Assert
            Assert.IsNotNull(contextWrapper);
        }
    }
}
