using System;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Application
{
    public class CommitmentBuilder : IBuilder<Commitment>
    {
        private long _commitmentId = 1;
        private string _versionId = "1-001";
        private long _uln = 1000000019;
        private long _ukprn = 10007459;
        private long _providerUkprn = 10007459;
        private long _accountId = 1;
        private DateTime _startDate = new DateTime(2016, 9, 1);
        private DateTime _endDate = new DateTime(2018, 12, 31);
        private decimal _negoriatedPrice = 15000.00m;
        private long? _standardCode;
        private int? _programmeType = 20;
        private int? _frameworkCode = 550;
        private int? _pathwayCode = 6;
        private int _paymentStatus = 1;
        private string _paymentStatusDescription = "Active";
        private DateTime _effectiveFrom = new DateTime(2016, 9, 1);
        private DateTime? _effectiveTo;

        public Commitment Build()
        {
            var entity = new CommitmentEntity
            {
                CommitmentId = _commitmentId,
                VersionId = _versionId,
                Uln = _uln,
                Ukprn = _ukprn,
                AccountId = _accountId,
                StartDate = _startDate,
                EndDate = _endDate,
                AgreedCost = _negoriatedPrice,
                StandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pathwayCode,
                PaymentStatus = _paymentStatus,
                PaymentStatusDescription = _paymentStatusDescription,
                EffectiveFrom = _effectiveFrom,
                EffectiveTo = _effectiveTo,
                ProviderUkprn = _providerUkprn,
            };
            return new Commitment(entity);
        }

        public CommitmentBuilder WithCommitmentId(long commitmentId)
        {
            _commitmentId = commitmentId;

            return this;
        }

        public CommitmentBuilder WithVersionId(string versionId)
        {
            _versionId = versionId;

            return this;
        }

        public CommitmentBuilder WithUln(long uln)
        {
            _uln = uln;
            
            return this;
        }

        public CommitmentBuilder Withukprn(long ukprn)
        {
            _ukprn = ukprn;
            
            return this;
        }

        public CommitmentBuilder WithProviderUkprn(long ukprn)
        {
            _providerUkprn = ukprn;

            return this;
        }

        public CommitmentBuilder WithAccountId(long accountId)
        {
            _accountId = accountId;

            return this;
        }

        public CommitmentBuilder WithStartDate(DateTime startDate)
        {
            _startDate = startDate;

            return this;
        }

        public CommitmentBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;

            return this;
        }

        public CommitmentBuilder WithAgreedCost(decimal agreedCost)
        {
            _negoriatedPrice = agreedCost;

            return this;
        }

        public CommitmentBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public CommitmentBuilder WithProgrammeType(int? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public CommitmentBuilder WithFrameworkCode(int? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }
        public CommitmentBuilder WithPathwayCode(int? pathwayCode)
        {
            _pathwayCode = pathwayCode;

            return this;
        }

        public CommitmentBuilder WithPaymentStatus(PaymentStatus paymentStatus)
        {
            _paymentStatus = (int)paymentStatus;
            _paymentStatusDescription = paymentStatus.ToString();

            return this;
        }

        public CommitmentBuilder WithEffectiveFrom(DateTime effectiveFrom)
        {
            _effectiveFrom = effectiveFrom;

            return this;
        }

        public CommitmentBuilder WithEffectiveTo(DateTime? effectiveTo)
        {
            _effectiveTo = effectiveTo;

            return this;
        }
    }
}