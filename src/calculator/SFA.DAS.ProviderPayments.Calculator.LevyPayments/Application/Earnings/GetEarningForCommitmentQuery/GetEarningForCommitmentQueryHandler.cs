using MediatR;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery
{
    public class GetEarningForCommitmentQueryHandler : IRequestHandler<GetEarningForCommitmentQueryRequest, GetEarningForCommitmentQueryResponse>
    {
        private readonly IEarningRepository _earningRepository;

        public GetEarningForCommitmentQueryHandler(IEarningRepository earningRepository)
        {
            _earningRepository = earningRepository;
        }

        public GetEarningForCommitmentQueryResponse Handle(GetEarningForCommitmentQueryRequest message)
        {
            var earningEntity = _earningRepository.GetEarningForCommitment(message.CommitmentId);
            return new GetEarningForCommitmentQueryResponse
            {
                Earning = earningEntity == null ? null : new PeriodEarning
                {
                    CommitmentId = earningEntity.CommitmentId,
                    LearnerRefNumber = earningEntity.LearnerRefNumber,
                    AimSequenceNumber = earningEntity.AimSequenceNumber,
                    Ukprn = earningEntity.Ukprn,
                    LearningStartDate = earningEntity.LearningStartDate,
                    LearningPlannedEndDate = earningEntity.LearningPlannedEndDate,
                    LearningActualEndDate = earningEntity.LearningActualEndDate,
                    CurrentPeriod = earningEntity.CurrentPeriod,
                    TotalNumberOfPeriods = earningEntity.TotalNumberOfPeriods,
                    MonthlyInstallment = earningEntity.MonthlyInstallment,
                    MonthlyInstallmentCapped = earningEntity.MonthlyInstallmentCapped,
                    CompletionPayment = earningEntity.CompletionPayment,
                    CompletionPaymentCapped = earningEntity.CompletionPaymentCapped
                }
            };
        }
    }
}