namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    public static class DasAccountsDataHelper
    {
        internal static void GetAll( )
        {
            const string sql = @"
            select *
            from Reference.DasAccounts;
            ";

            TestDataHelper.Execute(sql);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Reference.DasAccounts";
            TestDataHelper.Execute(sql);
        }
    }
}