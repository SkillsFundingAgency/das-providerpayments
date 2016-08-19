using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Exceptions;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Common;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Context.ContextWrapper.ContextWrapper
{
    public class WhenCalledWithInvalidContext
    {
        private static readonly object[] EmptyProperties =
        {
            new object[] {null},
            new object[] {new Dictionary<string, string>()}
        };

        [Test]
        public void ThenExpectingExceptionForNullContextProvided()
        {
            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<LevyPaymentsInvalidContextException>(() => new LevyPayments.Context.ContextWrapper(null));
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsExceptionMessages.ContextNull));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            // Arrange
            var context = new ExternalContextStub { Properties = properties };

            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<LevyPaymentsInvalidContextException>(() => new LevyPayments.Context.ContextWrapper(context));
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsExceptionMessages.ContextNoProperties));
        }
    }
}
