using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForKnownProperty
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
        public void ThenCorrectValueReturned()
        {
            // Act
            var val = _contextWrapper.GetPropertyValue("key");

            // Assert
            Assert.AreEqual("value", val);
        }
    }
}
