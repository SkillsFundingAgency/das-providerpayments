using SFA.DAS.Payments.Automation.Application.Entities;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Application.GetAllUsedUlns
{
    public class GetAllUsedUlnsQueryResponse : ApplicationResponse
    {

        public List<UsedUlnRecord> UsedUlns { get; set; }


    }
}