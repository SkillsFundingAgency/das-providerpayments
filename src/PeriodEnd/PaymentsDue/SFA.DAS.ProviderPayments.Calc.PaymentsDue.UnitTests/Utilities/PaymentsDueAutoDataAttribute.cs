using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities
{
    public class PaymentsDueAutoDataAttribute : AutoDataAttribute
    {
        public PaymentsDueAutoDataAttribute() : base(FixtureFactory)
        { }

        private static IFixture FixtureFactory()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            
            return fixture;
        }
    }
}
