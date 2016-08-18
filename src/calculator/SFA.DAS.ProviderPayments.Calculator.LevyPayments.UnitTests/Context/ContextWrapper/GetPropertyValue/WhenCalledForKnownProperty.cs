using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Common;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForKnownProperty
    {
        private LevyPayments.Context.ContextWrapper _contextWrapper;

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

            _contextWrapper = new LevyPayments.Context.ContextWrapper(context);
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
