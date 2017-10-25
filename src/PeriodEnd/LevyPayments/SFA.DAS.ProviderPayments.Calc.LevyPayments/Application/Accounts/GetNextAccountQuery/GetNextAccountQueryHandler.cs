using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetNextAccountQuery
{
    public class GetNextAccountQueryHandler : IRequestHandler<GetNextAccountQueryRequest, GetNextAccountQueryResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICommitmentRepository _commitmentRepositoryObject;

        public GetNextAccountQueryHandler(IAccountRepository accountRepository, ICommitmentRepository commitmentRepositoryObject)
        {
            _accountRepository = accountRepository;
            _commitmentRepositoryObject = commitmentRepositoryObject;
        }

        public GetNextAccountQueryResponse Handle(GetNextAccountQueryRequest message)
        {
            var accountEntity = _accountRepository.GetNextAccountRequiringProcessing();
            if (accountEntity == null)
            {
                return new GetNextAccountQueryResponse();
            }

            var commitmentEntities = _commitmentRepositoryObject.GetCommitmentsForAccount(accountEntity.Id.ToString()) ?? new CommitmentEntity[0];

            return new GetNextAccountQueryResponse
            {
                Account = new Account
                {
                    Id = accountEntity.Id.ToString(),
                    Name = accountEntity.Name,
                    Commitments = commitmentEntities.Select(e => new Commitment { Id = e.Id }).ToArray()
                }
            };
        }
    }
}
