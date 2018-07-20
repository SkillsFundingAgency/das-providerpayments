using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities
{
    public class RefundsAutoDataAttribute : AutoDataAttribute
    {
        public RefundsAutoDataAttribute() : base(FixtureBuilder.FixtureFactory)
        { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RefundsAutoDataAttributeInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public RefundsAutoDataAttributeInlineAutoDataAttribute(params object[] arguments)
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
