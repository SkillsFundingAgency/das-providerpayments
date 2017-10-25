using System;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    public class IlrBuilderResponse
    {
        public string FileName { get; set; }
        public string IlrContent { get; set; }
        public string CommitmentsContent { get; set; }
        public string AccountsContent { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }

        public string UsedUlnCSV { get; set; }
        public string CommitmentsBulkCsv { get; set; }



    }
}
