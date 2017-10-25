namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    public class IlrBuilderRequest
    {
        public string Gherkin { get; set; }

        public int? ShiftToMonth { get; set; }
        public int? ShiftToYear { get; set; }
        public long Ukprn { get; set; }

        public string AcademicYear { get; set; }

    }
}