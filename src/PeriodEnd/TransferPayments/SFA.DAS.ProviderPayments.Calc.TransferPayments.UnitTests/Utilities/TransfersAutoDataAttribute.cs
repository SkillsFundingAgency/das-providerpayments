using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.Utilities
{
    public class TransfersAutoDataAttribute : AutoDataAttribute
    {
        public TransfersAutoDataAttribute() : base(FixtureFactory)
        { }

        private static IFixture FixtureFactory()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            fixture.Register(() => ContextMother.CreateContext().Object);
            
            return fixture;
        }
    }
}
