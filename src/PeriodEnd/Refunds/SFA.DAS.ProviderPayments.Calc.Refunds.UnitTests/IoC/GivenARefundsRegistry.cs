using System.Collections.Generic;
using CS.Common.External.Interfaces;
using FluentAssertions;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.Refunds.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.IoC
{
    [TestFixture]
    public class GivenARefundsRegistry
    {
        private ContextWrapper _contextWrapper;
        private TaskDependencyResolver _resolver;

        [SetUp]
        public void Setup()
        {
            var externalContext = new Mock<IExternalContext>();
            externalContext.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, "foo bar" }
                });

            _contextWrapper = new ContextWrapper(externalContext.Object);

            _resolver = new TaskDependencyResolver();
            _resolver.Init(typeof(RefundsProcessor), _contextWrapper);
        }

        [Test]
        public void ThenResolvesContextWrapper() =>
            _resolver.GetInstance<ContextWrapper>()
                .Should().BeSameAs(_contextWrapper);

        [Test]
        public void ThenResolvesILogger() =>
            _resolver.GetInstance<ILogger>()
                .Should().BeOfType<Logger>();//todo with .name "Refunds"

        // Processors

        [Test]
        public void ThenResolvesIProviderProcessor() =>
            _resolver.GetInstance<IProviderProcessor>()
                .Should().BeOfType<ProviderRefundsProcessor>();

        [Test]
        public void ThenResolvesIProcessLearnerRefunds() =>
            _resolver.GetInstance<IProcessLearnerRefunds>()
                .Should().BeOfType<LearnerRefundProcessor>();

        // Services

        [Test]
        public void ThenResolvesIDasAccountService() =>
            _resolver.GetInstance<IDasAccountService>()
                .Should().BeOfType<DasAccountService>();

        [Test]
        public void ThenResolvesILearnerBuilder() =>
            _resolver.GetInstance<ILearnerBuilder>()
                .Should().BeOfType<LearnerBuilder>();

        [Test]
        public void ThenResolvesISummariseAccountBalances() =>
            _resolver.GetInstance<ISummariseAccountBalances>()
                .Should().BeOfType<SummariseAccountBalances>();

        // Repositories

        [Test]
        public void ThenResolvesIProviderRepository() =>
            _resolver.GetInstance<IProviderRepository>()
                .Should().BeOfType<ProviderRepository>();

        [Test]
        public void ThenResolvesIPaymentRepository() =>
            _resolver.GetInstance<IPaymentRepository>()
                .Should().BeOfType<PaymentRepository>();

        [Test]
        public void ThenResolvesIHistoricalPaymentsRepository() =>
            _resolver.GetInstance<IHistoricalPaymentsRepository>()
                .Should().BeOfType<HistoricalPaymentsRepository>();

        [Test]
        public void ThenResolvesIRequiredPaymentRepository() =>
            _resolver.GetInstance<IRequiredPaymentRepository>()
                .Should().BeOfType<RequiredPaymentRepository>();

        [Test]
        public void ThenResolvesIDasAccountRepository() =>
            _resolver.GetInstance<IDasAccountRepository>()
                .Should().BeOfType<DasAccountRepository>();
    }
}