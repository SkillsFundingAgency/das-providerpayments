using System;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    public class Learner
    {
        public long Uln { get; set; }
        public string LearnerRefNumber { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 10);
        public string GivenNames { get; set; } = "GivenNames";
        public string FamilyName { get; set; } = "FamilyName";
        public DateTime DateOfBirth { get; set; } = new DateTime(1992, 4, 25);
        public string NiNumber { get; set; } = "AB123456C";
        public string PostCode { get; set; } = "OX17 1EZ";
        public List<EmploymentStatus> EmploymentStatuses { get; set; } = new List<EmploymentStatus>();
        public List<LearningDelivery> LearningDeliveries { get; set; } = new List<LearningDelivery>();
    }
}