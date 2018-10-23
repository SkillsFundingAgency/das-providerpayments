using SFA.DAS.Payments.DCFS.Domain;
using System;
using SFA.DAS.ProviderPayments.Calc.Common.Domain;

namespace SFA.DAS.Provider.Events.DataLock.Domain
{
    public class DataLockEventPeriod
    {
        public Guid DataLockEventId { get; set; }
        public CollectionPeriod CollectionPeriod { get; set; }
        public string CommitmentVersion { get; set; }
        public bool IsPayable { get; set; }
        public TransactionType TransactionType { get; set; }

        public CensusDateType TransactionTypesFlag { get; set; }
    }
}