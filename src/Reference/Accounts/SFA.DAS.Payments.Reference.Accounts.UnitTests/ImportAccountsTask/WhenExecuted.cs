using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.Payments.Reference.Accounts.Context;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.ImportAccountsTask
{
    public class WhenExecuted
    {
        private Mock<IApiProcessor> _processor;
        private Mock<IDependencyResolver> _dependencyResolver;
        private Accounts.ImportAccountsTask _task;
        private Mock<IExternalContext> _context;
        private string _contextValue;

        [SetUp]
        public void Arrange()
        {
            _contextValue = Guid.NewGuid().ToString();
            _context = new Mock<IExternalContext>();
            _context.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { "TestKey", _contextValue },
                    { ContextPropertyKeys.TransientDatabaseConnectionString, "TransientDatabaseConnectionString" },
                    { ContextPropertyKeys.LogLevel, "DEBUG" },
                    { KnownContextKeys.AccountsApiBaseUrl, "http://test" },
                    { KnownContextKeys.AccountsApiClientId, "the-client" },
                    { KnownContextKeys.AccountsApiClientSecret, "super-secret" },
                    { KnownContextKeys.AccountsApiIdentifierUri, "http://unit.tests" },
                    { KnownContextKeys.AccountsApiTenant, "http://ad.test" }
                });

            _processor = new Mock<IApiProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(r => r.GetInstance<IApiProcessor>())
                .Returns(_processor.Object);

            _task = new Accounts.ImportAccountsTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenItShouldInitDependencyResolver()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(r => r.Init(typeof(Accounts.ApiProcessor), It.Is<ContextWrapper>(cw => cw.Context == _context.Object)), Times.Once);
        }

        [Test]
        public void ThenItShouldRunProcessor()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}
