using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUlnCommand;
using SFA.DAS.Payments.Automation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.ReferenceData.GetNextUlnCommand
{
    public class WhenHandlingGetNextUlnCommand
    {

        GetNextUlnQueryRequest _request = null;
        GetNextUlnQueryHandler _handler= null;
        private Mock<IReferenceDataRepository> _repository;


        [SetUp]
        public void Arrange()
        {
            _request = new GetNextUlnQueryRequest
            {
                LearnerKey = "learner a"
            };

            _repository = new Mock<IReferenceDataRepository>();
            _repository.Setup(r => r.GetNextUln(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<long>()))
                .Returns<long>(null);

            _handler = new GetNextUlnQueryHandler(_repository.Object);


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

            _repository.Setup(r => r.GetNextUln(It.IsAny<string>(),It.IsAny<string>(), It.IsAny<long>()))
                 .Returns(999);


            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccess, actual.Error?.Message);
            Assert.AreEqual(actual.Value, 999);
        }

        [Test]
        public void ThenItShouldReturnSameUln()
        {

            _repository.Setup(r => r.GetNextUln(It.IsAny<string>(),It.IsAny<string>(), It.IsAny<long>()))
                 .Returns(999);


            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccess, actual.Error?.Message);
            Assert.AreEqual(actual.Value, 999);

            //call again
            actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Value, 999);
        }


    }
}
