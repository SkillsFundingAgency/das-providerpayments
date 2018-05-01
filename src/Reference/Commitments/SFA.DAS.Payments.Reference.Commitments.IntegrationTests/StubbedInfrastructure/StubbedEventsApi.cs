using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.StubbedInfrastructure
{
    internal class StubbedEventsApi : Events.Api.Client.IEventsApi
    {
        internal static List<ApprenticeshipEventView> Events { get; } = new List<ApprenticeshipEventView>();

        public Task CreateApprenticeshipEvent(ApprenticeshipEvent apprenticeshipEvent)
        {
            return Task.FromResult<object>(null);
        }

        public Task<List<ApprenticeshipEventView>> GetApprenticeshipEventsById(long fromEventId = 0, int pageSize = 1000, int pageNumber = 1)
        {
            var skip = (pageNumber - 1) * pageSize;
            var page = Events.Where(e => e.Id > fromEventId)
                             .Skip(skip)
                             .Take(pageSize);
            return Task.FromResult(page.ToList());
        }

        public Task<List<ApprenticeshipEventView>> GetApprenticeshipEventsByDateRange(DateTime? fromDate = null, DateTime? toDate = null, int pageSize = 1000,
            int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        public Task CreateAgreementEvent(AgreementEvent agreementEvent)
        {
            throw new NotImplementedException();
        }

        public Task<List<AgreementEventView>> GetAgreementEventsById(long fromEventId = 0, int pageSize = 1000, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        public Task<List<AgreementEventView>> GetAgreementEventsByDateRange(DateTime? fromDate = null, DateTime? toDate = null, int pageSize = 1000,
            int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        
        Task IEventsApi.BulkCreateApprenticeshipEvent(IList<ApprenticeshipEvent> apprenticeshipEvents)
        {
            throw new NotImplementedException();
        }

      
        Task IEventsApi.CreateAgreementEvent(AgreementEvent agreementEvent)
        {
            throw new NotImplementedException();
        }

        Task<List<AgreementEventView>> IEventsApi.GetAgreementEventsById(long fromEventId, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        
        Task IEventsApi.CreateAccountEvent(AccountEvent accountEvent)
        {
            throw new NotImplementedException();
        }

        Task<List<AccountEventView>> IEventsApi.GetAccountEventsById(long fromEventId, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        Task<List<AccountEventView>> IEventsApi.GetAccountEventsByDateRange(DateTime? fromDate, DateTime? toDate, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        Task IEventsApi.CreateGenericEvent(GenericEvent genericEvent)
        {
            throw new NotImplementedException();
        }

        Task<List<GenericEvent>> IEventsApi.GetGenericEventsById(string eventType, long fromEventId, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        Task<List<GenericEvent>> IEventsApi.GetGenericEventsByDateRange(string eventType, DateTime? fromDate, DateTime? toDate, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<List<GenericEvent>> GetGenericEventsByResourceId(string resourceType, string resourceId, DateTime? fromDate = default(DateTime?), DateTime? toDate = default(DateTime?), int pageSize = 1000, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        public Task CreateGenericEvent<T>(IGenericEvent<T> @event)
        {
            return null;
        }

        public Task<List<GenericEvent>> GetGenericEventsByResourceUri(string resourceUri, DateTime? fromDate = default(DateTime?), DateTime? toDate = default(DateTime?), int pageSize = 1000, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        public Task CreateGenericEvent<T>(T payLoad)
        {
            throw new NotImplementedException();
        }

        public Task<List<GenericEvent<T>>> GetGenericEventsById<T>(long fromEventId = 0, int pageSize = 1000, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        public Task<List<GenericEvent<T>>> GetGenericEventsByDateRange<T>(DateTime? fromDate = default(DateTime?), DateTime? toDate = default(DateTime?), int pageSize = 1000, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        public Task<List<GenericEvent<T>>> GetGenericEventsByResourceId<T>(string resourceType, string resourceId, DateTime? fromDate = default(DateTime?), DateTime? toDate = default(DateTime?), int pageSize = 1000, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        public Task<List<GenericEvent<T>>> GetGenericEventsByResourceUri<T>(string resourceUri, DateTime? fromDate = default(DateTime?), DateTime? toDate = default(DateTime?), int pageSize = 1000, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }
    }
}
