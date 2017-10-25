using System;

namespace SFA.DAS.Payments.Automation.Lars
{
    public interface ILarsRepository
    {
        LearningAim[] GetComponentLearningAims(int frameworkCode, int programmeType, int pathwayCode, DateTime effectiveAt);
        LearningAim[] GetComponentLearningAims(long standardCode, DateTime effectiveAt);
    }
}