using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.Reference.Accounts.Application.AddManyAccountLegalEntitiesCommand;
using SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers;
using SFA.DAS.Payments.Reference.Accounts.IntegrationTests.StubbedInfrastructure;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.GivenApiAvailable
{
    public class WhenReadingAccountLegalEntities
    {
        [Test, MoqAutoData]
        public void ThenItShouldAddAccountLegalEntities(
            List<AccountLegalEntityViewModel> models,
            ImportAccountsTask sut)
        {
            AccountLegalEntityDataHelper.Truncate();
            models.ForEach(model => model.AccountLegalEntityPublicHashedId = model.AccountLegalEntityPublicHashedId.Substring(0,6));
            StubbedApiClient.AccountLegalEntities.Clear();
            StubbedApiClient.AccountLegalEntities.AddRange(models);

            sut.Execute(new IntegrationTaskContext());

            var actualEntities = AccountLegalEntityDataHelper.GetAll();
            actualEntities.Should().BeEquivalentTo(models.Select(model => model.ToEntity()));
        }

        [Test, MoqAutoData]
        public void ThenItShouldWriteAuditRecord(
            List<AccountLegalEntityViewModel> models,
            ImportAccountsTask sut)
        {
            AccountLegalEntityDataHelper.Truncate();
            models.ForEach(model => model.AccountLegalEntityPublicHashedId = model.AccountLegalEntityPublicHashedId.Substring(0, 6));
            StubbedApiClient.AccountLegalEntities.Clear();
            StubbedApiClient.AccountLegalEntities.AddRange(models);

            sut.Execute(new IntegrationTaskContext());

            var audit = AuditDataHelper.GetLatestAccountLegalEntityAuditRecord();

            audit.ReadDateTime.Should().Be(DateTime.Today);
            audit.AccountsRead.Should().Be(models.Count);
            audit.CompletedSuccessfully.Should().BeTrue();
        }
    }
}