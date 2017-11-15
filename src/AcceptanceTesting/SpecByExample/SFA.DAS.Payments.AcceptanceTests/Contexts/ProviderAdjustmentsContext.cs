using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProviderPayments.TestStack.Core.Domain;
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
        private static Dictionary<string, PaymentType> _paymentTypeNameLookup =  new Dictionary<string, PaymentType>(
                PaymentTypes.ToDictionary(x => x.PaymentName, x => x));

        public int CollectionYear { get; private set; }
        public int CollectionMonth { get; private set; }

        private int CollectionPeriod(string collectionPeriod)
        {
            return 0;
        }

        public void SetCollectionPeriod(string collectionPeriod)
        {

            var collectionPeriodForEnvironment = PeriodDefinition.ParsePeriod(collectionPeriod);
            var id = 0;

            // Will never have r13 or r14
            if (collectionPeriodForEnvironment.Item1 >= 8)
            {
                id = collectionPeriodForEnvironment.Item1 - 7;
            }
            else
            {
                id = collectionPeriodForEnvironment.Item1 + 5;
            }

            TestEnvironment.Variables.CollectionPeriod = new CollectionPeriod
            {
                CalendarMonth = collectionPeriodForEnvironment.Item1,
                CalendarYear = collectionPeriodForEnvironment.Item2,
                CollectionOpen = 1,
                PeriodId = id,
            };
            
            TestEnvironment.ProcessService.RunPrepareForEas(TestEnvironment.Variables);
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
                    var paymentType = PaymentTypeFromSpec(rowDefinition.Name);
                    
                    var easValue = new EasSubmissionValues
                    {
                        CollectionPeriod = collectionPeriod.CollectionPeriod,
                        PaymentId = paymentType.Payment_Id,
                        PaymentValue = rowDefinition.Amount,
                    };
                    submission.AddValue(easValue);
                }
                _easSubmissions.Add(submission);
            }

            foreach (var easSubmission in EasSubmissions)
            {
                ProviderAdjustmentsRepository.SaveSubmittedEas(easSubmission);
            }
        }
            
        private static PaymentType PaymentTypeFromSpec(string specValue)
        {
            var paymentName = ProviderAdjustmentsTableParser.PaymentNameFromSpec(specValue);
            if (_paymentTypeNameLookup.ContainsKey(paymentName))
            {
                return _paymentTypeNameLookup[paymentName];
            }
            throw new ArgumentOutOfRangeException($"Could not find a payment type of {paymentName} \n " +
                                                  $"Generated from payment type: {specValue}");
        }
    }

    

    class PeriodDefinition
    {
        public PeriodDefinition(string period)
        {
            var parsedPeriod = ParsePeriod(period);

            CollectionMonth = parsedPeriod.Item1;
            CollectionYear = parsedPeriod.Item2;
            var collectionPeriods = ProviderAdjustmentsContext.CollectionPeriods;
            var periodEntity = collectionPeriods.SingleOrDefault(x =>
                x.Calendar_Month == CollectionMonth &&
                x.Calendar_Year == CollectionYear);
            if (periodEntity == null)
            {
                throw new ArgumentOutOfRangeException($"Could not find collection period {period}");
            }

            CollectionPeriod = periodEntity.Period_ID;
            AcademicYear = periodEntity.Collection_Year;
            PeriodName = periodEntity.Collection_Period_Name;
        }

        public static Tuple<int, int> ParsePeriod(string period)
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
            return new Tuple<int, int>(month, year);
        }

        public int CollectionPeriod { get; private set; }
        public int AcademicYear { get; private set; }

        public string PeriodName { get; private set; }
        public int CollectionMonth { get; private set; }
        public int CollectionYear { get; private set; }
    }
}