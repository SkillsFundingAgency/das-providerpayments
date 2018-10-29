DECLARE @CollectionPeriodName varchar(8) = (SELECT [AcademicYear] FROM Reference.CollectionPeriods WHERE [Open] = 1) + '-' + (SELECT [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1)


DELETE FROM ${DAS_PeriodEnd.FQ}.[Payments].[Payments]
    WHERE [CollectionPeriodName] = @CollectionPeriodName
        AND [FundingSource] = 5
GO
