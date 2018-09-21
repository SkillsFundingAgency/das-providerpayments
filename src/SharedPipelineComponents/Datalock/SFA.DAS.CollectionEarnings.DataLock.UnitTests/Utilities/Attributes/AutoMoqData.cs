using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Attributes
{
    class AutoMoqData : AutoDataAttribute
    {
        public AutoMoqData() : base(FixtureFactory)
        { }

        private static IFixture FixtureFactory()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            return fixture;
        }
    }
}
