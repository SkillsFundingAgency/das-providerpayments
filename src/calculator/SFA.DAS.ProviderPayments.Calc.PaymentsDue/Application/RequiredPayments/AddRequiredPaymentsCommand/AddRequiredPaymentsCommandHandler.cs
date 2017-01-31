using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand
{
    public class AddRequiredPaymentsCommandHandler : IRequestHandler<AddRequiredPaymentsCommandRequest, AddRequiredPaymentsCommandResponse>
    {
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public AddRequiredPaymentsCommandHandler(IRequiredPaymentRepository requiredPaymentRepository)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
        }

        public AddRequiredPaymentsCommandResponse Handle(AddRequiredPaymentsCommandRequest message)
        {
            try
            {
                var paymentEntities = message.Payments
                    .Select(
                        p => new RequiredPaymentEntity
                        {
                            CommitmentId = p.CommitmentId,
                            CommitmentVersionId = p.CommitmentVersionId,
                            AccountId = p.AccountId,
                            AccountVersionId = p.AccountVersionId,
                            Uln = p.Uln,
                            LearnRefNumber = p.LearnerRefNumber,
                            AimSeqNumber = p.AimSequenceNumber,
                            Ukprn = p.Ukprn,
                            DeliveryMonth = p.DeliveryMonth,
                            DeliveryYear = p.DeliveryYear,
                            TransactionType = (int) p.TransactionType,
                            AmountDue = p.AmountDue,
                            IlrSubmissionDateTime = p.IlrSubmissionDateTime,
                            StandardCode = p.StandardCode,
                            FrameworkCode = p.FrameworkCode,
                            ProgrammeType = p.ProgrammeType,
                            PathwayCode = p.PathwayCode,
                            ApprenticeshipContractType = p.ApprenticeshipContractType,
                            PriceEpisodeIdentifier = p.PriceEpisodeIdentifier,
                            SfaContributionPercentage = p.SfaContributionPercentage,
                            FundingLineType = p.FundingLineType
                        })
                    .ToArray();

                _requiredPaymentRepository.AddRequiredPayments(paymentEntities);

                return new AddRequiredPaymentsCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new AddRequiredPaymentsCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}