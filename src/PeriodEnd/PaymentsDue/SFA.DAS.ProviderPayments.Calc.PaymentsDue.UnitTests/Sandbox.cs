using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class Sandbox
    {
        [Test]
        public void Except()
        {
            var source = new List<int> {1, 2, 3, 4};
            var second = new List<int> {3, 4, 5, 6};

            var actual = source.Except(second);

            actual.Should().HaveCount(2);
        }
    }
}
