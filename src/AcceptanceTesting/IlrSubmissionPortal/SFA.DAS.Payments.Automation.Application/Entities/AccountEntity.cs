using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Application.Entities
{
    public class AccountEntity
    {
        public long AccountId { get; set; }
        public string VersionId { get; set; }
        public string AccountHashId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public bool IsLevyPayer { get; set; }
    }
}
