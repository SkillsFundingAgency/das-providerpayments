using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.UnitTests
{
    [TestFixture]
    public class Sandbox
    {
        class TestStructure
        {
            public int Id { get; set; }
            public string Thing { get; set; }
        }

        [Test]
        public void WhenAccessingToLookupWithNoValues()
        {
            var values = new List<TestStructure>
            {
                new TestStructure{Id = 1, Thing = "One"},
                new TestStructure{Id = 2, Thing = "Two"},
            };

            var sut = values.ToLookup(x => x.Id);

            var actual = sut[0];

            actual.Should().BeEmpty();
        }
    }
}
