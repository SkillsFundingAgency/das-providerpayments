using System;
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
        private const int ExpectedPageSize = 1000;

        [TestFixture]
        public class WhenCallingHandle
        {
            [Test, AccountsAutoData]
            public void ThenItShouldReturnAValidResponse(
                GetPageOfAccountLegalEntitiesQueryRequest request,
                PagedApiResponseViewModel<AccountLegalEntityViewModel> apiResponse,
                [Frozen] Mock<IAccountApiClient> mockApi,
                GetPageOfAccountLegalEntitiesQueryHandler sut)
            {
                mockApi
                    .Setup(client => client.GetPageOfAccountLegalEntities(request.PageNumber, ExpectedPageSize))
                    .ReturnsAsync(apiResponse);

                var response = sut.Handle(request);

                response.IsValid.Should().BeTrue();
                response.Exception.Should().BeNull();
            }

            [Test, AccountsAutoData]
            public void ThenItShouldReturnModelsFromApi(
                GetPageOfAccountLegalEntitiesQueryRequest request,
                PagedApiResponseViewModel<AccountLegalEntityViewModel> apiResponse,
                [Frozen] Mock<IAccountApiClient> mockApi,
                GetPageOfAccountLegalEntitiesQueryHandler sut)
            {
                mockApi
                    .Setup(client => client.GetPageOfAccountLegalEntities(request.PageNumber, ExpectedPageSize))
                    .ReturnsAsync(apiResponse);

                var response = sut.Handle(request);

                response.Items.Should().BeEquivalentTo(apiResponse.Data);
            }

            [Test, AccountsAutoData]
            public void AndMorePages_ThenHasMorePagesTrue(
                GetPageOfAccountLegalEntitiesQueryRequest request,
                PagedApiResponseViewModel<AccountLegalEntityViewModel> apiResponse,
                [Frozen] Mock<IAccountApiClient> mockApi,
                GetPageOfAccountLegalEntitiesQueryHandler sut)
            {
                apiResponse.Page = apiResponse.TotalPages -1;
                mockApi
                    .Setup(client => client.GetPageOfAccountLegalEntities(request.PageNumber, ExpectedPageSize))
                    .ReturnsAsync(apiResponse);

                var response = sut.Handle(request);

                response.HasMorePages.Should().BeTrue();
            }

            [Test, AccountsAutoData]
            public void AndNoMorePages_ThenHasMorePagesFalse(
                GetPageOfAccountLegalEntitiesQueryRequest request,
                PagedApiResponseViewModel<AccountLegalEntityViewModel> apiResponse,
                [Frozen] Mock<IAccountApiClient> mockApi,
                GetPageOfAccountLegalEntitiesQueryHandler sut)
            {
                apiResponse.Page = apiResponse.TotalPages;
                mockApi
                    .Setup(client => client.GetPageOfAccountLegalEntities(request.PageNumber, ExpectedPageSize))
                    .ReturnsAsync(apiResponse);

                var response = sut.Handle(request);

                response.HasMorePages.Should().BeFalse();
            }

            [Test, AccountsAutoData]
            public void AndException_ThenReturnsIsValidFalse(
                GetPageOfAccountLegalEntitiesQueryRequest request,
                PagedApiResponseViewModel<AccountLegalEntityViewModel> apiResponse,
                [Frozen] Mock<IAccountApiClient> mockApi,
                GetPageOfAccountLegalEntitiesQueryHandler sut)
            {
                mockApi
                    .Setup(client => client.GetPageOfAccountLegalEntities(request.PageNumber, ExpectedPageSize))
                    .Throws<Exception>();

                var response = sut.Handle(request);

                response.IsValid.Should().BeFalse();
                response.Exception.Should().NotBeNull();
            }
        }
    }
}