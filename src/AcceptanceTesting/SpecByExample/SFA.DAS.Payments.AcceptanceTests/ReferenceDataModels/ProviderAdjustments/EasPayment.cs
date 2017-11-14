using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;

namespace SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels.ProviderAdjustments
{
    public class EasPayment
    {
        public void SetPeriod(int periodMonth, int periodYear)
        {
            CollectionPeriodMonth = periodMonth;
            CollectionPeriodYear = periodYear;

            var period = ProviderAdjustmentsContext.CollectionPeriods.Single(x =>
                x.Calendar_Month == CollectionPeriodMonth && 
                x.Calendar_Year == CollectionPeriodYear);

            SubmissionCollectionPeriod = period.Period_ID;
            SubmissionAcademicYear = period.Collection_Year;
            CollectionPeriodName = period.Collection_Period_Name;
        }

        public long Ukprn { get; set; }
        public Guid SubmissionId { get; set; }
        public int PaymentType { get; set; }

        public string PaymentTypeName { get; set; }
        public decimal Amount { get; set; }


        public int SubmissionCollectionPeriod { get; private set; }
        public int SubmissionAcademicYear { get; private set; }

        public string CollectionPeriodName { get; private set; }
        public int CollectionPeriodMonth { get; private set; }
        public int CollectionPeriodYear { get; private set; }
    }
}
