using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Common;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Context.ContextWrapper.Constructor
{
    public class WhenCalledWithValidContext
    {
        public void ThenExpectingInstance()
        {
            // Arrange
            var context = new ExternalContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"key", "value"}
                }
            };

            // Act
            var contextWrapper = new LevyPayments.Context.ContextWrapper(context);

            // Assert
            Assert.IsNotNull(contextWrapper);
        }
    }
}
