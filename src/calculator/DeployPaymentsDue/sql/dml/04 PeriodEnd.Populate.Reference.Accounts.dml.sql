TRUNCATE TABLE [Reference].[DasAccounts]
GO

INSERT INTO [Reference].[DasAccounts]
    SELECT
        [AccountId],
		[AccountHashId],
        [AccountName],
        [Balance],
		[VersionId]
	FROM ${DAS_Accounts.FQ}.[dbo].[DasAccounts]
GO
