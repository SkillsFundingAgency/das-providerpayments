using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.DCFS.UnitTests.Context.ContextWrapper.Constructor
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
            var ex = Assert.Throws<InvalidContextException>(() => new DCFS.Context.ContextWrapper(null));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextNullMessage));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            // Arrange
            var context = new Mock<IExternalContext>();
            context.Setup(c => c.Properties).Returns(properties);

            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<InvalidContextException>(() => new DCFS.Context.ContextWrapper(context.Object));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextNoPropertiesMessage));
        }
    }
}
