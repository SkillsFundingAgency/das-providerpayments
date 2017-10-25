namespace SFA.DAS.Payments.Automation.IlrBuilder.Validation.UkprnRules
{
    public class MustHaveUkprnRule : ValidationRule
    {

        public override ValidationError[] Validate(IndividualLearningRecord ilr)
        {
            if (ilr.Ukprn < 1)
            {
                return new[]
                {
                    new ValidationError {Code = ErrorCodes.UkprnMissingCode, Description = "You must specify a UKPRN"}
                };
            }

            return new ValidationError[0];
        }
    }
}
