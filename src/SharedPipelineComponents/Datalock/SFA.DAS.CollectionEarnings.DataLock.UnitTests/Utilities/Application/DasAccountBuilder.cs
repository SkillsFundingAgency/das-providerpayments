using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Application
{
    public class DasAccountBuilder : IBuilder<CollectionEarnings.DataLock.Application.DasAccount.DasAccount>
    {
        private long _accountId = 1;
        private bool _isLevyPayer = true;
      
        public DasAccount Build()
        {
            return new DasAccount
            {
                AccountId =_accountId,
                IsLevyPayer=_isLevyPayer
            };
        }

        public DasAccountBuilder WithAccountId(long accountId)
        {
            _accountId = accountId;

            return this;
        }

        public DasAccountBuilder WithIsLevyPayer(bool isLevyPayer)
        {
            _isLevyPayer = isLevyPayer;

            return this;
        }

    }
}