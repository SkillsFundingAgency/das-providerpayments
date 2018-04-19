TRUNCATE TABLE Staging.ApprenticeshipEarnings1
GO

INSERT INTO Staging.ApprenticeshipEarnings1
SELECT
    ae.CommitmentId,
    ae.CommitmentVersionId,
    ae.AccountId,
    ae.AccountVersionId,
    ae.Ukprn,
    ae.Uln,
    ae.LearnRefNumber,
    ae.AimSeqNumber,
    ae.Period,
    ae.PriceEpisodeEndDate,
    ae.StandardCode,
    ae.ProgrammeType,
    ae.FrameworkCode,
    ae.PathwayCode,
    ae.ApprenticeshipContractType,
	ae.ApprenticeshipContractTypeCode,
	ae.ApprenticeshipContractTypeStartDate,
	ae.ApprenticeshipContractTypeEndDate,
    ae.PriceEpisodeIdentifier,
    ae.PriceEpisodeFundLineType,
    ae.PriceEpisodeSfaContribPct,
    ae.PriceEpisodeLevyNonPayInd,
    ae.TransactionType,
    (CASE ae.TransactionType
        WHEN 1 THEN ae.PriceEpisodeOnProgPayment
        WHEN 2 THEN ae.PriceEpisodeCompletionPayment
        WHEN 3 THEN ae.PriceEpisodeBalancePayment
        WHEN 4 THEN ae.PriceEpisodeFirstEmp1618Pay
        WHEN 5 THEN ae.PriceEpisodeFirstProv1618Pay
        WHEN 6 THEN ae.PriceEpisodeSecondEmp1618Pay
        WHEN 7 THEN ae.PriceEpisodeSecondProv1618Pay
        WHEN 8 THEN ae.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment
        WHEN 9 THEN ae.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment
        WHEN 10 THEN ae.PriceEpisodeApplic1618FrameworkUpliftBalancing
        WHEN 11 THEN ae.PriceEpisodeFirstDisadvantagePayment
        WHEN 12 THEN ae.PriceEpisodeSecondDisadvantagePayment
        WHEN 15 THEN ae.LearningSupportPayment
    END) AS EarningAmount,
    ae.[EpisodeStartDate],
    IsNull(ae.IsSuccess,0) As IsSuccess, 
    IsNull(ae.Payable,0) As Payable,
	ae.LearnAimref,
	ae.LearningStartDate,
	ae.LearningPlannedEndDate,
	ae.LearningActualEndDate ,
	ae.CompletionStatus,
	ae.CompletionAmount,
	ae.TotalInstallments ,	
	ae.MonthlyInstallment,
	ae.EndpointAssessorId ,
	ae.IsSmallEmployer,
	ae.IsOnEHCPlan,
	ae.IsCareLeaver
FROM Staging.ApprenticeshipEarnings ae
WHERE ae.EpisodeStartDate >= (
    Select
    Case WHEN  [Name] = 'R01' OR [Name] = 'R02' OR [Name] = 'R03' OR [Name] = 'R04' OR [Name] = 'R05'  THEN CONVERT(VARCHAR(10), '08/01/' +  Cast(CalendarYear as varchar) , 101) 
        ELSE CONVERT(VARCHAR(10), '08/01/' +  Cast(CalendarYear -1  as varchar) , 101) END
        From  Reference.CollectionPeriods Where [Open] = 1)
    AND
        ae.EpisodeStartDate <= ( Select 
        Case WHEN  [Name] = 'R01' OR [Name] = 'R02' OR [Name] = 'R03' OR [Name] = 'R04' OR [Name] = 'R05'  THEN CONVERT(VARCHAR(10), '07/31/' +  Cast(CalendarYear +1 as varchar) , 101) 
        ELSE CONVERT(VARCHAR(10), '07/31/' +  Cast(CalendarYear as varchar) , 101) END
        From  Reference.CollectionPeriods Where [Open] = 1)
AND    ae.TransactionType In (1,2,3,4,5,6,7,8,9,10,11,12,15)
GO

IF EXISTS (
		SELECT 1
		FROM [sys].[indexes] i
		JOIN sys.objects t ON i.object_id = t.object_id
		WHERE t.name = 'ApprenticeshipEarnings1'
		AND i.[name] = 'IX_ApprenticeshipEarnings1_UKPRN'
		)
BEGIN
	DROP INDEX [IX_ApprenticeshipEarnings1_UKPRN]
		ON [Staging].[ApprenticeshipEarnings1]
END

CREATE CLUSTERED INDEX [IX_ApprenticeshipEarnings1_UKPRN] ON [Staging].[ApprenticeshipEarnings1]
(
	[Ukprn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

