TRUNCATE TABLE [Reference].[Providers]
GO

INSERT INTO [Reference].[Providers]
    SELECT
        [p].[UKPRN] AS [Ukprn],
		MAX([fd].[SubmittedTime]) AS [IlrSubmissionDateTime]
	FROM ${ILR_Deds.FQ}.[Valid].[LearningProvider] p
	INNER JOIN ${ILR_Deds.FQ}.[dbo].[FileDetails] fd
		ON p.UKPRN = fd.UKPRN
	GROUP BY
		[p].[UKPRN]
GO
