using System;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.TransfersTaskTests
{
    public class WhenExecutingWithValidContext
    {
        private Mock<IExternalContext> _context;
        private Mock<TransfersProcessor> _processor;
        private Mock<IDependencyResolver> _dependencyResolver;
        private TransfersTask _task;

        [SetUp]
        public void Arrange()
        {
            _context = ContextMother.CreateContext();

            _processor = new Mock<TransfersProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<TransfersProcessor>())
                .Returns(_processor.Object);

            _task = new TransfersTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenItShouldInitialiseTheDependencyResolver()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(dr => dr.Init(It.Is<Type>(t=>t==typeof(TransfersProcessor)), It.IsAny<Payments.DCFS.Context.ContextWrapper>()), Times.Once);
        }

        [Test]
        public void ThenItShouldExecuteTheProcessor()
        {
            //Act
            _task.Execute(_context.Object);

            // Assert
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}
