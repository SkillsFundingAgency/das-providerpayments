using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Payments.DCFS.UnitTests.Context.ContextWrapper.Constructor
{
    public class WhenCalledWithValidContext
    {
        public void ThenExpectingInstance()
        {
            // Arrange
            var context = new Mock<IExternalContext>();
            context.Setup(c => c.Properties).Returns(new Dictionary<string, string>
                {
                    {"key", "value"}
                });

            // Act
            var contextWrapper = new DCFS.Context.ContextWrapper(context.Object);

            // Assert
            Assert.IsNotNull(contextWrapper);
        }
    }
}
