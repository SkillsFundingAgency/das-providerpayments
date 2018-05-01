﻿using System;
using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Contexts
{
    public class EarningsAndPaymentsContext
    {
        public EarningsAndPaymentsContext()
        {
            PeriodDates = new List<DateTime>();

            OverallEarningsAndPayments = new List<EarningsAndPaymentsBreakdown>();
            LearnerOverallEarningsAndPayments = new List<LearnerEarningsAndPaymentsBreakdown>();

            EmployerEarnedFor16To18Incentive = new List<EmployerAccountProviderPeriodValue>();
            ProviderEarnedForOnProgramme = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForCompletion = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForBalancing = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedFor16To18Incentive = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForEnglishAndMathOnProgramme = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForEnglishAndMathBalancing = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForDisadvantageUplift = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForFrameworkUpliftOnProgramme = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForFrameworkUpliftOnCompletion = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForFrameworkUpliftOnBalancing = new List<ProviderEarnedPeriodValue>();
            ProviderEarnedForLearningSupport = new List<ProviderEarnedPeriodValue>();
        }

        public List<DateTime> PeriodDates { get; set; }

        public List<EarningsAndPaymentsBreakdown> OverallEarningsAndPayments { get; set; }
        public List<LearnerEarningsAndPaymentsBreakdown> LearnerOverallEarningsAndPayments { get; set; }

        public List<EmployerAccountProviderPeriodValue> EmployerEarnedFor16To18Incentive { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForOnProgramme { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForCompletion { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForBalancing { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedFor16To18Incentive { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForEnglishAndMathOnProgramme { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForEnglishAndMathBalancing { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForDisadvantageUplift { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForFrameworkUpliftOnProgramme { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForFrameworkUpliftOnCompletion { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForFrameworkUpliftOnBalancing { get; set; }
        public List<ProviderEarnedPeriodValue> ProviderEarnedForLearningSupport { get; set; }
    }
}
