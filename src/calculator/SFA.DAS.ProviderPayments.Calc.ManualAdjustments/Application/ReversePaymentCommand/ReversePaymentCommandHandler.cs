using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.ReversePaymentCommand
{
    public class ReversePaymentCommandHandler : IRequestHandler<ReversePaymentCommandRequest, ReversePaymentCommandResponse>
    {
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;
        private readonly string _yearOfCollection;

        public ReversePaymentCommandHandler(IRequiredPaymentRepository requiredPaymentRepository,
                                            IPaymentRepository paymentRepository,
                                            IAccountRepository accountRepository,
                                            ICollectionPeriodRepository collectionPeriodRepository,
                                            string yearOfCollection)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
            _collectionPeriodRepository = collectionPeriodRepository;
            _yearOfCollection = yearOfCollection;
        }

        public ReversePaymentCommandResponse Handle(ReversePaymentCommandRequest message)
        {
            var requiredPaymentToReverse = _requiredPaymentRepository.GetRequiredPayment(message.RequiredPaymentIdToReverse);
            var paymentsToReverse = _paymentRepository.GetPaymentsForRequiredPayment(message.RequiredPaymentIdToReverse);
            var openCollectionPeriod = _collectionPeriodRepository.GetOpenCollectionPeriod();

            var requiredPaymentIdForReversal = ReverseRequiredPayment(requiredPaymentToReverse, openCollectionPeriod);
            foreach (var payment in paymentsToReverse)
            {
                ReversePayment(payment, requiredPaymentIdForReversal, openCollectionPeriod);
            }
            AdjustLevyAccountBalance(requiredPaymentToReverse.AccountId, paymentsToReverse);

            return new ReversePaymentCommandResponse
            {
                IsValid = true,
                RequiredPaymentIdForReversal = requiredPaymentIdForReversal
            };
        }

        private string ReverseRequiredPayment(RequiredPaymentEntity requiredPaymentToReverse, CollectionPeriodEntity openCollectionPeriod)
        {
            var requiredPaymentIdForReversal = Guid.NewGuid().ToString();
            _requiredPaymentRepository.CreateRequiredPayment(new Infrastructure.Entities.RequiredPaymentEntity
            {
                Id = requiredPaymentIdForReversal,
                CommitmentId = requiredPaymentToReverse.CommitmentId,
                CommitmentVersionId = requiredPaymentToReverse.CommitmentVersionId,
                AccountId = requiredPaymentToReverse.AccountId,
                AccountVersionId = requiredPaymentToReverse.AccountVersionId,
                Uln = requiredPaymentToReverse.Uln,
                LearnRefNumber = requiredPaymentToReverse.LearnRefNumber,
                AimSeqNumber = requiredPaymentToReverse.AimSeqNumber,
                Ukprn = requiredPaymentToReverse.Ukprn,
                IlrSubmissionDateTime = requiredPaymentToReverse.IlrSubmissionDateTime,
                PriceEpisodeIdentifier = requiredPaymentToReverse.PriceEpisodeIdentifier,
                StandardCode = requiredPaymentToReverse.StandardCode,
                ProgrammeType = requiredPaymentToReverse.ProgrammeType,
                FrameworkCode = requiredPaymentToReverse.FrameworkCode,
                PathwayCode = requiredPaymentToReverse.PathwayCode,
                ApprenticeshipContractType = requiredPaymentToReverse.ApprenticeshipContractType,
                DeliveryMonth = requiredPaymentToReverse.DeliveryMonth,
                DeliveryYear = requiredPaymentToReverse.DeliveryYear,
                TransactionType = requiredPaymentToReverse.TransactionType,
                SfaContributionPercentage = requiredPaymentToReverse.SfaContributionPercentage,
                FundingLineType = requiredPaymentToReverse.FundingLineType,
                UseLevyBalance = requiredPaymentToReverse.UseLevyBalance,

                AmountDue = -requiredPaymentToReverse.AmountDue,

                CollectionPeriodName = $"{_yearOfCollection}-{openCollectionPeriod.Name}",
                CollectionPeriodMonth = openCollectionPeriod.CalendarMonth,
                CollectionPeriodYear = openCollectionPeriod.CalendarYear
            });
            return requiredPaymentIdForReversal;
        }
        private void ReversePayment(PaymentEntity paymentToReverse, string requiredPaymentIdForReversal, CollectionPeriodEntity openCollectionPeriod)
        {
            _paymentRepository.CreatePayment(new PaymentEntity
            {
                PaymentId = Guid.NewGuid().ToString(),
                RequiredPaymentId = requiredPaymentIdForReversal,
                DeliveryMonth = paymentToReverse.DeliveryMonth,
                DeliveryYear = paymentToReverse.DeliveryYear,
                CollectionPeriodName = $"{_yearOfCollection}-{openCollectionPeriod.Name}",
                CollectionPeriodMonth = openCollectionPeriod.CalendarMonth,
                CollectionPeriodYear = openCollectionPeriod.CalendarYear,
                FundingSource = paymentToReverse.FundingSource,
                TransactionType = paymentToReverse.TransactionType,
                Amount = -paymentToReverse.Amount
            });
        }
        private void AdjustLevyAccountBalance(string accountId, PaymentEntity[] paymentsToReverse)
        {
            var amountToAdjustBy = paymentsToReverse.Where(p => p.FundingSource == 1).Sum(p => p.Amount);
            _accountRepository.AdjustAccountBalance(accountId, amountToAdjustBy);
        }

    }
}