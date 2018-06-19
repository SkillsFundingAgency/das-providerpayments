using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class DatalockOutput
    {
        public long Ukprn { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public long CommitmentId { get; set; }
        [StringLength(25)]
        public string VersionId { get; set; }
        [Range(1, 12)]
        public int Period { get; set; }
        public bool Payable { get; set; }
        [Range(1, 15)]
        public int TransactionType { get; set; }
        [Range(1, 3)]
        public int TransactionTypesFlag { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = 31 * hash + Ukprn.GetHashCode();
                hash = 31 * hash + PriceEpisodeIdentifier.GetHashCode();
                hash = 31 * hash + LearnRefNumber.GetHashCode();
                hash = 31 * hash + CommitmentId.GetHashCode();
                hash = 31 * hash + TransactionType.GetHashCode();
                hash = 31 * hash + Period.GetHashCode();
                hash = 31 * hash + Payable.GetHashCode();
                hash = 31 * hash + TransactionTypesFlag.GetHashCode();

                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            var testObject = obj as DatalockOutput;
            if (testObject == null)
            {
                return false;
            }

            return Ukprn == testObject.Ukprn &&
                   PriceEpisodeIdentifier == testObject.PriceEpisodeIdentifier &&
                   LearnRefNumber == testObject.LearnRefNumber &&
                   CommitmentId == testObject.CommitmentId &&
                   Period == testObject.Period &&
                   Payable == testObject.Payable &&
                   TransactionTypesFlag == testObject.TransactionTypesFlag;
        }
    }
}

