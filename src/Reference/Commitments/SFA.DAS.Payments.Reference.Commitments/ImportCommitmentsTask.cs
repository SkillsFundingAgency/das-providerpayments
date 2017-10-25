using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.DependencyResolution;

namespace SFA.DAS.Payments.Reference.Commitments
{
    public class ImportCommitmentsTask : DcfsTask
    {
        private IDependencyResolver _dependencyResolver;
        private const string CommmitmentsSchema = "dbo";

        public ImportCommitmentsTask()
            : base(CommmitmentsSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        public ImportCommitmentsTask(IDependencyResolver dependencyResolver)
            : base(CommmitmentsSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(ApiProcessor), context);

            var processor = _dependencyResolver.GetInstance<ApiProcessor>();

            processor.Process();
        }

        protected override bool IsValidContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ImportCommitmentsContextKeys.BaseUrl)))
            {
                throw new InvalidContextException("Context must contain value for " + ImportCommitmentsContextKeys.BaseUrl);
            }
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ImportCommitmentsContextKeys.ClientToken)))
            {
                throw new InvalidContextException("Context must contain value for " + ImportCommitmentsContextKeys.ClientToken);
            }
            return base.IsValidContext(contextWrapper);
        }
    }
}
