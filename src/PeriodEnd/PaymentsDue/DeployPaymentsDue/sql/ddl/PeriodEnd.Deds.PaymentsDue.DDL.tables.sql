IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
	EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- RequiredPayments
-----------------------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='RequiredPayments' AND s.name='PaymentsDue'
)
BEGIN
	CREATE TABLE PaymentsDue.RequiredPayments
	(
		Id uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
		CommitmentId bigint,
		CommitmentVersionId varchar(50),
		AccountId varchar(50),
		AccountVersionId varchar(50),
		Uln bigint,
		LearnRefNumber varchar(12),
		AimSeqNumber int,
		Ukprn bigint,
		IlrSubmissionDateTime datetime,
		PriceEpisodeIdentifier varchar(25),
		StandardCode bigint,
		ProgrammeType int,
		FrameworkCode int,
		PathwayCode int,
		ApprenticeshipContractType int,
		DeliveryMonth int,
		DeliveryYear int,
		CollectionPeriodName varchar(8) NOT NULL,
		CollectionPeriodMonth int NOT NULL,
		CollectionPeriodYear int NOT NULL,
		TransactionType int,
		AmountDue decimal(15,5),
		SfaContributionPercentage decimal(15,5),
		FundingLineType varchar(60),
		UseLevyBalance bit
	)
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- Earnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='Earnings' AND s.name='PaymentsDue'
)
BEGIN
CREATE TABLE PaymentsDue.Earnings
(
    RequiredPaymentId uniqueidentifier NOT NULL, 
    StartDate datetime NOT NULL,
    PlannedEndDate datetime NOT NULL,
	ActualEnddate datetime,
    CompletionStatus int,
    CompletionAmount decimal(15,5),
	MonthlyInstallment decimal(15,5) NOT NULL,
	TotalInstallments int NOT NULL,
	EndpointAssessorId varchar(7) NULL
)
END



IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Valid')
BEGIN
	EXEC('CREATE SCHEMA Valid')
END
GO

IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='EmploymentStatusMonitoring' AND s.name='Valid'
)
BEGIN
CREATE TABLE [Valid].[EmploymentStatusMonitoring](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[DateEmpStatApp] [date] NOT NULL,
	[ESMType] [varchar](3) NOT NULL,
	[ESMCode] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[DateEmpStatApp] ASC,
	[ESMType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END