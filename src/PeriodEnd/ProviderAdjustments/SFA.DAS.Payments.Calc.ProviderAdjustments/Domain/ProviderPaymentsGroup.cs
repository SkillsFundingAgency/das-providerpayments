using System;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Domain
{
    public class ProviderPaymentsGroup : IEquatable<ProviderPaymentsGroup>
    {
        public long Ukprn { get; }
        public int PaymentType { get; }
        public int Period { get; }

        // Non-equality members
        public Guid SubmissionId { get; }
        public string PaymentTypeName { get; set; }

        public ProviderPaymentsGroup(AdjustmentEntity adjustment)
        {
            Ukprn = adjustment.Ukprn;
            PaymentType = adjustment.PaymentType;
            Period = adjustment.SubmissionCollectionPeriod;
            SubmissionId = adjustment.SubmissionId;
            PaymentTypeName = adjustment.PaymentTypeName;
        }

        public bool Equals(ProviderPaymentsGroup other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Ukprn == other.Ukprn &&
                   PaymentType == other.PaymentType &&
                   Period == other.Period 
                   //SubmissionId == other.SubmissionId
                   ;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as ProviderPaymentsGroup;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = 31 * hash + Ukprn.GetHashCode();
                hash = 31 * hash + PaymentType.GetHashCode();
                hash = 31 * hash + Period.GetHashCode();
                //hash = 31 * hash + SubmissionId.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(ProviderPaymentsGroup left, ProviderPaymentsGroup right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProviderPaymentsGroup left, ProviderPaymentsGroup right)
        {
            return !Equals(left, right);
        }
    }
}
