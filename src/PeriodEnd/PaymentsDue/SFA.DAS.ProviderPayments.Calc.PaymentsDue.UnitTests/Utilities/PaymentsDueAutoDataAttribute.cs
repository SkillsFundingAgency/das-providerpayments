using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities
{
    public class PaymentsDueAutoDataAttribute : AutoDataAttribute
    {
        public PaymentsDueAutoDataAttribute() : base(FixtureBuilder.FixtureFactory)
        { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PaymentsDueInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public PaymentsDueInlineAutoDataAttribute(params object[] arguments)
            : base(FixtureBuilder.FixtureFactory, arguments)
        {
        }
    }

    static class FixtureBuilder
    {
        public static IFixture FixtureFactory()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            return fixture;
        }
    }
}
