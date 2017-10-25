using System;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ParseGherkinQuery.ParseGherkinQueryRequestHandlerTests
{
    public class WhenHandlingGherkinWithCommitments
    {
        private ParseGherkinQueryRequest _request;
        private ParseGherkinQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ParseGherkinQueryRequest
            {
                GherkinSpecs = Properties.Resources.WhenHandlingGherkinWithCommitments
            };

            _handler = new ParseGherkinQueryRequestHandler();
        }

        [Test]
        public void ThenItShouldReturnSuccessfulResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess);
        }

        [Test]
        public void ThenItShouldReturnEachScenario()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response.Results);
            Assert.AreEqual(5, response.Results.Length);
        }

        [Test]
        public void ThenItShouldPopulateEachScenarioWithCommitments()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess, $"Response is not success: {response.Error?.Message}");

            // --- Spec1
            var actualSpec1 = response.Results[0];
            Assert.IsNotNull(actualSpec1);
            Assert.AreEqual("Multiple commitments for different employers", actualSpec1.Name);
            Assert.IsNotNull(actualSpec1.Arrangement);
            Assert.IsNotNull(actualSpec1.Arrangement.Commitments);
            Assert.AreEqual(3, actualSpec1.Arrangement.Commitments.Count);

            var spec1Commitment1 = actualSpec1.Arrangement.Commitments[0];
            Assert.IsNotNull(spec1Commitment1);
            Assert.AreEqual("employer 1", spec1Commitment1.EmployerKey);
            Assert.AreEqual(Defaults.ProviderKey, spec1Commitment1.ProviderKey);
            Assert.AreEqual(1, spec1Commitment1.CommitmentId);
            Assert.AreEqual(1, spec1Commitment1.VersionId);
            Assert.AreEqual("learner a", spec1Commitment1.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec1Commitment1.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 31), spec1Commitment1.EndDate);
            Assert.AreEqual(15000, spec1Commitment1.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Active, spec1Commitment1.Status);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec1Commitment1.EffectiveFrom);
            Assert.AreEqual(new DateTime(2017, 10, 31), spec1Commitment1.EffectiveTo);
            Assert.AreEqual(Defaults.StandardCode, spec1Commitment1.StandardCode);

            var spec1Commitment2 = actualSpec1.Arrangement.Commitments[1];
            Assert.IsNotNull(spec1Commitment2);
            Assert.AreEqual("employer 1", spec1Commitment2.EmployerKey);
            Assert.AreEqual(Defaults.ProviderKey, spec1Commitment2.ProviderKey);
            Assert.AreEqual(1, spec1Commitment2.CommitmentId);
            Assert.AreEqual(2, spec1Commitment2.VersionId);
            Assert.AreEqual("learner a", spec1Commitment2.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec1Commitment2.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 31), spec1Commitment2.EndDate);
            Assert.AreEqual(15000, spec1Commitment2.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Cancelled, spec1Commitment2.Status);
            Assert.AreEqual(new DateTime(2017, 11, 1), spec1Commitment2.EffectiveFrom);
            Assert.IsNull(spec1Commitment2.EffectiveTo);
            Assert.AreEqual(Defaults.StandardCode, spec1Commitment2.StandardCode);

            var spec1Commitment3 = actualSpec1.Arrangement.Commitments[2];
            Assert.IsNotNull(spec1Commitment3);
            Assert.AreEqual("employer 2", spec1Commitment3.EmployerKey);
            Assert.AreEqual(Defaults.ProviderKey, spec1Commitment3.ProviderKey);
            Assert.AreEqual(2, spec1Commitment3.CommitmentId);
            Assert.AreEqual(1, spec1Commitment3.VersionId);
            Assert.AreEqual("learner a", spec1Commitment3.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 11, 1), spec1Commitment3.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 31), spec1Commitment3.EndDate);
            Assert.AreEqual(5625, spec1Commitment3.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Active, spec1Commitment3.Status);
            Assert.AreEqual(new DateTime(2017, 11, 1), spec1Commitment3.EffectiveFrom);
            Assert.IsNull(spec1Commitment3.EffectiveTo);
            Assert.AreEqual(Defaults.StandardCode, spec1Commitment3.StandardCode);

            // --- Spec2
            var actualSpec2 = response.Results[1];
            Assert.IsNotNull(actualSpec2);
            Assert.AreEqual("Multiple commitments for different providers", actualSpec2.Name);
            Assert.IsNotNull(actualSpec2.Arrangement);
            Assert.IsNotNull(actualSpec2.Arrangement.Commitments);
            Assert.AreEqual(3, actualSpec2.Arrangement.Commitments.Count);

            var spec2Commitment1 = actualSpec2.Arrangement.Commitments[0];
            Assert.IsNotNull(spec2Commitment1);
            Assert.AreEqual(Defaults.EmployerKey, spec2Commitment1.EmployerKey);
            Assert.AreEqual("provider a", spec2Commitment1.ProviderKey);
            Assert.AreEqual(1, spec2Commitment1.CommitmentId);
            Assert.AreEqual(1, spec2Commitment1.VersionId);
            Assert.AreEqual("learner a", spec2Commitment1.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec2Commitment1.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 1), spec2Commitment1.EndDate);
            Assert.AreEqual(7500, spec2Commitment1.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Active, spec2Commitment1.Status);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec2Commitment1.EffectiveFrom);
            Assert.AreEqual(new DateTime(2018, 3, 4), spec2Commitment1.EffectiveTo);
            Assert.AreEqual(Defaults.StandardCode, spec2Commitment1.StandardCode);

            var spec2Commitment2 = actualSpec2.Arrangement.Commitments[1];
            Assert.IsNotNull(spec2Commitment2);
            Assert.AreEqual(Defaults.EmployerKey, spec2Commitment2.EmployerKey);
            Assert.AreEqual("provider a", spec2Commitment2.ProviderKey);
            Assert.AreEqual(1, spec2Commitment2.CommitmentId);
            Assert.AreEqual(2, spec2Commitment2.VersionId);
            Assert.AreEqual("learner a", spec2Commitment2.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec2Commitment2.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 1), spec2Commitment2.EndDate);
            Assert.AreEqual(7500, spec2Commitment2.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Cancelled, spec2Commitment2.Status);
            Assert.AreEqual(new DateTime(2018, 3, 5), spec2Commitment2.EffectiveFrom);
            Assert.IsNull(spec2Commitment2.EffectiveTo);
            Assert.AreEqual(Defaults.StandardCode, spec2Commitment2.StandardCode);

            var spec2Commitment3 = actualSpec2.Arrangement.Commitments[2];
            Assert.IsNotNull(spec2Commitment3);
            Assert.AreEqual(Defaults.EmployerKey, spec2Commitment2.EmployerKey);
            Assert.AreEqual("provider b", spec2Commitment3.ProviderKey);
            Assert.AreEqual(2, spec2Commitment3.CommitmentId);
            Assert.AreEqual(1, spec2Commitment3.VersionId);
            Assert.AreEqual("learner a", spec2Commitment3.LearnerKey);
            Assert.AreEqual(new DateTime(2018, 6, 1), spec2Commitment3.StartDate);
            Assert.AreEqual(new DateTime(2018, 11, 1), spec2Commitment3.EndDate);
            Assert.AreEqual(4500, spec2Commitment3.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Active, spec2Commitment3.Status);
            Assert.AreEqual(new DateTime(2018, 6, 6), spec2Commitment3.EffectiveFrom);
            Assert.IsNull(spec2Commitment3.EffectiveTo);
            Assert.AreEqual(Defaults.StandardCode, spec2Commitment3.StandardCode);

            // --- Spec3
            var actualSpec3 = response.Results[2];
            Assert.IsNotNull(actualSpec3);
            Assert.AreEqual("Commitments is not first given", actualSpec3.Name);
            Assert.IsNotNull(actualSpec3.Arrangement);
            Assert.IsNotNull(actualSpec3.Arrangement.Commitments);
            Assert.AreEqual(1, actualSpec3.Arrangement.Commitments.Count);

            var spec3Commitment1 = actualSpec3.Arrangement.Commitments[0];
            Assert.IsNotNull(spec3Commitment1);
            Assert.AreEqual(Defaults.EmployerKey, spec3Commitment1.EmployerKey);
            Assert.AreEqual(Defaults.ProviderKey, spec3Commitment1.ProviderKey);
            Assert.AreEqual(1, spec3Commitment1.CommitmentId);
            Assert.AreEqual(1, spec3Commitment1.VersionId);
            Assert.AreEqual("learner a", spec3Commitment1.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec3Commitment1.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 1), spec3Commitment1.EndDate);
            Assert.AreEqual(7500, spec3Commitment1.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Active, spec3Commitment1.Status);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec3Commitment1.EffectiveFrom);
            Assert.IsNull(spec3Commitment1.EffectiveTo);
            Assert.AreEqual(Defaults.StandardCode, spec3Commitment1.StandardCode);

            // --- Spec4
            var actualSpec4 = response.Results[3];
            Assert.IsNotNull(actualSpec4);
            Assert.AreEqual("Commitment has standard", actualSpec4.Name);
            Assert.IsNotNull(actualSpec4.Arrangement);
            Assert.IsNotNull(actualSpec4.Arrangement.Commitments);
            Assert.AreEqual(1, actualSpec4.Arrangement.Commitments.Count);

            var spec4Commitment1 = actualSpec4.Arrangement.Commitments[0];
            Assert.IsNotNull(spec4Commitment1);
            Assert.AreEqual(Defaults.EmployerKey, spec4Commitment1.EmployerKey);
            Assert.AreEqual(Defaults.ProviderKey, spec4Commitment1.ProviderKey);
            Assert.AreEqual(1, spec4Commitment1.CommitmentId);
            Assert.AreEqual(1, spec4Commitment1.VersionId);
            Assert.AreEqual("learner a", spec4Commitment1.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec4Commitment1.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 1), spec4Commitment1.EndDate);
            Assert.AreEqual(7500, spec4Commitment1.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Active, spec4Commitment1.Status);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec4Commitment1.EffectiveFrom);
            Assert.IsNull(spec4Commitment1.EffectiveTo);
            Assert.AreEqual(65, spec4Commitment1.StandardCode);
            Assert.IsNull(spec4Commitment1.FrameworkCode);
            Assert.IsNull(spec4Commitment1.ProgrammeType);
            Assert.IsNull(spec4Commitment1.PathwayCode);

            // --- Spec5
            var actualSpec5 = response.Results[4];
            Assert.IsNotNull(actualSpec5);
            Assert.AreEqual("Commitment has framework", actualSpec5.Name);
            Assert.IsNotNull(actualSpec5.Arrangement);
            Assert.IsNotNull(actualSpec5.Arrangement.Commitments);
            Assert.AreEqual(1, actualSpec5.Arrangement.Commitments.Count);

            var spec5Commitment1 = actualSpec5.Arrangement.Commitments[0];
            Assert.IsNotNull(spec5Commitment1);
            Assert.AreEqual(Defaults.EmployerKey, spec5Commitment1.EmployerKey);
            Assert.AreEqual(Defaults.ProviderKey, spec5Commitment1.ProviderKey);
            Assert.AreEqual(1, spec5Commitment1.CommitmentId);
            Assert.AreEqual(1, spec5Commitment1.VersionId);
            Assert.AreEqual("learner a", spec5Commitment1.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec5Commitment1.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 1), spec5Commitment1.EndDate);
            Assert.AreEqual(7500, spec5Commitment1.AgreedPrice);
            Assert.AreEqual(CommitmentPaymentStatus.Active, spec5Commitment1.Status);
            Assert.AreEqual(new DateTime(2017, 8, 1), spec5Commitment1.EffectiveFrom);
            Assert.IsNull(spec5Commitment1.EffectiveTo);
            Assert.IsNull(spec5Commitment1.StandardCode);
            Assert.AreEqual(403, spec5Commitment1.FrameworkCode);
            Assert.AreEqual(2, spec5Commitment1.ProgrammeType);
            Assert.AreEqual(1, spec5Commitment1.PathwayCode);
        }
    }
}
