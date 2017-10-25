using System.Collections.Generic;
using System.Linq;

namespace ProviderPayments.TestStack.Domain
{
    public class ProcessStatus
    {
        public string Id { get; set; }
        public IEnumerable<ProcessStep> Steps { get; set; }

        public bool IsComplete
        {
            get { return !Steps.Any(s => !s.EndTime.HasValue); }
        }
        public bool IsErrored
        {
            get { return Steps.Any(s => !string.IsNullOrEmpty(s.ErrorMessage)); }
        }
        public IEnumerable<string> ErrorMessages
        {
            get { return Steps.Where(s => !string.IsNullOrEmpty(s.ErrorMessage)).Select(s => s.ErrorMessage); }
        }
    }
}
