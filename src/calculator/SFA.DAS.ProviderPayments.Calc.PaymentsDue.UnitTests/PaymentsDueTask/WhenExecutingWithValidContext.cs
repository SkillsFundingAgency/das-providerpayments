using System;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueTask
{
    public class WhenExecutingWithValidContext
    {
        private Mock<IExternalContext> _context;
        private Mock<PaymentsDue.PaymentsDuePassThroughProcessor> _processor;
        private Mock<IDependencyResolver> _dependencyResolver;
        private PaymentsDue.PaymentsDueTask _task;

        [SetUp]
        public void Arrange()
        {
            _context = ContextMother.CreateContext();

            _processor = new Mock<PaymentsDue.PaymentsDuePassThroughProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<PaymentsDue.PaymentsDuePassThroughProcessor>())
                .Returns(_processor.Object);

            _task = new PaymentsDue.PaymentsDueTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenItShouldInitialiseTheDependencyResolver()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(dr => dr.Init(It.Is<Type>(t=>t==typeof(PaymentsDue.PaymentsDuePassThroughProcessor)), It.IsAny<ContextWrapper>()), Times.Once);
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
