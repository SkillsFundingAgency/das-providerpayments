using System.Linq;

namespace SFA.DAS.Payments.Automation.IlrBuilder.Validation.PreparationDateRules
{
    public class PrepDateMustNotBeforeBeforeLearnStartDatesRule : ValidationRule
    {
        public override ValidationError[] Validate(IndividualLearningRecord ilr)
        {
            var latestStartDate = ilr.Learners.SelectMany(x => x.LearningDeliveries).Max(x => x.LearningStartDate);
            if (ilr.PreparationDate < latestStartDate)
            {
                return new[]
                {
                    new ValidationError {Code = ErrorCodes.PrepDateBeforeLearnStart, Description = $"File preparation date of '{ilr.PreparationDate}' is before the lastest learning start date of '{latestStartDate}'"}
                };
            }
            return new ValidationError[0];
        }
    }
}
