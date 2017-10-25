namespace SFA.DAS.Payments.Automation.IlrBuilder.Validation.PreparationDateRules
{
    public class PrepDateMustBeInAcademicYearRule : ValidationRule
    {
        public override ValidationError[] Validate(IndividualLearningRecord ilr)
        {
            var prepAcademicYear = ilr.PreparationDate.ToAcademicYear();
            if (prepAcademicYear != ilr.AcademicYear)
            {
                return new[]
                {
                    new ValidationError {Code = ErrorCodes.PrepDateNotInYearCode, Description = $"Preparation date is in academic year {prepAcademicYear} but ILR is for {ilr.AcademicYear}"}
                };
            }
            return new ValidationError[0];
        }
    }
}
