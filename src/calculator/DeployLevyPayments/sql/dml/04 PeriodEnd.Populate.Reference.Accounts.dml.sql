TRUNCATE TABLE [Reference].[DasAccounts]
GO

INSERT INTO [Reference].[DasAccounts]
    SELECT
        [AccountId],
		[AccountHashId],
        [AccountName],
        [Balance],
		[VersionId],
		[IsLevyPayer]
	FROM ${DAS_Accounts.FQ}.[dbo].[DasAccounts]
GO
