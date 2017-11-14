using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels.ProviderAdjustments;
using SFA.DAS.Payments.AcceptanceTests.TableParsers;

namespace SFA.DAS.Payments.AcceptanceTests.Contexts
{
    public class ProviderAdjustmentsContext
    {
        public IReadOnlyList<EasSubmission> EasSubmissions { get { return _easSubmissions; } }
        private readonly List<EasSubmission> _easSubmissions = new List<EasSubmission>();

        public IReadOnlyList<EasPayment> EasPayments { get { return _easPayments; } }
        private readonly List<EasPayment> _easPayments = new List<EasPayment>();

        private readonly long _ukprn = new LookupContext().AddOrGetUkprn("eas_provider");

        public static List<CollectionPeriodMapping> CollectionPeriods { get { return _collectionPeriods.Value; } }

        private static readonly Lazy<List<CollectionPeriodMapping>> _collectionPeriods =
            new Lazy<List<CollectionPeriodMapping>>(() =>
            {
                var collectionPeriods = ProviderAdjustmentsRepository.GetPeriodMappings();
                return collectionPeriods;
            }, LazyThreadSafetyMode.ExecutionAndPublication);


        public static List<PaymentType> PaymentTypes { get { return _paymentTypes.Value; } }

        private static readonly Lazy<List<PaymentType>> _paymentTypes =
            new Lazy<List<PaymentType>>(() =>
            {
                var collectionPeriods = ProviderAdjustmentsRepository.GetPaymentTypes();
                return collectionPeriods;
            }, LazyThreadSafetyMode.ExecutionAndPublication);

        public int CollectionYear { get; private set; }
        public int CollectionMonth { get; private set; }

        private int CollectionPeriod(string collectionPeriod)
        {
            return 0;
        }

        public void SetCollectionPeriod(string collectionPeriod)
        {
            
        }

        public void AddSubmissions(List<GenericPeriodBasedRow> periods)
        {
            foreach (var row in periods)
            {
                var collectionPeriod = new PeriodDefinition(row.Period);

                var submission = new EasSubmission
                {
                    Ukprn = _ukprn,
                    CollectionPeriod = collectionPeriod.CollectionPeriod,
                    ProviderName = "test provider",
                };
                foreach (var rowDefinition in row.Rows)
                {
                    
                }
                _easSubmissions.Add(submission);
            }
        }
    }



    class PeriodDefinition
    {
        public PeriodDefinition(string period)
        {
            int result;
            int month;
            int year;

            if (int.TryParse(period.Substring(0, 2), out result))
            {
                month = result;
            }
            else
            {
                throw new ArgumentException("The collection period should be of the form MM/yy");
            }

            if (int.TryParse(period.Substring(3), out result))
            {
                year = result + 2000;
            }
            else
            {
                throw new ArgumentException("The collection period should be of the form MM/yy");
            }

            CollectionMonth = month;
            CollectionYear = year;

            var periodEntity = ProviderAdjustmentsContext.CollectionPeriods.Single(x =>
                x.Calendar_Month == month &&
                x.Calendar_Year == year);

            CollectionPeriod = periodEntity.Period_ID;
            AcademicYear = periodEntity.Collection_Year;
            PeriodName = periodEntity.Collection_Period_Name;
        }
        public int CollectionPeriod { get; private set; }
        public int AcademicYear { get; private set; }

        public string PeriodName { get; private set; }
        public int CollectionMonth { get; private set; }
        public int CollectionYear { get; private set; }
    }
}