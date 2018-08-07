using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock
{
    public class MatchResult
    {
   
        public List<string> ErrorCodes { get; set; } = new List<string>();
        public CommitmentEntity[] Commitments { get; set; } 
    }
}