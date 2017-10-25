using System.Collections.Generic;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;
using ProviderPayments.TestStack.Domain.Mapping;

namespace ProviderPayments.TestStack.Application
{
    public interface ICommitmentService
    {
        Task<IEnumerable<Commitment>> GetAllCommitments();
        Task<Commitment> GetCommitmentByPk(long id, long version);
        Task CreateCommitment(Commitment commitment);
        Task UpdateCommitment(Commitment commitment);
        Task DeleteCommitment(long id, long version);
    }

    public class CommitmentService : ICommitmentService
    {
        private readonly ICommitmentRepository _commitmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly ITrainingRepository _trainingRepository;
        private readonly IMapper _mapper;

        public CommitmentService(ICommitmentRepository commitmentRepository,
                                 IAccountRepository accountRepository,
                                 IProviderRepository providerRepository,
                                 ILearnerRepository learnerRepository,
                                 ITrainingRepository trainingRepository,
                                 IMapper mapper)
        {
            _commitmentRepository = commitmentRepository;
            _accountRepository = accountRepository;
            _providerRepository = providerRepository;
            _learnerRepository = learnerRepository;
            _trainingRepository = trainingRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Commitment>> GetAllCommitments()
        {
            var commitmentEntities = await _commitmentRepository.All();
            var commitments = _mapper.Map<Commitment[]>(commitmentEntities);

            await AddReferenceObjectsToCommitments(commitments);

            return commitments;
        }

        public async Task<Commitment> GetCommitmentByPk(long id, long version)
        {
            var entity = await _commitmentRepository.Single(new CommitmentEntityId { Id = id, Version = version});
            if (entity == null)
            {
                return null;
            }
            var commitment = _mapper.Map<Commitment>(entity);

            await AddReferenceObjectsToCommitments(new[] { commitment });

            return commitment;
        }

        public async Task CreateCommitment(Commitment commitment)
        {
            await ValidateCommitment(commitment);

            var entity = ConvertCommitmentToEntity(commitment);

            await _commitmentRepository.Create(entity);

            await _commitmentRepository.UpdateEventStreamPointer();
        }

        public async Task UpdateCommitment(Commitment commitment)
        {
            await ValidateCommitment(commitment);

            var entity = ConvertCommitmentToEntity(commitment);

            await _commitmentRepository.Update(entity);

            await _commitmentRepository.UpdateEventStreamPointer();
        }

        public async Task DeleteCommitment(long id, long version)
        {
            await _commitmentRepository.Delete(new CommitmentEntityId { Id = id, Version = version });

            await _commitmentRepository.UpdateEventStreamPointer();
        }



        private async Task ValidateCommitment(Commitment commitment)
        {
            if (string.IsNullOrEmpty(commitment.Id))
            {
                throw new System.Exception("Id is required");
            }

            if (commitment.Account == null || commitment.Account.Id == 0)
            {
                throw new System.Exception("Account is required");
            }
            if (await _accountRepository.Single(commitment.Account.Id) == null)
            {
                throw new System.Exception($"Account {commitment.Account.Id} not found");
            }

            if (commitment.Provider == null || commitment.Provider.Ukprn < 1)
            {
                throw new System.Exception("Provider is required");
            }
            if (await _providerRepository.Single(commitment.Provider.Ukprn) == null)
            {
                throw new System.Exception($"Provider {commitment.Provider.Ukprn} not found");
            }

            if (commitment.Learner == null || commitment.Learner.Uln < 1)
            {
                throw new System.Exception("Provider is required");
            }
            if (await _learnerRepository.Single(commitment.Learner.Uln) == null)
            {
                throw new System.Exception($"Learner {commitment.Learner.Uln} not found");
            }

            if (commitment.Course == null ||
                (commitment.Course.StandardCode <= 0 && commitment.Course.PathwayCode <= 0 &&
                 commitment.Course.FrameworkCode <= 0 && commitment.Course.ProgrammeType <= 0))
            {
                throw new System.Exception("Course is required");
            }
            if (commitment.Course.StandardCode > 0 &&
                await _trainingRepository.SingleStandard(commitment.Course.StandardCode) == null)
            {
                throw new System.Exception($"Standard {commitment.Course.StandardCode} not found");
            }
            if (commitment.Course.StandardCode <= 0 &&
                await _trainingRepository.SingleFramework(commitment.Course.PathwayCode, commitment.Course.FrameworkCode, commitment.Course.ProgrammeType) == null)
            {
                throw new System.Exception($"Framework {commitment.Course.PathwayCode}/{commitment.Course.FrameworkCode}/{commitment.Course.ProgrammeType} not found");
            }

            if (commitment.Cost <= 0)
            {
                throw new System.Exception("Agreed cost is required");
            }

            if (commitment.Priority <= 0)
            {
                throw new System.Exception("Priority is required");
            }
        }
        private async Task AddReferenceObjectsToCommitments(IEnumerable<Commitment> commitments)
        {
            var accountLookup = new Dictionary<long, Account>();
            var providerLookup = new Dictionary<long, Provider>();
            var learnerLookup = new Dictionary<long, Learner>();
            foreach (var commitment in commitments)
            {
                Account account;
                if (!accountLookup.TryGetValue(commitment.Account.Id, out account))
                {
                    account = _mapper.Map<Account>(await _accountRepository.Single(commitment.Account.Id));
                    accountLookup.Add(account.Id, account);
                }
                commitment.Account = account;

                Provider provider;
                if (!providerLookup.TryGetValue(commitment.Provider.Ukprn, out provider))
                {
                    provider = _mapper.Map<Provider>(await _providerRepository.Single(commitment.Provider.Ukprn));
                    providerLookup.Add(provider.Ukprn, provider);
                }
                commitment.Provider = provider;

                Learner learner;
                if (!learnerLookup.TryGetValue(commitment.Learner.Uln, out learner))
                {
                    learner = _mapper.Map<Learner>(await _learnerRepository.Single(commitment.Learner.Uln));
                    learnerLookup.Add(learner.Uln, learner);
                }
                commitment.Learner = learner;

                if (commitment.Course.StandardCode > 0)
                {
                    commitment.Course.DisplayName =
                        (await _trainingRepository.SingleStandard(commitment.Course.StandardCode))?.DisplayName;
                }
                else
                {
                    commitment.Course.DisplayName =
                        (await _trainingRepository.SingleFramework(commitment.Course.PathwayCode, commitment.Course.FrameworkCode, commitment.Course.ProgrammeType))?.DisplayName;
                }
            }
        }
        private CommitmentEntity ConvertCommitmentToEntity(Commitment commitment)
        {
            var entity = _mapper.Map<CommitmentEntity>(commitment);
            if (entity.StandardCode == 0)
            {
                entity.StandardCode = null;
            }
            if (entity.PathwayCode == 0)
            {
                entity.PathwayCode = null;
            }
            if (entity.FrameworkCode == 0)
            {
                entity.FrameworkCode = null;
            }
            if (entity.ProgrammeType == 0)
            {
                entity.ProgrammeType = null;
            }
            return entity;
        }
    }
}
