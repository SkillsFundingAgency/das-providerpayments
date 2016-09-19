using System;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution;
using SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.Tools;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.CoInvestedPaymentsTask
{
    public class WhenExecutingWithValidContext
    {
        private Mock<IExternalContext> _context;
        private Mock<CoInvestedPayments.CoInvestedPaymentsPassThroughProcessor> _processor;
        private Mock<IDependencyResolver> _dependencyResolver;
        private CoInvestedPayments.CoInvestedPaymentsTask _task;

        [SetUp]
        public void Arrange()
        {
            _context = ContextMother.CreateContext();

            _processor = new Mock<CoInvestedPayments.CoInvestedPaymentsPassThroughProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<CoInvestedPayments.CoInvestedPaymentsPassThroughProcessor>())
                .Returns(_processor.Object);

            _task = new CoInvestedPayments.CoInvestedPaymentsTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenItShouldInitialiseTheDependencyResolver()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(dr => dr.Init(It.Is<Type>(t=>t==typeof(CoInvestedPayments.CoInvestedPaymentsPassThroughProcessor)), It.IsAny<ContextWrapper>()), Times.Once);
        }

        [Test]
        public void ThenItShouldCallProcessOnTheProcessor()
        {
            //Act
            _task.Execute(_context.Object);

            // Assert
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}
