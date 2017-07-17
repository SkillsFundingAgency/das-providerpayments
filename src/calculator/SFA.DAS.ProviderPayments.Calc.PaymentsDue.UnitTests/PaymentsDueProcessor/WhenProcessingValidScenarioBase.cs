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
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    public abstract class WhenProcessingValidScenarioBase
    {
        protected PaymentsDue.PaymentsDueProcessor Processor;
        protected Mock<ILogger> Logger;
        protected Mock<IMediator> Mediator;
        protected PeriodEarning PeriodEarning1;
        protected PeriodEarning PeriodEarning2;
        protected PeriodEarning PeriodEarning3;
        protected Mock<IExternalContext> ExternalContext;

        [SetUp]
        public void Arrange()
        {
            Logger = new Mock<ILogger>();

            Mediator = new Mock<IMediator>();
            ArrangeCurrentCollectionPeriod();
            ArrangeProviders();
            ArrangeProviderEarnings();
            ArrangePaymentHistory();
            Mediator.Setup(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()))
                .Returns(new AddRequiredPaymentsCommandResponse { IsValid = true });
            Mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryWhereNoEarningQueryRequest>()))
                .Returns(new GetPaymentHistoryWhereNoEarningQueryResponse
                {
                    IsValid = true,
                    Items = new RequiredPayment[0]
                });

            ExternalContext = new Mock<IExternalContext>();
            ExternalContext.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, "" },
                    { ContextPropertyKeys.LogLevel, "DEBUG" },
                    { PaymentsContextPropertyKeys.YearOfCollection, "1718" }
                });

            Processor = new PaymentsDue.PaymentsDueProcessor(Logger.Object, Mediator.Object, new ContextWrapper(ExternalContext.Object));
        }

        protected void ArrangeCurrentCollectionPeriod()
        {
            Mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = 9, Year = 2017, PeriodNumber = 1 }
                });
        }
        protected void ArrangeProviders()
        {
            Mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[] { new Provider { Ukprn = 10007459 } }
                });
        }

        protected abstract void ArrangeProviderEarnings();

        protected void ArrangePaymentHistory()
        {
            Mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = new RequiredPayment[0]
                });
        }

        protected bool PaymentForEarning(RequiredPayment payment, PeriodEarning earning, decimal expectedAmountDue)
        {
            return payment.CommitmentId == earning.CommitmentId
                   && payment.CommitmentVersionId == earning.CommitmentVersionId
                   && payment.AccountId == earning.AccountId
                   && payment.AccountVersionId == earning.AccountVersionId
                   && payment.Ukprn == earning.Ukprn
                   && payment.Uln == earning.Uln
                   && payment.LearnerRefNumber == earning.LearnerReferenceNumber
                   && payment.AimSequenceNumber == earning.AimSequenceNumber
                   && payment.DeliveryMonth == earning.CalendarMonth
                   && payment.DeliveryYear == earning.CalendarYear
                   && payment.AmountDue == expectedAmountDue
                   && (int)payment.TransactionType == (int)earning.Type
                   && payment.StandardCode == earning.StandardCode
                   && payment.FrameworkCode == earning.FrameworkCode
                   && payment.ProgrammeType == earning.ProgrammeType
                   && payment.PathwayCode == earning.PathwayCode
                   && payment.ApprenticeshipContractType == earning.ApprenticeshipContractType
                   && payment.PriceEpisodeIdentifier == earning.PriceEpisodeIdentifier
                   && payment.SfaContributionPercentage == earning.SfaContributionPercentage
                   && payment.FundingLineType == earning.FundingLineType;
        }
    }
}