using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.Submission.TransformSubmissionCommand;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.Submission.TransformSubmissionCommand.TransformSubmissionCommandHandlerTests
{
    public class WhenHandlingRequestWithDateShifts
    {

        private TransformSubmissionCommandRequest _request;
        private TransformSubmissionCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new TransformSubmissionCommandRequest
            {
                Specification = new Specification
                {
                    Arrangement = new SpecificationArrangement
                    {
                        Commitments = new List<Commitment>
                        {
                            new Commitment
                            {
                                StartDate = new DateTime(2017, 9, 1),
                                EndDate = new DateTime(2020, 10, 1),
                                EffectiveFrom = new DateTime(2017,9,1),
                                EffectiveTo = new DateTime(2020, 10, 1)
                            }
                        },
                        ContractTypes = new List<ContractTypeRecord>
                        {
                            new ContractTypeRecord
                            {
                                DateFrom = new DateTime(2017, 9, 1),
                                DateTo = new DateTime(2020, 10, 1)
                            }
                        },
                        EmploymentStatuses = new List<LearnerEmploymentStatus>
                        {
                            new LearnerEmploymentStatus
                            {
                                EmploymentStatusApplies = new DateTime(2017, 8, 31)
                            }
                        },
                        LearnerRecords = new List<LearnerRecord>
                        {
                            new LearnerRecord
                            {
                                StartDate = new DateTime(2017, 9, 6),
                                PlannedEndDate = new DateTime(2020, 10, 11),
                                ActualEndDate = new DateTime(2020, 10, 12),
                                TotalTrainingPrice1EffectiveDate = new DateTime(2017, 9, 6),
                                TotalAssessmentPrice1EffectiveDate = new DateTime(2017, 9, 6),
                                TotalTrainingPrice2EffectiveDate = new DateTime(2018, 6, 14),
                                TotalAssessmentPrice2EffectiveDate = new DateTime(2018, 6, 14),
                                ResidualTrainingPrice1EffectiveDate = new DateTime(2019, 5, 6),
                                ResidualAssessmentPrice1EffectiveDate = new DateTime(2019, 5, 6),
                                ResidualTrainingPrice2EffectiveDate = new DateTime(2020, 6, 28),
                                ResidualAssessmentPrice2EffectiveDate = new DateTime(2020, 6, 28)
                            }
                        }
                    }
                },
                ShiftToMonth = 5,
                ShiftToYear = 2017
            };

            _handler = new TransformSubmissionCommandHandler();
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
        public void ThenItShouldShiftAllDatesByNumberOfMonthsBetweenEarliestLearnerStartDateAndShiftDate()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            Assert.AreEqual(new DateTime(2017, 5, 1), _request.Specification.Arrangement.Commitments[0].StartDate);
            Assert.AreEqual(new DateTime(2020, 6, 1), _request.Specification.Arrangement.Commitments[0].EndDate);
            Assert.AreEqual(new DateTime(2017, 5, 1), _request.Specification.Arrangement.Commitments[0].EffectiveFrom);
            Assert.AreEqual(new DateTime(2020, 6, 1), _request.Specification.Arrangement.Commitments[0].EffectiveTo);

            Assert.AreEqual(new DateTime(2017, 5, 1), _request.Specification.Arrangement.ContractTypes[0].DateFrom);
            Assert.AreEqual(new DateTime(2020, 6, 1), _request.Specification.Arrangement.ContractTypes[0].DateTo);

            Assert.AreEqual(new DateTime(2017, 4, 30), _request.Specification.Arrangement.EmploymentStatuses[0].EmploymentStatusApplies);

            Assert.AreEqual(new DateTime(2017, 5, 6), _request.Specification.Arrangement.LearnerRecords[0].StartDate);
            Assert.AreEqual(new DateTime(2020, 6, 11), _request.Specification.Arrangement.LearnerRecords[0].PlannedEndDate);
            Assert.AreEqual(new DateTime(2020, 6, 12), _request.Specification.Arrangement.LearnerRecords[0].ActualEndDate);
            Assert.AreEqual(new DateTime(2017, 5, 6), _request.Specification.Arrangement.LearnerRecords[0].TotalTrainingPrice1EffectiveDate);
            Assert.AreEqual(new DateTime(2017, 5, 6), _request.Specification.Arrangement.LearnerRecords[0].TotalAssessmentPrice1EffectiveDate);
            Assert.AreEqual(new DateTime(2018, 2, 14), _request.Specification.Arrangement.LearnerRecords[0].TotalTrainingPrice2EffectiveDate);
            Assert.AreEqual(new DateTime(2018, 2, 14), _request.Specification.Arrangement.LearnerRecords[0].TotalAssessmentPrice2EffectiveDate);
            Assert.AreEqual(new DateTime(2019, 1, 6), _request.Specification.Arrangement.LearnerRecords[0].ResidualTrainingPrice1EffectiveDate);
            Assert.AreEqual(new DateTime(2019, 1, 6), _request.Specification.Arrangement.LearnerRecords[0].ResidualAssessmentPrice1EffectiveDate);
            Assert.AreEqual(new DateTime(2020, 2, 28), _request.Specification.Arrangement.LearnerRecords[0].ResidualTrainingPrice2EffectiveDate);
            Assert.AreEqual(new DateTime(2020, 2, 28), _request.Specification.Arrangement.LearnerRecords[0].ResidualAssessmentPrice2EffectiveDate);
        }

        [TestCase(null, 2017)]
        [TestCase(5, null)]
        [TestCase(0, 2017)]
        [TestCase(13, 2017)]
        [TestCase(0, null)]
        [TestCase(13, null)]
        public void ThenItShouldReturnInvalidResponseIfShiftDatesAreInvalid(int? shiftToMonth, int? shiftToYear)
        {
            // Arrange
            _request.ShiftToMonth = shiftToMonth;
            _request.ShiftToYear = shiftToYear;

            // Act + Assert
            var ex = Assert.Throws<Exception>(() => _handler.Handle(_request));
            Assert.AreEqual("Both ShiftToMonth and ShiftToYear must be set together or null together and represent a valid month and year", ex.Message);
        }
    }
}
