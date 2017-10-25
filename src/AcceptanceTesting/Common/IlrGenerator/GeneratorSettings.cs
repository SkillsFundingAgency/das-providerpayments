using System.Collections.Generic;

namespace IlrGenerator
{
    public class GeneratorSettings
    {
        public GeneratorSettings()
        {
            AimRefLookups = new List<AimRefLookup>();

            DefaultStandardComponentLearnAimRef = "Z0001875";
            DefaultStandardMathsAndEnglishLearnAimRef = "50086832";

            DefaultFrameworkComponentLearnAimRef = "60051255";
            DefaultFrameworkMathsAndEnglishLearnAimRef = "50086832";

            OpaRulebaseYear = "1617";
        }
        public List<AimRefLookup> AimRefLookups { get; set; }
        public string DefaultStandardComponentLearnAimRef { get; set; }
        public string DefaultStandardMathsAndEnglishLearnAimRef { get; set; }
        public string DefaultFrameworkComponentLearnAimRef { get; set; }
        public string DefaultFrameworkMathsAndEnglishLearnAimRef { get; set; }
        public string OpaRulebaseYear { get; set; }


    }
}