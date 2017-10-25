using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GetAllUsedUlns;
using SFA.DAS.Payments.Automation.Infrastructure.Data;
using System.Collections.Generic;
using SFA.DAS.Payments.Automation.Infrastructure.Entities;



namespace SFA.DAS.Payments.Automation.Application.UnitTests.Submission
{
    public class WhenHandlingGetAllUsedUlnsCommand
    {
        GetAllUsedUlnsQueryRequest _request = null;
        GetAllUsedUlnsQueryHandler _handler = null;
        private Mock<ILearnersRepository> _repository;


        [SetUp]
        public void Arrange()
        {
            _request = new GetAllUsedUlnsQueryRequest
            {

            };

            _repository = new Mock<ILearnersRepository>();
            _repository.Setup(r => r.GetAllUsedUlns()).
                Returns<List<UsedUlnEntity>>(null);


            _handler = new GetAllUsedUlnsQueryHandler(_repository.Object);


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
        public void ThenItShouldReturnUln()
        {
            var ulns = new List<UsedUlnEntity>();
            ulns.Add(new UsedUlnEntity {
                LearnRefNumber = "test",
                ScenarioName = "scenario 1",
                Ukprn = 999,
                Uln = 100
            });

            _repository.Setup(r => r.GetAllUsedUlns())
                 .Returns(ulns);


            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccess, actual.Error?.Message);
            Assert.AreEqual(actual.UsedUlns.Count, 1);

            Assert.AreEqual(actual.UsedUlns[0].LearnRefNumber, "test");
            Assert.AreEqual(actual.UsedUlns[0].Ukprn, 999);
            Assert.AreEqual(actual.UsedUlns[0].Uln, 100);
            Assert.AreEqual(actual.UsedUlns[0].ScenarioName, "scenario 1");

        }

       

    }
}
