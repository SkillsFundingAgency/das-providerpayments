using System;
using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents
{
    internal static class TestDataSets
    {
        internal static ManualAdjustmentEntity GetManualAdjustmentEntity(string requiredPaymentIdToReverse = null)
        {
            Guid id = Guid.Empty;
            if (!string.IsNullOrEmpty(requiredPaymentIdToReverse) && !Guid.TryParse(requiredPaymentIdToReverse, out id))
            {
                throw new ArgumentOutOfRangeException(nameof(requiredPaymentIdToReverse), "requiredPaymentIdToReverse is not a valid guid");
            }
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }

            return new ManualAdjustmentEntity
            {
                RequiredPaymentIdToReverse = id,
                RequestorName = "Integration tests",
                ReasonForReversal = "Testing",
                DateUploaded = DateTime.Now
            };
        }

        internal static RequiredPaymentEntity GetRequiredPaymentEntity(string requiredPaymentId = null)
        {
            Guid id = Guid.Empty;
            if (!string.IsNullOrEmpty(requiredPaymentId) && !Guid.TryParse(requiredPaymentId, out id))
            {
                throw new ArgumentOutOfRangeException(nameof(requiredPaymentId), "requiredPaymentId is not a valid guid");
            }
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }

            return new RequiredPaymentEntity
            {
                Id = id,
                CommitmentId = 1,
                CommitmentVersionId = "1-001",
                AccountId = "1",
                AccountVersionId = "20170717",
                Uln = 1478523960,
                LearnRefNumber = "LRN-001",
                AimSeqNumber = 1,
                Ukprn = 10023658,
                IlrSubmissionDateTime = new DateTime(2016, 8, 1),
                PriceEpisodeIdentifier = "25-36-00-01/08/2016",
                StandardCode = 36,
                ApprenticeshipContractType = 1,
                DeliveryMonth = 8,
                DeliveryYear = 2016,
                TransactionType = 1,
                AmountDue = 500m,
                SfaContributionPercentage = 0.9m,
                FundingLineType = "abc",
                UseLevyBalance = true
            };
        }

        internal static PaymentEntity[] GetPayments(RequiredPaymentEntity requiredPayment, bool includeLevyPayment = false, bool includeCoInvestedPayments = true)
        {
            if (!includeLevyPayment && !includeCoInvestedPayments)
            {
                return new PaymentEntity[0];
            }

            var payments = new List<PaymentEntity>();
            var amountToPay = requiredPayment.AmountDue;

            if (includeLevyPayment)
            {
                var useFromLevy = includeCoInvestedPayments ? (new Random()).Next((int)(amountToPay / 4), (int)(amountToPay / 2)) : amountToPay;

                payments.Add(new PaymentEntity
                {
                    PaymentId = Guid.NewGuid(),
                    RequiredPaymentId = requiredPayment.Id,
                    CommitmentId = requiredPayment.CommitmentId,
                    DeliveryMonth = requiredPayment.DeliveryMonth,
                    DeliveryYear = requiredPayment.DeliveryYear,
                    CollectionPeriodMonth = requiredPayment.CollectionPeriodMonth,
                    CollectionPeriodYear = requiredPayment.CollectionPeriodYear,
                    CollectionPeriodName = requiredPayment.CollectionPeriodName,
                    FundingSource = 1,
                    TransactionType = requiredPayment.TransactionType,
                    Amount = useFromLevy
                });

                amountToPay -= useFromLevy;
            }

            if (includeCoInvestedPayments)
            {
                var employerContribution = amountToPay * 0.1m;
                var governmentContribution = amountToPay - employerContribution;

                payments.Add(new PaymentEntity
                {
                    PaymentId = Guid.NewGuid(),
                    RequiredPaymentId = requiredPayment.Id,
                    CommitmentId = requiredPayment.CommitmentId,
                    DeliveryMonth = requiredPayment.DeliveryMonth,
                    DeliveryYear = requiredPayment.DeliveryYear,
                    CollectionPeriodMonth = requiredPayment.CollectionPeriodMonth,
                    CollectionPeriodYear = requiredPayment.CollectionPeriodYear,
                    CollectionPeriodName = requiredPayment.CollectionPeriodName,
                    FundingSource = 2,
                    TransactionType = requiredPayment.TransactionType,
                    Amount = governmentContribution
                }); payments.Add(new PaymentEntity
                {
                    PaymentId = Guid.NewGuid(),
                    RequiredPaymentId = requiredPayment.Id,
                    CommitmentId = requiredPayment.CommitmentId,
                    DeliveryMonth = requiredPayment.DeliveryMonth,
                    DeliveryYear = requiredPayment.DeliveryYear,
                    CollectionPeriodMonth = requiredPayment.CollectionPeriodMonth,
                    CollectionPeriodYear = requiredPayment.CollectionPeriodYear,
                    CollectionPeriodName = requiredPayment.CollectionPeriodName,
                    FundingSource = 3,
                    TransactionType = requiredPayment.TransactionType,
                    Amount = employerContribution
                });
            }

            return payments.ToArray();
        }
    }
}
