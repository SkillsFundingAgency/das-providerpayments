using System;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    public class FinancialRecord
    {
        public const string TotalNegotiatedPriceType = "TNP";
        public const int TotalNegotiatedPrice1Code = 1;
        public const int TotalNegotiatedPrice2Code = 2;
        public const int TotalNegotiatedPrice3Code = 3;
        public const int TotalNegotiatedPrice4Code = 4;

        public string Type { get; set; } = TotalNegotiatedPriceType;
        public int Code { get; set; } = TotalNegotiatedPrice1Code;
        public DateTime From { get; set; }
        public decimal Amount { get; set; }
    }
}