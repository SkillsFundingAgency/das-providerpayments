using SFA.DAS.Payments.Automation.Application.Entities;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Application.Submission
{
    public interface ISqlRefenceDataGenerator
    {
        string GenerateAccountsReferenceDataScript(List<AccountEntity> accounts);
        string GenerateCommitmentsReferenceDataScript(List<CommitmentEntity> commitments );
    }
}
