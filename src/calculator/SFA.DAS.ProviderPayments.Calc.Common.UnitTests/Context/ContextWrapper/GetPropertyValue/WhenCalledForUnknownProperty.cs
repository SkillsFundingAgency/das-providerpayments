using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.Common.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForUnknownProperty
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
