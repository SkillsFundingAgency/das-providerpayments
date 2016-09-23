using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentCommand;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue.GetPaymentsDueForUkprnQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Providers;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.Common.Application;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.CoInvestedPaymentsProcessor
{
    // ACCEPTANCE TEST FOR DPP-195 SCENARIO 2
    public class WhenDpp195AcceptanceScenario2
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new Provider
                        {
                            Ukprn = 1
                        }
                    }
                });


            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentsDueForUkprnQueryRequest>())).Returns(
                new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 10,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 11,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 12,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 1,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 2,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 3,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 4,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 5,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 6,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 7,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 8,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 9,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 10,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 3923.07692m
                        }
                    }
                });
        }

        [Test]
        public void The26PaymentsAreMade()
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.Ukprn == 1
                        )),
                Times.Exactly(26));
        }
        [Test]
        public void ThenResultShouldBe_()
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.FundingSource == FundingSource.CoInvestedSfa && it.Payment.Amount == 830.76923m
                        )),
                Times.Exactly(12));

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.FundingSource == FundingSource.CoInvestedEmployer && it.Payment.Amount == 92.30769m
                        )),
                Times.Exactly(12));

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.FundingSource == FundingSource.CoInvestedSfa && it.Payment.Amount == 3530.76923m
                        )),
                Times.Exactly(1));

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.FundingSource == FundingSource.CoInvestedEmployer && it.Payment.Amount == 392.30769m
                        )),
                Times.Exactly(1));
        }

        private void Act()
        {
            _processor.Process();
        }
    }
    // ACCEPTANCE TEST FOR DPP-195 SCENARIO 1
    public class WhenDpp195AcceptanceScenario1
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();
            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new Provider
                        {
                            Ukprn = 1
                        }
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentsDueForUkprnQueryRequest>())).Returns(
                new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 10,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 11,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 12,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 1,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 2,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 3,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 4,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 5,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 6,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 7,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 8,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 9,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            CommitmentId = "1",
                            Ukprn = 1,
                            DeliveryMonth = 10,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 3000
                        }
                    }
                });
        }

        [Test]
        public void The26PaymentsAreMade()
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.Ukprn == 1
                        )),
                Times.Exactly(26));
        }
        [Test]
        public void ThenResultShouldBe_()
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.FundingSource == FundingSource.CoInvestedSfa && it.Payment.Amount == 900
                        )),
                Times.Exactly(12));

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.FundingSource == FundingSource.CoInvestedEmployer && it.Payment.Amount == 100
                        )),
                Times.Exactly(12));

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.FundingSource == FundingSource.CoInvestedSfa && it.Payment.Amount == 2700
                        )),
                Times.Exactly(1));

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.FundingSource == FundingSource.CoInvestedEmployer && it.Payment.Amount == 300
                        )),
                Times.Exactly(1));
        }
        private void Act()
        {
            _processor.Process();
        }
    }
    public class WhenProcessCalledWithAProviderAnd1PaymentDueReturnedWithDifferentTransactionType
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private PaymentDue _paymentDue;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();
            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new Provider()
                    }
                });
        }

        [TestCase(TransactionType.Completion)]
        [TestCase(TransactionType.Learning)]
        public void ShouldCallProcessPaymentCommandWithCoInvestedPassingTheTransactionTypeTwice(TransactionType transactionType)
        {
            _paymentDue = new PaymentDue
            {
                CommitmentId = "1",
                AimSequenceNumber = 2,
                DeliveryMonth = 3,
                DeliveryYear = 4,
                LearnerRefNumber = "5",
                Ukprn = 6,
                TransactionType = transactionType,
                AmountDue = 10000
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentsDueForUkprnQueryRequest>())).Returns(
                new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        _paymentDue
                    }
                });

            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.TransactionType == _paymentDue.TransactionType
                        )),
                Times.Exactly(2));
        }

        private void Act()
        {
            _processor.Process();
        }
    }
    public class WhenProcessCalledWithAProviderAnd1PaymentDueReturned
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private PaymentDue _paymentDue;
        private CollectionPeriod _period;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _period = new CollectionPeriod();

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new Provider()
                    }
                });

            _paymentDue = new PaymentDue
            {
                CommitmentId = "1",
                AimSequenceNumber = 2,
                DeliveryMonth = 3,
                DeliveryYear = 4,
                LearnerRefNumber = "5",
                Ukprn = 6,
                TransactionType = TransactionType.Learning,
                AmountDue = 10000
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentsDueForUkprnQueryRequest>())).Returns(
                new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        _paymentDue
                    }
                });
        }

        [Test]
        public void ShouldCallProcessPaymentCommandTwice()
        {
            Act();

            _mediator.Verify(m => m.Send(It.IsAny<ProcessPaymentCommandRequest>()), Times.Exactly(2));
        }
        [TestCase(FundingSource.CoInvestedSfa, 9000)]
        [TestCase(FundingSource.CoInvestedEmployer, 1000)]
        public void ShouldCallProcessPaymentCommandWithCoInvestedFundingSourceOf_AndValueOf_(FundingSource fundingSource, decimal paymentAmount)
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(it => it.Payment.FundingSource == fundingSource && it.Payment.Amount == paymentAmount)),
                Times.Once);
        }
        [Test]
        public void ShouldCallProcessPaymentCommandWithCoInvestedGeneralPropertiesTwice()
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentCommandRequest>(
                        it => it.Payment.AimSequenceNumber == _paymentDue.AimSequenceNumber &&
                        it.Payment.CollectionPeriodMonth == _period.Month &&
                        it.Payment.CollectionPeriodYear == _period.Year &&
                        it.Payment.DeliveryMonth == _paymentDue.DeliveryMonth &&
                        it.Payment.DeliveryYear == _paymentDue.DeliveryYear &&
                        it.Payment.Ukprn == _paymentDue.Ukprn &&
                        it.Payment.CommitmentId == _paymentDue.CommitmentId &&
                        it.Payment.LearnerRefNumber == _paymentDue.LearnerRefNumber &&
                        it.Payment.TransactionType == _paymentDue.TransactionType
                        )),
                Times.Exactly(2));
        }

        [Test]
        public void ShouldNotThrowException()
        {
            Act();
        }

        [Test]
        public void ThenOutputsLogMessages()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
                // ignored
            }

            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Processing co-invested payments for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("learner co-invested payment entries for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished")), Times.Once);
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(6));

            _logger.Verify(l => l.Info(It.IsRegex("No payments due for found for provider with ukprn")), Times.Never);
        }

        private void Act()
        {
            _processor.Process();
        }
    }
    public class WhenProcessCalledWithAProviderAnd0PaymentsDue
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new Provider()
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentsDueForUkprnQueryRequest>())).Returns(
                new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = true
                });
        }

        [Test]
        public void ShouldNotThrowException()
        {
            Act();
        }

        [Test]
        public void ThenOutputsLogMessages()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
                // ignored
            }

            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Processing co-invested payments for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("No payments due for found for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished")), Times.Once);
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(4));
        }

        private void Act()
        {
            _processor.Process();
        }
    }
    public class WhenProcessCalledWithAProviderReturnedButProviderPaymentsDueForUkprnQueryIsInvalid
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new []
                    {
                        new Provider()
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentsDueForUkprnQueryRequest>())).Returns(
                new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = false
                });
            
        }

        [Test]
        public void ShouldThrowExceptionWithSpecificMessage()
        {
            var ex = Assert.Throws<CoInvestedPaymentsProcessorException>(Act);
            Assert.That(ex.Message, Is.EqualTo(CoInvestedPaymentsProcessorException.ErrorReadingPaymentsDueForUkprn));
        }

        [Test]
        public void ThenOutputsLogMessageThatStartedAndFinished()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        private void Act()
        {
            _processor.Process();
        }
    }

    public class WhenProcessCalledWithNoProvidersReturned
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = true
                });
        }

        [Test]
        public void ShouldNotThrowAnException()
        {
            Act();
        }

        [Test]
        public void ThenOutputsLogMessageThatStartedAndFinished()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(3));
            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished")), Times.Once);
        }

        private void Act()
        {
            _processor.Process();
        }
    }

    public class WhenProcessCalledWithValidProviderQuery
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = true
                });
        }

        [Test]
        public void ShouldNotThrowException()
        {
            Act();
        }


        private void Act()
        {
            _processor.Process();
        }

        [Test]
        public void ThenShouldMakeGetCurrentCollectionPeriodQueryRequest()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _mediator.Verify(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()), Times.Once);
        }

        [Test]
        public void ThenOutputsLogMessageThatStartedAndFinished()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(3));
            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished")), Times.Once);
        }
    }
    public class WhenProcessCalledWithInvalidProviderQuery
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod()
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>())).Returns(
                new GetProvidersQueryResponse
                {
                    IsValid = false
                });
        }

        [Test]
        public void ShouldThrowExceptionWithSpecificMessage()
        {
            var ex = Assert.Throws<CoInvestedPaymentsProcessorException>(Act);
            Assert.That(ex.Message, Is.EqualTo(CoInvestedPaymentsProcessorException.ErrorReadingProviders));
        }

        [Test]
        public void ThenOutputsLogMessageThatStartedAndFinished()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(1));
            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
        }

        private void Act()
        {
            _processor.Process();
        }
    }
    public class WhenProcessCalledWithValidCurrentCollectionPeriodButPeriodIsEmpty
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = null
                });
        }

        [Test]
        public void ShouldThrowException()
        {
            Assert.Throws<CoInvestedPaymentsProcessorException>(Act);
        }

        [Test]
        public void ShouldThrowExceptionWithSpecificMessage()
        {
            var ex = Assert.Throws<CoInvestedPaymentsProcessorException>(Act);
            Assert.That(ex.Message, Is.EqualTo(CoInvestedPaymentsProcessorException.ErrorNoCollectionPeriodMessage));
        }

        private void Act()
        {
            _processor.Process();
        }

        [Test]
        public void ThenShouldMakeGetCurrentCollectionPeriodQueryRequest()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _mediator.Verify(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()), Times.Once);
        }
        [Test]
        public void ThenOutputsLogMessageThatStartedOnly()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
        }
    }
    public class WhenProcessCalledWithInvalidCurrentCollectionPeriod
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>())).Returns(
                new GetCurrentCollectionPeriodQueryResponse
            {
                    IsValid = false
                
            });
        }

        [Test]
        public void ShouldThrowExceptionWithSpecificMessage()
        {
            var ex = Assert.Throws<CoInvestedPaymentsProcessorException>(Act);
            Assert.That(ex.Message, Is.EqualTo(CoInvestedPaymentsProcessorException.ErrorReadingCollectionPeriodMessage));
        }

        private void Act()
        {
            _processor.Process();
        }

        [Test]
        public void ThenShouldMakeGetCurrentCollectionPeriodQueryRequest()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _mediator.Verify(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()), Times.Once);
        }
        [Test]
        public void ThenOutputsLogMessageThatStartedOnly()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
        }
    }
}
