﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Earnings
{
   public  class IncentiveEarnings
    {
        public long Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public int Period { get; set; }
        public decimal PriceEpisodeFirstEmp1618Pay { get; set; }
        public decimal PriceEpisodeSecondEmp1618Pay { get; set; }

    }
}
