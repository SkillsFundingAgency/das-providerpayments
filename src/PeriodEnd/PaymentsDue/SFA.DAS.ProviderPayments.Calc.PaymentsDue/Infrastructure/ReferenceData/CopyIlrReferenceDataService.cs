using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Repositories.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.ReferenceData
{
    public interface ICopyIlrReferenceData
    {
        void CopyRawEarnings(int lastPeriodToInclude);
        void CopyEarningsInformation();
    }

    class CopyIlrReferenceDataService : ICopyIlrReferenceData
    {
        private readonly string _transientConnectionString;
        private readonly string _ilrConnectionString;
        private readonly IBulkCopyTables _bulkCopy;

        public CopyIlrReferenceDataService(
            string connectionString, 
            string ilrConnectionString, 
            IBulkCopyTables bulkCopy)
        {
            _transientConnectionString = connectionString;
            _ilrConnectionString = ilrConnectionString;
            _bulkCopy = bulkCopy;
        }

        public void CopyRawEarnings(int lastPeriodToInclude)
        {

        }

        public void CopyEarningsInformation()
        {
            var columnList = new List<string>
            {
                "Ukprn",
                "LearnRefNumber",
                "PriceEpisodeIdentifier",
                "StartDate",
                "PlannedEndDate",
                "ActualEndDate",
                "CompletionStatus",
                "PlannedInstalments",
                "CompletionPayment",
                "Instalment",
                "EmployerPayments",
                "EndpointAssessorId"
            };
            _bulkCopy.CopyTable("PaymentsDue.vw_IlrBreakdown", _ilrConnectionString, columnList, 
                "Reference.IlrBreakdown", _transientConnectionString);
        }
    }
}

