using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Payments.DCFS.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForKnownProperty
    {
        private DCFS.Context.ContextWrapper _contextWrapper;

        [SetUp]
        public void Arrange()
        {
            var context = new Mock<IExternalContext>();
            context.Setup(c => c.Properties).Returns(new Dictionary<string, string>
                {
                    {"key", "value"}
                });

            _contextWrapper = new DCFS.Context.ContextWrapper(context.Object);
        }

        [Test]
        public void ThenCorrectValueReturned()
        {
            // Act
            var val = _contextWrapper.GetPropertyValue("key");

            // Assert
            Assert.AreEqual("value", val);
        }
    }
}
