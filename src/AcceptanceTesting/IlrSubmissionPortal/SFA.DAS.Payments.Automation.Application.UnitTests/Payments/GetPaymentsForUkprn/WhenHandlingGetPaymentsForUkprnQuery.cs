using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.Payments.GetPaymentsForUkprn;
using SFA.DAS.Payments.Automation.Infrastructure.Data;
using SFA.DAS.Payments.Automation.Infrastructure.PaymentResults;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.Payments.GetPaymentsForUkprn
{
    public class WhenHandlingGetPaymentsForUkprnQuery
    {

        GetPaymentsForUkprnRequest _request = null;
        GetPaymentsForUkprnQueryHandler _handler = null;
        private Mock<IPaymentsClient> _paymentsClient;


        [SetUp]
        public void Arrange()
        {
            _request = new GetPaymentsForUkprnRequest
            {
                Ukprn=1000
            };

            _paymentsClient = new Mock<IPaymentsClient>();
            _paymentsClient.Setup(r => r.GetPayments(It.IsAny<long>()))
                .Returns<long>(null);

            _handler = new GetPaymentsForUkprnQueryHandler(_paymentsClient.Object);


        }

        [Test]
        public void ThenItShouldReturnSuccessfulResponse()
        {

            
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccess, actual.Error?.Message);
         
        }

        [Test]
        public void ThenItShouldReturnCorrectUln()
        {
            var data = new List<PaymentResult>();
            data.Add( new PaymentResult {
                Amount=100,
                ApprenticeshipId=1,
                CollectionPeriodMonth =10,
                CollectionPeriodYear=2017,
                DeliveryMonth = 11,
                DeliveryYear=2017,
                ContractType=ContractType.ContractWithEmployer,
                Ukprn=1000,
                TransactionType=TransactionType.Balancing,
                FundingSource=FundingSource.CoInvestedEmployer,
                EmployerAccountId="1",
                CalculationPeriod="10/17",
                DeliveryPeriod="11/17",
                Uln=1100

            });

            _paymentsClient.Setup(r => r.GetPayments(It.IsAny<long>()))
                 .Returns(data);


            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccess, actual.Error?.Message);
            Assert.AreEqual(actual.Payments[0].Amount, 100);
            Assert.AreEqual(actual.Payments[0].Ukprn, 1000);
            Assert.AreEqual(actual.Payments[0].Uln, 1100);
            Assert.AreEqual(actual.Payments[0].ApprenticeshipId, 1);
            Assert.AreEqual(actual.Payments[0].CalculationPeriod, "10/17");
            Assert.AreEqual(actual.Payments[0].CollectionPeriodMonth, 10);
            Assert.AreEqual(actual.Payments[0].CollectionPeriodYear, 2017);
            Assert.AreEqual(actual.Payments[0].ContractType, ContractType.ContractWithEmployer);
            Assert.AreEqual(actual.Payments[0].FundingSource, FundingSource.CoInvestedEmployer);
            Assert.AreEqual(actual.Payments[0].TransactionType, TransactionType.Balancing);
            Assert.AreEqual(actual.Payments[0].DeliveryPeriod, "11/17");
            Assert.AreEqual(actual.Payments[0].DeliveryMonth, 11);
            Assert.AreEqual(actual.Payments[0].DeliveryYear, 2017);
        }

        
    }
}
