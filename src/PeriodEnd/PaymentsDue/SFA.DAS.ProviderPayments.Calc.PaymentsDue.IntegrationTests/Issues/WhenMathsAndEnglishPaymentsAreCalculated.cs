using System;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Issues
{
    [TestFixture]
    public class WhenMathsAndEnglishPaymentsAreCalculated
    {
        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        [Ignore("Harness does not support multi-period-end tests")]
        [TestCase(1)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        public void ThenPaymentsForStandardsMustHaveProgrammeType(int transactionType)
        {
            var ukprn = 863145;
            var uln = 834734;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var standardCode = 125;
            var programmeType = 25;

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(1);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, 
                uln: uln,
                standardCode: standardCode, 
                programmeType: programmeType,
                transactionType: transactionType);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            
            Assert.AreEqual(standardCode, duePayments[0].StandardCode);
            Assert.AreEqual(programmeType, duePayments[0].ProgrammeType);
        }
    }
}
