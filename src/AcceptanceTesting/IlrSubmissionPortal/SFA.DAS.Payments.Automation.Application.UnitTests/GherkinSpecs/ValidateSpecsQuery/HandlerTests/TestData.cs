using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ValidateSpecificationsQuery.ValidateSpecificationsQueryRequestHandlerTests
{
    internal static class TestData
    {
        internal static Specification GetValidSpecification(bool useStandard, bool isDasEmployer)
        {
            var startDate = new DateTime(2017, 8, 12);
            var endDate = startDate.AddDays(450);

            var spec = new Specification
            {
                Name = Guid.NewGuid().ToString(),
                Arrangement = new SpecificationArrangement
                {
                    LearnerRecords = new List<LearnerRecord>
                    {
                        new LearnerRecord
                        {
                            LearnerKey = Defaults.LearnerKey,
                            StartDate = startDate,
                            PlannedEndDate = endDate
                        }
                    },
                    EmploymentStatuses = new List<LearnerEmploymentStatus>
                    {
                        new LearnerEmploymentStatus
                        {
                            EmployerKey = Defaults.EmployerKey,
                            EmploymentStatus = EmploymentStatus.InPaidEmployment,
                            EmploymentStatusApplies = startDate
                        }
                    }
                }
            };

            if (useStandard)
            {
                spec.Arrangement.LearnerRecords[0].StandardCode = 23;
                spec.Arrangement.LearnerRecords[0].TotalAssessmentPrice1 = 3000;
                spec.Arrangement.LearnerRecords[0].TotalAssessmentPrice1EffectiveDate = startDate;
                spec.Arrangement.LearnerRecords[0].TotalTrainingPrice1 = 12000;
                spec.Arrangement.LearnerRecords[0].TotalTrainingPrice1EffectiveDate = startDate;
            }
            else
            {
                spec.Arrangement.LearnerRecords[0].ProgrammeType = 23;
                spec.Arrangement.LearnerRecords[0].FrameworkCode = 450;
                spec.Arrangement.LearnerRecords[0].FrameworkCode = 1;
                spec.Arrangement.LearnerRecords[0].TotalTrainingPrice1 = 15000;
                spec.Arrangement.LearnerRecords[0].TotalTrainingPrice1EffectiveDate = startDate;
            }

            return spec;
        }
    }
}
