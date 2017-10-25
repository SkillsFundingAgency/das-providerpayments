using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Payments.DCFS.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForNullOrEmptyProperty
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
        [TestCase(null)]
        [TestCase("")]
        public void ThenNullReturnedWhenDefaultValueProvided(string key)
        {
            // Act
            var defaultVal = "defaultvalue";
            var val = _contextWrapper.GetPropertyValue(key, defaultVal);

            // Assert
            Assert.IsNull(val);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ThenNullReturnedWhenNoDefaultValueProvided(string key)
        {
            // Act
            var val = _contextWrapper.GetPropertyValue(key);

            // Assert
            Assert.IsNull(val);
        }
    }
}
