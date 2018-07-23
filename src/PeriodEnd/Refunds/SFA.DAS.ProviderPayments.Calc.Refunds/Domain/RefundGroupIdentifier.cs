using System;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Domain
{
    class RefundGroupIdentifier : IEquatable<RefundGroupIdentifier>
    {
        public RefundGroupIdentifier(RequiredPaymentEntity entity)
        {
            AccountId = entity.AccountId;
            TransactionType = entity.TransactionType;
            ApprenticeshipContractType = entity.ApprenticeshipContractType;
            FundingLineType = entity.FundingLineType ?? string.Empty;
        }

        public RefundGroupIdentifier(HistoricalPayment entity)
        {
            AccountId = entity.AccountId;
            TransactionType = entity.TransactionType;
            ApprenticeshipContractType = entity.ApprenticeshipContractType;
            FundingLineType = entity.FundingLineType ?? string.Empty;
        }

        public long AccountId { get; }
        public TransactionType TransactionType { get; }
        public int ApprenticeshipContractType { get; }
        public string FundingLineType { get; }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as RefundGroupIdentifier;
            return other != null && Equals(other);
        }

        public bool Equals(RefundGroupIdentifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return AccountId == other.AccountId &&
                   TransactionType == other.TransactionType &&
                   ApprenticeshipContractType == other.ApprenticeshipContractType &&
                   FundingLineType == other.FundingLineType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 17;
                hashCode = hashCode * 31 + AccountId.GetHashCode();
                hashCode = hashCode * 31 + TransactionType.GetHashCode();
                hashCode = hashCode * 31 + ApprenticeshipContractType.GetHashCode();
                hashCode = hashCode * 31 + FundingLineType.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(RefundGroupIdentifier left, RefundGroupIdentifier right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RefundGroupIdentifier left, RefundGroupIdentifier right)
        {
            return !Equals(left, right);
        }
    }
}
