using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    [TestFixture]
    public class WhenApprenticeshipContractTypeChanges
    {
        private PaymentsDue.PaymentsDueProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IExternalContext> _externalContext;
        private readonly DateTime _learningStartDate = new DateTime(2017, 12, 1);
        private const decimal AmountDue = 1000M;
        private const long StandardCode = 1L;
        private const int AimSeqNumber = 1;
        private const string LearnerRefNumber = "LEARNREF";
        private const long Ukprn = 1L;
        private const long CommitmentId = 1L;

        private RequiredPayment[] _requiredPayments;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();

            _mediator = new Mock<IMediator>();

            _externalContext = new Mock<IExternalContext>();
            _externalContext.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, "" },
                    { ContextPropertyKeys.LogLevel, "DEBUG" },
                    { PaymentsContextPropertyKeys.YearOfCollection, "1718" }
                });

            _processor = new PaymentsDue.PaymentsDueProcessor(_logger.Object, _mediator.Object, new ContextWrapper(_externalContext.Object));

            InitialMockSetup();
        }

        private void InitialMockSetup()
        {
            SetCurrentCollectionPeriod();

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[] { new Provider { Ukprn = 10007459 } }
                });


            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryWhereNoEarningQueryRequest>()))
                .Returns(new GetPaymentHistoryWhereNoEarningQueryResponse
                {
                    IsValid = true,
                    Items = new RequiredPayment[0]
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()))
                .Returns(new AddRequiredPaymentsCommandResponse
                {
                    IsValid = true
                })
                .Callback<AddRequiredPaymentsCommandRequest>(a => _requiredPayments = a.Payments)
                .Verifiable();
        }

        private void SetCurrentCollectionPeriod(int month = 9)
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod {PeriodId = 1, Month = month, Year = 2017}
                });
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void ThenApprenticeIsPaidWhenContractTypeDoesNotChange(int apprenticeshipContractType)
        {
            SetupPaymentHistory(apprenticeshipContractType);

            SetupProviderEarnings(apprenticeshipContractType);

            _processor.Process();

            _mediator
                .Verify(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Once);

            Assert.AreEqual(1, _requiredPayments.Length);
        }

        [Test]
        public void ThenApprenticeIsPaidCorrectAmountWhenChangingFromNonDasToDas()
        {
            SetupPaymentHistory(2);

            SetupProviderEarnings(1);

            _processor.Process();

            _mediator
                .Verify(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Once);

            Assert.AreEqual(1, _requiredPayments.Length);
        }

        [Test]
        public void ThenApprenticeIsPaidCorrectAmountWhenChangingFromDasToNonDas()
        {
            SetupPaymentHistory(1);

            SetupProviderEarnings(2);

            _processor.Process();

            _mediator
                .Verify(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Once);

            Assert.AreEqual(1, _requiredPayments.Length);
        }

        [Test]
        public void ThenPreviousPaymentsAreRefundedWhenApprenticeshipContractTypeChangesFromDasToNonDas()
        {
            SetupPaymentHistory(1);

            SetupProviderEarnings(2, new DateTime(2017,8,1), 8);

            _processor.Process();

            _mediator
                .Verify(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Once);

            Assert.AreEqual(2, _requiredPayments.Length);
        }

        [Test]
        public void ThenPreviousPaymentsThatHaveBeenRefundedAreNotPaidAgain()
        {
            SetupPaymentHistory(1, true);

            SetupProviderEarnings(2, new DateTime(2017, 8, 1), 8);

            _processor.Process();

            _mediator
                .Verify(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Once);

            Assert.AreEqual(1, _requiredPayments.Length);
        }

        [Test]
        public void ThenInFuturePeriodsWherePreviousPeriodsHaveBeenRefundedOnlyTheLatestIsTakenIntoConsideration()
        {
            SetCurrentCollectionPeriod(10);

            var payments = new List<RequiredPayment>
            {
                new RequiredPayment
                {
                    CommitmentId = CommitmentId,
                    Ukprn = Ukprn,
                    LearnerRefNumber = LearnerRefNumber,
                    AimSequenceNumber = AimSeqNumber,
                    StandardCode = StandardCode,
                    FrameworkCode = null,
                    PathwayCode = null,
                    ProgrammeType = null,
                    TransactionType = TransactionType.Learning,
                    LearningStartDate = _learningStartDate,
                    AmountDue = AmountDue,
                    ApprenticeshipContractType = 1,
                    DeliveryMonth = 8,
                    DeliveryYear = 2017,
                    CollectionPeriodMonth = 8,
                    CollectionPeriodYear = 2017
                },
                new RequiredPayment
                {
                    CommitmentId = CommitmentId,
                    Ukprn = Ukprn,
                    LearnerRefNumber = LearnerRefNumber,
                    AimSequenceNumber = AimSeqNumber,
                    StandardCode = StandardCode,
                    FrameworkCode = null,
                    PathwayCode = null,
                    ProgrammeType = null,
                    TransactionType = TransactionType.Learning,
                    LearningStartDate = _learningStartDate,
                    AmountDue = -AmountDue,
                    ApprenticeshipContractType = 1,
                    DeliveryMonth = 8,
                    DeliveryYear = 2017,
                    CollectionPeriodMonth = 8,
                    CollectionPeriodYear = 2017
                },

                new RequiredPayment
                {
                    CommitmentId = CommitmentId,
                    Ukprn = Ukprn,
                    LearnerRefNumber = LearnerRefNumber,
                    AimSequenceNumber = AimSeqNumber,
                    StandardCode = StandardCode,
                    FrameworkCode = null,
                    PathwayCode = null,
                    ProgrammeType = null,
                    TransactionType = TransactionType.Learning,
                    LearningStartDate = _learningStartDate,
                    AmountDue = AmountDue,
                    ApprenticeshipContractType = 2,
                    DeliveryMonth = 8,
                    DeliveryYear = 2017,
                    CollectionPeriodMonth = 10,
                    CollectionPeriodYear = 2017
                },

                new RequiredPayment
                {
                    CommitmentId = CommitmentId,
                    Ukprn = Ukprn,
                    LearnerRefNumber = LearnerRefNumber,
                    AimSequenceNumber = AimSeqNumber,
                    StandardCode = StandardCode,
                    FrameworkCode = null,
                    PathwayCode = null,
                    ProgrammeType = null,
                    TransactionType = TransactionType.Learning,
                    LearningStartDate = _learningStartDate,
                    AmountDue = AmountDue,
                    ApprenticeshipContractType = 1,
                    DeliveryMonth = 9,
                    DeliveryYear = 2017,
                    CollectionPeriodMonth = 9,
                    CollectionPeriodYear = 2017
                },
                new RequiredPayment
                {
                    CommitmentId = CommitmentId,
                    Ukprn = Ukprn,
                    LearnerRefNumber = LearnerRefNumber,
                    AimSequenceNumber = AimSeqNumber,
                    StandardCode = StandardCode,
                    FrameworkCode = null,
                    PathwayCode = null,
                    ProgrammeType = null,
                    TransactionType = TransactionType.Learning,
                    LearningStartDate = _learningStartDate,
                    AmountDue = -AmountDue,
                    ApprenticeshipContractType = 1,
                    DeliveryMonth = 9,
                    DeliveryYear = 2017,
                    CollectionPeriodMonth = 9,
                    CollectionPeriodYear = 2017
                },

                new RequiredPayment
                {
                    CommitmentId = CommitmentId,
                    Ukprn = Ukprn,
                    LearnerRefNumber = LearnerRefNumber,
                    AimSequenceNumber = AimSeqNumber,
                    StandardCode = StandardCode,
                    FrameworkCode = null,
                    PathwayCode = null,
                    ProgrammeType = null,
                    TransactionType = TransactionType.Learning,
                    LearningStartDate = _learningStartDate,
                    AmountDue = AmountDue,
                    ApprenticeshipContractType = 2,
                    DeliveryMonth = 9,
                    DeliveryYear = 2017,
                    CollectionPeriodMonth = 10,
                    CollectionPeriodYear = 2017
                },

                new RequiredPayment
                {
                    CommitmentId = CommitmentId,
                    Ukprn = Ukprn,
                    LearnerRefNumber = LearnerRefNumber,
                    AimSequenceNumber = AimSeqNumber,
                    StandardCode = StandardCode,
                    FrameworkCode = null,
                    PathwayCode = null,
                    ProgrammeType = null,
                    TransactionType = TransactionType.Learning,
                    LearningStartDate = _learningStartDate,
                    AmountDue = AmountDue,
                    ApprenticeshipContractType = 1,
                    DeliveryMonth = 10,
                    DeliveryYear = 2017,
                    CollectionPeriodMonth = 10,
                    CollectionPeriodYear = 2017
                }
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = payments.ToArray()
                });

            SetupProviderEarnings(2, new DateTime(2018,8,1), 10);

            _processor.Process();

            _mediator
                .Verify(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Once);

            Assert.AreEqual(1, _requiredPayments.Length);
        }

        private void SetupProviderEarnings(int apprenticeshipContractType, DateTime? startDate = null, int calendarMonth = 9)
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new PeriodEarning
                        {
                            CommitmentId = CommitmentId,
                            Ukprn = Ukprn,
                            LearnerReferenceNumber = LearnerRefNumber,
                            AimSequenceNumber = AimSeqNumber,
                            LearningStartDate = _learningStartDate,
                            CollectionPeriodNumber = 2,
                            CollectionAcademicYear = "1718",
                            CalendarMonth = calendarMonth,
                            CalendarYear = 2017,
                            EarnedValue = AmountDue,
                            Type = TransactionType.Learning,
                            StandardCode = StandardCode,
                            FrameworkCode = null,
                            PathwayCode = null,
                            ProgrammeType = null,
                            IsSuccess = true,
                            Payable = true,
                            ApprenticeshipContractType = apprenticeshipContractType,
                            ApprenticeshipContractTypeStartDate = startDate
                        }
                    }
                });
        }

        private void SetupPaymentHistory(int apprenticeshipContractType, bool addRefund = false)
        {
            var payments = new List<RequiredPayment>
            {
                new RequiredPayment
                {
                    CommitmentId = CommitmentId,
                    Ukprn = Ukprn,
                    LearnerRefNumber = LearnerRefNumber,
                    AimSequenceNumber = AimSeqNumber,
                    StandardCode = StandardCode,
                    FrameworkCode = null,
                    PathwayCode = null,
                    ProgrammeType = null,
                    TransactionType = TransactionType.Learning,
                    LearningStartDate = _learningStartDate,
                    AmountDue = AmountDue,
                    ApprenticeshipContractType = apprenticeshipContractType,
                    DeliveryMonth = 8,
                    DeliveryYear = 2017
                }
            };

            if (addRefund)
            {
                payments.Add(
                    new RequiredPayment
                    {
                        CommitmentId = CommitmentId,
                        Ukprn = Ukprn,
                        LearnerRefNumber = LearnerRefNumber,
                        AimSequenceNumber = AimSeqNumber,
                        StandardCode = StandardCode,
                        FrameworkCode = null,
                        PathwayCode = null,
                        ProgrammeType = null,
                        TransactionType = TransactionType.Learning,
                        LearningStartDate = _learningStartDate,
                        AmountDue = -AmountDue,
                        ApprenticeshipContractType = apprenticeshipContractType,
                        DeliveryMonth = 8,
                        DeliveryYear = 2017
                    });
            }

            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = payments.ToArray()
                });
        }
    }
}