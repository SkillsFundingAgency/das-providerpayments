using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForNullOrEmptyProperty
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
