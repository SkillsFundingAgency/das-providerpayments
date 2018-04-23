﻿namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data
{
    public class DasAccount
    {
        public long AccountId { get; set; }
        public decimal Balance { get; set; }
        public bool IsLevyPayer { get; set; }
        public decimal TransferBalance { get; set; }
    }
}
