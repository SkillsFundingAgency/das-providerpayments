﻿using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class DatalockOutput : IEquatable<DatalockOutput>
    {
        public DatalockOutput(DatalockOutputEntity entity)
        {
            Ukprn = entity.Ukprn;
            PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier;
            LearnRefNumber = entity.LearnRefNumber;
            CommitmentId = entity.CommitmentId;
            Period = entity.Period;
            Payable = entity.Payable;
            TransactionTypesFlag = entity.TransactionTypesFlag;
        }
        public long Ukprn { get; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; }
        [StringLength(12)]
        public string LearnRefNumber { get; }
        public long CommitmentId { get; }
        [Range(1, 12)]
        public int Period { get; }
        public bool Payable { get; }
        [Range(1, 3)]
        public int TransactionTypesFlag { get; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = 31 * hash + Ukprn.GetHashCode();
                hash = 31 * hash + PriceEpisodeIdentifier.GetHashCode();
                hash = 31 * hash + LearnRefNumber.GetHashCode();
                hash = 31 * hash + CommitmentId.GetHashCode();
                hash = 31 * hash + Period.GetHashCode();
                hash = 31 * hash + Payable.GetHashCode();
                hash = 31 * hash + TransactionTypesFlag.GetHashCode();

                return hash;
            }
        }

        public bool Equals(DatalockOutput obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Ukprn == obj.Ukprn &&
                   PriceEpisodeIdentifier == obj.PriceEpisodeIdentifier &&
                   LearnRefNumber == obj.LearnRefNumber &&
                   CommitmentId == obj.CommitmentId &&
                   Period == obj.Period &&
                   Payable == obj.Payable &&
                   TransactionTypesFlag == obj.TransactionTypesFlag;
        }
    }
}
