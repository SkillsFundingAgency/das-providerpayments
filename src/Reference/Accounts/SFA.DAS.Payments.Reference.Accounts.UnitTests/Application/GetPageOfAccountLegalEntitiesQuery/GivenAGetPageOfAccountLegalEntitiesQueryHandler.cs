using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountLegalEntitiesQuery;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Application.GetPageOfAccountLegalEntitiesQuery
{
    [TestFixture]
    public class GivenAGetPageOfAccountLegalEntitiesQueryHandler
    {
        [TestFixture]
        public class WhenCallingHandle
        {
            [Test, AccountsAutoData]
            public void ThenItShouldReturnModelsFromApi(
                GetPageOfAccountLegalEntitiesQueryRequest request,
                PagedApiResponseViewModel<AccountLegalEntityViewModel> apiResponse,
                [Frozen] Mock<IAccountApiClient> mockApi,
                GetPageOfAccountLegalEntitiesQueryHandler sut)
            {
                mockApi
                    .Setup(client => client.GetPageOfAccountLegalEntities(request.PageNumber, 1000))
                    .ReturnsAsync(apiResponse);

                var response = sut.Handle(request);

                response.Items.Should().BeEquivalentTo(apiResponse.Data);
            }

            [Test]
            public void AndMorePages_ThenHasMorePagesTrue()
            {
                
            }

            [Test]
            public void AndNoPages_ThenHasMorePagesFalse()
            {

            }

            [Test]
            public void AndException_ThenReturnsIsValidFalse()
            {
                
            }
        }
    }
}