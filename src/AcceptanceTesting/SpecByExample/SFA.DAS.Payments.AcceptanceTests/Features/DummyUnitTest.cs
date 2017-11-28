using NUnit.Framework;

namespace SFA.DAS.Payments.AcceptanceTests.Features
{
    [TestFixture]
    public class DummyUnitTest
    {
        [Test]
        public void Suceed()
        {
            Assert.That(true);
        }

        [Test]
        public void Fail()
        {
            Assert.That(false);
        }
    }
}
