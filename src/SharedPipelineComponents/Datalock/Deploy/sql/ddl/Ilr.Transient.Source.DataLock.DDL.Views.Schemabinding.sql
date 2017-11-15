IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
    EXEC('CREATE SCHEMA DataLock')
END

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_Commitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vw_commitments' AND TABLE_SCHEMA = 'DataLock')
BEGIN
	DROP VIEW DataLock.vw_Commitments
END
GO

CREATE VIEW DataLock.vw_Commitments
WITH SCHEMABINDING
AS
SELECT c.[CommitmentId]
      ,c.[VersionId]
      ,c.[Uln]
      ,c.[Ukprn] as UKPRN
	  ,x.UKPRN as ProviderUKPRN
      ,c.[AccountId]
      ,c.[StartDate]
      ,c.[EndDate]
      ,c.[AgreedCost]
      ,c.[StandardCode]
      ,c.[ProgrammeType]
      ,c.[FrameworkCode]
      ,c.[PathwayCode]
      ,c.[PaymentStatus]
      ,c.[PaymentStatusDescription]
      ,c.[Priority]
	  ,c.[EffectiveFrom]
	  ,c.[EffectiveTo]
      
  FROM Reference.[DasCommitments] c
  INNER JOIN Valid.Learner l ON l.ULN = c.Uln
  ,Valid.LearningProvider x
GO


IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'Learner'
AND i.name = 'IX_ValidLearner_ULN')
BEGIN
	DROP INDEX [IX_ValidLearner_ULN] ON Valid.Learner
END
GO

CREATE NONCLUSTERED INDEX [IX_ValidLearner_ULN]
ON [Valid].[Learner] ([ULN])
GO

IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'Learner'
AND i.name = 'IX_DasCommitments_ULN')
BEGIN
	DROP INDEX [IX_DasCommitments_ULN] ON [Reference].[DasCommitments]
END
GO

CREATE NONCLUSTERED INDEX [IX_DasCommitments_ULN]
ON [Reference].[DasCommitments] ([Uln])
INCLUDE ([CommitmentId],[VersionId],[Ukprn],[AccountId],[StartDate],[EndDate],[AgreedCost],[StandardCode],[ProgrammeType],[FrameworkCode],[PathwayCode],[PaymentStatus],[PaymentStatusDescription],[Priority],[EffectiveFrom],[EffectiveTo])
GO

