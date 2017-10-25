using System;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    public class EmploymentStatus
    {
        public int StatusCode { get; set; }
        public DateTime DateFrom { get; set; }
        public long EmployerId { get; set; }
        public EmploymentStatusMonitoring EmploymentStatusMonitoring { get; set; }
    }
}