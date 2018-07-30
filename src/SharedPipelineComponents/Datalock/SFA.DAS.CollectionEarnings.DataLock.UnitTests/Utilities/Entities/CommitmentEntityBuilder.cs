using System;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Enums;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Entities
{
    public class CommitmentEntityBuilder : IBuilder<CommitmentEntity>
    {
        private long _commitmentId = 1;
        private string _versionId = "1-001";
        private long _uln = 1000000019;
        private long _ukprn = 10007459;
        private long _accountId = 1;
        private DateTime _startDate = new DateTime(2016, 9, 1);
        private DateTime _endDate = new DateTime(2018, 12, 31);
        private decimal _agreedCost = 15000.00m;
        private long? _standardCode;
        private int? _programmeType = 20;
        private int? _frameworkCode = 550;
        private int? _pathwayCode = 6;
        private int _paymentStatus = 1;
        private string _paymentStatusDescription = "Active";
        private DateTime _effectiveFrom = new DateTime(2016, 9, 1);
        private DateTime? _effectiveTo;
        private int _priority;

        public CommitmentEntity Build()
        {
            return new CommitmentEntity
            {
                CommitmentId = _commitmentId,
                VersionId = _versionId,
                Uln = _uln,
                Ukprn = _ukprn,
                AccountId = _accountId,
                StartDate = _startDate,
                EndDate = _endDate,
                AgreedCost = _agreedCost,
                StandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pathwayCode,
                PaymentStatus = _paymentStatus,
                PaymentStatusDescription = _paymentStatusDescription,
                EffectiveFrom = _effectiveFrom,
                EffectiveTo = _effectiveTo,
                Priority=_priority
            };
        }

        public CommitmentEntityBuilder WithCommitmentId(long commitmentId)
        {
            _commitmentId = commitmentId;

            return this;
        }

        public CommitmentEntityBuilder WithVersionId(string versionId)
        {
            _versionId = versionId;

            return this;
        }

        public CommitmentEntityBuilder WithUln(long uln)
        {
            _uln = uln;

            return this;
        }

        public CommitmentEntityBuilder Withukprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public CommitmentEntityBuilder WithAccountId(long accountId)
        {
            _accountId = accountId;

            return this;
        }

        public CommitmentEntityBuilder WithStartDate(DateTime startDate)
        {
            _startDate = startDate;

            return this;
        }

        public CommitmentEntityBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;

            return this;
        }

        public CommitmentEntityBuilder WithAgreedCost(decimal agreedCost)
        {
            _agreedCost = agreedCost;

            return this;
        }

        public CommitmentEntityBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public CommitmentEntityBuilder WithProgrammeType(int? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public CommitmentEntityBuilder WithFrameworkCode(int? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }
        public CommitmentEntityBuilder WithPathwayCode(int? pathwayCode)
        {
            _pathwayCode = pathwayCode;

            return this;
        }

        public CommitmentEntityBuilder WithPaymentStatus(PaymentStatus paymentStatus)
        {
            _paymentStatus = (int)paymentStatus;
            _paymentStatusDescription = paymentStatus.ToString();

            return this;
        }

        public CommitmentEntityBuilder WithEffectiveFrom(DateTime effectiveFrom)
        {
            _effectiveFrom = effectiveFrom;

            return this;
        }

        public CommitmentEntityBuilder WithEffectiveTo(DateTime? effectiveTo)
        {
            _effectiveTo = effectiveTo;

            return this;
        }
        public CommitmentEntityBuilder WithPriority(int priority)
        {
            _priority= priority;

            return this;
        }
    }
}