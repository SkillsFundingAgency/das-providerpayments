using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Context.ContextWrapper.ContextWrapper
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
            var ex = Assert.Throws<InvalidContextException>(() => new Common.Context.ContextWrapper(null));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextNullMessage));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            // Arrange
            var context = new ExternalContextStub { Properties = properties };

            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<InvalidContextException>(() => new Common.Context.ContextWrapper(context));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextNoPropertiesMessage));
        }
    }
}
