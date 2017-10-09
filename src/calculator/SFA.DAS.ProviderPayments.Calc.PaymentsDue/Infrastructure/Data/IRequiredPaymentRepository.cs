using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data
{
    public interface IRequiredPaymentRepository
    {
        void AddRequiredPayments(RequiredPaymentEntity[] payments);

        HistoricalRequiredPaymentEntity[] GetPreviousPayments(long ukprn, string learnRefNumber);

        /// <summary>
        /// Previous payments made for all learners in <paramref name="learnRefNumbers"/> for provider with <paramref name="ukprn"/>
        ///     Sourced from PaymentsDue.vw_PaymentHistoryWithoutEarnings
        /// </summary>
        /// <param name="ukprn">The provider's ukprn</param>
        /// <param name="learnRefNumbers">The list of learners' provider supplied reference numbers</param>
        /// <returns>A list of payments</returns>
        HistoricalRequiredPaymentEntity[] GetPreviousPaymentsForMultipleLearners(long ukprn, IEnumerable<string> learnRefNumbers);

        RequiredPaymentEntity[] GetPreviousPaymentsWithoutEarnings();
    }
}