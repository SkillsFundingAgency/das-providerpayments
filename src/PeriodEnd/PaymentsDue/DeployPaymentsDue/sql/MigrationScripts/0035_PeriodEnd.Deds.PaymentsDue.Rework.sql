 -----------------------------------------------------------------------------------------------------------------------------------------------
-- TABLES
-----------------------------------------------------------------------------------------------------------------------------------------------


-----------------------------------------------------------------------------------------------------------------------------------------------
-- NonPayableEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='NonPayableEarnings' AND s.name='PaymentsDue'
)
BEGIN
	CREATE TABLE PaymentsDue.NonPayableEarnings
	(
		Id uniqueidentifier DEFAULT(NEWID()),
		CommitmentId bigint,
		CommitmentVersionId varchar(25),
		AccountId bigint,
		AccountVersionId varchar(50),
		Uln bigint,
		LearnRefNumber varchar(12) NOT NULL,
		AimSeqNumber int,
		Ukprn bigint,
		IlrSubmissionDateTime datetime,
		PriceEpisodeIdentifier varchar(25) NULL,
		StandardCode bigint,
		ProgrammeType int,
		FrameworkCode int,
		PathwayCode int,
		ApprenticeshipContractType int,
		DeliveryMonth int,
		DeliveryYear int,
		CollectionPeriodName varchar(8) NOT NULL,
		CollectionPeriodMonth int,
		CollectionPeriodYear int,
		TransactionType int,
		AmountDue decimal(15,5),
		SfaContributionPercentage decimal(15,5),
		FundingLineType varchar(100),
		UseLevyBalance bit,
		LearnAimRef varchar(8) NOT NULL,
		LearningStartDate datetime,
		PaymentFailureMessage varchar(1000) NOT NULL,
		PaymentFailureReason int NOT NULL,
	)

	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_CollectionPeriodName ON PaymentsDue.NonPayableEarnings (CollectionPeriodName)
	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_UkprnLearnRefNumber ON PaymentsDue.NonPayableEarnings (Ukprn, LearnRefNumber)
	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_Uln ON PaymentsDue.NonPayableEarnings (Uln)
	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_CommitmentId ON PaymentsDue.NonPayableEarnings (CommitmentId)
	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_PaymentFailureReason ON PaymentsDue.NonPayableEarnings (PaymentFailureReason)
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- VIEWS
-----------------------------------------------------------------------------------------------------------------------------------------------


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_RequiredPaymentsForThisAcademicYear
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RequiredPaymentsForThisAcademicYear' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_RequiredPaymentsForThisAcademicYear
END
GO

CREATE VIEW PaymentsDue.vw_RequiredPaymentsForThisAcademicYear
AS
WITH [Period] AS (
	SELECT CONCAT(Collection_Year, '-R%') [CollectionYear]
	FROM ${DAS_PeriodEnd.FQ}.dbo.Collection_Period_Mapping
	WHERE Collection_Open = 1
)
SELECT 
	[Id]
	,[CommitmentId]
	,[CommitmentVersionId]
	,[AccountId]
	,[AccountVersionId]
	,[Uln]
	,[LearnRefNumber]
	,[AimSeqNumber]
	,[Ukprn]
	,[IlrSubmissionDateTime]
	,[PriceEpisodeIdentifier]
	,[StandardCode]
	,[ProgrammeType]
	,[FrameworkCode]
	,[PathwayCode]
	,[ApprenticeshipContractType]
	,[DeliveryMonth]
	,[DeliveryYear]
	,[CollectionPeriodName]
	,[CollectionPeriodMonth]
	,[CollectionPeriodYear]
	,[TransactionType]
	,[AmountDue]
	,[SfaContributionPercentage]
	,[FundingLineType]
	,[UseLevyBalance]
	,[LearnAimRef]
	,[LearningStartDate]
FROM PaymentsDue.RequiredPayments, [Period]
WHERE CollectionPeriodName LIKE CollectionYear
GO



-----------------------------------------------------------------------------------------------------------------------------------------------
-- INDEXES
-----------------------------------------------------------------------------------------------------------------------------------------------

IF IndexProperty(
	Object_Id('PaymentsDue.RequiredPayments'), 
	'IX_PaymentsDue_PaymentsDue_RequiredPayments_CollectionPeriodName', 
	'IndexId') IS NULL
BEGIN
	CREATE NONCLUSTERED INDEX [IX_PaymentsDue_PaymentsDue_RequiredPayments_CollectionPeriodName]
		ON [PaymentsDue].[RequiredPayments] (CollectionPeriodName)
END
GO