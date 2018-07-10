using AutoFixture.Kernel;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.SpecimenBuilders
{
    public class KnownAccountIdBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null ||
                request.GetType() != typeof(long))
                return new NoSpecimen();

            return context.Resolve(new RangedNumberRequest(
                typeof(long),
                Constants.AccountId.KnownAccountIdLowerLimit,
                Constants.AccountId.KnownAccountIdUpperLimit));
        }
    }
}