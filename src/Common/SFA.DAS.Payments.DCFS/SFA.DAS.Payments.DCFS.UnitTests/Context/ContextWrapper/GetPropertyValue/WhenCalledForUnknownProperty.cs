using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Payments.DCFS.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForUnknownProperty
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
        public void ThenDefaultValueReturnedWhenDefaultValueProvided()
        {
            // Act
            var defaultVal = "defaultvalue";
            var val = _contextWrapper.GetPropertyValue("unknownkey", defaultVal);

            // Assert
            Assert.AreEqual(defaultVal, val);
        }

        [Test]
        public void ThenNullValueReturnedWhenNoDefaultValueProvided()
        {
            // Act
            var val = _contextWrapper.GetPropertyValue("unknownkey");

            // Assert
            Assert.IsNull(val);
        }
    }
}
