using System;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    public static class CommonIndividualLearningRecords
    {
        public static IndividualLearningRecord SingleDasLearnerOnAStandard()
        {
            var learningStartDate = new DateTime(2017, 5, 6);
            var learningEndDate = learningStartDate.AddDays(373);

            return new IndividualLearningRecord
            {
                Ukprn = 10012345,
                PreparationDate = learningStartDate.AddDays(1).Add(DateTime.Now.TimeOfDay),
                AcademicYear = "1617",
                Learners = new List<Learner>
                {
                    new Learner
                    {
                        LearnerRefNumber = "1",
                        Uln = 1234567890,
                        EmploymentStatuses = new List<EmploymentStatus>
                        {
                            new EmploymentStatus
                            {
                                EmployerId = 101183291,
                                StatusCode = 10,
                                DateFrom = learningStartDate.AddDays(-1)
                            }
                        },
                        LearningDeliveries = new List<LearningDelivery>
                        {
                            new LearningDelivery
                            {
                                AimSequenceNumber = 1,
                                LearningStartDate = learningStartDate,
                                PlannedLearningEndDate = learningEndDate,
                                StandardCode = 21,
                                FundingAndMonitoringCodes = new List<FundingAndMonitoringCode>
                                {
                                    new FundingAndMonitoringCode
                                    {
                                        Type = FundingAndMonitoringCode.DasType,
                                        Code = FundingAndMonitoringCode.DasLevyCode,
                                        From = learningStartDate,
                                        To = learningEndDate
                                    }
                                },
                                FinancialRecords = new List<FinancialRecord>
                                {
                                    new FinancialRecord
                                    {
                                        Type = FinancialRecord.TotalNegotiatedPriceType,
                                        Code = FinancialRecord.TotalNegotiatedPrice1Code,
                                        From = learningStartDate,
                                        Amount = 12000
                                    },
                                    new FinancialRecord
                                    {
                                        Type = FinancialRecord.TotalNegotiatedPriceType,
                                        Code = FinancialRecord.TotalNegotiatedPrice2Code,
                                        From = learningStartDate,
                                        Amount = 3000
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
