﻿using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.Provider.Events.DataLock.Infrastructure.Context;

namespace SFA.DAS.Provider.Events.DataLock.UnitTests.DataLockEventsTask
{
    public class WhenExecuted
    {
        private Mock<DataLock.DataLockEventsProcessor> _processor;
        private Mock<IDependencyResolver> _dependencyResolver;
        private DataLock.DataLockEventsTask _task;
        private Mock<IExternalContext> _context;

        [SetUp]
        public void Arrange()
        {
            _processor = new Mock<DataLock.DataLockEventsProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(r => r.GetInstance<DataLock.DataLockEventsProcessor>())
                .Returns(_processor.Object);

            _task = new DataLock.DataLockEventsTask(_dependencyResolver.Object);

            _context = new Mock<IExternalContext>();
            _context.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "some-connection-string"},
                    {ContextPropertyKeys.LogLevel, "Debug"},
                    {DataLockContextPropertyKeys.YearOfCollection, "1617"},
                    {DataLockContextPropertyKeys.DataLockEventsSource, "Submission"}
                });
        }

        [Test]
        public void ThenItShouldInitaliseTheDependencyResolver()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(r => r.Init(It.Is<Type>(t => t == typeof(DataLock.DataLockEventsProcessor)), It.IsAny<ContextWrapper>()), Times.Once);
        }

        [Test]
        public void ThenItShouldUseAResolvedProcessorToStartProcessing()
        {
            // Act
            _task.Execute(_context.Object);

            // Assert
            _dependencyResolver.Verify(r => r.GetInstance<DataLock.DataLockEventsProcessor>(), Times.Once);
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}
