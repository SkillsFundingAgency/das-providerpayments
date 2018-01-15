TRUNCATE TABLE [Reference].[Providers]
GO

INSERT INTO [Reference].[Providers] WITH (TABLOCKX) (
		[Ukprn],
		[IlrFilename],
		[IlrSubmissionDateTime]
	)
    SELECT
        [p].[UKPRN] AS [Ukprn],
		[fd].[Filename],
		[fd].[SubmittedTime]
	FROM ${ILR_Deds.FQ}.[Valid].[LearningProvider] p
		JOIN ${ILR_Deds.FQ}.[dbo].[FileDetails] fd
			ON p.UKPRN = fd.UKPRN
		JOIN (
			SELECT MAX(ID) AS ID FROM ${ILR_Deds.FQ}.[dbo].[FileDetails] GROUP BY UKPRN
		) LatestByUkprn
			ON fd.ID = LatestByUkprn.ID
	ORDER BY
		[p].[UKPRN]
GO
