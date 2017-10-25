using System.Collections.Generic;

namespace ProviderPayments.TestStack.Domain.Data.Entities
{
    public class ProcessStatusEntity
    {
        public string Id { get; set; }
        public IEnumerable<ProcessStepEntity> Steps { get; set; }
    }
}
