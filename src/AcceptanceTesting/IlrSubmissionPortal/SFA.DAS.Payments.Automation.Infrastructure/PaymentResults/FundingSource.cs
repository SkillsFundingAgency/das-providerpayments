﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Infrastructure.PaymentResults
{
    public enum FundingSource
    {
        Levy = 1,
        CoInvestedSfa = 2,
        CoInvestedEmployer = 3,
        FullyFundedSfa = 4
    }
}
