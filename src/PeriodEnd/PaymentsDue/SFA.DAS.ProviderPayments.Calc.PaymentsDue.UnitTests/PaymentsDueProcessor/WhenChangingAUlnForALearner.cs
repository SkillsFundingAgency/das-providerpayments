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
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    [TestFixture]
    public class WhenChangingAUlnForALearner
    {
        private ILogger _logger;

        private Mock<IMediator> _mediator;

        private Mock<IExternalContext> _externalContext;

        private PaymentsDue.PaymentsDueProcessor _processor;

        [SetUp]
        public void Setup()
        {
            _logger = Mock.Of<ILogger>();
            _mediator = new Mock<IMediator>();
            _externalContext = new Mock<IExternalContext>();
            _externalContext.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, "" },
                    { ContextPropertyKeys.LogLevel, "DEBUG" },
                    { PaymentsContextPropertyKeys.YearOfCollection, "1718" }
                });

            _processor = new PaymentsDue.PaymentsDueProcessor(_logger, _mediator.Object, new ContextWrapper(_externalContext.Object));
        }

        [Test]
        public void ThenPaymentsAreStillMadeCorrectly()
        {
            const int ukprn = 1000;

            const string learnerReferenceNumber = "123456";
            const long tempUln = 9999999L;
            const long newUln = 12345L;
            const int month = 8;
            const int year = 2017;
            const decimal earnedValue = 123M;

            // arrange
            _mediator.Setup(x => x.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse {IsValid = true, Period = new CollectionPeriod {Month = 8, PeriodId = 1, PeriodNumber = 1, Year = 2017}});

            _mediator.Setup(x => x.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse{IsValid = true,
                    Items = new []
                    {
                        new Provider{Ukprn = ukprn, IlrSubmissionDateTime = DateTime.Now}
                    }});

            _mediator.Setup(x => x.Send(It.IsAny<GetPaymentHistoryWhereNoEarningQueryRequest>()))
                .Returns(new GetPaymentHistoryWhereNoEarningQueryResponse {IsValid = true, Items = new RequiredPayment[0]});

            _mediator.Setup(x => x.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new []
                        {new PeriodEarning {Ukprn = ukprn, LearnerReferenceNumber = learnerReferenceNumber, Uln = tempUln, CalendarMonth = month, CalendarYear = year, EarnedValue = earnedValue}}
                });

            _mediator.Setup(x => x.Send(It.IsAny<GetPaymentHistoryQueryRequest>())).Returns(new GetPaymentHistoryQueryResponse
            {
                IsValid = true,
                Items = new[] {new RequiredPayment {Uln = tempUln, Ukprn = ukprn, LearnerRefNumber = learnerReferenceNumber, AmountDue = earnedValue, DeliveryMonth = month, DeliveryYear = year}}
            });

            _mediator.Setup(x => x.Send(It.IsAny<AddRequiredPaymentsCommandRequest>())).Verifiable();

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(x => x.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Never);
        }
    }
}
