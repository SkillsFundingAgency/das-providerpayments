using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public class Learner
    {
        public List<RawEarningEntity> RawEarnings { get; set; } = new List<RawEarningEntity>();
        public List<RawEarningMathsEnglishEntity> RawEarningsMathsEnglish { get; set; } = new List<RawEarningMathsEnglishEntity>();
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLocks { get; set; } = new List<DataLockPriceEpisodePeriodMatchEntity>();
        public List<RequiredPaymentsHistoryEntity> HistoricalPayments { get; set; } = new List<RequiredPaymentsHistoryEntity>();
        public List<RequiredPaymentEntity> RequiredPayments { get; set; } = new List<RequiredPaymentEntity>();

        public List<string> DatalockErrors { get; private set; } = new List<string>();

        private IEnumerable<RawEarningEntity> Act1RawEarnings => RawEarnings.Where(x => x.ApprenticeshipContractType == 1);
        private IEnumerable<RawEarningEntity> Act2RawEarnings => RawEarnings.Where(x => x.ApprenticeshipContractType == 2);

        public List<FundingDue> PayableEarnings { get; set; }
        //public List<NonPayableFundingDue> NonPayableEarnings { get; set; }

        
        public void ValidateDatalocks()
        {
            var act1 = Act1RawEarnings.ToList();
            // if there are *no* ACT1 earnings, then everything is ACT2 and payable
            if (act1.Count == 0)
            {
                foreach (var rawEarningEntity in RawEarnings)
                {
                    AddFundingDue(rawEarningEntity);
                }

                foreach (var rawEarningMathsEnglishEntity in RawEarningsMathsEnglish)
                {
                    AddFundingDue(rawEarningMathsEnglishEntity);
                }

                return;
            }
            
            // If there is a datalock, then ignore the price episode
        }

        private void CreatePriceEpisodes()
        {

        }

        public void CalculateFundingDue()
        {
            
        }

        private static readonly int[] RawEarningsTransactionTypes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 15 };

        private void AddFundingDue(RawEarningEntity rawEarnings)
        {

        }
        
        private static readonly int[] RawMathsAndEnglishTransactionTypes = { 13, 14, 15 };

        private void AddFundingDue(RawEarningMathsEnglishEntity mathsAndEnglish)
        {
            foreach (var transactionType in RawEarningsTransactionTypes)
            {

            }
        }

        private FundingDue CreateFundingDue(RawEarningEntity rawEarnings, int transactionType)
        {
            return new FundingDue();
        }

        private FundingDue CreateFundingDue(RawEarningMathsEnglishEntity rawMathsAndEnglishEarnings, int transactionType)
        {
            return new FundingDue();
        }

        private FundingDue CreateBasicFundingDue(IFundingDue fundingDue)
        {
            return new FundingDue();
        }
    }
}