using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.Datalocks
{
    public class SetupValidationErrorsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            DatalockValidationErrorDataHelper.Truncate();

            var fixture = new Fixture();
            var priceEpisodeIdentifier = $"{fixture.Create<string>().Substring(0, 5)}-01/03/2018";

            var validationErrors = fixture.Build<DatalockValidationError>()
                .With(earning => earning.Ukprn,
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != SharedTestContext.Ukprn))
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            var validationErrorsMatchingUkprn = fixture.Build<DatalockValidationError>()
                .With(earning => earning.Ukprn, SharedTestContext.Ukprn)
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            validationErrors.AddRange(validationErrorsMatchingUkprn);

            foreach (var validationError in validationErrors)
            {
                DatalockValidationErrorDataHelper.CreateEntity(validationError);
            }

            SharedTestContext.DatalockValidationErrors = validationErrors;

            base.BeforeTest(test);
        }
    }
}