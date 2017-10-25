using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.Payments.Reference.Accounts.Context;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.DependencyResolution;

namespace SFA.DAS.Payments.Reference.Accounts
{
    public class ImportAccountsTask : DcfsTask
    {
        private IDependencyResolver _dependencyResolver;
        private const string AccountsSchema = "dbo";

        public ImportAccountsTask() 
            : base(AccountsSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        public ImportAccountsTask(IDependencyResolver dependencyResolver) 
            : base(AccountsSchema)
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
            CheckContextHasKey(contextWrapper, KnownContextKeys.AccountsApiBaseUrl);
            CheckContextHasKey(contextWrapper, KnownContextKeys.AccountsApiClientId);
            CheckContextHasKey(contextWrapper, KnownContextKeys.AccountsApiClientSecret);
            CheckContextHasKey(contextWrapper, KnownContextKeys.AccountsApiIdentifierUri);
            CheckContextHasKey(contextWrapper, KnownContextKeys.AccountsApiTenant);

            return base.IsValidContext(contextWrapper);
        }


        private void CheckContextHasKey(ContextWrapper contextWrapper, string key)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(key)))
            {
                throw new InvalidContextException("Context must contain value for " + key);
            }
        }
    }
}
