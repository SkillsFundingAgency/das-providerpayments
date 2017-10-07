using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery
{
    public class GetPaymentHistoryWhereNoEarningQueryHandler : IRequestHandler<GetPaymentHistoryWhereNoEarningQueryRequest, GetPaymentHistoryWhereNoEarningQueryResponse>
    {
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public GetPaymentHistoryWhereNoEarningQueryHandler(IRequiredPaymentRepository requiredPaymentRepository)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
        }
        public GetPaymentHistoryWhereNoEarningQueryResponse Handle(GetPaymentHistoryWhereNoEarningQueryRequest message)
        {
            try
            {
                var entities = _requiredPaymentRepository
                                   .GetPreviousPaymentsWithoutEarnings() ??
                               new Infrastructure.Data.Entities.RequiredPaymentEntity[0];

                return new GetPaymentHistoryWhereNoEarningQueryResponse
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
                            ApprenticeshipContractType = e.ApprenticeshipContractType,
                            FundingLineType = e.FundingLineType,
                            IlrSubmissionDateTime = e.IlrSubmissionDateTime,
                            PriceEpisodeIdentifier = e.PriceEpisodeIdentifier,
                            SfaContributionPercentage = e.SfaContributionPercentage,
                            UseLevyBalance = e.UseLevyBalance,
                            LearnAimRef = e.LearnAimRef,
                            LearningStartDate =e.LearningStartDate
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetPaymentHistoryWhereNoEarningQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}