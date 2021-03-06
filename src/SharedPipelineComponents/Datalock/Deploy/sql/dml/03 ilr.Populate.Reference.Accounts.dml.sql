IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DasAccounts'
AND i.name = 'IX_DasAccount_AccountId')
BEGIN
	DROP INDEX IX_DasAccount_AccountId ON Reference.DasAccounts
END
GO


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
		[TransferAllowance]
	FROM ${DAS_Accounts.FQ}.[dbo].[DasAccounts]
GO

CREATE INDEX [IX_DasAccount_AccountId] ON [Reference].[DasAccounts] (AccountId)
GO
