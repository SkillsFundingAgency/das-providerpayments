namespace ProviderPayments.TestStack.Domain
{
    public class Course
    {
        public long StandardCode { get; set; }
        
        public int PathwayCode { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgrammeType { get; set; }

        public string DisplayName { get; set; }
    }
}