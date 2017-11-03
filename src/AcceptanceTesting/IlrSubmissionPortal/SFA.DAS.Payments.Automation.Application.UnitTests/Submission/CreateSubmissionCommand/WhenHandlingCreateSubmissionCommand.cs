using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.CreateSubmissionCommand;
using SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUlnCommand;
using SFA.DAS.Payments.Automation.Application.Submission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using SFA.DAS.Payments.Automation.Lars;
using SFA.DAS.Payments.Automation.Application.Entities;
using CsvHelper;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.Submission.CreateSubmissionCommand
{
    public class WhenHandlingCreateSubmissionCommand
    {
        private const string IlrNamespace = "SFA/ILR/2016-17";
        private const string ComponentAimRef = "CompAimRef23";

        private CreateSubmissionCommandRequest _request = null;
        private CreateSubmissionCommandHandler _handler = null;
        private Mock<IMediator> _mediatr = null;
        private Mock<ISqlRefenceDataGenerator> _referenceDataGenerator = null;
        private Mock<ILarsRepository> _larsRepository;

        [SetUp]
        public void Arrange()
        {
            var learnerScenarios = new Dictionary<string, string>();
            learnerScenarios.Add("learner a", "scenario 1");
            learnerScenarios.Add("learner b", "scenario 1");
            _request = new CreateSubmissionCommandRequest
            {
                Accounts = new List<EmployerAccountLevyBalances>(),
                Commitments = new List<Commitment>(),
                ContractTypes = new List<ContractTypeRecord>(),
                LearnerRecords = new List<LearnerRecord>
                {
                    new LearnerRecord
                    {
                        AimType = AimType.Programme,
                        LearnerKey = "learner a",
                        ProgrammeType = 451,
                        FrameworkCode= 23,
                        PathwayCode = 1,
                        StartDate = new DateTime(2017, 10, 10),
                        ActualEndDate = new DateTime(2017, 10, 10),
                        TotalTrainingPrice1 = 100,
                        CompletionStatus = CompletionStatus.Continuing,
                        LearnerType = LearnerType.ProgrammeOnlyNonDas
                    }
                },
                LearningSupports = new List<LearningSupportRecord>(),
                LearnerScenarios = learnerScenarios,
                Ukprn = 999,
                AcademicYear = "1617"
            };

            _mediatr = new Mock<IMediator>();
            _referenceDataGenerator = new Mock<ISqlRefenceDataGenerator>();

            _mediatr.Setup(m => m.Send(It.IsAny<GetNextUlnQueryRequest>()))
               .Returns(new GetNextUlnQueryApplicationScalarResponse
               {
                   Value = 1
               });

            _larsRepository = new Mock<ILarsRepository>();
            _larsRepository.Setup(r => r.GetComponentLearningAims(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new[] { new LearningAim { AimRef = "FwkAim", IsEnglishAndMathsAim = false } });
            _larsRepository.Setup(r => r.GetComponentLearningAims(It.IsAny<long>(), It.IsAny<DateTime>()))
                .Returns(new[] { new LearningAim { AimRef = "StdAim", IsEnglishAndMathsAim = false } });
            _larsRepository.Setup(r => r.GetComponentLearningAims(23, 451, 1, new DateTime(2017, 10, 10)))
                .Returns(new[] { new LearningAim { AimRef = ComponentAimRef, IsEnglishAndMathsAim = false } });

            _handler = new CreateSubmissionCommandHandler(_mediatr.Object, _referenceDataGenerator.Object, _larsRepository.Object);
        }

        [Test]
        public void ThenItShouldReturnSuccessfulResponse()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccess, actual.Error?.Message);
            Assert.IsNotEmpty(actual.IlrContent);
            Assert.IsNull(actual.AccountsSql);
            Assert.IsNull(actual.CommitmentsSql);

        }

        [Test]
        public void ThenItShouldThrowExceptionNoLearnersDataSet()
        {
            // Arrange
            _request.LearnerRecords.Clear();

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsSuccess);
            Assert.IsInstanceOf<Exception>(actual.Error);
            Assert.IsNotNull(actual.Error);

        }

        [Test]
        public void ThenItShouldGroupLearnerRecordsByKey()
        {
            // Arrange
            _request.LearnerRecords.Add(new LearnerRecord
            {
                AimType = AimType.MathsOrEnglish,
                LearnerKey = "learner a",
                ProgrammeType = 451,
                FrameworkCode = 23,
                PathwayCode = 1
            });
            _request.LearnerRecords.Add(new LearnerRecord
            {
                AimType = AimType.Programme,
                LearnerKey = "learner b",
                StandardCode = 78
            });



            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsTrue(actual.IsSuccess, actual.Error?.Message);
            var actualDoc = XDocument.Parse(actual.IlrContent);
            var learnerElements = actualDoc.Element(XName.Get("Message", IlrNamespace))
                                           .Elements(XName.Get("Learner", IlrNamespace))
                                           .ToArray();
            Assert.AreEqual(2, learnerElements.Length);
        }

        [Test]
        public void ThenItShouldAddComponentAimIfOnlyOnProgForLearner()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            var actualDoc = XDocument.Parse(actual.IlrContent);
            var deliveryElements = actualDoc.Element(XName.Get("Message", IlrNamespace))
                                            .Element(XName.Get("Learner", IlrNamespace))
                                            .Elements(XName.Get("LearningDelivery", IlrNamespace))
                                            .ToArray();
            Assert.AreEqual(2, deliveryElements.Length);
            Assert.AreEqual("ZPROG001", deliveryElements[0].Element(XName.Get("LearnAimRef", IlrNamespace)).Value);
            Assert.AreEqual(ComponentAimRef, deliveryElements[1].Element(XName.Get("LearnAimRef", IlrNamespace)).Value);
        }

        [Test]
        public void ThenItShouldReturnSuccessfulCommitmentsBulkCSv()
        {

            _referenceDataGenerator.Setup(x =>
                x.GenerateCommitmentsReferenceDataScript(It.IsAny<List<CommitmentEntity>>()))
                .Returns("sql data");


            _request.Accounts.Add(new EmployerAccountLevyBalances
            {
                EmployerKey = "Employer 1",
                BalanceForAllPeriods = 100
            });
            _request.Commitments.Add(new Commitment
            {
                AgreedPrice = 1000,
                CommitmentId = 1,
                LearnerKey = "learner a",
                EmployerKey = "Employer 1",
                StartDate = new DateTime(2017, 10, 10),
                EndDate = new DateTime(2018, 10, 10),
                FrameworkCode = 21,
                ProgrammeType = 10,
                PathwayCode = 11,
                Status = CommitmentPaymentStatus.Active
            });
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccess, actual.Error?.Message);
            Assert.IsNotEmpty(actual.IlrContent);
            Assert.IsNotNull(actual.CommitmentsSql);
            Assert.IsNotNull(actual.CommitmentsBulkCsv);


            var commitments = actual.CommitmentsBulkCsv.Split(',').Skip(13);

            using (var reader = new System.IO.StringReader(actual.CommitmentsBulkCsv))
            {
                var csv = new CsvReader(reader);

                while (csv.Read())
                {
                    Assert.AreEqual(csv.GetField("CohortRef"), "COHORTREF");
                    Assert.AreEqual(csv.GetField("FworkCode"), "21");
                    Assert.AreEqual(csv.GetField("StartDate"), "2017-10");
                    Assert.AreEqual(csv.GetField("EndDate"), "2018-10");
                    Assert.AreEqual(csv.GetField("TotalPrice"), "1000");
                    Assert.AreEqual(csv.GetField("ProgType"), "10");
                    Assert.AreEqual(csv.GetField("PwayCode"), "11");
                }

            }

        }
    }
}
