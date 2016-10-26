TRUNCATE TABLE [Reference].[DasAccounts]
GO

INSERT INTO [Reference].[DasAccounts]
    SELECT
        [AccountId],
        [AccountName],
        [Balance]
	FROM ${DAS_Accounts.FQ}.[dbo].[DasAccounts]
GO
