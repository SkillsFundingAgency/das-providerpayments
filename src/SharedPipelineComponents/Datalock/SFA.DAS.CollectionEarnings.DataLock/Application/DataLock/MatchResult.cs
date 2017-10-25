using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock
{
    public class MatchResult
    {
   
        public List<string> ErrorCodes { get; set; } = new List<string>();
        public Commitment.Commitment[] Commitments { get; set; } 
    }
}