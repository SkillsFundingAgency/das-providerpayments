using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Services;

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

            fixture.Register(() => new DatalockValidationService(MatcherFactory.CreateMatcher(), "1718"));

            return fixture;
        }
    }
}
