using System;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.UnitTests.PaymentScheduleTask
{
    public class WhenExecutingWithValidContext
    {
        private Mock<IExternalContext> _context;
        private Mock<PaymentSchedule.PaymentScheduleProcessor> _processor;
        private Mock<IDependencyResolver> _dependencyResolver;
        private PaymentSchedule.PaymentScheduleTask _task;

        [SetUp]
        public void Arrange()
        {
            _context = ContextMother.CreateContext();

            _processor = new Mock<PaymentSchedule.PaymentScheduleProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<PaymentSchedule.PaymentScheduleProcessor>())
                .Returns(_processor.Object);

            _task = new PaymentSchedule.PaymentScheduleTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenItShouldInitialiseTheDependencyResolver()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(dr => dr.Init(It.Is<Type>(t=>t==typeof(PaymentSchedule.PaymentScheduleProcessor)), It.IsAny<ContextWrapper>()), Times.Once);
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
