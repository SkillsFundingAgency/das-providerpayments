using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ProviderPayments.TestStack.Core.Context;
using ProviderPayments.TestStack.Core.ExecutionStatus;

namespace ProviderPayments.TestStack.Core.Workflow
{
    internal abstract class Workflow
    {
        private WorkflowTask[] _tasks;

        protected Workflow(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; private set; }
        
        protected void SetTasks(WorkflowTask[] tasks)
        {
            _tasks = tasks;
        }

        internal virtual void Execute(TestStackContext context, StatusWatcherBase statusWatcherBase)
        {
            statusWatcherBase.ExecutionStarted(GetTaskDescriptors());
            try
            {
                PrepareTransient(context);
                PrepareTasks(context);

                var didATaskError = false;
                foreach (var task in _tasks)
                {
                    statusWatcherBase.TaskStarted(task.Id);
                    try
                    {
                        task.PreRun(context);
                        task.Execute(context);
                        statusWatcherBase.TaskCompleted(task.Id, null);
                    }
                    catch (Exception ex)
                    {
                        statusWatcherBase.TaskCompleted(task.Id, ex);
                        throw;
                    }
                }

                statusWatcherBase.ExecutionCompleted(didATaskError ? new Exception("Not all tasks completed successfully") : null);
            }
            catch (Exception ex)
            {
                statusWatcherBase.ExecutionCompleted(ex);
            }
        }

        private TaskDescriptor[] GetTaskDescriptors()
        {
            return _tasks.Select(t => new TaskDescriptor {Id = t.Id, Description = t.Description}).ToArray();
        }
        private void PrepareTransient(TestStackContext context)
        {
            var connectionString = context.Properties[KnownContextKeys.TransientDatabaseConnectionString];
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute("EXEC sys.sp_msforeachtable 'TRUNCATE TABLE ?'");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void PrepareTasks(TestStackContext context)
        {
            foreach (var task in _tasks)
            {
                task.Prepare(context);
            }
        }
    }
}
