using System;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Enums;
using SFA.DAS.CollectionEarnings.DataLock.Application.Earnings;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application
{
    public class IncentiveEarningsBuilder : IBuilder<IncentiveEarnings>
    {
        private long _ukprn = 1;
        private string _learnRefNumber = "1";
        private int _period = 1;
        private decimal _priceEpisodeFirstEmp1618Pay = 0;
        private decimal _priceEpisodeSecondEmp1618Pay = 0;
        private string _priceEpisodeIdentifier = null;


        public IncentiveEarnings Build()
        {
            return new IncentiveEarnings
            {
                LearnRefNumber=_learnRefNumber,
                Period =_period,
                PriceEpisodeFirstEmp1618Pay=_priceEpisodeFirstEmp1618Pay,
                PriceEpisodeIdentifier =_priceEpisodeIdentifier,
                PriceEpisodeSecondEmp1618Pay = _priceEpisodeSecondEmp1618Pay,
                Ukprn =_ukprn
            };
        }

        public IncentiveEarningsBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public IncentiveEarningsBuilder WithUkprn(long ukprn)
        {
            _ukprn= ukprn;

            return this;
        }

        public IncentiveEarningsBuilder WithPeriod(int period)
        {
            _period = period;

            return this;
        }

        public IncentiveEarningsBuilder WithFirstIncentiveAmount()
        {
            _priceEpisodeFirstEmp1618Pay= 500;
            
            return this;
        }


        public IncentiveEarningsBuilder WithSecondIncentiveAmount()
        {
            _priceEpisodeSecondEmp1618Pay= 500;

            return this;
        }

        public IncentiveEarningsBuilder WithPriceEpisodeIdentifier(string priceEpisodeIdentifier)
        {
            _priceEpisodeIdentifier= priceEpisodeIdentifier;
            return this;
        }

    }
}