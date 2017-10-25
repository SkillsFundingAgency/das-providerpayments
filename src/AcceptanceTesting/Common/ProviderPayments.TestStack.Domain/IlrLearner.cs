using System;

namespace ProviderPayments.TestStack.Domain
{
    public class IlrLearner
    {
        public long Uln { get; set; }
        public short FamCode { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public decimal TrainingCost { get; set; }
        public decimal EndpointAssementCost { get; set; }
        public long StandardCode { get; set; }
        public int PathwayCode { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgrammeType { get; set; }

    }
}