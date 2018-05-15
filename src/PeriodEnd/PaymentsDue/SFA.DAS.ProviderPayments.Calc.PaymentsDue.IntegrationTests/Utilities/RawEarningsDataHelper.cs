using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    internal static class RawEarningsDataHelper
    {
        internal static void CreateRawEarning(RawEarning rawEarning)
        {
            const string sql = @"
            INSERT INTO Staging.RawEarnings (
                LearnRefNumber,
                Ukprn,
                PriceEpisodeAimSeqNumber,
                PriceEpisodeIdentifier,
                EpisodeStartDate,
                Period,
                ULN,
                ProgType,
                FworkCode,
                PwayCode,
                StdCode,
                PriceEpisodeSFAContribPct,
                PriceEpisodeFundLineType,
                LearnAimRef,
                LearnStartDate,
                TransactionType01,
                TransactionType02,
                TransactionType03,
                TransactionType04,
                TransactionType05,
                TransactionType06,
                TransactionType07,
                TransactionType08,
                TransactionType09,
                TransactionType10,
                TransactionType11,
                TransactionType12,
                TransactionType15,
                ACT
            ) VALUES (
                @LearnRefNumber,
                @Ukprn,
                @PriceEpisodeAimSeqNumber,
                @PriceEpisodeIdentifier,
                @EpisodeStartDate,
                @Period,
                @ULN,
                @ProgType,
                @FworkCode,
                @PwayCode,
                @StdCode,
                @PriceEpisodeSFAContribPct,
                @PriceEpisodeFundLineType,
                @LearnAimRef,
                @LearnStartDate,
                @TransactionType01,
                @TransactionType02,
                @TransactionType03,
                @TransactionType04,
                @TransactionType05,
                @TransactionType06,
                @TransactionType07,
                @TransactionType08,
                @TransactionType09,
                @TransactionType10,
                @TransactionType11,
                @TransactionType12,
                @TransactionType15,
                @ACT
            );";

            TestDataHelper.Execute(sql, rawEarning.ToDynamic());
        }

        internal static void Truncate()
        {
            throw new NotImplementedException();
        }
    }

    internal static class ObjectExtensions
    {
        internal static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
            {
                expando.Add(property.Name, property.GetValue(value));
            }

            return (ExpandoObject) expando;
        }
    }
    

    public class RawEarning
    {
        public string LearnRefNumber { get; set; }
        public long Ukprn { get; set; }
        public int PriceEpisodeAimSeqNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public DateTime EpisodeStartDate { get; set; }
        public int Period { get; set; }
        public long Uln { get; set; }
        public int ProgType { get; set; }
        public int FworkCode { get; set; }
        public int PwayCode { get; set; }
        public int StdCode { get; set; }
        public decimal PriceEpisodeSfaContribPct { get; set; }
        public string PriceEpisodeFundLineType { get; set; }
        public string LearnAimRef { get; set; }
        public DateTime LearnStartDate { get; set; }
        public decimal TransactionType01 { get; set; }
        public decimal TransactionType02 { get; set; }
        public decimal TransactionType03 { get; set; }
        public decimal TransactionType04 { get; set; }
        public decimal TransactionType05 { get; set; }
        public decimal TransactionType06 { get; set; }
        public decimal TransactionType07 { get; set; }
        public decimal TransactionType08 { get; set; }
        public decimal TransactionType09 { get; set; }
        public decimal TransactionType10 { get; set; }
        public decimal TransactionType11 { get; set; }
        public decimal TransactionType12 { get; set; }
        public decimal TransactionType15 { get; set; }
        public int Act { get; set; }
    }
}