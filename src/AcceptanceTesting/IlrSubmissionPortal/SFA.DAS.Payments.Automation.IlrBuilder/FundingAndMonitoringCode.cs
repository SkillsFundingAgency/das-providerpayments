using System;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    public class FundingAndMonitoringCode
    {
        public const string DasType = "ACT";
        public const int DasLevyCode = 1;
        public const int DasNonLevyCode = 2;

        public string Type { get; set; } = DasType;
        public int Code { get; set; } = DasLevyCode;
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}