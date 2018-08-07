using System.Linq;
using MediatR;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddManyAccountLegalEntitiesCommand
{
    public class AddManyAccountLegalEntitiesCommandHandler : IRequestHandler<AddManyAccountLegalEntitiesCommandRequest, Unit>
    {
        private readonly IAccountLegalEntityRepository _accountLegalEntityRepository;

        public AddManyAccountLegalEntitiesCommandHandler(IAccountLegalEntityRepository accountLegalEntityRepository)
        {
            _accountLegalEntityRepository = accountLegalEntityRepository;
        }

        public Unit Handle(AddManyAccountLegalEntitiesCommandRequest message)
        {
            var entities = message.AccountLegalEntityViewModels.Select(model => model.ToEntity());
            _accountLegalEntityRepository.AddMany(entities);

            return Unit.Value;
        }
    }
}