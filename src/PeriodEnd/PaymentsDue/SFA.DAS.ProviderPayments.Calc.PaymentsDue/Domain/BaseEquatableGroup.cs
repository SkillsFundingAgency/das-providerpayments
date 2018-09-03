using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public abstract class BaseEquatableGroup<T> : IEquatable<T> where T : class
    {
        public static bool operator ==(BaseEquatableGroup<T> left, BaseEquatableGroup<T> right)
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
        public static bool operator !=(BaseEquatableGroup<T> left, BaseEquatableGroup<T> right)
        {
            return !(left == right);
        }

        public virtual bool Equals(T other)
        {
            if (ReferenceEquals(this, other)) return true;
            return false;
        }

        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            var other = obj as T;
            return other != null && Equals(other);
        }
    }
}