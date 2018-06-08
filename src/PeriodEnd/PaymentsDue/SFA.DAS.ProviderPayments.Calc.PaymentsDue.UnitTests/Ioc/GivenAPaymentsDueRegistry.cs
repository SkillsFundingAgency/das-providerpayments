using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using StructureMap;
using StructureMap.AutoFactory;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Ioc
{
    [TestFixture]
    public class GivenAnAutoFactory
    {
        public interface IFoo
        {
        }

        public interface IFooFactory
        {
            IFoo CreateFoo(string bar, int count);
        }

        public class Foo : IFoo
        {
            public string Bar { get; }
            public int Count { get; }

            public Foo(string bar, int count)
            {
                Bar = bar;
                Count = count;
            }
        }

        private class FooRegistry : Registry
        {
            public FooRegistry()
            {
                For<IFoo>().Use<Foo>();
                For<IFooFactory>().CreateFactory();
            }
        }

        [Test, AutoData]
        public void ThenItPassesParametersToInstanceCtor(
            string expectedBar,
            int expectedCount)
        {
            var container = new Container(new FooRegistry());
            var factory = container.GetInstance<IFooFactory>();

            var foo = factory.CreateFoo(expectedBar, expectedCount) as Foo;

            foo.Bar.Should().Be(expectedBar);
            foo.Count.Should().Be(expectedCount);
        }
    }
}
