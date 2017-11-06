using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.Payments.DCFS.Context;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.AcceptanceCriteria
{
    public class WhenRunningAcceptanceCriteria1
    {
        private readonly string _transientConnectionString = GlobalTestContext.Instance.SubmissionConnectionString;

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            SetUpIlrDatabase();

            _task = new DataLock.DataLockTask();

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, _transientConnectionString},
                    {ContextPropertyKeys.LogLevel, "Trace"},
                    { DataLockContextPropertyKeys.YearOfCollection, "1617" }
                }
            };
        }

        private void SetUpIlrDatabase()
        {
            TestDataHelper.Clean();

            // ILR data
            TestDataHelper.ExecuteScript(@"DPP-222\IlrAcceptanceCriteria1.sql");

            // Commitment data
            TestDataHelper.AddCommitment(
                new CommitmentEntityBuilder()
                    .WithUln(1000000000)
                    .WithStartDate(new DateTime(2017, 9, 1))
                    .WithStandardCode(999)
                    .WithProgrammeType(null)
                    .WithFrameworkCode(null)
                    .WithPathwayCode(null)
                    .WithAgreedCost(3000)
                    .Build());

            TestDataHelper.CopyReferenceData();
        }

        [Test]
        public void ThenValidationErrorAddedSuccessfully()
        {
            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.EarlierStartDate));
        }
    }
}