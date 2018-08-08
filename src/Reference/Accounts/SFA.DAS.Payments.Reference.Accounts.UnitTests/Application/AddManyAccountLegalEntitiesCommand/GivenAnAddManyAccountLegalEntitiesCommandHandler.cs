using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Accounts.Application.AddManyAccountLegalEntitiesCommand;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Application.AddManyAccountLegalEntitiesCommand
{
    [TestFixture]
    public class GivenAnAddManyAccountLegalEntitiesCommandHandler
    {
        [TestFixture]
        public class WhenCallingHandle
        {
            [Test, AccountsAutoData]
            public void ThenItUsesRepositoryToAddMany(
                AddManyAccountLegalEntitiesCommandRequest request,
                [Frozen] Mock<IAccountLegalEntityRepository> mockRepository,
                AddManyAccountLegalEntitiesCommandHandler sut)
            {
                var expectedEntities = request.AccountLegalEntityViewModels
                    .Select(model => model.ToEntity())
                    .ToList();

                var actualEntities = new List<AccountLegalEntityEntity>();

                mockRepository
                    .Setup(repository => repository.AddMany(It.IsAny<IEnumerable<AccountLegalEntityEntity>>()))
                    .Callback((IEnumerable<AccountLegalEntityEntity>  entities) => actualEntities = entities.ToList());

                sut.Handle(request);

                actualEntities.Should().BeEquivalentTo(expectedEntities);
            }
        }
    }
}