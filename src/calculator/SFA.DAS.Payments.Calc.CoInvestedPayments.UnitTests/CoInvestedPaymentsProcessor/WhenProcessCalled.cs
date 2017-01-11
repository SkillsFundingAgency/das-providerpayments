using System;
using System.Linq;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentsCommand;
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
        private string _yearOfCollection = "1617";
        private Guid _requiredPaymentId = Guid.NewGuid();

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
                            Id = _requiredPaymentId,
                            DeliveryMonth = 10,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 11,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 12,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 1,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 2,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 3,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 4,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 5,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 6,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 7,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 8,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 9,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 923.07692m
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 10,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 3923.07692m
                        }
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentsCommandRequest>())).Returns(
                new ProcessPaymentsCommandResponse
                {
                    IsValid = true
                });
        }

        [Test]
        public void The26PaymentsAreMade()
        {
            Act();

            _mediator.Verify(
               m => m.Send(
                   It.Is<ProcessPaymentsCommandRequest>(
                       it => it.Payments.Count(p => p.RequiredPaymentId == _requiredPaymentId) == 26
                       )),
               Times.Once);
        }
        [Test]
        public void ThenResultShouldBe_()
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.FundingSource == FundingSource.CoInvestedSfa && p.Amount == 830.76923m) == 12
                        )),
                Times.Once);

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.FundingSource == FundingSource.CoInvestedEmployer && p.Amount == 92.30769m) == 12
                        )),
                Times.Once);

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.FundingSource == FundingSource.CoInvestedSfa && p.Amount == 3530.76923m) == 1
                        )),
                Times.Once);

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.FundingSource == FundingSource.CoInvestedEmployer && p.Amount == 392.30769m) == 1
                        )),
                Times.Once);
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
        private string _yearOfCollection = "1617";
        private Guid _requiredPaymentId = Guid.NewGuid();

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();
            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
                            Id = _requiredPaymentId,
                            DeliveryMonth = 10,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 11,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 12,
                            DeliveryYear = 17,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 1,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 2,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 3,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 4,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 5,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 6,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 7,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 8,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 9,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000
                        },
                        new PaymentDue
                        {
                            Id = _requiredPaymentId,
                            DeliveryMonth = 10,
                            DeliveryYear = 18,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 3000
                        }
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentsCommandRequest>())).Returns(
                new ProcessPaymentsCommandResponse
                {
                    IsValid = true
                });
        }

        [Test]
        public void The26PaymentsAreMade()
        {
            Act();

            _mediator.Verify(
               m => m.Send(
                   It.Is<ProcessPaymentsCommandRequest>(
                       it => it.Payments.Count(p => p.RequiredPaymentId == _requiredPaymentId) == 26
                       )),
               Times.Once);
        }
        [Test]
        public void ThenResultShouldBe_()
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.FundingSource == FundingSource.CoInvestedSfa && p.Amount == 900) == 12
                        )),
                Times.Once);

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.FundingSource == FundingSource.CoInvestedEmployer && p.Amount == 100) == 12
                        )),
                Times.Once);

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.FundingSource == FundingSource.CoInvestedSfa && p.Amount == 2700) == 1
                        )),
                Times.Once);

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.FundingSource == FundingSource.CoInvestedEmployer && p.Amount == 300) == 1
                        )),
                Times.Once);
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
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();
            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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

            _mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentsCommandRequest>())).Returns(
                new ProcessPaymentsCommandResponse
                {
                    IsValid = true
                });
        }

        [TestCase(TransactionType.Completion)]
        [TestCase(TransactionType.Learning)]
        public void ShouldCallProcessPaymentCommandWithCoInvestedPassingTheTransactionTypeTwice(TransactionType transactionType)
        {
            _paymentDue = new PaymentDue
            {
                Id = Guid.NewGuid(),
                DeliveryMonth = 3,
                DeliveryYear = 4,
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
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.TransactionType == _paymentDue.TransactionType && p.RequiredPaymentId == _paymentDue.Id) == 2
                        )),
                Times.Once);
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
        private string _yearOfCollection = "1617";
        private PaymentDue _paymentDue;
        private CollectionPeriod _period;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
                Id = Guid.NewGuid(),
                DeliveryMonth = 3,
                DeliveryYear = 4,
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

            _mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentsCommandRequest>())).Returns(
                new ProcessPaymentsCommandResponse
                {
                    IsValid = true
                });
        }

        [Test]
        public void ShouldCallProcessPaymentsCommandOnce()
        {
            Act();

            _mediator.Verify(m => m.Send(It.IsAny<ProcessPaymentsCommandRequest>()), Times.Once);
        }
        [TestCase(FundingSource.CoInvestedSfa, 9000)]
        [TestCase(FundingSource.CoInvestedEmployer, 1000)]
        public void ShouldCallProcessPaymentsCommandWithCoInvestedFundingSourceOf_AndValueOf_(FundingSource fundingSource, decimal paymentAmount)
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(it => it.Payments.Count(p => p.FundingSource == fundingSource && p.Amount == paymentAmount) == 1)),
                Times.Once);
        }
        [Test]
        public void ShouldCallProcessPaymentsCommandWithCoInvestedGeneralPropertiesOnTwoPayments()
        {
            Act();

            _mediator.Verify(
                m => m.Send(
                    It.Is<ProcessPaymentsCommandRequest>(
                        it => it.Payments.Count(p => p.RequiredPaymentId == _paymentDue.Id &&
                                                     p.CollectionPeriodMonth == _period.Month &&
                                                     p.CollectionPeriodYear == _period.Year &&
                                                     p.DeliveryMonth == _paymentDue.DeliveryMonth &&
                                                     p.DeliveryYear == _paymentDue.DeliveryYear &&
                                                     p.TransactionType == _paymentDue.TransactionType) == 2)),
                Times.Once);
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
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
    public class WhenProcessCalledWithInvalidProcessPaymentsCommand
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private string _yearOfCollection = "1617";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();
            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

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
                    IsValid = true,
                    Items = new[]
                    {
                        new PaymentDue
                        {
                            Id = Guid.NewGuid(),
                            DeliveryMonth = 3,
                            DeliveryYear = 4,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 10000
                        }
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentsCommandRequest>())).Returns(
                new ProcessPaymentsCommandResponse
                {
                    IsValid = false
                });
        }

        [Test]
        public void ShouldThrowExceptionWithSpecificMessage()
        {
            var ex = Assert.Throws<CoInvestedPaymentsProcessorException>(Act);
            Assert.That(ex.Message, Is.EqualTo(CoInvestedPaymentsProcessorException.ErrorWritingPaymentsForUkprn));
        }

        [Test]
        public void ThenShouldMakeProcessPaymentsCommandRequest()
        {
            try
            {
                Act();
            }
            catch (Exception)
            {
            }

            _mediator.Verify(m => m.Send(It.IsAny<ProcessPaymentsCommandRequest>()), Times.Once);
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

            _logger.Verify(l => l.Info(It.IsRegex("Started")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Processing co-invested payments for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("learner co-invested payment entries for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(5));

            _logger.Verify(l => l.Info(It.IsRegex("No payments due for found for provider with ukprn")), Times.Never);
        }

        private void Act()
        {
            _processor.Process();
        }
    }
}
