using System;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution;
using SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.Tools;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.CoInvestmentPaymentsTask
{
    public class WhenExecutingWithValidContext
    {
        private Mock<IExternalContext> _context;
        private Mock<CoInvestedPayments.CoInvestmentPaymentsPassThroughProcessor> _processor;
        private Mock<IDependencyResolver> _dependencyResolver;
        private CoInvestedPayments.CoInvestmentPaymentsTask _task;

        [SetUp]
        public void Arrange()
        {
            _context = ContextMother.CreateContext();

            _processor = new Mock<CoInvestedPayments.CoInvestmentPaymentsPassThroughProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<CoInvestedPayments.CoInvestmentPaymentsPassThroughProcessor>())
                .Returns(_processor.Object);

            _task = new CoInvestedPayments.CoInvestmentPaymentsTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenItShouldInitialiseTheDependencyResolver()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(dr => dr.Init(It.Is<Type>(t=>t==typeof(CoInvestedPayments.CoInvestmentPaymentsPassThroughProcessor)), It.IsAny<ContextWrapper>()), Times.Once);
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
