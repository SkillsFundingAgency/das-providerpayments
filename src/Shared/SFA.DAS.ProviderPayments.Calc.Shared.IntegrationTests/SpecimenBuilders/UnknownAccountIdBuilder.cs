using System.Reflection;
using AutoFixture.Kernel;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.SpecimenBuilders
{
    public class UnknownAccountIdBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;
            if (propInfo == null ||
                propInfo.Name != "AccountId" ||
                propInfo.PropertyType != typeof(long))
                return new NoSpecimen();

            return context.Resolve(new RangedNumberRequest(
                typeof(long), 
                Constants.AccountId.UnknownAccountIdLowerLimit, 
                Constants.AccountId.UnknownAccountIdUpperLimit));
        }
    }
}