
using System;

namespace IlrGenerator
{
    public class Learner
    {
        public long Uln { get; set; }
        public string LearnRefNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public LearningDelivery[] LearningDeliveries { get; set; }
        public EmploymentStatus[] EmploymentStatuses { get; set; }
    }
}