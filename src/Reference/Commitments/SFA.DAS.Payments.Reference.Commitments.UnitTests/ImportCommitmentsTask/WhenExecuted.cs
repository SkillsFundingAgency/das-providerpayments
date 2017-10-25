using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;

namespace SFA.DAS.Payments.Reference.Commitments.UnitTests.ImportCommitmentsTask
{
    public class WhenExecuted
    {
        private Mock<Commitments.ApiProcessor> _processor;
        private Mock<IDependencyResolver> _dependencyResolver;
        private Commitments.ImportCommitmentsTask _task;
        private Mock<IExternalContext> _context;
        private string _contextValue;

        [SetUp]
        public void Arrange()
        {
            _contextValue = Guid.NewGuid().ToString();
            _context = new Mock<IExternalContext>();
            _context
                .Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    {"TestKey", _contextValue},
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "TransientDatabaseConnectionString"},
                    {ContextPropertyKeys.LogLevel, "DEBUG"},
                    {ImportCommitmentsContextKeys.BaseUrl, "http://test" },
                    {ImportCommitmentsContextKeys.ClientToken, "token" }
                });

            _processor = new Mock<Commitments.ApiProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver
                .Setup(r => r.GetInstance<Commitments.ApiProcessor>())
                .Returns(_processor.Object);

            _task = new Commitments.ImportCommitmentsTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenItShouldInitDependencyResolver()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(r => r.Init(typeof(Commitments.ApiProcessor), It.Is<ContextWrapper>(cw => cw.Context == _context.Object)), Times.Once);
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