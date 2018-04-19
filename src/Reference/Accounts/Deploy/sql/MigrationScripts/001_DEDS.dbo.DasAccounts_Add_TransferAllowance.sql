 
IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasAccounts'
	AND s.name='dbo'
	AND c.name='TransferAllowance'
)
BEGIN
ALTER TABLE ${DAS_Accounts.FQ}.dbo.DasAccounts
	Add TransferAllowance decimal(15, 2) NULL
END