using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher
{
    public class MatchResult
    {
   
        public List<string> ErrorCodes { get; set; } = new List<string>();
        public List<Commitment> Commitments { get; set; } 
    }
}