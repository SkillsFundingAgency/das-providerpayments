using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery
{
    public class GetPaymentHistoryQueryHandler : IRequestHandler<GetPaymentHistoryQueryRequest, GetPaymentHistoryQueryResponse>
    {
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public GetPaymentHistoryQueryHandler(IRequiredPaymentRepository requiredPaymentRepository)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
        }

        public GetPaymentHistoryQueryResponse Handle(GetPaymentHistoryQueryRequest message)
        {
            try
            {
                var entities =
                    _requiredPaymentRepository.GetPreviousPayments(message.Ukprn, message.LearnRefNumber)
                    ?? new Infrastructure.Data.Entities.HistoricalRequiredPaymentEntity[0];

                return new GetPaymentHistoryQueryResponse
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
                            ApprenticeshipContractType =e.ApprenticeshipContractType,
                            FundingLineType =e.FundingLineType,
                            IlrSubmissionDateTime=e.IlrSubmissionDateTime,
                            PriceEpisodeIdentifier =e.PriceEpisodeIdentifier,
                            SfaContributionPercentage =e.SfaContributionPercentage,
                            UseLevyBalance =e.UseLevyBalance,
                            IsSmallEmployer = e.IsSmallEmployer
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetPaymentHistoryQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}