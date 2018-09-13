using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Entities
{
    public class IncentiveEarningsEntityBuilder : IBuilder<IncentiveEarningsEntity>
    {
        private long _ukprn = 1;
        private string _learnRefNumber = "1";
        private int _period = 1;
        private decimal _priceEpisodeFirstEmp1618Pay = 0;
        private decimal _priceEpisodeSecondEmp1618Pay = 0;
        private string _priceEpisodeIdentifier = null;
        
        public IncentiveEarningsEntity Build()
        {
            return new IncentiveEarningsEntity
            {
                LearnRefNumber=_learnRefNumber,
                Period =_period,
                PriceEpisodeFirstEmp1618Pay=_priceEpisodeFirstEmp1618Pay,
                PriceEpisodeIdentifier =_priceEpisodeIdentifier,
                PriceEpisodeSecondEmp1618Pay = _priceEpisodeSecondEmp1618Pay,
                Ukprn =_ukprn
            };
        }
        public IncentiveEarningsEntityBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public IncentiveEarningsEntityBuilder WithUkprn(long ukprn)
        {
            _ukprn= ukprn;

            return this;
        }

        public IncentiveEarningsEntityBuilder WithPeriod(int period)
        {
            _period = period;

            return this;
        }

        public IncentiveEarningsEntityBuilder WithFirstIncentiveAmount()
        {
            _priceEpisodeFirstEmp1618Pay= 500;
            
            return this;
        }


        public IncentiveEarningsEntityBuilder WithSecondIncentiveAmount()
        {
            _priceEpisodeSecondEmp1618Pay= 500;

            return this;
        }

        public IncentiveEarningsEntityBuilder WithPriceEpisodeIdentifier(string priceEpisodeIdentifier)
        {
            _priceEpisodeIdentifier= priceEpisodeIdentifier;
            return this;
        }

    }
}