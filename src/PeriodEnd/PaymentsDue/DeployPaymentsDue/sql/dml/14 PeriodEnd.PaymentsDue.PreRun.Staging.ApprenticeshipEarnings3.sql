TRUNCATE TABLE Staging.ApprenticeshipEarnings3
GO

INSERT INTO Staging.ApprenticeshipEarnings3 WITH (TABLOCKX)
		 SELECT
		    pepm.CommitmentId,
            pepm.VersionId CommitmentVersionId,
            a.AccountId,
            a.VersionId AccountVersionId,
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
            IsNull(pepm.Payable,0) As Payable ,
			ade.LearnAimref,
			ade.LearningStartDate,
			ade.LearningPlannedEndDate,
			ade.LearningActualEndDate ,
			ade.CompletionStatus,
			ade.CompletionAmount,
			ade.TotalInstallments ,
			ade.MonthlyInstallment	,
			ade.EndpointAssessorId 
        FROM (SELECT MAX(PriceEpisodeEndDate) as LatestPriceEpisodeEndDate , 
				Ukprn, LearnRefNumber,StandardCode,FrameworkCode,ProgrammeType,PathwayCode
				from Reference.ApprenticeshipEarnings 
				Group By Ukprn, LearnRefNumber,StandardCode,FrameworkCode,ProgrammeType,PathwayCode ) ae1
			Join 	Reference.ApprenticeshipEarnings ae on ae1.LatestPriceEpisodeEndDate = ae.PriceEpisodeEndDate
					AND ISNULL(ae.StandardCode, -1) = ISNULL(ae1.StandardCode, -1)
					AND ISNULL(ae.FrameworkCode, -1) = ISNULL(ae1.FrameworkCode, -1)
					AND ISNULL(ae.ProgrammeType, -1) = ISNULL(ae1.ProgrammeType, -1)
					AND ISNULL(ae.PathwayCode, -1) = ISNULL(ae1.PathwayCode, -1)
					and ae1.LearnRefNumber = ae.LearnRefNumber
					and ae1.Ukprn = ae.Ukprn
			Join Reference.ApprenticeshipDeliveryEarnings ade 
			ON ae.Ukprn = ade.Ukprn
                AND ae.LearnRefNumber = ade.LearnRefNumber
                AND ISNULL(ae.StandardCode, -1) = ISNULL(ade.StandardCode, -1)
               AND ISNULL(ae.FrameworkCode, -1) = ISNULL(ade.FrameworkCode, -1)
                AND ISNULL(ae.ProgrammeType, -1) = ISNULL(ade.ProgrammeType, -1)
			  AND ISNULL(ae.PathwayCode, -1) = ISNULL(ade.PathwayCode, -1)
			LEFT JOIN Staging.NonDasTransactionTypes ndtt ON ndtt.ApprenticeshipContractType = ade.ApprenticeshipContractType

            LEFT JOIN DataLock.PriceEpisodeMatch pem ON ae.Ukprn = pem.Ukprn
                AND ae.PriceEpisodeIdentifier = pem.PriceEpisodeIdentifier
                AND ae.LearnRefNumber = pem.LearnRefNumber
                AND ae.AimSeqNumber = pem.AimSeqNumber
            LEFT JOIN DataLock.PriceEpisodePeriodMatch pepm ON ae.Ukprn = pepm.Ukprn
                AND ae.PriceEpisodeIdentifier = pepm.PriceEpisodeIdentifier
                AND ae.LearnRefNumber = pepm.LearnRefNumber
                AND ae.AimSeqNumber = pepm.AimSeqNumber
                AND ade.Period >= pepm.Period
            LEFT JOIN Reference.DasCommitments c ON c.CommitmentId = pepm.CommitmentId
                AND c.VersionId = pepm.VersionId
            LEFT JOIN Reference.DasAccounts a ON c.AccountId = a.AccountId
       WHERE 
	  
			(Select
				Case  ade.Period 
				WHEN 1 THEN CONVERT(VARCHAR(10), '08/01/' +  Cast(CalendarYear as varchar) , 101) 
				WHEN 2 THEN CONVERT(VARCHAR(10), '09/01/' +  Cast(CalendarYear as varchar) , 101) 
				WHEN 3 THEN CONVERT(VARCHAR(10), '10/01/' +  Cast(CalendarYear as varchar) , 101) 
				WHEN 4 THEN CONVERT(VARCHAR(10), '11/01/' +  Cast(CalendarYear as varchar) , 101) 
				WHEN 5 THEN CONVERT(VARCHAR(10), '12/01/' +  Cast(CalendarYear as varchar) , 101) 
				WHEN 6 THEN CONVERT(VARCHAR(10), '01/01/' +  Cast(CalendarYear  as varchar) , 101) 
				WHEN 7 THEN CONVERT(VARCHAR(10), '02/01/' +  Cast(CalendarYear  as varchar) , 101) 
				WHEN 8 THEN CONVERT(VARCHAR(10), '03/01/' +  Cast(CalendarYear  as varchar) , 101) 
				WHEN 9 THEN CONVERT(VARCHAR(10), '04/01/' +  Cast(CalendarYear  as varchar) , 101) 
				WHEN 10 THEN CONVERT(VARCHAR(10), '05/01/' +  Cast(CalendarYear  as varchar) , 101) 
				WHEN 11 THEN CONVERT(VARCHAR(10), '06/01/' +  Cast(CalendarYear  as varchar) , 101) 
				WHEN 12 THEN CONVERT(VARCHAR(10), '07/01/' +  Cast(CalendarYear  as varchar) , 101) 
				END From  Reference.CollectionPeriods Where [Open] = 1) > ae.PriceEpisodeEndDate 

  		And   (
				(COALESCE(pepm.TransactionTypesFlag, 1) = 1  And ndtt.TransactionType = 13  AND ade.MathEngOnProgPayment <> 0 ) OR
				(COALESCE(pepm.TransactionTypesFlag, 1) = 1 AND  ndtt.TransactionType = 14 And ade.MathEngBalPayment <> 0 ) OR
				(COALESCE(pepm.TransactionTypesFlag, 1) = 1  And ndtt.TransactionType = 15 AND ade.LearningSupportPayment <> 0 )
			)
GO
GO