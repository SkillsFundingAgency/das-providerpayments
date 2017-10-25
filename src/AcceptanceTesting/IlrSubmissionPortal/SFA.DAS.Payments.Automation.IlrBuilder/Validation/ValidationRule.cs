namespace SFA.DAS.Payments.Automation.IlrBuilder.Validation
{
    public abstract class ValidationRule
    {
        public abstract ValidationError[] Validate(IndividualLearningRecord ilr);
    }
}
