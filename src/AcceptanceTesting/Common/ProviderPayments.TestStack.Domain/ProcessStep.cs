using System;

namespace ProviderPayments.TestStack.Domain
{
    public class ProcessStep
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ErrorMessage { get; set; }
    }
}