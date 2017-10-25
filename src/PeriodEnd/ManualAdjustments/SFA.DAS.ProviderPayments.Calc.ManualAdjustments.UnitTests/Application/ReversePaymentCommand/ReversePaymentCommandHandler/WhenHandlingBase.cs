using System;
using Moq;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.ReversePaymentCommand;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;
using NLog;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.UnitTests.Application.ReversePaymentCommand.ReversePaymentCommandHandler
{
    public abstract class WhenHandlingBase
    {
        protected readonly static Guid RequiredPaymentIdToReverse = Guid.Parse("5cebe7f2-dbee-42d3-a5ed-89158a3a9134");

        protected readonly RequiredPaymentEntity OriginalRequiredPayment = new RequiredPaymentEntity
        {
            Id = RequiredPaymentIdToReverse,
            CommitmentId = 1,
            CommitmentVersionId = "99-001",
            AccountId = "2",
            AccountVersionId = "234",
            Uln = 9876543210,
            LearnRefNumber = "LRN-001",
            AimSeqNumber = 3,
            Ukprn = 12563987,
            IlrSubmissionDateTime = new System.DateTime(2016, 8, 3),
            PriceEpisodeIdentifier = "25-36-0-01/08/2016",
            StandardCode = 36,
            ApprenticeshipContractType = 1,
            DeliveryMonth = 8,
            DeliveryYear = 2016,
            CollectionPeriodName = "1617-R01",
            CollectionPeriodMonth = 8,
            CollectionPeriodYear = 2016,
            TransactionType = 1,
            AmountDue = 123.4567m,
            SfaContributionPercentage = 0.9m,
            FundingLineType = "abc",
            UseLevyBalance = true
        };
        protected readonly PaymentEntity OriginalPayment1 = new PaymentEntity
        {
            PaymentId = "6df36cdb-3ad8-4218-a195-bb59b90b911e",
            RequiredPaymentId = RequiredPaymentIdToReverse,
            DeliveryMonth = 8,
            DeliveryYear = 2016,
            CollectionPeriodName = "1617-R01",
            CollectionPeriodMonth = 8,
            CollectionPeriodYear = 2016,
            FundingSource = 1,
            TransactionType = 1,
            Amount = 73.4567m
        };
        protected readonly PaymentEntity OriginalPayment2 = new PaymentEntity
        {
            PaymentId = "4bcc480b-b506-4802-aee7-4415541d5f4b",
            RequiredPaymentId = RequiredPaymentIdToReverse,
            DeliveryMonth = 8,
            DeliveryYear = 2016,
            CollectionPeriodName = "1617-R01",
            CollectionPeriodMonth = 8,
            CollectionPeriodYear = 2016,
            FundingSource = 2,
            TransactionType = 1,
            Amount = 45m
        };
        protected readonly PaymentEntity OriginalPayment3 = new PaymentEntity
        {
            PaymentId = "840bf906-9d15-491f-be9a-81f87f82906e",
            RequiredPaymentId = RequiredPaymentIdToReverse,
            DeliveryMonth = 8,
            DeliveryYear = 2016,
            CollectionPeriodName = "1617-R01",
            CollectionPeriodMonth = 8,
            CollectionPeriodYear = 2016,
            FundingSource = 3,
            TransactionType = 1,
            Amount = 5m
        };

        protected Mock<IRequiredPaymentRepository> RequiredPaymentRepository;
        protected Mock<IPaymentRepository> PaymentRepository;
        protected Mock<IAccountRepository> AccountRepository;
        protected Mock<ICollectionPeriodRepository> CollectionPeriodRepository;
        protected string YearOfCollection = "1617";
        protected CollectionPeriodEntity OpenCollectionPeriod;
        protected ManualAdjustments.Application.ReversePaymentCommand.ReversePaymentCommandHandler Handler;
        protected ReversePaymentCommandRequest Request;
        protected Mock<ILogger> _logger;

        public virtual void Arrange()
        {
            RequiredPaymentRepository = new Mock<IRequiredPaymentRepository>();
            RequiredPaymentRepository.Setup(r => r.GetRequiredPayment(RequiredPaymentIdToReverse.ToString()))
                .Returns(OriginalRequiredPayment);

            PaymentRepository = new Mock<IPaymentRepository>();
            PaymentRepository.Setup(r => r.GetPaymentsForRequiredPayment(RequiredPaymentIdToReverse.ToString()))
                .Returns(new[] { OriginalPayment1, OriginalPayment2, OriginalPayment3 });

            AccountRepository = new Mock<IAccountRepository>();
            _logger = new Mock<ILogger>();

            OpenCollectionPeriod = new CollectionPeriodEntity
            {
                Name = "R02",
                CalendarMonth = 9,
                CalendarYear = 2016
            };
            CollectionPeriodRepository = new Mock<ICollectionPeriodRepository>();
            CollectionPeriodRepository.Setup(r => r.GetOpenCollectionPeriod())
                .Returns(OpenCollectionPeriod);

            Handler = new ManualAdjustments.Application.ReversePaymentCommand.ReversePaymentCommandHandler(RequiredPaymentRepository.Object,
                PaymentRepository.Object, AccountRepository.Object, CollectionPeriodRepository.Object,_logger.Object);

            Request = new ReversePaymentCommandRequest
            {
                RequiredPaymentIdToReverse = RequiredPaymentIdToReverse.ToString(),
                YearOfCollection = YearOfCollection
            };
        }
    }
}
