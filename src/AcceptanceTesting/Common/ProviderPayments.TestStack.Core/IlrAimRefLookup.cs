namespace ProviderPayments.TestStack.Core
{
    public class IlrAimRefLookup
    {
        public long StandardCode { get; set; }
        public int ProgrammeType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }

        public string ComponentLearnAimRef { get; set; }
        public string MathsAndEnglishLearnAimRef { get; set; }
    }
}