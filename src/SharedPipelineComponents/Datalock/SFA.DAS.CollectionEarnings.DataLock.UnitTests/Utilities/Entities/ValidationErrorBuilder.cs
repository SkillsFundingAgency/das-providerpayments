using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Entities
{
    public class ValidationErrorBuilder : IBuilder<DatalockValidationError>
    {
        private long _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private int _aimSeqNumber = 1;
        private string _ruleId = DataLockErrorCodes.MismatchingUkprn;
        private string _priceEpisodeIdentifier = "27-25-2016-09-01";

        public DatalockValidationError Build()
        {
            return new DatalockValidationError
            {
                Ukprn = _ukprn,
                LearnRefNumber = _learnRefNumber,
                AimSeqNumber = _aimSeqNumber,
                RuleId = _ruleId,
                PriceEpisodeIdentifier=_priceEpisodeIdentifier
            };
        }

        public ValidationErrorBuilder WithUkprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public ValidationErrorBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public ValidationErrorBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public ValidationErrorBuilder WithRuleId(string ruleId)
        {
            _ruleId = ruleId;

            return this;
        }

        public ValidationErrorBuilder WithPriceEpisodeIdentifier(string priceEpisodeIdentifier)
        {
            _priceEpisodeIdentifier = priceEpisodeIdentifier;

            return this;
        }
    }
}