using System;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Commitments.Application;
using SFA.DAS.Payments.Reference.Commitments.Application.AddOrUpdateCommitmentCommand;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Payments.Reference.Commitments.UnitTests.Application.AddOrUpdateCommitmentCommand.AddOrUpdateCommitmentCommandHandler
{
    public class WhenHandlingRequest
    {
        private AddOrUpdateCommitmentCommandRequest _request;
        private Mock<ICommitmentRepository> _commitmentRepository;
        private Mock<ILogger> _logger;
        private Commitments.Application.AddOrUpdateCommitmentCommand.AddOrUpdateCommitmentCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new AddOrUpdateCommitmentCommandRequest
            {
                CommitmentId = 1,
                Uln = 2,
                Ukprn = 3,
                AccountId = 1,
                StartDate = new DateTime(2017, 4, 1),
                EndDate = new DateTime(2018, 5, 1),
                StandardCode = 987,
                VersionId = 222,
                Priority = 5,
                PaymentStatus = PaymentStatus.Active,
                LegalEntityName = "ACME Ltd.",
                PriceEpisodes = new List<PriceEpisode>() {
                    new PriceEpisode {
                        AgreedPrice=12345,
                        EffectiveFromDate= new DateTime(2017, 4, 1)
                    }
                }
            };

            _commitmentRepository = new Mock<ICommitmentRepository>();
            _commitmentRepository.Setup(r => r.GetById(1))
                .Returns<CommitmentEntity>(null);

            _logger = new Mock<ILogger>();

            _handler = new Commitments.Application.AddOrUpdateCommitmentCommand.AddOrUpdateCommitmentCommandHandler(_commitmentRepository.Object, _logger.Object);
        }

        [Test]
        public void ThenItShouldInsertCommitmentIfItDoesNotExist()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _commitmentRepository.Verify(r => r.Insert(It.Is<CommitmentEntity>(e => IsEntityForRequest(e, _request))), Times.Once);
        }

        [Test]
        public void ThenItShouldAddNewVersionCommitmentIdItExists()
        {
            // Arrange
            var newRequest = new AddOrUpdateCommitmentCommandRequest
            {
                CommitmentId = 1,
                Uln = 2,
                Ukprn = 3,
                AccountId = 1,
                StartDate = new DateTime(2017, 4, 1),
                EndDate = new DateTime(2018, 5, 1),
                StandardCode = 987,
                VersionId = 223,
                Priority = 5,
                PaymentStatus = PaymentStatus.Active,
                LegalEntityName = "ACME Ltd.",
                PriceEpisodes = new List<PriceEpisode>() {
                    new PriceEpisode {
                        AgreedPrice = 45678,
                        EffectiveFromDate = new DateTime(2017, 9, 1),
                        EffectiveToDate=new DateTime(2017,09,30)
                    },
                    new PriceEpisode {
                        AgreedPrice = 123456,
                        EffectiveFromDate = new DateTime(2017, 10, 1)
                    }
                }
            };

            var commitment = new CommitmentEntity
            {
                CommitmentId = _request.CommitmentId,
                AccountId = _request.AccountId,
                AgreedCost = _request.PriceEpisodes[0].AgreedPrice,
                EffectiveFromDate = _request.PriceEpisodes[0].EffectiveFromDate,
                VersionId = $"{_request.VersionId}-001",
                Priority = _request.Priority,
                EffectiveToDate = _request.PriceEpisodes[0].EffectiveToDate,

            };

            _commitmentRepository.Setup(r => r.GetById(1))
                .Returns(commitment);

            // Act
            _handler.Handle(newRequest);

            // Assert

            _commitmentRepository.Verify(r => r.Insert(It.Is<CommitmentEntity>(e => IsEntityForRequest(e, newRequest))), Times.Exactly(2));
            


        }

        [Test]
        public void ThenItShouldNotAddNewVersionIfCommitmentDataMatches()
        {
            // Arrange
            var newRequest = new AddOrUpdateCommitmentCommandRequest
            {
                CommitmentId = 1,
                Uln = 2,
                Ukprn = 3,
                AccountId = 1,
                StartDate = new DateTime(2017, 4, 1),
                EndDate = new DateTime(2018, 5, 1),
                StandardCode = 987,
                VersionId = 223,
                Priority = 5,
                PaymentStatus = PaymentStatus.Active,
                LegalEntityName = "ACME Ltd.",
                PriceEpisodes = new List<PriceEpisode>() {
                    new PriceEpisode {
                        AgreedPrice = 45678,
                        EffectiveFromDate =  new DateTime(2017, 9, 1)
                    }
                }
            };

            var commitment = new CommitmentEntity
            {
                CommitmentId = 1,
                Uln = 2,
                Ukprn = 3,
                AccountId = 1,
                StartDate = new DateTime(2017, 4, 1),
                EndDate = new DateTime(2018, 5, 1),
                AgreedCost = 45678,
                StandardCode = 987,
                VersionId = "223-001",
                Priority = 5,
                PaymentStatus = 1,
                EffectiveFromDate = new DateTime(2017, 9, 1),
                LegalEntityName = "ACME Ltd."
            };

            _commitmentRepository.Setup(r => r.GetById(1))
                .Returns(commitment);

            _commitmentRepository.Setup(r => r.CommitmentExistsAndDetailsAreIdentical(It.IsAny<CommitmentEntity>()))
                .Returns(true);

            // Act
            _handler.Handle(newRequest);

            // Assert
            _commitmentRepository.Verify(r => r.CommitmentExistsAndDetailsAreIdentical(It.Is<CommitmentEntity>(e => IsEntityForRequest(e, newRequest))), Times.Once);

            _commitmentRepository.Verify(r => r.Insert(It.Is<CommitmentEntity>(e => IsEntityForRequest(e, newRequest))), Times.Never);

            _commitmentRepository.Verify(r => r.Update(It.Is<CommitmentEntity>(e => e.EffectiveToDate == newRequest.PriceEpisodes[0].EffectiveFromDate.AddDays(-1))), Times.Never);
        }

        private bool IsEntityForRequest(CommitmentEntity commitmentEntity, AddOrUpdateCommitmentCommandRequest request)
        {
            var result = false;
            result = commitmentEntity.CommitmentId == request.CommitmentId
                   && commitmentEntity.Uln == request.Uln
                   && commitmentEntity.Ukprn == request.Ukprn
                   && commitmentEntity.AccountId == request.AccountId
                   && commitmentEntity.StartDate == request.StartDate
                   && commitmentEntity.EndDate == request.EndDate
                   && commitmentEntity.StandardCode == request.StandardCode
                   && commitmentEntity.FrameworkCode == request.FrameworkCode
                   && commitmentEntity.ProgrammeType == request.ProgrammeType
                   && commitmentEntity.PathwayCode == request.PathwayCode
                   && commitmentEntity.Priority == request.Priority
                   && commitmentEntity.VersionId.Split('-')[0] == request.VersionId.ToString()
                   && commitmentEntity.PaymentStatus == (int)request.PaymentStatus
                   && commitmentEntity.PaymentStatusDescription == request.PaymentStatus.ToString()
                   && commitmentEntity.LegalEntityName == request.LegalEntityName;
            if (result)
            {
                List<PriceEpisode> priceEpisodes = request.PriceEpisodes;
                result = request.PriceEpisodes.Any(x => x.AgreedPrice == commitmentEntity.AgreedCost) &&
                          request.PriceEpisodes.Any(x => x.EffectiveFromDate == commitmentEntity.EffectiveFromDate) &&
                          request.PriceEpisodes.Any(x => x.EffectiveToDate == commitmentEntity.EffectiveToDate);
            }
            return result;
        }
    }
}
