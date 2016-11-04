TRUNCATE TABLE [Reference].[ApprenticeshipEarnings]
GO

INSERT INTO [Reference].[ApprenticeshipEarnings]
    SELECT
        pv.Ukprn,
		l.Uln,
        pv.LearnRefNumber,
        pv.AimSeqNumber,
		pv.AttributeName,
        ld.MonthlyInstallment,
        ld.CompletionPayment,
        pv.Period_1,
        pv.Period_2,
        pv.Period_3,
        pv.Period_4,
        pv.Period_5,
        pv.Period_6,
        pv.Period_7,
        pv.Period_8,
        pv.Period_9,
        pv.Period_10,
        pv.Period_11,
        pv.Period_12
    FROM ${ILR_Deds.FQ}.Rulebase.AE_LearningDelivery ld
    INNER JOIN ${ILR_Deds.FQ}.Rulebase.AE_LearningDelivery_PeriodisedValues pv 
		ON ld.Ukprn = pv.Ukprn
        AND ld.LearnRefNumber = pv.LearnRefNumber
        AND ld.AimSeqNumber = pv.AimSeqNumber
	INNER JOIN ${ILR_Deds.FQ}.Valid.Learner l
		ON l.Ukprn = ld.Ukprn
		AND l.LearnRefNumber = ld.LearnRefNumber
    WHERE ld.Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
GO
