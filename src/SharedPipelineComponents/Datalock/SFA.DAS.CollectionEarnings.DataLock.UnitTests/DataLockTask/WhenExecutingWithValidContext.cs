using System.Collections.Generic;
using CS.Common.External.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.DependencyResolution;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockTask
{
    public class WhenExecutingWithValidContext
    {
        private IExternalContext _context;
        
        [SetUp]
        public void Arrange()
        {
            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String"},
                    {ContextPropertyKeys.LogLevel, "Info"},
                    {DataLockContextPropertyKeys.YearOfCollection, "1617"}
                }
            };

        }

        [Test]
        public void ThenProcessorIsExecuted()
        {
            // Arrange
            var dependencyResolver = new TaskDependencyResolver();

            // Act
            dependencyResolver.Init(typeof(DataLockProcessor), new ContextWrapper(_context));

            var processor = dependencyResolver.GetInstance<DataLockProcessor>();

            // Assert
            processor.Should().BeOfType<DataLockProcessor>();
        }
    }
}