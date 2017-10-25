using SFA.DAS.Payments.Automation.Infrastructure.Entities;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Infrastructure.Data
{
    public interface ILearnersRepository
    {
        List<UsedUlnEntity> GetAllUsedUlns();
    }
}
