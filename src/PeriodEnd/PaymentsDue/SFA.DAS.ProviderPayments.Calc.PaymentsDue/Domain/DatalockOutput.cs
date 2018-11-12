using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    /// <summary>
    /// Datalock value object
    ///     Account id is assumed to be equal if the commitment id is equal
    ///     Account version id is not used in equality comparisons
    /// </summary>
    public sealed class DatalockOutput : IEquatable<DatalockOutput>, IHoldCommitmentInformation
    {
        public DatalockOutput(DatalockOutputEntity entity, Commitment commitment)
        {
            Ukprn = entity.Ukprn;
            PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier;
            LearnRefNumber = entity.LearnRefNumber??string.Empty;
            CommitmentId = entity.CommitmentId;
            Period = entity.Period;
            Payable = entity.Payable;
            TransactionTypeGroup = (TransactionTypeGroup)entity.TransactionTypesFlag;
            AccountId = commitment.AccountId;
            AccountVersionId = commitment.AccountVersionId;
            CommitmentVersionId = commitment.CommitmentVersionId;
        }

        public DatalockOutput(DatalockOutput entity, TransactionTypeGroup transactionTypeGroup)
        {
            Ukprn = entity.Ukprn;
            PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier;
            LearnRefNumber = entity.LearnRefNumber ?? string.Empty;
            CommitmentId = entity.CommitmentId;
            Period = entity.Period;
            Payable = entity.Payable;
            TransactionTypeGroup = transactionTypeGroup;
            AccountId = entity.AccountId;
            AccountVersionId = entity.AccountVersionId;
            CommitmentVersionId = entity.CommitmentVersionId;
        }


        public long Ukprn { get; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; }
        [StringLength(12)]
        public string LearnRefNumber { get; }
        public long CommitmentId { get; }
        public string CommitmentVersionId { get; }
        public long AccountId { get; }
        public string AccountVersionId { get; }

        [Range(1, 12)]
        public int Period { get; }
        public bool Payable { get; }
        public TransactionTypeGroup TransactionTypeGroup { get; }

        public override int GetHashCode()
        {
            // Assumptions:
            //  If the commitment id is the same, the account id will be the same
            //  We ignore the account version id
            unchecked
            {
                var hash = 17;
                hash = 31 * hash + Ukprn.GetHashCode();
                hash = 31 * hash + PriceEpisodeIdentifier.GetHashCode();
                hash = 31 * hash + LearnRefNumber.GetHashCode();
                hash = 31 * hash + CommitmentId.GetHashCode();
                hash = 31 * hash + Period.GetHashCode();
                hash = 31 * hash + Payable.GetHashCode();
                hash = 31 * hash + TransactionTypeGroup.GetHashCode();
                hash = 31 * hash + CommitmentVersionId.GetHashCode();

                return hash;
            }
        }

        public bool Equals(DatalockOutput obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Ukprn == obj.Ukprn &&
                   PriceEpisodeIdentifier == obj.PriceEpisodeIdentifier &&
                   LearnRefNumber == obj.LearnRefNumber &&
                   CommitmentId == obj.CommitmentId &&
                   Period == obj.Period &&
                   Payable == obj.Payable &&
                   CommitmentVersionId == obj.CommitmentVersionId &&
                   TransactionTypeGroup == obj.TransactionTypeGroup;
        }

        public override bool Equals(object obj)
        {
            var other = obj as DatalockOutput;
            return other != null && Equals(other);
        }

        public static bool operator == (DatalockOutput left, DatalockOutput right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null))
            {
                return false;
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator != (DatalockOutput left, DatalockOutput right)
        {
            return !(left == right);
        }
    }
}
