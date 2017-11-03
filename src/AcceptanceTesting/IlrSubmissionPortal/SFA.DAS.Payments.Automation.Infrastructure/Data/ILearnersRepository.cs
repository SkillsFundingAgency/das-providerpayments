using SFA.DAS.Payments.Automation.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Infrastructure.Data
{
    public interface ILearnersRepository
    {
        List<UsedUlnEntity> GetAllUsedUlns();
    }
}
