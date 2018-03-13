TRUNCATE TABLE Staging.ApprenticeshipEarnings2
GO

INSERT INTO Staging.ApprenticeshipEarnings2
SELECT
	DISTINCT 
    pepm.CommitmentId,
    pepm.VersionId CommitmentVersionId,
    a.AccountId,
    a.VersionId as AccountVersionId,
    ade.[Ukprn],
    ade.[Uln],
    ade.[LearnRefNumber],
    ade.[AimSeqNumber],
    ade.[Period],
    ae.PriceEpisodeEndDate,
    ade.StandardCode,
    (CASE WHEN ade.StandardCode IS NULL THEN ade.ProgrammeType ELSE NULL END) ProgrammeType,
    ade.FrameworkCode,
    ade.PathwayCode,
    ade.ApprenticeshipContractType,
	ae.ApprenticeshipContractTypeCode,
	ae.ApprenticeshipContractTypeStartDate,
	ae.ApprenticeshipContractTypeEndDate,
    ae.PriceEpisodeIdentifier,
    ade.FundingLineType AS PriceEpisodeFundLineType,
    ade.SfaContributionPercentage AS PriceEpisodeSfaContribPct,
    ade.LevyNonPayIndicator AS PriceEpisodeLevyNonPayInd,
    ndtt.TransactionType AS TransactionType,
    (CASE ndtt.TransactionType
        WHEN 13 THEN ade.MathEngOnProgPayment
        WHEN 14 THEN ade.MathEngBalPayment
        WHEN 15 THEN ade.LearningSupportPayment
    END) AS EarningAmount,
		ae.[EpisodeStartDate],
		IsNull(pem.IsSuccess,0) As IsSuccess, 
		IsNull(pepm.Payable,0) As Payable,
		ade.LearnAimref,
		ade.LearningStartDate ,
		ade.LearningPlannedEndDate,
		ade.LearningActualEndDate ,
		ade.CompletionStatus,
		ade.CompletionAmount,
		ade.TotalInstallments ,
		ade.MonthlyInstallment,
		ade.EndpointAssessorId,
		ae.IsSmallEmployer

        FROM Reference.ApprenticeshipEarnings ae
			JOIN  Staging.LearnerPriceEpisodePerPeriod pae
			ON ae.Ukprn = pae.Ukprn
			AND ae.LearnRefNumber = pae.LearnRefNumber
			AND ae.AimSeqNumber = pae.AimSeqNumber
			AND ae.Period = pae.Period
			AND ae.EpisodeStartDate = pae.MaxEpisodeStartDate
            JOIN Reference.ApprenticeshipDeliveryEarnings ade ON ae.Ukprn = ade.Ukprn
                AND ae.LearnRefNumber = ade.LearnRefNumber
                AND ISNULL(ae.StandardCode, -1) = ISNULL(ade.StandardCode, -1)
                AND ISNULL(ae.FrameworkCode, -1) = ISNULL(ade.FrameworkCode, -1)
                AND ISNULL(ae.ProgrammeType, -1) = ISNULL(ade.ProgrammeType, -1)
                AND ISNULL(ae.PathwayCode, -1) = ISNULL(ade.PathwayCode, -1)
                AND ae.Period = ade.Period
			JOIN Staging.CollectionPeriods cp
              ON ade.Period = cp.PeriodNumber
            LEFT JOIN DataLock.PriceEpisodeMatch pem ON ae.Ukprn = pem.Ukprn
                AND ae.PriceEpisodeIdentifier = pem.PriceEpisodeIdentifier
                AND ae.LearnRefNumber = pem.LearnRefNumber
                AND ae.AimSeqNumber = pem.AimSeqNumber
            LEFT JOIN DataLock.PriceEpisodePeriodMatch pepm ON ae.Ukprn = pepm.Ukprn
                AND ae.PriceEpisodeIdentifier = pepm.PriceEpisodeIdentifier
                AND ae.LearnRefNumber = pepm.LearnRefNumber
                AND ae.AimSeqNumber = pepm.AimSeqNumber
                AND ae.Period = pepm.Period
            LEFT JOIN Reference.DasCommitments c ON c.CommitmentId = pepm.CommitmentId
                AND c.VersionId = pepm.VersionId
            LEFT JOIN Reference.DasAccounts a ON c.AccountId = a.AccountId
			LEFT JOIN Staging.NonDasTransactionTypes ndtt ON ndtt.ApprenticeshipContractType = ae.ApprenticeshipContractType

			LEFT JOIN Reference.RequiredPaymentsHistory ph 
		ON	ae.Ukprn = ph.Ukprn
              AND ae.LearnrefNumber = ph.LearnrefNumber
              AND ae.StandardCode = ph.StandardCode
              AND ISNULL(ae.ProgrammeType,0) = ISNULL(ph.ProgrammeType,0)
              AND ISNULL(ae.FrameworkCode,0) = ISNULL(ph.FrameworkCode,0)
              AND ISNULL(ae.PathwayCode,0) = ISNULL(ph.PathwayCode,0)
              AND ndtt.TransactionType = ph.TransactionType
			  AND ndtt.ApprenticeshipContractType = ph.ApprenticeshipContractType
              AND cp.CalendarMonth = ph.DeliveryMonth
              AND cp.CalendarYear = ph.DeliveryYear
	WHERE 
	(
	(COALESCE(pepm.TransactionTypesFlag, 1) = 1  And ndtt.TransactionType = 13  AND ade.MathEngOnProgPayment <> 0 ) OR
	(COALESCE(pepm.TransactionTypesFlag, 1) = 1 AND  ndtt.TransactionType = 14 And ade.MathEngBalPayment <> 0 ) OR
	(COALESCE(pepm.TransactionTypesFlag, 1) = 1  And ndtt.TransactionType = 15 AND ade.LearningSupportPayment <> 0 )
	OR ph.AmountDue> 0)
	

	ANd ae.EpisodeStartDate >= (
			Select
			Case WHEN  [Name] = 'R01' OR [Name] = 'R02' OR [Name] = 'R03' OR [Name] = 'R04' OR [Name] = 'R05'  THEN CONVERT(VARCHAR(10), '08/01/' +  Cast(CalendarYear as varchar) , 101) 
				ELSE CONVERT(VARCHAR(10), '08/01/' +  Cast(CalendarYear -1  as varchar) , 101) END
				From  Reference.CollectionPeriods Where [Open] = 1)
			AND
				ae.EpisodeStartDate <= ( Select 
				Case WHEN  [Name] = 'R01' OR [Name] = 'R02' OR [Name] = 'R03' OR [Name] = 'R04' OR [Name] = 'R05'  THEN CONVERT(VARCHAR(10), '07/31/' +  Cast(CalendarYear +1 as varchar) , 101) 
				ELSE CONVERT(VARCHAR(10), '07/31/' +  Cast(CalendarYear as varchar) , 101) END
				From  Reference.CollectionPeriods Where [Open] = 1)
		 And   ndtt.TransactionType In ( 13,14,15)
GO

IF EXISTS (
		SELECT 1
		FROM [sys].[indexes] i
		JOIN sys.objects t ON i.object_id = t.object_id
		WHERE t.name = 'ApprenticeshipEarnings2'
		AND i.[name] = 'IX_ApprenticeshipEarnings2_UKPRN'
		)
BEGIN
	DROP INDEX [IX_ApprenticeshipEarnings2_UKPRN]
		ON [Staging].[ApprenticeshipEarnings2]
END

CREATE CLUSTERED INDEX [IX_ApprenticeshipEarnings2_UKPRN] ON [Staging].[ApprenticeshipEarnings2]
(
	[Ukprn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
