using System;

namespace IlrGenerator
{
    public class EmploymentStatus
    {
        public int StatusCode { get; set; }
        public DateTime DateFrom { get; set; }
        public long EmployerId { get; set; }
        public EmploymentStatusMonitoring EmploymentStatusMonitoring { get; set; }
    }
}
