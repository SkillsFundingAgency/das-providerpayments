


ALTER TABLE PaymentsDue.RequiredPayments
	ALTER COLUMN AccountId bigint
GO


IF NOT EXISTS(SELECT NULL FROM sys.indexes WHERE name='IX_PaymentsDue_TransactionType_UseLevy_Commitment_Query')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_PaymentsDue_TransactionType_UseLevy_Commitment_Query]
		ON [PaymentsDue].[RequiredPayments] ([CommitmentId],[UseLevyBalance],[TransactionType])
END
GO


