using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Payments.PaymentHistoryForMultipleLearnersQuery
{
    public class PaymentHistoryForMultipleLearnersQueryHandler : IRequestHandler<PaymentHistoryForMultipleLearnersRequest, PaymentHistoryForMultipleLearnersResponse>
    {
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public PaymentHistoryForMultipleLearnersQueryHandler(IRequiredPaymentRepository requiredPaymentRepository)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
        }

        public PaymentHistoryForMultipleLearnersResponse Handle(PaymentHistoryForMultipleLearnersRequest message)
        {
            try
            {
                var entities = _requiredPaymentRepository
                                   .GetPreviousPaymentsForMultipleLearners(message.Ukprn, message.LearnRefNumbers) ??
                               new Infrastructure.Data.Entities.HistoricalRequiredPaymentEntity[0];

                return new PaymentHistoryForMultipleLearnersResponse
                {
                    IsValid = true,
                    Items = entities.Select(e =>
                        new RequiredPayment
                        {
                            CommitmentId = e.CommitmentId,
                            Ukprn = e.Ukprn,
                            LearnerRefNumber = e.LearnRefNumber,
                            AimSequenceNumber = e.AimSeqNumber,
                            DeliveryMonth = e.DeliveryMonth,
                            DeliveryYear = e.DeliveryYear,
                            AmountDue = e.AmountDue,
                            Uln = e.Uln,
                            StandardCode = e.StandardCode,
                            ProgrammeType = e.ProgrammeType,
                            FrameworkCode = e.FrameworkCode,
                            PathwayCode = e.PathwayCode,
                            TransactionType = (TransactionType)e.TransactionType,
                            CommitmentVersionId = e.CommitmentVersionId,
                            AccountId = e.AccountId,
                            AccountVersionId = e.AccountVersionId,
                            LearnAimRef = e.LearnAimRef,
                            LearningStartDate = e.LearningStartDate,
                            CollectionPeriodMonth = e.CollectionPeriodMonth,
                            CollectionPeriodYear = e.CollectionPeriodYear,
                            ApprenticeshipContractType = e.ApprenticeshipContractType,
                            FundingLineType = e.FundingLineType,
                            IlrSubmissionDateTime = e.IlrSubmissionDateTime,
                            PriceEpisodeIdentifier = e.PriceEpisodeIdentifier,
                            SfaContributionPercentage = e.SfaContributionPercentage,
                            UseLevyBalance = e.UseLevyBalance
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new PaymentHistoryForMultipleLearnersResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
