﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels
{
    public class EarningsAndPaymentsBreakdown
    {
        public EarningsAndPaymentsBreakdown()
        {
            PeriodDates = new List<DateTime>();
            ProviderEarnedTotal = new List<PeriodValue>();
            ProviderEarnedFromSfa = new List<PeriodValue>();
            ProviderEarnedFromEmployers = new List<EmployerAccountPeriodValue>();
            ProviderPaidBySfa = new List<PeriodValue>();
            ProviderPaidBySfaForUln = new List<UlnPeriodValue>();
            PaymentDueFromEmployers = new List<EmployerAccountPeriodValue>();
            EmployersLevyAccountDebited = new List<EmployerAccountPeriodValue>();
            EmployersLevyAccountDebitedForUln = new List<EmployerAccountUlnPeriodValue>();
            SfaLevyBudget = new List<PeriodValue>();
            SfaLevyCoFundBudget = new List<PeriodValue>();
            SfaNonLevyCoFundBudget = new List<PeriodValue>();
            SfaLevyAdditionalPayments = new List<PeriodValue>();
            SfaNonLevyAdditionalPayments = new List<PeriodValue>();
            RefundTakenBySfa = new List<PeriodValue>();
            EmployersLevyAccountCredited= new List<EmployerAccountPeriodValue>();
            EmployerLevyTransactions = new List<EmployerAccountPeriodValue>();
            RefundDueToEmployer = new List<EmployerAccountPeriodValue>();
            EmployersLevyAccountDebitedViaTransfer = new List<EmployerAccountPeriodValue>();
            EmployersLevyAccountDebitedForUlnViaTransfer = new List<EmployerAccountUlnPeriodValue>();
        }

        public List<DateTime> PeriodDates { get; set; }
        public string ProviderId { get; set; }
        public List<PeriodValue> ProviderEarnedTotal { get; set; }
        public List<PeriodValue> ProviderEarnedFromSfa { get; set; }
        public List<EmployerAccountPeriodValue> ProviderEarnedFromEmployers { get; set; }
        public List<PeriodValue> ProviderPaidBySfa { get; set; }
        public List<UlnPeriodValue> ProviderPaidBySfaForUln { get; set; }
        public List<EmployerAccountPeriodValue> PaymentDueFromEmployers { get; set; }
        public List<EmployerAccountPeriodValue> EmployersLevyAccountDebited { get; set; }
        public List<EmployerAccountUlnPeriodValue> EmployersLevyAccountDebitedForUln { get; set; }
        public List<EmployerAccountPeriodValue> EmployersLevyAccountDebitedViaTransfer { get; set; }
        public List<EmployerAccountUlnPeriodValue> EmployersLevyAccountDebitedForUlnViaTransfer { get; set; }
        public List<PeriodValue> SfaLevyBudget { get; set; }
        public List<PeriodValue> SfaLevyCoFundBudget { get; set; }
        public List<PeriodValue> SfaNonLevyCoFundBudget { get; set; }
        public List<PeriodValue> SfaLevyAdditionalPayments { get; set; }
        public List<PeriodValue> SfaNonLevyAdditionalPayments { get; set; }

        public List<PeriodValue> RefundTakenBySfa { get; set; }
        public List<EmployerAccountPeriodValue> EmployersLevyAccountCredited { get; set; }
        public List<EmployerAccountPeriodValue> EmployerLevyTransactions { get; set; }
        public List<EmployerAccountPeriodValue> RefundDueToEmployer { get; set; }

    }
}