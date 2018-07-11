using System.Collections.Generic;
using CS.Common.External.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.Refunds.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.IoC
{
    [TestFixture]
    public class GivenARefundsRegistry
    {
        private Mock<IExternalContext> _externalContext;
        private ContextWrapper _contextWrapper;
        private TaskDependencyResolver _resolver;

        [SetUp]
        public void Setup()
        {
            _externalContext = new Mock<IExternalContext>();
            _externalContext.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, "foo bar" }
                });

            _contextWrapper = new ContextWrapper(_externalContext.Object);

            _resolver = new TaskDependencyResolver();
            _resolver.Init(typeof(RefundsProcessor), _contextWrapper);
        }

        [Test]
        public void ThenResolvesIProviderProcessor() =>
            _resolver.GetInstance<IProviderProcessor>()
                .Should().BeOfType<ProviderRefundsProcessor>();
    }
}