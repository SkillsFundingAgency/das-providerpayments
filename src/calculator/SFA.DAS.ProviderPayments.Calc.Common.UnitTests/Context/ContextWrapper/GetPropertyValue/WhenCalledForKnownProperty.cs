using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForKnownProperty
    {
        private Common.Context.ContextWrapper _contextWrapper;

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

            _contextWrapper = new Common.Context.ContextWrapper(context);
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
