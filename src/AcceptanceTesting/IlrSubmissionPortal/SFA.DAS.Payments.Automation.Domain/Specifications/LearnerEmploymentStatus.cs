using System;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class LearnerEmploymentStatus
    {
        public virtual string EmployerKey { get; set; }
        public virtual EmploymentStatus EmploymentStatus { get; set; }
        public virtual DateTime EmploymentStatusApplies { get; set; }
                
        public virtual EmploymentStatusMonitoringType MonitoringType { get; set; }
        public virtual int MonitoringCode { get; set; }
    }
}