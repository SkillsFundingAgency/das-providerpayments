using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities
{
    public class PriceEpisodePeriodMatchEntityBuilder : IBuilder<PriceEpisodePeriodMatchEntity>
    {
        private long _ukprn = 10007459;
        private string _priceEpisodeIdentifier = "27-25-2016-09-01";
        private string _learnRefNumber = "Lrn001";
        private long _aimSeqNumber = 1;
        private long _commitmentId = 1;
        private string _versionId = "1-001";
        private int _period = 1;
        private bool _payable = true;
        private TransactionType _transactionType = TransactionType.Learning;

        public PriceEpisodePeriodMatchEntity Build()
        {
            return new PriceEpisodePeriodMatchEntity
            {
                Ukprn = _ukprn,
                PriceEpisodeIdentifier = _priceEpisodeIdentifier,
                LearnRefNumber = _learnRefNumber,
                AimSeqNumber = _aimSeqNumber,
                CommitmentId = _commitmentId,
                VersionId = _versionId,
                Period = _period,
                Payable = _payable,
                TransactionType=_transactionType
            };
        }

        public PriceEpisodePeriodMatchEntityBuilder WithUkprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public PriceEpisodePeriodMatchEntityBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public PriceEpisodePeriodMatchEntityBuilder WithAimSeqNumber(long aimSeqNumber)
        {
            _aimSeqNumber = aimSeqNumber;

            return this;
        }

        public PriceEpisodePeriodMatchEntityBuilder WithCommitmentId(long commitmentId)
        {
            _commitmentId = commitmentId;

            return this;
        }

        public PriceEpisodePeriodMatchEntityBuilder WithVersionId(string versionId)
        {
            _versionId = versionId;

            return this;
        }

        public PriceEpisodePeriodMatchEntityBuilder WithPriceEpisodeIdentifier(string priceEpisodeIdentifier)
        {
            _priceEpisodeIdentifier = priceEpisodeIdentifier;

            return this;
        }

        public PriceEpisodePeriodMatchEntityBuilder WithPeriod(int period)
        {
            _period = period;

            return this;
        }

        public PriceEpisodePeriodMatchEntityBuilder WithPayable(bool payable)
        {
            _payable = payable;

            return this;
        }

    }
}