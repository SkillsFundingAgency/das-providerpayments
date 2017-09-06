using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;
using NLog;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.ReversePaymentCommand
{
    public class ReversePaymentCommandHandler : IRequestHandler<ReversePaymentCommandRequest, ReversePaymentCommandResponse>
    {
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;
        private readonly ILogger _logger;


        public ReversePaymentCommandHandler(IRequiredPaymentRepository requiredPaymentRepository,
                                            IPaymentRepository paymentRepository,
                                            IAccountRepository accountRepository,
                                            ICollectionPeriodRepository collectionPeriodRepository,
                                            ILogger logger)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
            _collectionPeriodRepository = collectionPeriodRepository;
            _logger = logger;
        }

        public ReversePaymentCommandResponse Handle(ReversePaymentCommandRequest message)
        {
            var requiredPaymentToReverse = _requiredPaymentRepository.GetRequiredPayment(message.RequiredPaymentIdToReverse);
            var paymentsToReverse = _paymentRepository.GetPaymentsForRequiredPayment(message.RequiredPaymentIdToReverse);
            var openCollectionPeriod = _collectionPeriodRepository.GetOpenCollectionPeriod();

            if (requiredPaymentToReverse != null)
            {
                _logger.Info($"Found requiredPaymentToReverse : {message.RequiredPaymentIdToReverse}");
            }
            else
            {
                _logger.Info($"Invalid requiredPaymentToReverse supplied, reversal will not happen for : {message.RequiredPaymentIdToReverse}");
            }

            var requiredPaymentForReversal = ReverseRequiredPayment(requiredPaymentToReverse, openCollectionPeriod, message.YearOfCollection);

            _logger.Info($"Reversed original requiredPaymentToReverse {message.RequiredPaymentIdToReverse}, new Requiredpayment id : {requiredPaymentForReversal.Id}");

            _logger.Info($"Found total {paymentsToReverse.Count()} for requiredPaymentToReverse : {message.RequiredPaymentIdToReverse}");

            foreach (var payment in paymentsToReverse)
            {
                ReversePayment(payment, requiredPaymentForReversal, openCollectionPeriod, message.YearOfCollection);
            }
            if (!string.IsNullOrEmpty(requiredPaymentToReverse.AccountId))
            {
                AdjustLevyAccountBalance(requiredPaymentToReverse.AccountId, paymentsToReverse);
            }

            return new ReversePaymentCommandResponse
            {
                IsValid = true,
                RequiredPaymentIdForReversal = requiredPaymentForReversal.Id.ToString()
            };
        }

        private RequiredPaymentEntity ReverseRequiredPayment(RequiredPaymentEntity requiredPaymentToReverse, CollectionPeriodEntity openCollectionPeriod, string yearOfCollection)
        {
            var requiredPaymentForReversal = new RequiredPaymentEntity
            {
                Id = Guid.NewGuid(),
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
                LearnAimRef = requiredPaymentToReverse.LearnAimRef,
                LearningStartDate = requiredPaymentToReverse.LearningStartDate,

                AmountDue = -requiredPaymentToReverse.AmountDue,

                CollectionPeriodName = $"{yearOfCollection}-{openCollectionPeriod.Name}",
                CollectionPeriodMonth = openCollectionPeriod.CalendarMonth,
                CollectionPeriodYear = openCollectionPeriod.CalendarYear
            };
            _requiredPaymentRepository.CreateRequiredPayment(requiredPaymentForReversal);

            return requiredPaymentForReversal;
        }
        private void ReversePayment(PaymentEntity paymentToReverse, RequiredPaymentEntity requiredPaymentForReversal, CollectionPeriodEntity openCollectionPeriod, string yearOfCollection)
        {
            var paymentId = Guid.NewGuid().ToString();
            _paymentRepository.CreatePayment(new PaymentEntity
            {
                PaymentId = paymentId,
                RequiredPaymentId = requiredPaymentForReversal.Id,
                DeliveryMonth = paymentToReverse.DeliveryMonth,
                DeliveryYear = paymentToReverse.DeliveryYear,
                CollectionPeriodName = $"{yearOfCollection}-{openCollectionPeriod.Name}",
                CollectionPeriodMonth = openCollectionPeriod.CalendarMonth,
                CollectionPeriodYear = openCollectionPeriod.CalendarYear,
                FundingSource = paymentToReverse.FundingSource,
                TransactionType = paymentToReverse.TransactionType,
                Amount = -paymentToReverse.Amount,
                CommitmentId = paymentToReverse.CommitmentId
            },requiredPaymentForReversal);

            _logger.Info($"For {requiredPaymentForReversal.Id} created new payment with Id : {paymentId} for old payment id {paymentToReverse.PaymentId}");

        }
        private void AdjustLevyAccountBalance(string accountId, PaymentEntity[] paymentsToReverse)
        {
            var amountToAdjustBy = paymentsToReverse.Where(p => p.FundingSource == 1).Sum(p => p.Amount);
            _accountRepository.AdjustAccountBalance(accountId, amountToAdjustBy);
        }

    }
}