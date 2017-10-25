namespace SFA.DAS.Payments.Automation.Application.CreateSubmissionCommand
{
    public class CreateSubmissionCommandResponse : ApplicationResponse
    {
        public string FileName { get; set; }
        public string IlrContent { get; set; }
        public string CommitmentsSql { get; set; }
        public string AccountsSql { get; set; }
        public string UsedUlnCsv { get; set; }
        public string CommitmentsBulkCsv { get; set; }
    }
}