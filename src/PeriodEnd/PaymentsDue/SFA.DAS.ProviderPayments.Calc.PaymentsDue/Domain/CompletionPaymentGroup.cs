using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    /// <summary>
    /// Value object for determining whether two completion payments should be
    /// </summary>
    public sealed class CompletionPaymentGroup : IEquatable<CompletionPaymentGroup>
    {
        public CompletionPaymentGroup(decimal priceEpisodeCumulativePmrs,
            int priceEpisodeCompExemCode)
        {
            PriceEpisodeCumulativePmrs = priceEpisodeCumulativePmrs;
            PriceEpisodeCompExemCode = priceEpisodeCompExemCode;
        }

        public decimal PriceEpisodeCumulativePmrs { get; }
        public int PriceEpisodeCompExemCode { get; }
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = 31 * hash + PriceEpisodeCumulativePmrs.GetHashCode();
                hash = 31 * hash + PriceEpisodeCompExemCode.GetHashCode();

                return hash;
            }
        }

        public static bool operator ==(CompletionPaymentGroup left, CompletionPaymentGroup right)
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

        public static bool operator !=(CompletionPaymentGroup left, CompletionPaymentGroup right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PaymentGroup;
            return other != null && Equals(other);
        }

        public bool Equals(CompletionPaymentGroup test)
        {
            if (ReferenceEquals(null, test)) return false;
            if (ReferenceEquals(this, test)) return true;

            return PriceEpisodeCumulativePmrs == test.PriceEpisodeCumulativePmrs &&
                   PriceEpisodeCompExemCode == test.PriceEpisodeCompExemCode;
        }
    }
}