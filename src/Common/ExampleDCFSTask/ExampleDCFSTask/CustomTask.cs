using ExampleDCFSTask.Infrastructure.DependencyResolution;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;

namespace ExampleDCFSTask
{
    public class CustomTask : DcfsTask
    {
        private readonly IDependencyResolver _dependencyResolver;

        public CustomTask() 
            : base("MyTransientSchemaNameForLogging")
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        protected override void Execute(ContextWrapper context)
        {
            // Setup the dependency resolver
            _dependencyResolver.Init(typeof(CustomTask), context);

            // Get a processor instance
            var processor = _dependencyResolver.GetInstance<ExampleProcessor>();

            // Do the work
            processor.Process();
        }
    }
}
