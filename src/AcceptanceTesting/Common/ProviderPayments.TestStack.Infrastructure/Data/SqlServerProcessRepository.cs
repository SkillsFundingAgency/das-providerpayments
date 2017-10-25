using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerProcessRepository : SqlServerRepository, IProcessRepository
    {
        public SqlServerProcessRepository()
            : base("TransientConnectionString")
        {
        }

        public Task<IEnumerable<ProcessStatusEntity>> All()
        {
            throw new NotImplementedException();
        }

        public async Task<ProcessStatusEntity> Single(string id)
        {
            var steps = (await Query<ProcessStepEntity>(@"SELECT StepId [Id]
                                                                ,StepIndex [Index]
                                                                ,StepDescription [Description]
                                                                ,ExecutionStartTime [StartTime]
                                                                ,ExecutionEndTime [EndTime]
                                                                ,ErrorMessage
                                                          FROM TestStack.ProcessStatus
                                                          WHERE ProcessId = @Id", new { id })).ToArray();
            return new ProcessStatusEntity
            {
                Id = id,
                Steps = steps.ToArray()
            };
        }
    }
}
