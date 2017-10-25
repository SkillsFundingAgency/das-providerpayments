using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace SFA.DAS.Payments.Automation.Lars
{
    public class AutomationLarsSqlRepository : ILarsRepository
    {
        private readonly string _connectionString;

        public AutomationLarsSqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public LearningAim[] GetComponentLearningAims(int frameworkCode, int programmeType, int pathwayCode, DateTime effectiveAt)
        {
            effectiveAt = effectiveAt.Date;
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<LearningAim>("SELECT LearnAimRef AimRef, IsEnglishAndMathsAim FROM Lars.FrameworkAims " +
                                                "WHERE FworkCode = @frameworkCode AND ProgType = @programmeType AND PwayCode = @pathwayCode AND FrameworkComponentType = 3 " +
                                                "AND EffectiveFrom <= @effectiveAt AND (EffectiveTo IS NULL OR EffectiveTo >= @effectiveAt)",
                    new { frameworkCode, programmeType, pathwayCode, effectiveAt }).ToArray();
            }
        }
        public LearningAim[] GetComponentLearningAims(long standardCode, DateTime effectiveAt)
        {
            effectiveAt = effectiveAt.Date;
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<LearningAim>("SELECT LearnAimRef AimRef, IsEnglishAndMathsAim FROM Lars.StandardAims " +
                                                "WHERE StandardCode = @standardCode AND StandardComponentType = 3 " +
                                                "AND EffectiveFrom <= @effectiveAt AND (EffectiveTo IS NULL OR EffectiveTo >= @effectiveAt)",
                    new { standardCode, effectiveAt }).ToArray();
            }
        }
    }
}
