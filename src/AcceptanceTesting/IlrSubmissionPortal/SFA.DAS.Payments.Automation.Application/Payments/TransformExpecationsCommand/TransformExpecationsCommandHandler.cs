using System;
using System.Linq;
using MediatR;

namespace SFA.DAS.Payments.Automation.Application.Submission.TransformExpecationsCommand
{
    public class TransformExpecationsCommandHandler : IRequestHandler<TransformExpecationsCommandRequest, TransformExpecationsCommandResponse>
    {
        public TransformExpecationsCommandResponse Handle(TransformExpecationsCommandRequest message)
        {
            ShiftDates(message);
            return new TransformExpecationsCommandResponse();
        }

     
        private void ShiftDates(TransformExpecationsCommandRequest message)
        {
            if ((message.ShiftToMonth.HasValue && !message.ShiftToYear.HasValue)
                || (!message.ShiftToMonth.HasValue && message.ShiftToYear.HasValue)
                || message.ShiftToMonth.Value < 1 || message.ShiftToMonth.Value > 12)
            {
                throw new Exception("Both ShiftToMonth and ShiftToYear must be set together or null together and represent a valid month and year");
            }


            var startingPeriod = message.Expectations.EarningsAndPayments.
                SelectMany(x => x.EarningAndPaymentsByPeriod, (p, c) =>
                    new { Period = c.Period.ToPeriodDateTime() });

            var minPeriod = startingPeriod.Min(x=> x.Period);
            var monthsToShift = ((message.ShiftToYear.Value - minPeriod.Year) * 12) + message.ShiftToMonth.Value - minPeriod.Month;

            foreach(var exp in message.Expectations.EarningsAndPayments)
            {
                foreach(var period in exp.EarningAndPaymentsByPeriod)
                {
                    period.Period = period.Period.ToPeriodDateTime().AddMonths(monthsToShift).ToPeriodName();
                }
            }
          
        }
    }
}