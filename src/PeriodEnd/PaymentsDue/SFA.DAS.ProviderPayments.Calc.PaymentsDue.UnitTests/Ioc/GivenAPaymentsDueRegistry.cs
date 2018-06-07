using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Tools;
using StructureMap;
using StructureMap.AutoFactory;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Ioc
{
    [TestFixture]
    public class GivenAPaymentsDueRegistry
    {
        private Container _container;

        [SetUp]
        public void GivenAPaymentsDueRegistryInAContainer()
        {
            _container = new Container(
                new PaymentsDueRegistry(
                    typeof(PaymentsDueProcessorV2), 
                    new ContextWrapper(new ExternalContextStub())));
        }

        [Test, AutoData, Ignore("till merged with Carls changes")]
        public void WhenCreatingAnILearner_ThenTheConstructorIsUsedCorrectly(
            IEnumerable<RawEarning> rawEarnings,
            IEnumerable<RawEarningForMathsOrEnglish> mathsAndEnglishEarnings,
            IEnumerable<PriceEpisode> priceEpisodes,
            IEnumerable<RequiredPaymentEntity> pastPayments)
        {
            var factory = _container.GetInstance<ILearnerFactory>();
            var stubLearner = factory.CreateLearner(rawEarnings, mathsAndEnglishEarnings, priceEpisodes, pastPayments) as Learner;

            stubLearner.RawEarnings.ShouldAllBeEquivalentTo(rawEarnings);
        }
    }

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
