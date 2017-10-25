using System;
using System.Linq;
using MediatR;

namespace SFA.DAS.Payments.Automation.Application.Submission.TransformSubmissionCommand
{
    public class TransformSubmissionCommandHandler : IRequestHandler<TransformSubmissionCommandRequest, TransformSubmissionCommandResponse>
    {
        public TransformSubmissionCommandResponse Handle(TransformSubmissionCommandRequest message)
        {
            ShiftDates(message);
            return new TransformSubmissionCommandResponse();
        }

     
        private void ShiftDates(TransformSubmissionCommandRequest message)
        {
            if ((message.ShiftToMonth.HasValue && !message.ShiftToYear.HasValue)
                || (!message.ShiftToMonth.HasValue && message.ShiftToYear.HasValue)
                || message.ShiftToMonth.Value < 1 || message.ShiftToMonth.Value > 12)
            {
                throw new Exception("Both ShiftToMonth and ShiftToYear must be set together or null together and represent a valid month and year");
            }

            var specDate = message.Specification.Arrangement.LearnerRecords.Select(x => x.StartDate).Min();
            var monthsToShift = ((message.ShiftToYear.Value - specDate.Year) * 12) + message.ShiftToMonth.Value - specDate.Month;

            foreach (var commitment in message.Specification.Arrangement.Commitments)
            {
                commitment.StartDate = commitment.StartDate.AddMonths(monthsToShift);
                commitment.EndDate = commitment.EndDate.AddMonths(monthsToShift);
                commitment.EffectiveFrom = commitment.EffectiveFrom.HasValue ? (DateTime?)commitment.EffectiveFrom.Value.AddMonths(monthsToShift) : null;
                commitment.EffectiveTo = commitment.EffectiveTo.HasValue ? (DateTime?)commitment.EffectiveTo.Value.AddMonths(monthsToShift) : null;
            }

            foreach (var contractType in message.Specification.Arrangement.ContractTypes)
            {
                contractType.DateFrom = contractType.DateFrom.AddMonths(monthsToShift);
                contractType.DateTo = contractType.DateTo.AddMonths(monthsToShift);
            }

            foreach (var employmentStatus in message.Specification.Arrangement.EmploymentStatuses)
            {
                employmentStatus.EmploymentStatusApplies = employmentStatus.EmploymentStatusApplies.AddMonths(monthsToShift);
            }

            foreach (var learner in message.Specification.Arrangement.LearnerRecords)
            {
                learner.StartDate = learner.StartDate.AddMonths(monthsToShift);
                learner.PlannedEndDate = learner.PlannedEndDate.AddMonths(monthsToShift);
                learner.ActualEndDate = learner.ActualEndDate.HasValue ? (DateTime?)learner.ActualEndDate.Value.AddMonths(monthsToShift) : null;
                learner.TotalTrainingPrice1EffectiveDate = learner.TotalTrainingPrice1EffectiveDate != DateTime.MinValue ? learner.TotalTrainingPrice1EffectiveDate.AddMonths(monthsToShift) : learner.TotalTrainingPrice1EffectiveDate;
                learner.TotalAssessmentPrice1EffectiveDate = learner.TotalAssessmentPrice1EffectiveDate.HasValue ? (DateTime?)learner.TotalAssessmentPrice1EffectiveDate.Value.AddMonths(monthsToShift) : null;
                learner.TotalTrainingPrice2EffectiveDate = learner.TotalTrainingPrice2EffectiveDate.HasValue ? (DateTime?)learner.TotalTrainingPrice2EffectiveDate.Value.AddMonths(monthsToShift) : null;
                learner.TotalAssessmentPrice2EffectiveDate = learner.TotalAssessmentPrice2EffectiveDate.HasValue ? (DateTime?)learner.TotalAssessmentPrice2EffectiveDate.Value.AddMonths(monthsToShift) : null;
                learner.ResidualTrainingPrice1EffectiveDate = learner.ResidualTrainingPrice1EffectiveDate.HasValue ? (DateTime?)learner.ResidualTrainingPrice1EffectiveDate.Value.AddMonths(monthsToShift) : null;
                learner.ResidualAssessmentPrice1EffectiveDate = learner.ResidualAssessmentPrice1EffectiveDate.HasValue ? (DateTime?)learner.ResidualAssessmentPrice1EffectiveDate.Value.AddMonths(monthsToShift) : null;
                learner.ResidualTrainingPrice2EffectiveDate = learner.ResidualTrainingPrice2EffectiveDate.HasValue ? (DateTime?)learner.ResidualTrainingPrice2EffectiveDate.Value.AddMonths(monthsToShift) : null;
                learner.ResidualAssessmentPrice2EffectiveDate = learner.ResidualAssessmentPrice2EffectiveDate.HasValue ? (DateTime?)learner.ResidualAssessmentPrice2EffectiveDate.Value.AddMonths(monthsToShift) : null;
            }
        }
    }
}