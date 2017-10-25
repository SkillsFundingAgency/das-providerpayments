using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Mapping;
using SFA.DAS.Messaging;

namespace ProviderPayments.TestStack.Application
{
    public interface IProcessService
    {
        Task<string> SubmitIlr(IlrSubmission submission);
        Task<string> UploadIlr(byte[] doc, string yearOfCollection);

        Task<string> RunSummarisation(CollectionPeriod period);
        Task<string> RebuildDedsDatabase(ComponentType componentType);
        Task<string> RunAccountsReferenceData();
        Task<string> RunCommitmentsReferenceData();

        Task<ProcessStatus> GetProcessStatus(string id);
    }

    public class ProcessService : IProcessService
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly IProcessRepository _processRepository;
        private readonly IMapper _mapper;

        public ProcessService(IMessagePublisher messagePublisher, IProcessRepository processRepository, IMapper mapper)
        {
            _messagePublisher = messagePublisher;
            _processRepository = processRepository;
            _mapper = mapper;
        }

        public async Task<string> SubmitIlr(IlrSubmission submission)
        {
            submission.Id = Guid.NewGuid().ToString();

            await _messagePublisher.PublishAsync(new ProcessRequest
            {
                ProcessType = ProcessType.SubmitIlr,
                Content = JsonConvert.SerializeObject(submission)
            }).ConfigureAwait(false);

            return submission.Id;
        }

        public async Task<string> UploadIlr(byte[] doc, string yearOfCollection)
        {
            var request = new UploadIlrRequest
            {
                Id = Guid.NewGuid().ToString(),
                Data = doc,
                YearOfCollection = yearOfCollection
            };

            await _messagePublisher.PublishAsync(new ProcessRequest
            {
                ProcessType = ProcessType.UploadIlr,
                Content = JsonConvert.SerializeObject(request)
            }).ConfigureAwait(false);

            return request.Id;
        }

        public async Task<string> RunSummarisation(CollectionPeriod period)
        {
            period.Id = Guid.NewGuid().ToString();

            await _messagePublisher.PublishAsync(new ProcessRequest
            {
                ProcessType = ProcessType.RunSummarisation,
                Content = JsonConvert.SerializeObject(period)
            }).ConfigureAwait(false);

            return period.Id;
        }

        public async Task<string> RebuildDedsDatabase(ComponentType componentType)
        {
            var request = new RebuildDedsForComponentRequest
            {
                Id = Guid.NewGuid().ToString(),
                ComponentType = componentType
            };
            await _messagePublisher.PublishAsync(new ProcessRequest
            {
                ProcessType = ProcessType.RebuildDedsDatabase,
                Content = JsonConvert.SerializeObject(request)
            }).ConfigureAwait(false);
            
            return request.Id;
        }

        public async Task<string> RunAccountsReferenceData()
        {
            var request = new RunReferenceDataComponentRequest
            {
                Id = Guid.NewGuid().ToString(),
                ComponentType = ComponentType.ReferenceAccounts
            };

            await _messagePublisher.PublishAsync(new ProcessRequest
            {
                ProcessType = ProcessType.RunAccountsReferenceData,
                Content = JsonConvert.SerializeObject(request)
            }).ConfigureAwait(false);
        
            return request.Id;
        }

        public async Task<string> RunCommitmentsReferenceData()
        {
            var request = new RunReferenceDataComponentRequest
            {
                Id = Guid.NewGuid().ToString(),
                ComponentType = ComponentType.ReferenceCommitments
            };

            await _messagePublisher.PublishAsync(new ProcessRequest
            {
                ProcessType = ProcessType.RunCommitmentsReferenceData,
                Content = JsonConvert.SerializeObject(request)
            }).ConfigureAwait(false);
            
            return request.Id;
        }

        public async Task<ProcessStatus> GetProcessStatus(string id)
        {
            var entity = await _processRepository.Single(id).ConfigureAwait(false);
            var processStatus = _mapper.Map<ProcessStatus>(entity);
            return processStatus;
        }
    }
}
