using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForUnknownProperty
    {
        private Payments.DCFS.Context.ContextWrapper _contextWrapper;

        [SetUp]
        public void Arrange()
        {
            var context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {"key", "value"}
                }
            };

            _contextWrapper = new Payments.DCFS.Context.ContextWrapper(context);
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

        [Test]
        public void ThenNullValueReturnedWhenNullPropertyKeyProvided()
        {
            // Act
            var val = _contextWrapper.GetPropertyValue(null);

            // Assert
            Assert.IsNull(val);
        }
    }
}
