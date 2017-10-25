using System;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Azure;
using ProviderPayments.TestStack.Core.ExecutionStatus;

namespace ProviderPayments.TestStack.Engine
{
    public class RequestStatusWatcher : StatusWatcherBase
    {
        private readonly string _processId;

        public RequestStatusWatcher(string processId)
        {
            _processId = processId;
        }

        public override void ExecutionStarted(TaskDescriptor[] tasks)
        {
            var connectionString = CloudConfigurationManager.GetSetting("TransientConnectionString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute("DELETE FROM TestStack.ProcessStatus WHERE ProcessId=@ProcessId", new { ProcessId = _processId });

                    for (var i = 0; i < tasks.Length; i++)
                    {
                        var task = tasks[i];

                        connection.Execute("INSERT INTO TestStack.ProcessStatus"
                                         + "(ProcessId,StepId, StepIndex,StepDescription)"
                                         + "VALUES"
                                         + "(@ProcessId,@StepId,@StepIndex,@StepDescription)",
                                         new
                                         {
                                             ProcessId = _processId,
                                             StepId = task.Id,
                                             StepIndex = i,
                                             StepDescription = task.Description
                                         });
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public override void TaskStarted(string taskId)
        {
            var connectionString = CloudConfigurationManager.GetSetting("TransientConnectionString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute("UPDATE TestStack.ProcessStatus SET ExecutionStartTime=@StartTime WHERE ProcessId=@ProcessId AND StepId=@StepId",
                        new
                        {
                            ProcessId = _processId,
                            StepId = taskId,
                            StartTime = DateTime.Now
                        });
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public override void TaskCompleted(string taskId, Exception error)
        {
            var connectionString = CloudConfigurationManager.GetSetting("TransientConnectionString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute("UPDATE TestStack.ProcessStatus SET ExecutionEndTime=@EndTime, ErrorMessage=@ErrorMessage WHERE ProcessId=@ProcessId AND StepId=@StepId",
                        new
                        {
                            ProcessId = _processId,
                            StepId = taskId,
                            EndTime = DateTime.Now,
                            ErrorMessage = error?.Message
                        });
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
