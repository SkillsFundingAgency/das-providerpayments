using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    class RemoveCompletionPaymentsWithoutEvidence : IFilterOutCompletionPaymentsWithoutEvidence
    {
        private readonly ICheckEmployerPayments _employerPaymentsChecker;

        public RemoveCompletionPaymentsWithoutEvidence(
            ICheckEmployerPayments employerPaymentsChecker)
        {
            _employerPaymentsChecker = employerPaymentsChecker;
        }

        public FilteredEarningsResult Process(List<LearnerSummaryPaymentEntity> employerPayments, List<RawEarning> earnings)
        {
            var nonPayableEarnings = new List<NonPayableEarning>();

            foreach (var rawEarning in earnings)
            {
                if (rawEarning.TransactionType02 == 0)
                {
                    continue;
                }

                if (rawEarning.ExemptionCodeForCompletionHoldback != 0)
                {
                    continue;
                }

                if (!_employerPaymentsChecker.EvidenceOfSufficientEmployerPayments(
                    employerPayments,
                    rawEarning))
                {
                    var heldBackCompletionPayment = new NonPayableEarning(rawEarning)
                    {
                        PaymentFailureMessage = "Historic evidence does not show enough employer payments",
                        PaymentFailureReason = PaymentFailureType.HeldBackCompletionPayment
                    };

                    nonPayableEarnings.Add(heldBackCompletionPayment);
                    rawEarning.TransactionType02 = 0;
                }
            }

            return new FilteredEarningsResult {RawEarnings = earnings, NonPayableEarnings = nonPayableEarnings};
        }
    }
}
