using System;

namespace IlrGenerator
{
    public class FinancialRecord
    {
        public string Type { get; set; }
        public int Code { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }
}