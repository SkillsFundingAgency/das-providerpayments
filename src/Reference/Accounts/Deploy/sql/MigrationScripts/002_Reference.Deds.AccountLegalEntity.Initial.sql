-----------------------------------------------------------------------------------------------------------------------------------------------
-- AccountLegalEntity
-----------------------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='AccountLegalEntity' AND s.name='dbo'
)
BEGIN
	CREATE TABLE dbo.AccountLegalEntity
	(
		[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
		[AccountLegalEntityPublicHashedId] CHAR(6) NOT NULL, 
		[AccountId] BIGINT NOT NULL,
		[LegalEntityId] BIGINT NOT NULL
	)

	CREATE INDEX IX_TransferPayments_AccountTransfers_RequiredPaymentId ON TransferPayments.AccountTransfers (RequiredPaymentId)
END

CREATE INDEX [IX_AccountLegalEntity_AccountId] ON [dbo].[AccountLegalEntity] ([AccountId]);

CREATE INDEX [IX_AccountLegalEntity_LegalEntityId] ON [dbo].[AccountLegalEntity] ([LegalEntityId]);

CREATE UNIQUE INDEX [IX_AccountLegalEntity_AccountIdLegalEntityId] ON [dbo].[AccountLegalEntity] (AccountId, LegalEntityId);

CREATE UNIQUE INDEX [IX_AccountLegalEntity_PublicHashedId] ON [dbo].[AccountLegalEntity] ([AccountLegalEntityPublicHashedId]);