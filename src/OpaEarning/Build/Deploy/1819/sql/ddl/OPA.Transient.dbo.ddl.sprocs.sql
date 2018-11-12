DECLARE @sql NVARCHAR(max)
DECLARE @schema NVARCHAR(100) = 'dbo'
SELECT @sql = COALESCE(@sql,'') + 'DROP PROCEDURE [' + @schema + '].' + QUOTENAME(ROUTINE_NAME) + ';' + CHAR(13)
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = @Schema
    AND ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_NAME
EXEC sp_executesql @sql
GO



CREATE PROCEDURE [dbo].[AddCategory]
	-- Add the parameters for the function here
	@CategoryName nvarchar(64),
	@LogID int
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @CatID INT
	SELECT @CatID = CategoryID FROM Category WHERE CategoryName = @CategoryName
	IF @CatID IS NULL
	BEGIN
		INSERT INTO Category (CategoryName) VALUES(@CategoryName)
		SELECT @CatID = @@IDENTITY
	END

	EXEC InsertCategoryLog @CatID, @LogID 

	RETURN @CatID
END


GO
/****** Object:  StoredProcedure [dbo].[CalculateESFFunding]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =========================================================
 
 Calculates and populated ESF Funding Summary Report

 for ILR Data ... separate function to populate SUPPDATA
 to enable splitting between Online and FIS

 ========================================================= */

CREATE PROCEDURE [dbo].[CalculateESFFunding]
AS 
	
	-- # Set up data #############################
	
	-- Get all current contracts
	DECLARE @contracts TABLE ([ContractSortOrder] INT IDENTITY(1,1), [ConRefNumber] varchar(20))
	
	INSERT INTO @contracts ([ConRefNumber])
	-- ILR Contracts
	SELECT vld.[ConRefNumber]
	FROM [Valid].[LearningDelivery] vld
	INNER JOIN Rulebase.ESF_LearningDeliveryDeliverable_PeriodisedValues pv 
		ON vld.LearnRefNumber = pv.LearnRefNumber
		AND vld.AimSeqNumber = pv.AimSeqNumber
	WHERE vld.[ConRefNumber] IS NOT NULL
	GROUP BY vld.[ConRefNumber]
	-- SUPP Data
	UNION SELECT ConRefNumber
	FROM [Reference].[ESF_SupplementaryData]
	--WHERE CalendarYear = 2016 AND CalendarMonth BETWEEN 1 AND 7
	WHERE CalendarYear = 2017 -- all of last year : Jan-2016 to Dec-2016
		OR (CalendarYear = 2018 AND CalendarMonth BETWEEN 1 AND 7) --Until July of current collection year : July-2017
	GROUP BY ConRefNumber
	ORDER BY [ConRefNumber]

	-- Ensure we have some data to send to the RDLC to generate the report
	-- If there are no non-null values
	IF (SELECT Count(*) FROM @contracts) = 0
	BEGIN
		INSERT INTO @contracts ([ConRefNumber])
		SELECT 'Not Applicable' -- Text suggested by BA, to be reviewed
	END

	DECLARE @ilr varchar(10) = 'ILR'
	DECLARE @suppdata varchar(10) = 'SUPPDATA'
	DECLARE @showAllValues varchar(10) = 'ALL'

	DECLARE @attributePatterns TABLE (AttributePatternId int PRIMARY KEY, DataSetType varchar(10), AttributePatternName varchar(50) )
	DECLARE @attributeNamePatterns TABLE (id int IDENTITY(1,1), AttributePatternId int, AttributeSortOrder int, AttributeName varchar(50), NameAppendText varchar(50))
	DECLARE @dataSets TABLE (DataSetSortOrder int PRIMARY KEY, DataSetName varchar(100))
	DECLARE @progressionSuperGroups TABLE (SuperGroupSortOrder int PRIMARY KEY, SuperGroupSubTotalName varchar(150))
	DECLARE @progressionGroups TABLE (GroupSortOrder int PRIMARY KEY, DataSetSortOrder int, AttributePatternId int, GroupSubTotalName varchar(150),ShowGroupSubTotal bit)	
	DECLARE @progressionNames TABLE (NameSortOrder int IDENTITY(1001,1) PRIMARY KEY, GroupSortOrder int, DeliverableCode varchar(20), Name varchar(150),SuperGroupSortOrder int NULL)
	-- Starting identity at 1001 to avoid issues with alpha sorting of SortBy fields in the RDLC
		
	INSERT INTO @attributePatterns (AttributePatternId, DataSetType, AttributePatternName)
	SELECT 1, @ilr, 'All four attributes - Sections 1, 4 and 5' UNION ALL
	SELECT 2, @ilr, 'Two attributes split - Sections 2.1 and 3.1' UNION ALL
	SELECT 3, @suppdata, 'all reference types - Sections 2.2 and 3.2' UNION ALL -- and 5.2,5.4,5.6,5.8,5.10
	SELECT 4, @suppdata, 'two reference types - Sections 6, 7 and 8' 

	INSERT INTO @attributeNamePatterns (AttributePatternId, AttributeSortOrder, AttributeName, NameAppendText)
	-- ILR These four merged together and show as one row in report
	SELECT 1, 0, 'StartEarnings', '' UNION ALL
	SELECT 1, 0, 'AchievementEarnings', '' UNION ALL
	SELECT 1, 0, 'AdditionalProgCostEarnings', '' UNION ALL
	SELECT 1, 0, 'ProgressionEarnings', '' UNION ALL
	-- ILR These two show as separate rows in report
	SELECT 2, 1, 'StartEarnings', ' - Start Funding' UNION ALL
	SELECT 2, 2, 'AchievementEarnings', ' - Achievement Funding' UNION ALL

	-- SUPPDATA This shows as one row for all reference types
	-- TODO get this to work
	SELECT 3, 0, @showAllValues, '' UNION ALL
	-- SUPPDATA These two show as separate rows in report
	SELECT 4, 1, 'Audit Adjustment', ' Audit Adjustments' UNION ALL
	SELECT 4, 2, 'Authorised Claims', ' Authorised Claims'


	INSERT INTO @dataSets (DataSetSortOrder, DataSetName)
	SELECT 1, 'Learner Assessment and Plan' UNION ALL
	SELECT 2, 'Regulated Learning' UNION ALL
	SELECT 3, 'Non Regulated Activity' UNION ALL
	SELECT 4, 'Additional Programme Cost' UNION ALL
	SELECT 5, 'Progression and Sustained Progression'  UNION ALL
	SELECT 6, 'Actual Costs' UNION ALL
	SELECT 7, 'Community Grant' UNION ALL
	SELECT 8, 'Specification Defined'

	INSERT INTO @progressionSuperGroups (SuperGroupSortOrder,SuperGroupSubTotalName)
	SELECT 1, 'Paid Employment Progression' UNION ALL  --Sum of this Super group as 'Total Paid Employment Progression (£)'
	SELECT 2, 'Unpaid Employment Progression' UNION ALL --Sum of this Super group as 'Total Unpaid Employment Progression (£)'
	SELECT 3, 'Education Progression' UNION ALL  --Sum of this Super group as 'Total Education Progression (£)'
	SELECT 4, 'Apprenticeship Progression' UNION ALL  --Sum of this Super group as 'Total Apprenticeship Progression (£)'
	SELECT 5, 'Traineeship Progression' UNION ALL  --Sum of this Super group as 'Total Traineeship Progression (£)'
	SELECT 6, 'Job Search Progression'  --Sum of this Super group as 'Total Job Search Progression (£)'
	

	INSERT INTO @progressionGroups (GroupSortOrder, DataSetSortOrder, AttributePatternId, GroupSubTotalName,ShowGroupSubTotal)
	SELECT 101, 1, 1, 'Learner Assessment and Plan',0 UNION ALL
	SELECT 102, 1, 3, 'Learner Assessment and Plan Adjustments',0 UNION ALL-- SUPPDATA Group - New	
	
	SELECT 201, 2, 2, 'RQ01 Regulated Learning',1 UNION ALL
	SELECT 202, 2, 4, 'RQ01 Regulated Learning',1 UNION ALL
	 
	SELECT 301, 3, 2, 'NR01 Non Regulated Activity',1 UNION ALL
	SELECT 302, 3, 4, 'NR01 Non Regulated Activity',1 UNION ALL
	
	SELECT 401, 4, 1, 'Additional Programme Cost',0 UNION ALL
	SELECT 402, 4, 3, 'Additional Programme Cost',0 UNION ALL-- SUPPDATA Group - New	
	 
	SELECT 501, 5, 1, 'Paid Employment Progression',1 UNION ALL
	SELECT 502, 5, 3, 'Paid Employment Progression Adjustments',1 UNION ALL-- SUPPDATA Group - New	
	SELECT 503, 5, 1, 'Unpaid Employment Progression',1 UNION ALL
	SELECT 504, 5, 3, 'Unpaid Employment Progression Adjustments',1 UNION ALL-- SUPPDATA Group - New
	SELECT 505, 5, 1, 'Education Progression',1 UNION ALL
	SELECT 506, 5, 3, 'Education Progression Adjustments',1 UNION ALL-- SUPPDATA Group - New
	SELECT 507, 5, 1, 'Apprenticeship Progression',1 UNION ALL
	SELECT 508, 5, 3, 'Apprenticeship Progression Adjustments',1 UNION ALL-- SUPPDATA Group - New	
	SELECT 509, 5, 1, 'Traineeship Progression',1 UNION ALL
	SELECT 510, 5, 3, 'Traineeship Progression Adjustments',1 UNION ALL-- SUPPDATA Group - New	
	SELECT 511, 5, 1, 'Job Search Progression',0 UNION ALL
	SELECT 512, 5, 3, 'Job Search Progression Adjustments',0 UNION ALL
	
	SELECT 601, 6, 3, 'Actual Costs',1 UNION ALL
	 
	SELECT 701, 7, 3, 'Community Grant',1 UNION ALL
	
	SELECT 801, 8, 3, 'Specification Defined',1

	
	INSERT INTO @progressionNames (GroupSortOrder, DeliverableCode, Name)
	
	SELECT 101, 'ST01', 'Learner Assessment and Plan' UNION ALL
	SELECT 102, 'ST01', 'Learner Assessment and Plan Adjustments' UNION ALL -- SUPPDATA - New

	SELECT 201, 'RQ01', 'Regulated Learning' UNION ALL -- ILR
	SELECT 202, 'RQ01', 'Regulated Learning' UNION ALL -- SUPPDATA 

	SELECT 301, 'NR01', 'Non Regulated Activity' UNION ALL -- ILR
	SELECT 302, 'NR01', 'Non Regulated Activity' UNION ALL -- SUPPDATA

	SELECT 401, 'FS01', 'Additional Programme Cost' UNION ALL
	SELECT 402, 'FS01', 'Additional Programme Cost Adjustments'  -- SUPPDATA - New

	INSERT INTO @progressionNames (GroupSortOrder, DeliverableCode, Name, SuperGroupSortOrder)
	SELECT 501, 'PG01', 'Progression Paid Employment', SuperGroupSortOrder=1 UNION ALL
	SELECT 501, 'SU01', 'Sustained Paid Employment 3 months', SuperGroupSortOrder=1 UNION ALL
	SELECT 501, 'SU11', 'Sustained Paid Employment 6 months', SuperGroupSortOrder=1 UNION ALL
	SELECT 501, 'SU21', 'Sustained Paid Employment 12 months', SuperGroupSortOrder=1 UNION ALL

	SELECT 502, 'PG01', 'Progression Paid Employment Adjustments', SuperGroupSortOrder=1 UNION ALL -- SUPPDATA - New
	SELECT 502, 'SU01', 'Sustained Paid Employment 3 months Adjustments', SuperGroupSortOrder=1 UNION ALL -- SUPPDATA - New
	SELECT 502, 'SU11', 'Sustained Paid Employment 6 months Adjustments', SuperGroupSortOrder=1 UNION ALL -- SUPPDATA - New
	SELECT 502, 'SU21', 'Sustained Paid Employment 12 months Adjustments', SuperGroupSortOrder=1 UNION ALL -- SUPPDATA - New

	SELECT 503, 'PG02', 'Progression Unpaid Employment', SuperGroupSortOrder=2 UNION ALL
	SELECT 503, 'SU02', 'Sustained Unpaid Employment 3 months', SuperGroupSortOrder=2 UNION ALL
	SELECT 503, 'SU12', 'Sustained Unpaid Employment 6 months', SuperGroupSortOrder=2 UNION ALL
	SELECT 503, 'SU22', 'Sustained Unpaid Employment 12 months', SuperGroupSortOrder=2 UNION ALL

	SELECT 504, 'PG02', 'Progression Unpaid Employment Adjustments', SuperGroupSortOrder=2 UNION ALL -- SUPPDATA - New
	SELECT 504, 'SU02', 'Sustained Unpaid Employment 3 months Adjustments', SuperGroupSortOrder=2 UNION ALL -- SUPPDATA - New
	SELECT 504, 'SU12', 'Sustained Unpaid Employment 6 months Adjustments', SuperGroupSortOrder=2 UNION ALL -- SUPPDATA - New
	SELECT 504, 'SU22', 'Sustained Unpaid Employment 12 months Adjustments', SuperGroupSortOrder=2 UNION ALL -- SUPPDATA - New

	SELECT 505, 'PG03', 'Progression Education', SuperGroupSortOrder=3 UNION ALL
	SELECT 505, 'SU03', 'Sustained Education 3 months', SuperGroupSortOrder=3 UNION ALL
	SELECT 505, 'SU13', 'Sustained Education 6 months', SuperGroupSortOrder=3 UNION ALL
	SELECT 505, 'SU23', 'Sustained Education 12 months', SuperGroupSortOrder=3 UNION ALL

	SELECT 506, 'PG03', 'Progression Education Adjustments', SuperGroupSortOrder=3 UNION ALL -- SUPPDATA - New
	SELECT 506, 'SU03', 'Sustained Education 3 months Adjustments', SuperGroupSortOrder=3 UNION ALL -- SUPPDATA - New
	SELECT 506, 'SU13', 'Sustained Education 6 months Adjustments', SuperGroupSortOrder=3 UNION ALL -- SUPPDATA - New
	SELECT 506, 'SU23', 'Sustained Education 12 months Adjustments', SuperGroupSortOrder=3 UNION ALL -- SUPPDATA - New

	SELECT 507, 'PG04', 'Progression Apprenticeship', SuperGroupSortOrder=4 UNION ALL
	SELECT 507, 'SU04', 'Sustained Apprenticeship 3 months', SuperGroupSortOrder=4 UNION ALL
	SELECT 507, 'SU14', 'Sustained Apprenticeship 6 months', SuperGroupSortOrder=4 UNION ALL
	SELECT 507, 'SU24', 'Sustained Apprenticeship 12 months', SuperGroupSortOrder=4 UNION ALL

	SELECT 508, 'PG04', 'Progression Apprenticeship Adjustments', SuperGroupSortOrder=4 UNION ALL -- SUPPDATA - New
	SELECT 508, 'SU04', 'Sustained Apprenticeship 3 months Adjustments', SuperGroupSortOrder=4 UNION ALL -- SUPPDATA - New
	SELECT 508, 'SU14', 'Sustained Apprenticeship 6 months Adjustments', SuperGroupSortOrder=4 UNION ALL -- SUPPDATA - New
	SELECT 508, 'SU24', 'Sustained Apprenticeship 12 months Adjustments', SuperGroupSortOrder=4 UNION ALL -- SUPPDATA - New

	SELECT 509, 'PG05', 'Progression Traineeship', SuperGroupSortOrder=5 UNION ALL
	SELECT 509, 'SU05', 'Sustained Traineeship 3 months', SuperGroupSortOrder=5 UNION ALL
	SELECT 509, 'SU15', 'Sustained Traineeship 6 months', SuperGroupSortOrder=5 UNION ALL

	SELECT 510, 'PG05', 'Progression Traineeship Adjustments', SuperGroupSortOrder=5 UNION ALL -- SUPPDATA - New
	SELECT 510, 'SU05', 'Sustained Traineeship 3 months Adjustments', SuperGroupSortOrder=5 UNION ALL -- SUPPDATA - New
	SELECT 510, 'SU15', 'Sustained Traineeship 6 months Adjustments', SuperGroupSortOrder=5 UNION ALL -- SUPPDATA - New

	SELECT 511, 'PG06', 'Progression Job Search', SuperGroupSortOrder=6 UNION ALL
	SELECT 512, 'PG06', 'Progression Job Search Adjustments', SuperGroupSortOrder=6  -- SUPPDATA - New

	INSERT INTO @progressionNames (GroupSortOrder, DeliverableCode, Name)
	SELECT 601, 'AC01', 'Actual Costs'  UNION ALL -- SUPPDATA

	SELECT 701, 'CG01', 'Community Grant Payment'  UNION ALL
	SELECT 701, 'CG02', 'Community Grant Management Cost'

	
	-- SETUP Temporary Tables
		-- Generate values for items SD01 - SD10
	IF OBJECT_ID('tempdb..#oneTOtenWithLeadingZero') IS NOT NULL
		DROP TABLE #oneTOtenWithLeadingZero

	SELECT N=number, A=RIGHT('0' + CAST(number AS varchar(2)), 2) 
	INTO #oneTOtenWithLeadingZero
	FROM master..spt_values WHERE [type] = 'P' 
	AND number 
	BETWEEN  1 AND 10

	INSERT INTO @progressionNames (GroupSortOrder, DeliverableCode, Name)
	SELECT 801, 'SD' + A, '' -- Rule: If no data then default to empty string for description
	FROM #oneTOtenWithLeadingZero

	-- Sanity check to ensure all rows show in report
	If (SELECT Count(*) FROM @progressionNames) <> (SELECT COUNT(*) FROM @progressionNames pn INNER JOIN @progressionGroups pg ON pn.GroupSortOrder = pg.GroupSortOrder)
	BEGIN
		THROW 51000, 'CalculateESFFunding stored procedure: Progression name check failed. Likely a GroupSortOrder Id is wrong. Throwing an error to avoid missing rows in output report.', 1;
	END

	IF OBJECT_ID('tempdb..#reportNames') IS NOT NULL
		DROP TABLE #reportNames

	SELECT 
		ap.DataSetType, ds.DataSetName, pg.GroupSubTotalName,
		psg.SuperGroupSubTotalName,
		pn.DeliverableCode, 
		pn.Name + anp.NameAppendText AS [Name] ,
		anp.AttributeName ,
		c2.ConRefNumber ,
		pn.NameSortOrder + anp.AttributeSortOrder AS [NameSortOrder], pn.GroupSortOrder, ds.DataSetSortOrder, c2.ContractSortOrder,
		psg.SuperGroupSortOrder,
		pg.ShowGroupSubTotal
	INTO #reportNames
	FROM @contracts c2, @progressionNames pn
	INNER JOIN @progressionGroups pg ON pn.GroupSortOrder = pg.GroupSortOrder
	LEFT JOIN @progressionSuperGroups psg ON pn.SuperGroupSortOrder = psg.SuperGroupSortOrder
	INNER JOIN @dataSets ds ON ds.DataSetSortOrder = pg.DataSetSortOrder
	INNER JOIN @attributePatterns ap 
		ON ap.AttributePatternId = pg.AttributePatternId
		--AND ap.DataSetType = @suppdata | @ilr
	INNER JOIN @attributeNamePatterns anp ON anp.AttributePatternId = ap.AttributePatternId

--------------------------Supplementary Data 1516, 1617, 1718 & 1819------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#SUPP_Data') IS NOT NULL
		DROP TABLE #SUPP_Data

	SELECT 
		r.DeliverableCode,
		r.Name + CASE WHEN r.DeliverableCode IN ('SD01','SD02','SD03','SD04','SD05','SD06','SD07','SD08','SD09','SD10') 
						THEN ISNULL(fcsdd.DeliverableDescription, '') 
						ELSE '' END AS [Name],
		r.NameSortOrder, r.GroupSortOrder, r.ContractSortOrder, r.SuperGroupSortOrder,
		r.ConRefNumber AS [ContractRefNumber] , 
						
		[Period_6_1516]
      ,[Period_7_1516]
      ,[Period_8_1516]
      ,[Period_9_1516]
      ,[Period_10_1516]
      ,[Period_11_1516]
      ,[Period_12_1516]

      ,[Period_1_1617]
      ,[Period_2_1617]
      ,[Period_3_1617]
      ,[Period_4_1617]
      ,[Period_5_1617]
      ,[Period_6_1617]
      ,[Period_7_1617]
      ,[Period_8_1617]
      ,[Period_9_1617]
      ,[Period_10_1617]
      ,[Period_11_1617]
      ,[Period_12_1617]

      ,[Period_1_1718]
      ,[Period_2_1718]
      ,[Period_3_1718]
      ,[Period_4_1718]
      ,[Period_5_1718]
      ,[Period_6_1718]
      ,[Period_7_1718]
      ,[Period_8_1718]
      ,[Period_9_1718]
      ,[Period_10_1718]
      ,[Period_11_1718]
      ,[Period_12_1718]		

	  ,[Period_1_1819]
      ,[Period_2_1819]
      ,[Period_3_1819]
      ,[Period_4_1819]
      ,[Period_5_1819]
      ,[Period_6_1819]
      ,[Period_7_1819]
      ,[Period_8_1819]	 
	INTO #SUPP_Data
	FROM #reportNames r
	LEFT OUTER JOIN [dbo].[ESFFundingSummaryReportSupplementaryDataPivot] v
		ON r.ConRefNumber = v.ContractRefNumber
		AND r.DeliverableCode = v.DeliverableCode
		AND (r.AttributeName = @showAllValues OR r.AttributeName = v.ReferenceType)
	LEFT OUTER JOIN [Reference].[FCS_Deliverable_Description] fcsdd 
		ON r.DeliverableCode = fcsdd.DeliverableCode
		AND r.ConRefNumber = fcsdd.ContractAllocationNumber
	WHERE r.DataSetType = @suppdata


--------------------------1516 Data------------------------------------------------------------------------------------------
	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1516') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1516

	SELECT 
		r.DeliverableCode,r.Name,r.[ConRefNumber] AS [ContractRefNumber],
		r.NameSortOrder, r.GroupSortOrder, r.ContractSortOrder,	r.SuperGroupSortOrder,
		ISNULL(Period_6,CAST(0.00 AS [decimal](15, 5))) AS [Period_6_1516], --Jan 2016
		ISNULL(Period_7,CAST(0.00 AS [decimal](15, 5))) AS [Period_7_1516],
		ISNULL(Period_8,CAST(0.00 AS [decimal](15, 5))) AS [Period_8_1516],
		ISNULL(Period_9,CAST(0.00 AS [decimal](15, 5))) AS [Period_9_1516],
		ISNULL(Period_10,CAST(0.00 AS [decimal](15, 5))) AS [Period_10_1516],
		ISNULL(Period_11,CAST(0.00 AS [decimal](15, 5))) AS [Period_11_1516],
		ISNULL(Period_12,CAST(0.00 AS [decimal](15, 5))) AS [Period_12_1516] --Jul 2016
	INTO #ILR_Data_Prev_1516
	FROM #reportNames r
	LEFT OUTER JOIN [Reference].[ESF_FundingData] fd 	
	ON r.ConRefNumber = fd.ConRefNumber
	and r.DeliverableCode = fd.DeliverableCode
	and r.AttributeName = fd.AttributeName
	AND fd.AcademicYear ='2015/16'

--------------------------1617 Data------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1617') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1617

	SELECT 
		r.DeliverableCode,r.Name,r.[ConRefNumber] AS [ContractRefNumber],
		r.NameSortOrder, r.GroupSortOrder, r.ContractSortOrder,	r.SuperGroupSortOrder,

		ISNULL(Period_1,CAST(0.00 AS [decimal](15, 5))) AS [Period_1_1617], --Aug 2016
		ISNULL(Period_2,CAST(0.00 AS [decimal](15, 5))) AS [Period_2_1617],
		ISNULL(Period_3,CAST(0.00 AS [decimal](15, 5))) AS [Period_3_1617],
		ISNULL(Period_4,CAST(0.00 AS [decimal](15, 5))) AS [Period_4_1617],
		ISNULL(Period_5,CAST(0.00 AS [decimal](15, 5))) AS [Period_5_1617],
		ISNULL(Period_6,CAST(0.00 AS [decimal](15, 5))) AS [Period_6_1617], --Jan 2017
		ISNULL(Period_7,CAST(0.00 AS [decimal](15, 5))) AS [Period_7_1617],
		ISNULL(Period_8,CAST(0.00 AS [decimal](15, 5))) AS [Period_8_1617],
		ISNULL(Period_9,CAST(0.00 AS [decimal](15, 5))) AS [Period_9_1617],
		ISNULL(Period_10,CAST(0.00 AS [decimal](15, 5))) AS [Period_10_1617],
		ISNULL(Period_11,CAST(0.00 AS [decimal](15, 5))) AS [Period_11_1617],
		ISNULL(Period_12,CAST(0.00 AS [decimal](15, 5))) AS [Period_12_1617] --Jul 2017
	INTO #ILR_Data_Prev_1617
	FROM #reportNames r
	LEFT OUTER JOIN [Reference].[ESF_FundingData] fd 	
	ON r.ConRefNumber = fd.ConRefNumber
	and r.DeliverableCode = fd.DeliverableCode
	and r.AttributeName = fd.AttributeName
	AND fd.AcademicYear ='2016/17'

--------------------------1718 Data------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1718') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1718

	SELECT 
		r.DeliverableCode,r.Name,r.[ConRefNumber] AS [ContractRefNumber],
		r.NameSortOrder,r.GroupSortOrder, r.ContractSortOrder, r.SuperGroupSortOrder,		 			
		ISNULL(Period_1,CAST(0.00 AS [decimal](15, 5))) AS [Period_1_1718], --Aug 2017
		ISNULL(Period_2,CAST(0.00 AS [decimal](15, 5))) AS [Period_2_1718],
		ISNULL(Period_3,CAST(0.00 AS [decimal](15, 5))) AS [Period_3_1718],
		ISNULL(Period_4,CAST(0.00 AS [decimal](15, 5))) AS [Period_4_1718],
		ISNULL(Period_5,CAST(0.00 AS [decimal](15, 5))) AS [Period_5_1718],
		ISNULL(Period_6,CAST(0.00 AS [decimal](15, 5))) AS [Period_6_1718], --Jan 2018
		ISNULL(Period_7,CAST(0.00 AS [decimal](15, 5))) AS [Period_7_1718],
		ISNULL(Period_8,CAST(0.00 AS [decimal](15, 5))) AS [Period_8_1718],
		ISNULL(Period_9,CAST(0.00 AS [decimal](15, 5))) AS [Period_9_1718],
		ISNULL(Period_10,CAST(0.00 AS [decimal](15, 5))) AS [Period_10_1718],
		ISNULL(Period_11,CAST(0.00 AS [decimal](15, 5))) AS [Period_11_1718],
		ISNULL(Period_12,CAST(0.00 AS [decimal](15, 5))) AS [Period_12_1718] --Jul 2018
	INTO #ILR_Data_Prev_1718
	FROM #reportNames r
	LEFT OUTER JOIN [Reference].[ESF_FundingData] fd 	
	ON r.ConRefNumber = fd.ConRefNumber
	and r.DeliverableCode = fd.DeliverableCode
	and r.AttributeName = fd.AttributeName
	AND fd.AcademicYear ='2017/18'

--------------------------1819 Data------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#ILR_Data') IS NOT NULL
		DROP TABLE #ILR_Data

	SELECT 
		r.DeliverableCode,r.Name,r.[ConRefNumber] AS [ContractRefNumber],
		r.NameSortOrder,r.GroupSortOrder, r.ContractSortOrder, r.SuperGroupSortOrder,		 			
		pv.Period_1 AS [Period_1_1819],						-- Aug 2018 
		pv.Period_2 AS [Period_2_1819],
		pv.Period_3 AS [Period_3_1819],
		pv.Period_4 AS [Period_4_1819],
		pv.Period_5 AS [Period_5_1819],			
		pv.Period_6 AS [Period_6_1819],						-- Jan 2019
		pv.Period_7 AS [Period_7_1819],
		pv.Period_8 AS [Period_8_1819]		
	INTO #ILR_Data
	FROM #reportNames r
	LEFT OUTER JOIN Valid.LearningDelivery ld
		ON r.ConRefNumber = ld.ConRefNumber
	LEFT OUTER JOIN Rulebase.ESF_LearningDeliveryDeliverable_PeriodisedValues pv 
		ON ld.LearnRefNumber = pv.LearnRefNumber
		AND ld.AimSeqNumber = pv.AimSeqNumber
		AND r.DeliverableCode = pv.DeliverableCode
		AND r.AttributeName = pv.AttributeName		
	WHERE r.DataSetType = @ilr

--------------------------1516 Data Totals------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1516_Totals') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1516_Totals

	SELECT 
		[DeliverableCode],[Name],[ContractRefNumber],
		[NameSortOrder],[GroupSortOrder],[ContractSortOrder], [SuperGroupSortOrder],			  
		Sum(ISNULL(Period_6_1516, 0))	AS [FundCalcJan16], 
		Sum(ISNULL(Period_7_1516, 0))	AS [FundCalcFeb16], 
		Sum(ISNULL(Period_8_1516, 0))	AS [FundCalcMar16], 
		Sum(ISNULL(Period_9_1516, 0))	AS [FundCalcApr16], 
		Sum(ISNULL(Period_10_1516, 0))	AS [FundCalcMay16], 
		Sum(ISNULL(Period_11_1516, 0))	AS [FundCalcJun16], 
		Sum(ISNULL(Period_12_1516, 0))	AS [FundCalcJul16]
	INTO #ILR_Data_Prev_1516_Totals
	FROM #ILR_Data_Prev_1516		
	GROUP BY DeliverableCode, Name, NameSortOrder, SuperGroupSortOrder, GroupSortOrder, ContractSortOrder, ContractRefNumber

--------------------------1617 Data Totals------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1617_Totals') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1617_Totals

	SELECT 
		[DeliverableCode],[Name],[ContractRefNumber],
		[NameSortOrder],[GroupSortOrder],[ContractSortOrder], [SuperGroupSortOrder],			  
		Sum(ISNULL(Period_1_1617, 0))		AS [FundCalcAug16], 
		Sum(ISNULL(Period_2_1617, 0))		AS [FundCalcSep16], 
		Sum(ISNULL(Period_3_1617, 0))		AS [FundCalcOct16], 
		Sum(ISNULL(Period_4_1617, 0))		AS [FundCalcNov16], 
		Sum(ISNULL(Period_5_1617, 0))		AS [FundCalcDec16], 
		Sum(ISNULL(Period_6_1617, 0))		AS [FundCalcJan17], 
		Sum(ISNULL(Period_7_1617, 0))		AS [FundCalcFeb17], 
		Sum(ISNULL(Period_8_1617, 0))		AS [FundCalcMar17], 
		Sum(ISNULL(Period_9_1617, 0))		AS [FundCalcApr17] , 
		Sum(ISNULL(Period_10_1617, 0))		AS [FundCalcMay17], 
		Sum(ISNULL(Period_11_1617, 0))		AS [FundCalcJun17] , 
		Sum(ISNULL(Period_12_1617, 0))		AS [FundCalcJul17] 
	INTO #ILR_Data_Prev_1617_Totals
	FROM #ILR_Data_Prev_1617		
	GROUP BY DeliverableCode, Name, NameSortOrder, SuperGroupSortOrder, GroupSortOrder, ContractSortOrder, ContractRefNumber

--------------------------1718 Data Totals------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1718_Totals') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1718_Totals

	SELECT 
		[DeliverableCode], [Name], [ContractRefNumber],
		[NameSortOrder], [GroupSortOrder], [ContractSortOrder], [SuperGroupSortOrder],
		Sum(ISNULL(Period_1_1718, 0))		AS [FundCalcAug17], 
		Sum(ISNULL(Period_2_1718, 0))		AS [FundCalcSep17], 
		Sum(ISNULL(Period_3_1718, 0))		AS [FundCalcOct17], 
		Sum(ISNULL(Period_4_1718, 0))		AS [FundCalcNov17], 
		Sum(ISNULL(Period_5_1718, 0))		AS [FundCalcDec17], 
		Sum(ISNULL(Period_6_1718, 0))		AS [FundCalcJan18], 
		Sum(ISNULL(Period_7_1718, 0))		AS [FundCalcFeb18], 
		Sum(ISNULL(Period_8_1718, 0))		AS [FundCalcMar18], 
		Sum(ISNULL(Period_9_1718, 0))		AS [FundCalcApr18] , 
		Sum(ISNULL(Period_10_1718, 0))		AS [FundCalcMay18], 
		Sum(ISNULL(Period_11_1718, 0))		AS [FundCalcJun18] , 
		Sum(ISNULL(Period_12_1718, 0))		AS [FundCalcJul18] 
	INTO #ILR_Data_Prev_1718_Totals
	FROM #ILR_Data_Prev_1718 
	GROUP BY DeliverableCode, Name, NameSortOrder, SuperGroupSortOrder, GroupSortOrder, ContractSortOrder, ContractRefNumber

--------------------------1819 Data Totals------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#ILR_Data_Totals') IS NOT NULL
		DROP TABLE #ILR_Data_Totals

	SELECT 
		[DeliverableCode], [Name], [ContractRefNumber],
		[NameSortOrder], [GroupSortOrder], [ContractSortOrder], [SuperGroupSortOrder],
		Sum(ISNULL(Period_1_1819, 0))		AS [FundCalcAug18], 
		Sum(ISNULL(Period_2_1819, 0))		AS [FundCalcSep18], 
		Sum(ISNULL(Period_3_1819, 0))		AS [FundCalcOct18], 
		Sum(ISNULL(Period_4_1819, 0))		AS [FundCalcNov18], 
		Sum(ISNULL(Period_5_1819, 0))		AS [FundCalcDec18], 
		Sum(ISNULL(Period_6_1819, 0))		AS [FundCalcJan19], 
		Sum(ISNULL(Period_7_1819, 0))		AS [FundCalcFeb19], 
		Sum(ISNULL(Period_8_1819, 0))		AS [FundCalcMar19]  
	INTO #ILR_Data_Totals
	FROM #ILR_Data 
	GROUP BY DeliverableCode, Name, NameSortOrder, SuperGroupSortOrder, GroupSortOrder, ContractSortOrder, ContractRefNumber

--------------------------Data Totals------------------------------------------------------------------------------------------

	IF OBJECT_ID('tempdb..#totals') IS NOT NULL
		DROP TABLE #totals

		SELECT  
			t.[DeliverableCode], t.[Name], t.[ContractRefNumber],
			t.[NameSortOrder], t.[GroupSortOrder], t.[ContractSortOrder], t.[SuperGroupSortOrder],		
			
			[FundCalcJan16], ----1516 Data----
			[FundCalcFeb16], 
			[FundCalcMar16], 
			[FundCalcApr16], 
			[FundCalcMay16], 
			[FundCalcJun16], 
			[FundCalcJul16],

			[FundCalcAug16], ----1617 Data----
			[FundCalcSep16], 
			[FundCalcOct16], 
			[FundCalcNov16], 
			[FundCalcDec16], 
			[FundCalcJan17], 
			[FundCalcFeb17], 
			[FundCalcMar17], 
			[FundCalcApr17], 
			[FundCalcMay17], 
			[FundCalcJun17], 
			[FundCalcJul17],

			[FundCalcAug17], ----1718s Data----
			[FundCalcSep17], 
			[FundCalcOct17], 
			[FundCalcNov17], 
			[FundCalcDec17], 
			[FundCalcJan18], 
			[FundCalcFeb18], 
			[FundCalcMar18], 
			[FundCalcApr18] , 
			[FundCalcMay18], 
			[FundCalcJun18] , 
			[FundCalcJul18],

			[FundCalcAug18], ----1819s Data----
			[FundCalcSep18], 
			[FundCalcOct18], 
			[FundCalcNov18], 
			[FundCalcDec18], 
			[FundCalcJan19], 
			[FundCalcFeb19], 
			[FundCalcMar19] 
		INTO #totals
		FROM #ILR_Data_Prev_1516_Totals AS pT
		JOIN  #ILR_Data_Totals as t 
				ON pT.DeliverableCode = t.DeliverableCode
				AND pT.Name = t.Name
				AND pT.NameSortOrder = t.NameSortOrder
				AND pT.GroupSortOrder = t.GroupSortOrder
				AND pT.ContractSortOrder = t.ContractSortOrder
				AND pT.ContractRefNumber = t.ContractRefNumber		
		JOIN #ILR_Data_Prev_1617_Totals AS pT1617
				ON pT1617.DeliverableCode = t.DeliverableCode
				AND pT1617.Name = t.Name
				AND pT1617.NameSortOrder = t.NameSortOrder
				AND pT1617.GroupSortOrder = t.GroupSortOrder
				AND pT1617.ContractSortOrder = t.ContractSortOrder
				AND pT1617.ContractRefNumber = t.ContractRefNumber		
        JOIN #ILR_Data_Prev_1718_Totals AS pT1718
				ON pT1718.DeliverableCode = t.DeliverableCode
				AND pT1718.Name = t.Name
				AND pT1718.NameSortOrder = t.NameSortOrder
				AND pT1718.GroupSortOrder = t.GroupSortOrder
				AND pT1718.ContractSortOrder = t.ContractSortOrder
				AND pT1718.ContractRefNumber = t.ContractRefNumber			

		-- SUPP data ------------------------------------------
		UNION ALL SELECT 
			DeliverableCode, Name, ContractRefNumber,
			NameSortOrder, GroupSortOrder, ContractSortOrder, [SuperGroupSortOrder],
						
			Sum(ISNULL(Period_6_1516, 0))		AS [FundCalcJan16], ----1516 Supp Data----
			Sum(ISNULL(Period_7_1516, 0))		AS [FundCalcFeb16], 
			Sum(ISNULL(Period_8_1516, 0))		AS [FundCalcMar16], 
			Sum(ISNULL(Period_9_1516, 0))		AS [FundCalcApr16], 
			Sum(ISNULL(Period_10_1516, 0))		AS [FundCalcMay16], 
			Sum(ISNULL(Period_11_1516, 0))		AS [FundCalcJun16], 
			Sum(ISNULL(Period_12_1516, 0))		AS [FundCalcJul16],
						
			Sum(ISNULL(Period_1_1617, 0))		AS [FundCalcAug16], ----1617 Supp Data----
			Sum(ISNULL(Period_2_1617, 0))		AS [FundCalcSep16], 
			Sum(ISNULL(Period_3_1617, 0))		AS [FundCalcOct16], 
			Sum(ISNULL(Period_4_1617, 0))		AS [FundCalcNov16], 
			Sum(ISNULL(Period_5_1617, 0))		AS [FundCalcDec16], 
			Sum(ISNULL(Period_6_1617, 0))		AS [FundCalcJan17], 
			Sum(ISNULL(Period_7_1617, 0))		AS [FundCalcFeb17], 
			Sum(ISNULL(Period_8_1617, 0))		AS [FundCalcMar17], 
			Sum(ISNULL(Period_9_1617, 0))		AS [FundCalcApr17], 
			Sum(ISNULL(Period_10_1617, 0))		AS [FundCalcMay17], 
			Sum(ISNULL(Period_11_1617, 0))		AS [FundCalcJun17], 
			Sum(ISNULL(Period_12_1617, 0))		AS [FundCalcJul17], 

			Sum(ISNULL(Period_1_1718, 0))		AS [FundCalcAug17], ----1718 Supp Data----
			Sum(ISNULL(Period_2_1718, 0))		AS [FundCalcSep17], 
			Sum(ISNULL(Period_3_1718, 0))		AS [FundCalcOct17], 
			Sum(ISNULL(Period_4_1718, 0))		AS [FundCalcNov17], 
			Sum(ISNULL(Period_5_1718, 0))		AS [FundCalcDec17], 
			Sum(ISNULL(Period_6_1718, 0))		AS [FundCalcJan18], 
			Sum(ISNULL(Period_7_1718, 0))		AS [FundCalcFeb18], 
			Sum(ISNULL(Period_8_1718, 0))		AS [FundCalcMar18], 
			Sum(ISNULL(Period_9_1718, 0))		AS [FundCalcApr18], 
			Sum(ISNULL(Period_10_1718, 0))		AS [FundCalcMay18], 
			Sum(ISNULL(Period_11_1718, 0))		AS [FundCalcJun18], 
			Sum(ISNULL(Period_12_1718, 0))		AS [FundCalcJul18], 

			Sum(ISNULL(Period_1_1819, 0))		AS [FundCalcAug18], ----1819 Supp Data----
			Sum(ISNULL(Period_2_1819, 0))		AS [FundCalcSep18], 
			Sum(ISNULL(Period_3_1819, 0))		AS [FundCalcOct18], 
			Sum(ISNULL(Period_4_1819, 0))		AS [FundCalcNov18], 
			Sum(ISNULL(Period_5_1819, 0))		AS [FundCalcDec18], 
			Sum(ISNULL(Period_6_1819, 0))		AS [FundCalcJan19], 
			Sum(ISNULL(Period_7_1819, 0))		AS [FundCalcFeb19], 
			Sum(ISNULL(Period_8_1819, 0))		AS [FundCalcMar19] 

		FROM #SUPP_Data 
		GROUP BY DeliverableCode, Name, NameSortOrder,SuperGroupSortOrder, GroupSortOrder, ContractSortOrder, ContractRefNumber
	-- End Setup Temporary Tables

	-- # Main Code #############################


	INSERT INTO [Report].[FundingSummary_ESF]
			   ([Online]
			   ,[FIS]
			   ,[ContractRefNumber]
			   ,[ContractSortOrder]
			   ,[DataSetName]
			   ,[DataSetSortOrder]
			   ,[SuperGroupName]
			   ,[SuperGroupSortOrder]
			   ,[GroupName]
			   ,[GroupSortOrder]
			   ,[ShowGroupSubTotal]
			   ,[Name]
			   ,[NameSortOrder]

			   ,[FundCalcJan16]
			   ,[FundCalcFeb16]
			   ,[FundCalcMar16]
			   ,[FundCalcApr16]
			   ,[FundCalcMay16]
			   ,[FundCalcJun16]
			   ,[FundCalcJul16]

			   ,[FundCalcAug16]
			   ,[FundCalcSep16]
			   ,[FundCalcOct16]
			   ,[FundCalcNov16]
			   ,[FundCalcDec16]
			   ,[FundCalcJan17]
			   ,[FundCalcFeb17]
			   ,[FundCalcMar17]
			   ,[FundCalcApr17]
			   ,[FundCalcMay17]
			   ,[FundCalcJun17]
			   ,[FundCalcJul17]

			   ,[FundCalcAug17]
			   ,[FundCalcSep17]
			   ,[FundCalcOct17]
			   ,[FundCalcNov17]
			   ,[FundCalcDec17]
			   ,[FundCalcJan18]
			   ,[FundCalcFeb18]
			   ,[FundCalcMar18]
			   ,[FundCalcApr18]
			   ,[FundCalcMay18]
			   ,[FundCalcJun18]
			   ,[FundCalcJul18]

			   ,[FundCalcAug18]
			   ,[FundCalcSep18]
			   ,[FundCalcOct18]
			   ,[FundCalcNov18]
			   ,[FundCalcDec18]
			   ,[FundCalcJan19]
			   ,[FundCalcFeb19]
			   ,[FundCalcMar19]

			   ,[FundCalcSubTotal_2015_2016]
			   ,[FundCalcSubTotal_2016_2017]
			   ,[FundCalcSubTotal_2017_2018]
			   ,[FundCalcSubTotal_2018_2019]
			   ,[FundCalcTotal])
	SELECT	
				1 AS [Online], 
				CASE WHEN ap.DataSetType = @suppdata THEN 0 ELSE 1 END AS [FIS] ,
				t.[ContractRefNumber], 
				t.[ContractSortOrder] , 
				d.DataSetName AS [DataSetName] , 
				pg.DataSetSortOrder , 
				[SuperGroupSubTotalName] AS [SuperGroupName],
				t.[SuperGroupSortOrder],
				RTRIM(ap.DataSetType + ' Total ' + ISNULL(pg.GroupSubTotalName, '')) AS [GroupName] , -- GroupSubTotalName may be empty string
				t.GroupSortOrder AS [GroupSortOrder],
				ShowGroupSubTotal, --Some Subtotals are not required to be shown eg: when there is only one item in the group as in the case of 'Learner Assessment and Plan' - ILR group and SUPP group has only one item each
				RTRIM(ap.DataSetType + ' ' + t.DeliverableCode + ' ' + t.Name) AS [Name] , 
				t.NameSortOrder AS [NameSortOrder],
				t.[FundCalcJan16], 
				t.[FundCalcFeb16], 
				t.[FundCalcMar16], 
				t.[FundCalcApr16], 
				t.[FundCalcMay16], 
				t.[FundCalcJun16], 
				t.[FundCalcJul16],

				t.[FundCalcAug16], 
				t.[FundCalcSep16], 
				t.[FundCalcOct16], 
				t.[FundCalcNov16], 
				t.[FundCalcDec16], 
				t.[FundCalcJan17], 
				t.[FundCalcFeb17], 
				t.[FundCalcMar17], 
				t.[FundCalcApr17], 
				t.[FundCalcMay17], 
				t.[FundCalcJun17], 
				t.[FundCalcJul17],

				t.[FundCalcAug17],
				t.[FundCalcSep17],
				t.[FundCalcOct17],
				t.[FundCalcNov17],
				t.[FundCalcDec17],
				t.[FundCalcJan18],
				t.[FundCalcFeb18],
				t.[FundCalcMar18],
				t.[FundCalcApr18],
				t.[FundCalcMay18],
				t.[FundCalcJun18],
				t.[FundCalcJul18],

				t.[FundCalcAug18],
				t.[FundCalcSep18],
				t.[FundCalcOct18],
				t.[FundCalcNov18],
				t.[FundCalcDec18],
				t.[FundCalcJan19],
				t.[FundCalcFeb19],
				t.[FundCalcMar19],
		
				t.[FundCalcJan16]+t.[FundCalcFeb16]+t.[FundCalcMar16]+t.[FundCalcApr16]+t.[FundCalcMay16]+t.[FundCalcJun16]+t.[FundCalcJul16] AS [FundCalcSubTotal_2015_2016],

				t.[FundCalcAug16]+t.[FundCalcSep16]+t.[FundCalcOct16]+t.[FundCalcNov16]+t.[FundCalcDec16]+
				t.[FundCalcJan17]+t.[FundCalcFeb17]+t.[FundCalcMar17]+t.[FundCalcApr17]+t.[FundCalcMay17]+t.[FundCalcJun17]+
				t.[FundCalcJul17] AS [FundCalcSubTotal_2016_2017],

				t.[FundCalcAug17]+t.[FundCalcSep17]+t.[FundCalcOct17]+t.[FundCalcNov17]+t.[FundCalcDec17]+
				t.[FundCalcJan18]+t.[FundCalcFeb18]+t.[FundCalcMar18]+t.[FundCalcApr18]+t.[FundCalcMay18]+t.[FundCalcJun18]+
				t.[FundCalcJul18] AS [FundCalcSubTotal_2017_2018],

				(t.[FundCalcAug18]+t.[FundCalcSep18]+t.[FundCalcOct18]+t.[FundCalcNov18]+t.[FundCalcDec18]+t.[FundCalcJan19]+t.[FundCalcFeb19]+t.[FundCalcMar19]) AS [FundCalcSubTotal_2018_2019],

				--Total of all 31 months
				t.[FundCalcJan16]+t.[FundCalcFeb16]+t.[FundCalcMar16]+t.[FundCalcApr16]+t.[FundCalcMay16]+t.[FundCalcJun16]+
				t.[FundCalcJul16]+
				
				t.[FundCalcAug16]+t.[FundCalcSep16]+t.[FundCalcOct16]+t.[FundCalcNov16]+t.[FundCalcDec16]+t.[FundCalcJan17]+
				t.[FundCalcFeb17]+t.[FundCalcMar17]+t.[FundCalcApr17]+t.[FundCalcMay17]+t.[FundCalcJun17]+t.[FundCalcJul17]+     				

				t.[FundCalcAug17]+t.[FundCalcSep17]+t.[FundCalcOct17]+t.[FundCalcNov17]+t.[FundCalcDec17]+t.[FundCalcJan18]+
				t.[FundCalcFeb18]+t.[FundCalcMar18]+t.[FundCalcApr18]+t.[FundCalcMay18]+t.[FundCalcJun18]+t.[FundCalcJul18]+

				t.[FundCalcAug18]+t.[FundCalcSep18]+t.[FundCalcOct18]+t.[FundCalcNov18]+t.[FundCalcDec18]+t.[FundCalcJan19]+
				t.[FundCalcFeb19]+t.[FundCalcMar19]
				     
				AS [FundCalcTotal]  --Grand Total

	FROM #totals t
	INNER JOIN @progressionGroups pg ON t.GroupSortOrder = pg.GroupSortOrder
	LEFT JOIN @progressionSuperGroups psg ON t.[SuperGroupSortOrder] = psg.[SuperGroupSortOrder]
	INNER JOIN @dataSets d ON pg.DataSetSortOrder = d.DataSetSortOrder
	INNER JOIN @attributePatterns ap ON pg.AttributePatternId = ap.AttributePatternId

	ORDER BY t.ContractRefNumber, t.NameSortOrder


	-- Drop Temporary Tables

	IF OBJECT_ID('tempdb..#oneTOtenWithLeadingZero') IS NOT NULL
		DROP TABLE #oneTOtenWithLeadingZero

	IF OBJECT_ID('tempdb..#reportNames') IS NOT NULL
		DROP TABLE #reportNames

	IF OBJECT_ID('tempdb..#SUPP_Data') IS NOT NULL
		DROP TABLE #SUPP_Data

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1516') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1516

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1516_Totals') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1516_Totals

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1617') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1617

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1617_Totals') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1617_Totals

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1718') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1718

	IF OBJECT_ID('tempdb..#ILR_Data_Prev_1718_Totals') IS NOT NULL
		DROP TABLE #ILR_Data_Prev_1718_Totals

	IF OBJECT_ID('tempdb..#ILR_Data') IS NOT NULL
		DROP TABLE #ILR_Data

	IF OBJECT_ID('tempdb..#ILR_Data_Totals') IS NOT NULL
		DROP TABLE #ILR_Data_Totals

	IF OBJECT_ID('tempdb..#totals') IS NOT NULL
		DROP TABLE #totals

	-- End Drop

-- #############################################################



GO
/****** Object:  StoredProcedure [dbo].[ClearLogs]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClearLogs]
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM CategoryLog
	DELETE FROM [Log]
    DELETE FROM Category
END

GO
/****** Object:  StoredProcedure [dbo].[ImportFilenames]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[ImportFilenames](@pipeSeperatedList  NVARCHAR(4000)) AS
BEGIN
	Declare @filename NVARCHAR(50) = NULL
	Declare @item NVARCHAR(50) = NULL
	WHILE LEN(@pipeSeperatedList) > 0
	BEGIN
		IF PATINDEX('%|%',@pipeSeperatedList) > 0
		BEGIN
			SET @filename = SUBSTRING(@pipeSeperatedList, 0, PATINDEX('%|%',@pipeSeperatedList))
			SET @pipeSeperatedList = SUBSTRING(@pipeSeperatedList, LEN(@filename + '|') + 1, LEN(@pipeSeperatedList))
		END
		ELSE
		BEGIN
			SET @filename = @pipeSeperatedList
			SET @pipeSeperatedList = NULL		
		END

		-- Import Filename
		IF (@filename IS NOT NULL)
		BEGIN			

			DECLARE @UKPRN NVARCHAR(8) = SUBSTRING(@filename, 5,8)
			DECLARE @Year NVARCHAR(4) = SUBSTRING(@filename, 14,4)
			DECLARE @DateYear NVARCHAR(4) = SUBSTRING(@filename, 19,4)
			DECLARE @DateMonth NVARCHAR(2) = SUBSTRING(@filename, 23,2)
			DECLARE @DateDay NVARCHAR(2) = SUBSTRING(@filename, 25, 2)
			DECLARE @DateHour NVARCHAR(2) = SUBSTRING(@filename, 28,2)
			DECLARE @DateMinute NVARCHAR(2) = SUBSTRING(@filename, 30, 2)
			DECLARE @DateSecond NVARCHAR(2) = SUBSTRING(@filename, 32, 2)
			DECLARE @SerialNumber NVARCHAR(2) = SUBSTRING(@filename, 35, 2)

			INSERT INTO [XML_FileNames] ([FileName], [FN03], [FN05], [FN06], [FN07]) VALUES 
			(
				@filename				
				,@UKPRN
				,@Year
				,@DateYear + '-' + @DateMonth + '-' + @DateDay  + ' ' + @DateHour + ':' + @DateMinute + ':' + @DateSecond
				,@SerialNumber
			)
						
		END	    
	END
END

GO
/****** Object:  StoredProcedure [dbo].[InsertCategoryLog]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertCategoryLog]
	@CategoryID INT,
	@LogID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CatLogID INT
	SELECT @CatLogID FROM CategoryLog WHERE CategoryID=@CategoryID and LogID = @LogID
	IF @CatLogID IS NULL
	BEGIN
		INSERT INTO CategoryLog (CategoryID, LogID) VALUES(@CategoryID, @LogID)
		RETURN @@IDENTITY
	END
	ELSE RETURN @CatLogID
END

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid] as
begin
	exec [dbo].[TransformInputToInvalid_AppFinRecord]
	exec [dbo].[TransformInputToInvalid_CollectionDetails]
	exec [dbo].[TransformInputToInvalid_ContactPreference]
	exec [dbo].[TransformInputToInvalid_DPOutcome]
	exec [dbo].[TransformInputToInvalid_EmploymentStatusMonitoring]
	exec [dbo].[TransformInputToInvalid_Learner]
	exec [dbo].[TransformInputToInvalid_LearnerDestinationandProgression]
	exec [dbo].[TransformInputToInvalid_LearnerEmploymentStatus]
	exec [dbo].[TransformInputToInvalid_LearnerFAM]
	exec [dbo].[TransformInputToInvalid_LearnerHE]
	exec [dbo].[TransformInputToInvalid_LearnerHEFinancialSupport]
	exec [dbo].[TransformInputToInvalid_LearningDelivery]
	exec [dbo].[TransformInputToInvalid_LearningDeliveryFAM]
	exec [dbo].[TransformInputToInvalid_LearningDeliveryHE]
	exec [dbo].[TransformInputToInvalid_LearningDeliveryWorkPlacement]
	exec [dbo].[TransformInputToInvalid_LearningProvider]
	exec [dbo].[TransformInputToInvalid_LLDDandHealthProblem]
	exec [dbo].[TransformInputToInvalid_ProviderSpecDeliveryMonitoring]
	exec [dbo].[TransformInputToInvalid_ProviderSpecLearnerMonitoring]
	exec [dbo].[TransformInputToInvalid_Source]
	exec [dbo].[TransformInputToInvalid_SourceFile]
end 

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_AppFinRecord]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_AppFinRecord] as
begin
insert into [Invalid].[AppFinRecord]
(
	[AppFinRecord_Id],
	[LearningDelivery_Id],
	[LearnRefNumber],
	[AimSeqNumber],
	[AFinType],
	[AFinCode],
	[AFinDate],
	[AFinAmount]
)select
	AFR.AppFinRecord_Id,
	AFR.LearningDelivery_Id,
	AFR.LearnRefNumber,
	AFR.AimSeqNumber,
	AFR.AFinType,
	AFR.AFinCode,
	AFR.AFinDate,
	AFR.AFinAmount
from
	[Input].[AppFinRecord] as AFR
	join Input.LearningDelivery as ld
		on ld.LearningDelivery_Id = afr.LearningDelivery_Id
	join Input.Learner as l
		on ld.Learner_Id = l.Learner_Id
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = l.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_CollectionDetails]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_CollectionDetails] as
begin
insert into [Invalid].[CollectionDetails]
(
	[CollectionDetails_Id],
	[Collection],
	[Year],
	[FilePreparationDate]
)
select
	CD.CollectionDetails_Id,
	CD.Collection,
	CD.Year,
	CD.FilePreparationDate
from
	[Input].[CollectionDetails] as CD

end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_ContactPreference]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_ContactPreference] as
begin
insert into [Invalid].[ContactPreference]
(
	[ContactPreference_Id],
	[Learner_Id],
	[LearnRefNumber],
	[ContPrefType],
	[ContPrefCode]
)
select
	CP.ContactPreference_Id,
	CP.Learner_Id,
	CP.LearnRefNumber,
	CP.ContPrefType,
	CP.ContPrefCode
from
	[Input].[ContactPreference] as CP
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = CP.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_DPOutcome]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_DPOutcome] as
begin
insert into [Invalid].[DPOutcome]
(
	[DPOutcome_Id],
	[LearnerDestinationandProgression_Id],
	[LearnRefNumber],
	[OutType],
	[OutCode],
	[OutStartDate],
	[OutEndDate],
	[OutCollDate]
)
select
	DPO.DPOutcome_Id,
	DPO.LearnerDestinationandProgression_Id,
	DPO.LearnRefNumber,
	DPO.OutType,
	DPO.OutCode,
	DPO.OutStartDate,
	DPO.OutEndDate,
	DPO.OutCollDate
from
	[Input].[DPOutcome] as DPO
	left join dbo.ValidLearnerDestinationandProgressions as vl
	on DPO.LearnerDestinationandProgression_Id= vl.LearnerDestinationandProgression_Id
where
	vl.LearnerDestinationandProgression_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_EmploymentStatusMonitoring]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_EmploymentStatusMonitoring] as
begin
insert into [Invalid].[EmploymentStatusMonitoring]
(
	[EmploymentStatusMonitoring_Id],
	[LearnerEmploymentStatus_Id],
	[LearnRefNumber],
	[DateEmpStatApp],
	[ESMType],
	[ESMCode]
)
select Distinct
	ESM.EmploymentStatusMonitoring_Id,
	ESM.LearnerEmploymentStatus_Id,
	ESM.LearnRefNumber,
	ESM.DateEmpStatApp,
	ESM.ESMType,
	ESM.ESMCode
from
	[Input].[EmploymentStatusMonitoring] as ESM
	INNER JOIN Input.LearnerEmploymentStatus AS LES
		ON ESM.LearnerEmploymentStatus_Id = LES.LearnerEmploymentStatus_Id
	LEFT JOIN dbo.ValidLearners AS VL
		ON vl.Learner_Id = LES.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_Learner]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_Learner] as
begin
insert into [Invalid].[Learner]
(
	[Learner_Id],
	[LearnRefNumber],
	[PrevLearnRefNumber],
	[PrevUKPRN],
	[PMUKPRN],
	CampId,
	[ULN],
	[FamilyName],
	[GivenNames],
	[DateOfBirth],
	[Ethnicity],
	[Sex],
	[LLDDHealthProb],
	[NINumber],
	[PriorAttain],
	[Accom],
	[ALSCost],
	[PlanLearnHours],
	[PlanEEPHours],
	[MathGrade],
	[EngGrade],
	[PostcodePrior],
	[Postcode],
	[AddLine1],
	[AddLine2],
	[AddLine3],
	[AddLine4],
	[TelNo],
	[Email],
	OTJHours
)
select
	L.Learner_Id,
	L.LearnRefNumber,
	L.PrevLearnRefNumber,
	L.PrevUKPRN,
	L.PMUKPRN,
	L.CampId,
	L.ULN,
	L.FamilyName,
	L.GivenNames,
	L.DateOfBirth,
	L.Ethnicity,
	L.Sex,
	L.LLDDHealthProb,
	L.NINumber,
	L.PriorAttain,
	L.Accom,
	L.ALSCost,
	L.PlanLearnHours,
	L.PlanEEPHours,
	L.MathGrade,
	L.EngGrade,
	L.PostcodePrior,
	L.Postcode,
	L.AddLine1,
	L.AddLine2,
	L.AddLine3,
	L.AddLine4,
	L.TelNo,
	L.Email,
	L.OTJHours
from
	[Input].[Learner] as L
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = L.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearnerDestinationandProgression]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearnerDestinationandProgression] as
begin
insert into [Invalid].[LearnerDestinationandProgression]
(
	[LearnerDestinationandProgression_Id],
	[LearnRefNumber],
	[ULN]
)
	select
			[LearnerDestinationandProgression].[LearnerDestinationandProgression_Id],
			[LearnerDestinationandProgression].[LearnRefNumber],
			[LearnerDestinationandProgression].[ULN]
		from
			[Input].[LearnerDestinationandProgression]

			left join [dbo].[ValidLearnerDestinationandProgressions]
				on [ValidLearnerDestinationandProgressions].[LearnerDestinationandProgression_Id]=[LearnerDestinationandProgression].[LearnerDestinationandProgression_Id]
		where
			[ValidLearnerDestinationandProgressions].[LearnerDestinationandProgression_Id] is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearnerEmploymentStatus]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearnerEmploymentStatus] as
begin
insert into [Invalid].[LearnerEmploymentStatus]
(
	[LearnerEmploymentStatus_Id],
	[Learner_Id],
	[LearnRefNumber],
	[EmpStat],
	[DateEmpStatApp],
	[EmpId],
	[AgreeId]
)
select
	LES.LearnerEmploymentStatus_Id,
	LES.Learner_Id,
	LES.LearnRefNumber,
	LES.EmpStat,
	LES.DateEmpStatApp,
	LES.EmpId,
	LES.AgreeId
from
	[Input].[LearnerEmploymentStatus] as LES
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = LES.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearnerFAM]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearnerFAM] as
begin
insert into [Invalid].[LearnerFAM]
(
	[LearnerFAM_Id],
	[Learner_Id],
	[LearnRefNumber],
	[LearnFAMType],
	[LearnFAMCode]
)
select
	LFAM.LearnerFAM_Id,
	LFAM.Learner_Id,
	LFAM.LearnRefNumber,
	LFAM.LearnFAMType,
	LFAM.LearnFAMCode
from
	[Input].[LearnerFAM] as LFAM
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = LFAM.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearnerHE]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearnerHE] as
begin
insert into [Invalid].[LearnerHE]
(
	[LearnerHE_Id],
	[Learner_Id],
	[LearnRefNumber],
	[UCASPERID],
	[TTACCOM]
)
select
	LHE.LearnerHE_Id,
	LHE.Learner_Id,
	LHE.LearnRefNumber,
	LHE.UCASPERID,
	LHE.TTACCOM
from
	[Input].[LearnerHE] as LHE
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = LHE.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearnerHEFinancialSupport]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearnerHEFinancialSupport] as
begin
insert into [Invalid].[LearnerHEFinancialSupport]
(
	[LearnerHEFinancialSupport_Id],
	[LearnerHE_Id],
	[LearnRefNumber],
	[FINTYPE],
	[FINAMOUNT]
)
select
	LHEFS.LearnerHEFinancialSupport_Id,
	LHEFS.LearnerHE_Id,
	LHEFS.LearnRefNumber,
	LHEFS.FINTYPE,
	LHEFS.FINAMOUNT
from
	[Input].[LearnerHEFinancialSupport] AS LHEFS
	INNER JOIN Input.LearnerHE AS LHE
		ON LHEFS.LearnerHE_Id = LHE.LearnerHE_Id
	LEFT JOIN dbo.ValidLearners AS vl
		ON vl.Learner_Id = LHE.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearningDelivery]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearningDelivery] as
begin
insert into [Invalid].[LearningDelivery]
(
	[LearningDelivery_Id],
	[Learner_Id],
	[LearnRefNumber],
	[LearnAimRef],
	[AimType],
	[AimSeqNumber],
	[LearnStartDate],
	[OrigLearnStartDate],
	[LearnPlanEndDate],
	[FundModel],
	[ProgType],
	[FworkCode],
	[PwayCode],
	[StdCode],
	[PartnerUKPRN],
	[DelLocPostCode],
	[AddHours],
	[PriorLearnFundAdj],
	[OtherFundAdj],
	[ConRefNumber],
	[EPAOrgID],
	[EmpOutcome],
	[CompStatus],
	[LearnActEndDate],
	[WithdrawReason],
	[Outcome],
	[AchDate],
	[OutGrade],
	[SWSupAimId]
)
select
	LD.LearningDelivery_Id,
	LD.Learner_Id,
	LD.LearnRefNumber,
	LD.LearnAimRef,
	LD.AimType,
	LD.AimSeqNumber,
	LD.LearnStartDate,
	LD.OrigLearnStartDate,
	LD.LearnPlanEndDate,
	LD.FundModel,
	LD.ProgType,
	LD.FworkCode,
	LD.PwayCode,
	LD.StdCode,
	LD.PartnerUKPRN,
	LD.DelLocPostCode,
	LD.AddHours,
	LD.PriorLearnFundAdj,
	LD.OtherFundAdj,
	LD.ConRefNumber,
	LD.EPAOrgID,
	LD.EmpOutcome,
	LD.CompStatus,
	LD.LearnActEndDate,
	LD.WithdrawReason,
	LD.Outcome,
	LD.AchDate,
	LD.OutGrade,
	LD.SWSupAimId
from
	[Input].[LearningDelivery] as LD
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = LD.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearningDeliveryFAM]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearningDeliveryFAM] as
begin
insert into [Invalid].[LearningDeliveryFAM]
(
	[LearningDeliveryFAM_Id],
	[LearningDelivery_Id],
	[LearnRefNumber],
	[AimSeqNumber],
	[LearnDelFAMType],
	[LearnDelFAMCode],
	[LearnDelFAMDateFrom],
	[LearnDelFAMDateTo]
)
select Distinct
	LDFAM.LearningDeliveryFAM_Id,
	LDFAM.LearningDelivery_Id,
	LDFAM.LearnRefNumber,
	LDFAM.AimSeqNumber,
	LDFAM.LearnDelFAMType,
	LDFAM.LearnDelFAMCode,
	LDFAM.LearnDelFAMDateFrom,
	LDFAM.LearnDelFAMDateTo
from
	[Input].[LearningDeliveryFAM] as LDFAM
	INNER JOIN Input.LearningDelivery AS LD
		ON LDFAM.LearningDelivery_Id = LD.LearningDelivery_Id
	LEFT JOIN dbo.ValidLearners AS VL
		ON VL.Learner_Id = LD.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearningDeliveryHE]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearningDeliveryHE] as
begin
insert into [Invalid].[LearningDeliveryHE]
(
	[LearningDeliveryHE_Id],
	[LearningDelivery_Id],
	[LearnRefNumber],
	[AimSeqNumber],
	[NUMHUS],
	[SSN],
	[QUALENT3],
	[SOC2000],
	[SEC],
	[UCASAPPID],
	[TYPEYR],
	[MODESTUD],
	[FUNDLEV],
	[FUNDCOMP],
	[STULOAD],
	[YEARSTU],
	[MSTUFEE],
	[PCOLAB],
	[PCFLDCS],
	[PCSLDCS],
	[PCTLDCS],
	[SPECFEE],
	[NETFEE],
	[GROSSFEE],
	[DOMICILE],
	[ELQ],
	[HEPostCode]
)
select
	LDHE.LearningDeliveryHE_Id,
	LDHE.LearningDelivery_Id,
	LDHE.LearnRefNumber,
	LDHE.AimSeqNumber,
	LDHE.NUMHUS,
	LDHE.SSN,
	LDHE.QUALENT3,
	LDHE.SOC2000,
	LDHE.SEC,
	LDHE.UCASAPPID,
	LDHE.TYPEYR,
	LDHE.MODESTUD,
	LDHE.FUNDLEV,
	LDHE.FUNDCOMP,
	LDHE.STULOAD,
	LDHE.YEARSTU,
	LDHE.MSTUFEE,
	LDHE.PCOLAB,
	LDHE.PCFLDCS,
	LDHE.PCSLDCS,
	LDHE.PCTLDCS,
	LDHE.SPECFEE,
	LDHE.NETFEE,
	LDHE.GROSSFEE,
	LDHE.DOMICILE,
	LDHE.ELQ,
	LDHE.HEPostCode
from
	[Input].[LearningDeliveryHE] as LDHE
	INNER JOIN Input.LearningDelivery AS LD
		ON LDHE.LearningDelivery_Id = LD.LearningDelivery_Id
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = LD.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearningDeliveryWorkPlacement]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearningDeliveryWorkPlacement] as
begin
insert into [Invalid].[LearningDeliveryWorkPlacement]
(
	[LearningDeliveryWorkPlacement_Id],
	[LearningDelivery_Id],
	[LearnRefNumber],
	[AimSeqNumber],
	[WorkPlaceStartDate],
	[WorkPlaceEndDate],
	[WorkPlaceHours],
	[WorkPlaceMode],
	[WorkPlaceEmpId]
)
select
	LDWP.LearningDeliveryWorkPlacement_Id,
	LDWP.LearningDelivery_Id,
	LDWP.LearnRefNumber,
	LDWP.AimSeqNumber,
	LDWP.WorkPlaceStartDate,
	LDWP.WorkPlaceEndDate,
	LDWP.WorkPlaceHours,
	LDWP.WorkPlaceMode,
	LDWP.WorkPlaceEmpId
from
	[Input].[LearningDeliveryWorkPlacement] as LDWP
	INNER JOIN Input.LearningDelivery AS LD
		ON LDWP.LearningDelivery_Id = LD.LearningDelivery_Id
	left join dbo.ValidLearners as VL
	on VL.Learner_Id = LD.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LearningProvider]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LearningProvider] as
begin
insert into [Invalid].[LearningProvider]
(
	[LearningProvider_Id],
	[UKPRN]
)
select
	LP.LearningProvider_Id,
	LP.UKPRN
from
	[Input].[LearningProvider] as LP

end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_LLDDandHealthProblem]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_LLDDandHealthProblem] as
begin
insert into [Invalid].[LLDDandHealthProblem]
(
	[LLDDandHealthProblem_Id],
	[Learner_Id],
	[LearnRefNumber],
	[LLDDCat],
	[PrimaryLLDD]
)
select
	LLDDHP.LLDDandHealthProblem_Id,
	LLDDHP.Learner_Id,
	LLDDHP.LearnRefNumber,
	LLDDHP.LLDDCat,
	LLDDHP.PrimaryLLDD
from
	[Input].[LLDDandHealthProblem] as LLDDHP
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = LLDDHP.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_ProviderSpecDeliveryMonitoring]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_ProviderSpecDeliveryMonitoring] as
begin
insert into [Invalid].[ProviderSpecDeliveryMonitoring]
(
	[ProviderSpecDeliveryMonitoring_Id],
	[LearningDelivery_Id],
	[LearnRefNumber],
	[AimSeqNumber],
	[ProvSpecDelMonOccur],
	[ProvSpecDelMon]
)
select Distinct
	PSDM.ProviderSpecDeliveryMonitoring_Id,
	PSDM.LearningDelivery_Id,
	PSDM.LearnRefNumber,
	PSDM.AimSeqNumber,
	PSDM.ProvSpecDelMonOccur,
	PSDM.ProvSpecDelMon
from
	[Input].[ProviderSpecDeliveryMonitoring] as PSDM
	INNER JOIN Input.LearningDelivery AS LD
		ON LD.LearningDelivery_Id = PSDM.LearningDelivery_Id
	LEFT JOIN dbo.ValidLearners as VL
		ON VL.Learner_Id = LD.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_ProviderSpecLearnerMonitoring]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_ProviderSpecLearnerMonitoring] as
begin
insert into [Invalid].[ProviderSpecLearnerMonitoring]
(
	[ProviderSpecLearnerMonitoring_Id],
	[Learner_Id],
	[LearnRefNumber],
	[ProvSpecLearnMonOccur],
	[ProvSpecLearnMon]
)
select
	PSLM.ProviderSpecLearnerMonitoring_Id,
	PSLM.Learner_Id,
	PSLM.LearnRefNumber,
	PSLM.ProvSpecLearnMonOccur,
	PSLM.ProvSpecLearnMon
from
	[Input].[ProviderSpecLearnerMonitoring] as PSLM
	left join dbo.ValidLearners as vl
	on vl.Learner_Id = PSLM.Learner_Id
where
	vl.Learner_Id is null
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_Source]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_Source] as
begin
insert into [Invalid].[Source]
(
	[Source_Id],
	[ProtectiveMarking],
	[UKPRN],
	[SoftwareSupplier],
	[SoftwarePackage],
	[Release],
	[SerialNo],
	[DateTime],
	[ReferenceData],
	[ComponentSetVersion]
)
select
	S.Source_Id,
	S.ProtectiveMarking,
	S.UKPRN,
	S.SoftwareSupplier,
	S.SoftwarePackage,
	S.Release,
	S.SerialNo,
	S.DateTime,
	S.ReferenceData,
	S.ComponentSetVersion
from
	[Input].[Source] as S

end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToInvalid_SourceFile]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
create procedure [dbo].[TransformInputToInvalid_SourceFile] as
begin
insert into [Invalid].[SourceFile]
(
	[SourceFile_Id],
	[SourceFileName],
	[FilePreparationDate],
	[SoftwareSupplier],
	[SoftwarePackage],
	[Release],
	[SerialNo],
	[DateTime]
)
select
	SF.SourceFile_Id,
	SF.SourceFileName,
	SF.FilePreparationDate,
	SF.SoftwareSupplier,
	SF.SoftwarePackage,
	SF.Release,
	SF.SerialNo,
	SF.DateTime
from
	[Input].[SourceFile] as SF
end

GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid] as
begin
	exec [dbo].[TransformInputToValid_AppFinRecord]
	exec [dbo].[TransformInputToValid_CollectionDetails]
	exec [dbo].[TransformInputToValid_ContactPreference]
	exec [dbo].[TransformInputToValid_DPOutcome]
	exec [dbo].[TransformInputToValid_EmploymentStatusMonitoring]
	exec [dbo].[TransformInputToValid_Learner]
	exec [dbo].[TransformInputToValid_LearnerDestinationandProgression]
	exec [dbo].[TransformInputToValid_LearnerEmploymentStatus]
	exec [dbo].[TransformInputToValid_LearnerFAM]
	exec [dbo].[TransformInputToValid_LearnerHE]
	exec [dbo].[TransformInputToValid_LearnerHEFinancialSupport]
	exec [dbo].[TransformInputToValid_LearningDelivery]
	exec [dbo].[TransformInputToValid_LearningDeliveryFAM]
	exec [dbo].[TransformInputToValid_LearningDeliveryHE]
	exec [dbo].[TransformInputToValid_LearningDeliveryWorkPlacement]
	exec [dbo].[TransformInputToValid_LearningProvider]
	exec [dbo].[TransformInputToValid_LLDDandHealthProblem]
	exec [dbo].[TransformInputToValid_ProviderSpecDeliveryMonitoring]
	exec [dbo].[TransformInputToValid_ProviderSpecLearnerMonitoring]
	exec [dbo].[TransformInputToValid_Source]
	exec [dbo].[TransformInputToValid_SourceFile]

	--Insert to these tables at the end so that dependencies are in place.
	exec [dbo].[TransformInputToValid_LearningDeliveryDenormTbl]
	exec [dbo].[TransformInputToValid_LearnerEmploymentStatusDenormTbl] 

end 
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_AppFinRecord]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_AppFinRecord] as
begin
insert into [Valid].[AppFinRecord]
(
	[LearnRefNumber],
	[AimSeqNumber],
	[AFinType],
	[AFinCode],
	[AFinDate],
	[AFinAmount]
)
select
	AFR.LearnRefNumber,
	AFR.AimSeqNumber,
	AFR.AFinType,
	AFR.AFinCode,
	AFR.AFinDate,
	AFR.AFinAmount
from
	[Input].[AppFinRecord] as AFR
	inner join [Input].[LearningDelivery]
		on [LearningDelivery].[LearningDelivery_Id]=AFR.[LearningDelivery_Id]
	inner join [Input].[Learner]
		on [Learner].[Learner_Id]=[LearningDelivery].[Learner_Id]
	inner join [dbo].[ValidLearners]
		on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_CollectionDetails]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_CollectionDetails] as
begin
insert into [Valid].[CollectionDetails]
(
	[Collection],
	[Year],
	[FilePreparationDate]
)
select
	CD.Collection,
	CD.Year,
	CD.FilePreparationDate
from
	[Input].[CollectionDetails] as CD
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_ContactPreference]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_ContactPreference] as
begin
insert into [Valid].[ContactPreference]
(
	[LearnRefNumber],
	[ContPrefType],
	[ContPrefCode]
)
select
	CP.LearnRefNumber,
	CP.ContPrefType,
	CP.ContPrefCode
from
	[Input].[ContactPreference] as CP
	join dbo.ValidLearners as vl
	on vl.Learner_Id = CP.Learner_Id
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_DPOutcome]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_DPOutcome] as
begin
insert into [Valid].[DPOutcome]
(
	[LearnRefNumber],
	[OutType],
	[OutCode],
	[OutStartDate],
	[OutEndDate],
	[OutCollDate]
)
select
	DPO.LearnRefNumber,
	DPO.OutType,
	DPO.OutCode,
	DPO.OutStartDate,
	DPO.OutEndDate,
	DPO.OutCollDate
from
	[Input].[DPOutcome] as DPO
	inner join [Input].[LearnerDestinationandProgression] as dp
		on dp.[LearnerDestinationandProgression_Id]=dpo.[LearnerDestinationandProgression_Id]
	inner join [dbo].[ValidLearnerDestinationandProgressions] as vdp
		on dpo.[LearnerDestinationandProgression_Id]=vdp.[LearnerDestinationandProgression_Id]

end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_EmploymentStatusMonitoring]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_EmploymentStatusMonitoring] as
begin
insert into [Valid].[EmploymentStatusMonitoring]
(
	[LearnRefNumber],
	[DateEmpStatApp],
	[ESMType],
	[ESMCode]
)
select Distinct
	ESM.LearnRefNumber,
	ESM.DateEmpStatApp,
	ESM.ESMType,
	ESM.ESMCode
from
	[Input].[EmploymentStatusMonitoring] AS ESM
	INNER JOIN Input.LearnerEmploymentStatus LES
		ON ESM.LearnerEmploymentStatus_Id = LES.LearnerEmploymentStatus_Id
	INNER JOIN dbo.ValidLearners AS VL
		on VL.Learner_Id = LES.Learner_Id
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_Learner]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_Learner] as
begin
insert into [Valid].[Learner]
(
	[LearnRefNumber],
	[PrevLearnRefNumber],
	[PrevUKPRN],
	[PMUKPRN],
	[ULN],
	[FamilyName],
	[GivenNames],
	[DateOfBirth],
	[Ethnicity],
	[Sex],
	[LLDDHealthProb],
	[NINumber],
	[PriorAttain],
	[Accom],
	[ALSCost],
	[PlanLearnHours],
	[PlanEEPHours],
	[MathGrade],
	[EngGrade],
	[PostcodePrior],
	[Postcode],
	[AddLine1],
	[AddLine2],
	[AddLine3],
	[AddLine4],
	[TelNo],
	[Email]
)
select
	L.LearnRefNumber,
	L.PrevLearnRefNumber,
	L.PrevUKPRN,
	L.PMUKPRN,
	L.ULN,
	L.FamilyName,
	L.GivenNames,
	L.DateOfBirth,
	L.Ethnicity,
	L.Sex,
	L.LLDDHealthProb,
	L.NINumber,
	L.PriorAttain,
	L.Accom,
	L.ALSCost,
	L.PlanLearnHours,
	L.PlanEEPHours,
	L.MathGrade,
	L.EngGrade,
	L.PostcodePrior,
	L.Postcode,
	L.AddLine1,
	L.AddLine2,
	L.AddLine3,
	L.AddLine4,
	L.TelNo,
	L.Email
from
	[Input].[Learner] as L
	join dbo.ValidLearners as vl
	on vl.Learner_Id = L.Learner_Id
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerDestinationandProgression]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearnerDestinationandProgression] as
begin
insert into [Valid].[LearnerDestinationandProgression]
(
	[LearnRefNumber],
	[ULN]
)
select distinct
	LDP.LearnRefNumber,
	LDP.ULN
from
	[Input].[LearnerDestinationandProgression] as LDP
	join [dbo].[ValidLearnerDestinationandProgressions] as vdp
		on LDP.[LearnerDestinationandProgression_Id] = vdp.[LearnerDestinationandProgression_Id]
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerEmploymentStatus]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearnerEmploymentStatus] as
begin
insert into [Valid].[LearnerEmploymentStatus]
(
	[LearnRefNumber],
	[EmpStat],
	[DateEmpStatApp],
	[EmpId]
)
select Distinct
	LES.LearnRefNumber,
	LES.EmpStat,
	LES.DateEmpStatApp,
	LES.EmpId
from
	[Input].[LearnerEmploymentStatus] as LES
	join dbo.ValidLearners as vl
	on vl.Learner_Id = LES.Learner_Id
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerEmploymentStatusDenormTbl]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearnerEmploymentStatusDenormTbl] as
begin
INSERT [Valid].[LearnerEmploymentStatusDenormTbl]
(
	 [LearnRefNumber]
	,[EmpStat]
	,[EmpId]
	,[DateEmpStatApp]
	,[ESMCode_BSI]
	,[ESMCode_EII]
	,[ESMCode_LOE]
	,[ESMCode_LOU]
	,[ESMCode_PEI]
	,[ESMCode_SEI]
	,[ESMCode_SEM]
)
SELECT
	 [LearnRefNumber]
	,[EmpStat]
	,[EmpId]
	,[DateEmpStatApp]
	,[ESMCode_BSI]
	,[ESMCode_EII]
	,[ESMCode_LOE]
	,[ESMCode_LOU]
	,[ESMCode_PEI]
	,[ESMCode_SEI]
	,[ESMCode_SEM]
FROM [Valid].[LearnerEmploymentStatusDenorm]
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerFAM]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearnerFAM] as
begin
insert into [Valid].[LearnerFAM]
(
	[LearnRefNumber],
	[LearnFAMType],
	[LearnFAMCode]
)
select
	CAST(LFAM.LearnRefNumber AS varchar(12)),
	LFAM.LearnFAMType,
	LFAM.LearnFAMCode
from
	[Input].[LearnerFAM] as LFAM
	join dbo.ValidLearners as vl
	on vl.Learner_Id = LFAM.Learner_Id
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerHE]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearnerHE] as
begin
insert into [Valid].[LearnerHE]
(
	[LearnRefNumber],
	[UCASPERID],
	[TTACCOM]
)
select
	l.LearnRefNumber,
	lhe.UCASPERID,
	lhe.TTACCOM
from
	[Input].[LearnerHE] as lhe
	inner join [Input].[Learner] as l
		on lhe.[Learner_Id]=l.[Learner_Id]
	inner join [dbo].[ValidLearners] as vl
		on lhe.[Learner_Id]=vl.[Learner_Id]
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerHEFinancialSupport]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearnerHEFinancialSupport] as
begin
insert into [Valid].[LearnerHEFinancialSupport]
(
	[LearnRefNumber],
	[FINTYPE],
	[FINAMOUNT]
)
select
	LearnerHEFinancialSupport.LearnRefNumber,
	LearnerHEFinancialSupport.FINTYPE,
	LearnerHEFinancialSupport.FINAMOUNT
from
	[Input].[LearnerHEFinancialSupport]
	inner join [Input].[LearnerHE]
		on [LearnerHEFinancialSupport].[LearnerHE_Id]=[LearnerHE].[LearnerHE_Id]
	inner join [Input].[Learner]
		on [Learner].[Learner_Id]=[LearnerHE].[Learner_Id]
	inner join [dbo].[ValidLearners]
		on [LearnerHE].[Learner_Id]=[ValidLearners].[Learner_Id]
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDelivery]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearningDelivery] as
begin
insert into [Valid].[LearningDelivery]
(
	[LearnRefNumber],
	[LearnAimRef],
	[AimType],
	[AimSeqNumber],
	[LearnStartDate],
	[OrigLearnStartDate],
	[LearnPlanEndDate],
	[FundModel],
	[ProgType],
	[FworkCode],
	[PwayCode],
	[StdCode],
	[PartnerUKPRN],
	[DelLocPostCode],
	[AddHours],
	[PriorLearnFundAdj],
	[OtherFundAdj],
	[ConRefNumber],
	[EPAOrgID],
	[EmpOutcome],
	[CompStatus],
	[LearnActEndDate],
	[WithdrawReason],
	[Outcome],
	[AchDate],
	[OutGrade],
	[SWSupAimId]
)
select Distinct
	LD.LearnRefNumber,
	LD.LearnAimRef,
	LD.AimType,
	LD.AimSeqNumber,
	LD.LearnStartDate,
	LD.OrigLearnStartDate,
	LD.LearnPlanEndDate,
	LD.FundModel,
	LD.ProgType,
	LD.FworkCode,
	LD.PwayCode,
	LD.StdCode,
	LD.PartnerUKPRN,
	LD.DelLocPostCode,
	LD.AddHours,
	LD.PriorLearnFundAdj,
	LD.OtherFundAdj,
	LD.ConRefNumber,
	LD.EPAOrgID,
	LD.EmpOutcome,
	LD.CompStatus,
	LD.LearnActEndDate,
	LD.WithdrawReason,
	LD.Outcome,
	LD.AchDate,
	LD.OutGrade,
	LD.SWSupAimId
from
	[Input].[LearningDelivery] as LD
	join dbo.ValidLearners as vl
	on vl.Learner_Id = LD.Learner_Id
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryDenormTbl]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearningDeliveryDenormTbl] as
begin
INSERT [Valid].[LearningDeliveryDenormTbl]
(
	 [LearnRefNumber]		
	,[LearnAimRef]			
	,[AimType]				
	,[AimSeqNumber]			
	,[LearnStartDate]		
	,[OrigLearnStartDate]	
	,[LearnPlanEndDate]		
	,[FundModel]			
	,[ProgType]				
	,[FworkCode]			
	,[PwayCode]				
	,[StdCode]				
	,[PartnerUKPRN]			
	,[DelLocPostCode]		
	,[AddHours]				
	,[PriorLearnFundAdj]	
	,[OtherFundAdj]			
	,[ConRefNumber]			
	,[EPAOrgID]				
	,[EmpOutcome]			
	,[CompStatus]			
	,[LearnActEndDate]		
	,[WithdrawReason]		
	,[Outcome]				
	,[AchDate]				
	,[OutGrade]				
	,[SWSupAimId]			
	,[HEM1]					
	,[HEM2]					
	,[HEM3]					
	,[HHS1]					
	,[HHS2]					
	,[LDFAM_SOF]			
	,[LDFAM_EEF]			
	,[LDFAM_RES]			
	,[LDFAM_ADL]			
	,[LDFAM_FFI]			
	,[LDFAM_WPP]			
	,[LDFAM_POD]			
	,[LDFAM_ASL]			
	,[LDFAM_FLN]			
	,[LDFAM_NSA]			
	,[ProvSpecDelMon_A]		
	,[ProvSpecDelMon_B]		
	,[ProvSpecDelMon_C]		
	,[ProvSpecDelMon_D]		
	,[LDM1]					
	,[LDM2]					
	,[LDM3]					
	,[LDM4]					
)
SELECT
	 [LearnRefNumber]		
	,[LearnAimRef]			
	,[AimType]				
	,[AimSeqNumber]			
	,[LearnStartDate]		
	,[OrigLearnStartDate]	
	,[LearnPlanEndDate]		
	,[FundModel]			
	,[ProgType]				
	,[FworkCode]			
	,[PwayCode]				
	,[StdCode]				
	,[PartnerUKPRN]			
	,[DelLocPostCode]		
	,[AddHours]				
	,[PriorLearnFundAdj]	
	,[OtherFundAdj]			
	,[ConRefNumber]			
	,[EPAOrgID]				
	,[EmpOutcome]			
	,[CompStatus]			
	,[LearnActEndDate]		
	,[WithdrawReason]		
	,[Outcome]				
	,[AchDate]				
	,[OutGrade]				
	,[SWSupAimId]			
	,[HEM1]					
	,[HEM2]					
	,[HEM3]					
	,[HHS1]					
	,[HHS2]					
	,[LDFAM_SOF]			
	,[LDFAM_EEF]			
	,[LDFAM_RES]			
	,[LDFAM_ADL]			
	,[LDFAM_FFI]			
	,[LDFAM_WPP]			
	,[LDFAM_POD]			
	,[LDFAM_ASL]			
	,[LDFAM_FLN]			
	,[LDFAM_NSA]			
	,[ProvSpecDelMon_A]		
	,[ProvSpecDelMon_B]		
	,[ProvSpecDelMon_C]		
	,[ProvSpecDelMon_D]		
	,[LDM1]					
	,[LDM2]					
	,[LDM3]					
	,[LDM4]		
FROM	Valid.LearningDeliveryDenorm 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryFAM]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearningDeliveryFAM] as
begin
insert into [Valid].[LearningDeliveryFAM]
(
	[LearnRefNumber],
	[AimSeqNumber],
	[LearnDelFAMType],
	[LearnDelFAMCode],
	[LearnDelFAMDateFrom],
	[LearnDelFAMDateTo]
)
select
	LearningDeliveryFAM.LearnRefNumber,
	LearningDeliveryFAM.AimSeqNumber,
	LearningDeliveryFAM.LearnDelFAMType,
	LearningDeliveryFAM.LearnDelFAMCode,
	LearningDeliveryFAM.LearnDelFAMDateFrom,
	LearningDeliveryFAM.LearnDelFAMDateTo
from
	[Input].[LearningDeliveryFAM]
	inner join [Input].[LearningDelivery]
		on [LearningDeliveryFAM].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
	inner join [Input].[Learner]
		on [LearningDelivery].[Learner_Id]=[Learner].[Learner_Id]
	inner join [dbo].[ValidLearners]
		on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryHE]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearningDeliveryHE] as
begin
insert into [Valid].[LearningDeliveryHE]
(
	[LearnRefNumber],
	[AimSeqNumber],
	[NUMHUS],
	[SSN],
	[QUALENT3],
	[SOC2000],
	[SEC],
	[UCASAPPID],
	[TYPEYR],
	[MODESTUD],
	[FUNDLEV],
	[FUNDCOMP],
	[STULOAD],
	[YEARSTU],
	[MSTUFEE],
	[PCOLAB],
	[PCFLDCS],
	[PCSLDCS],
	[PCTLDCS],
	[SPECFEE],
	[NETFEE],
	[GROSSFEE],
	[DOMICILE],
	[ELQ],
	[HEPostCode]
)
select
	LDHE.LearnRefNumber,
	LDHE.AimSeqNumber,
	LDHE.NUMHUS,
	LDHE.SSN,
	LDHE.QUALENT3,
	LDHE.SOC2000,
	LDHE.SEC,
	LDHE.UCASAPPID,
	LDHE.TYPEYR,
	LDHE.MODESTUD,
	LDHE.FUNDLEV,
	LDHE.FUNDCOMP,
	LDHE.STULOAD,
	LDHE.YEARSTU,
	LDHE.MSTUFEE,
	LDHE.PCOLAB,
	LDHE.PCFLDCS,
	LDHE.PCSLDCS,
	LDHE.PCTLDCS,
	LDHE.SPECFEE,
	LDHE.NETFEE,
	LDHE.GROSSFEE,
	LDHE.DOMICILE,
	LDHE.ELQ,
	LDHE.HEPostCode
from
	[Input].[LearningDeliveryHE] AS LDHE
	INNER JOIN Input.LearningDelivery AS LD
		ON LDHE.LearningDelivery_Id = LD.LearningDelivery_Id
	INNER JOIN dbo.ValidLearners AS VL
		ON VL.Learner_Id = LD.Learner_Id
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryWorkPlacement]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearningDeliveryWorkPlacement] as
begin
insert into [Valid].[LearningDeliveryWorkPlacement]
(
	[LearnRefNumber],
	[AimSeqNumber],
	[WorkPlaceStartDate],
	[WorkPlaceEndDate],
	[WorkPlaceHours],
	[WorkPlaceMode],
	[WorkPlaceEmpId]
)
select
	LDWP.LearnRefNumber,
	LDWP.AimSeqNumber,
	LDWP.WorkPlaceStartDate,
	LDWP.WorkPlaceEndDate,
	LDWP.WorkPlaceHours,
	LDWP.WorkPlaceMode,
	LDWP.WorkPlaceEmpId
from
	[Input].[LearningDeliveryWorkPlacement] AS LDWP
	INNER JOIN Input.LearningDelivery AS LD
		ON LDWP.LearningDelivery_Id = LD.LearningDelivery_Id
	INNER JOIN dbo.ValidLearners AS VL
		on VL.Learner_Id = LD.Learner_Id
	where LDWP.WorkPlaceEmpId is not null
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningProvider]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LearningProvider] as
begin
insert into [Valid].[LearningProvider]
(
	[UKPRN]
)
select
	LP.UKPRN
from
	[Input].[LearningProvider] as LP
	--join Input.Learner as l
	--on l.LearnRefNumber = LP.LearnRefNumber
	--join dbo.ValidLearners as vl
	--on vl.Learner_Id = l.Learner_Id
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LLDDandHealthProblem]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_LLDDandHealthProblem] as
begin
insert into [Valid].[LLDDandHealthProblem]
(
	[LearnRefNumber],
	[LLDDCat],
	[PrimaryLLDD]
)
select
	[LLDDandHealthProblem].LearnRefNumber,
	[LLDDandHealthProblem].LLDDCat,
	[LLDDandHealthProblem].PrimaryLLDD
from
	[Input].[LLDDandHealthProblem]
	inner join [Input].[Learner]
		on [LLDDandHealthProblem].[Learner_Id]=[Learner].[Learner_Id]
	inner join [dbo].[ValidLearners]
		on [LLDDandHealthProblem].[Learner_Id]=[ValidLearners].[Learner_Id]
		WHERE PrimaryLLDD IS NOT NULL
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_ProviderSpecDeliveryMonitoring]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_ProviderSpecDeliveryMonitoring] as
begin
insert into [Valid].[ProviderSpecDeliveryMonitoring]
(
	[LearnRefNumber],
	[AimSeqNumber],
	[ProvSpecDelMonOccur],
	[ProvSpecDelMon]
)
select Distinct
	PSDM.LearnRefNumber,
	PSDM.AimSeqNumber,
	PSDM.ProvSpecDelMonOccur,
	PSDM.ProvSpecDelMon
from
	[Input].[ProviderSpecDeliveryMonitoring] AS PSDM
	INNER JOIN Input.LearningDelivery AS LD
		ON PSDM.LearningDelivery_Id = LD.LearningDelivery_Id
	INNER JOIN dbo.ValidLearners AS VL
		ON VL.Learner_Id = LD.Learner_Id
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_ProviderSpecLearnerMonitoring]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_ProviderSpecLearnerMonitoring] as
begin
insert into [Valid].[ProviderSpecLearnerMonitoring]
(
	[LearnRefNumber],
	[ProvSpecLearnMonOccur],
	[ProvSpecLearnMon]
)
select Distinct
	PSLM.LearnRefNumber,
	PSLM.ProvSpecLearnMonOccur,
	PSLM.ProvSpecLearnMon
from
	[Input].[ProviderSpecLearnerMonitoring] as PSLM
	join dbo.ValidLearners as vl
	on vl.Learner_Id = PSLM.Learner_Id
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_Source]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_Source] as
begin
insert into [Valid].[Source]
(
	[ProtectiveMarking],
	[UKPRN],
	[SoftwareSupplier],
	[SoftwarePackage],
	[Release],
	[SerialNo],
	[DateTime],
	[ReferenceData],
	[ComponentSetVersion]
)
select
	S.ProtectiveMarking,
	S.UKPRN,
	S.SoftwareSupplier,
	S.SoftwarePackage,
	S.Release,
	S.SerialNo,
	S.DateTime,
	S.ReferenceData,
	S.ComponentSetVersion
from
	[Input].[Source] as S
end
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_SourceFile]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TransformInputToValid_SourceFile] as
begin
insert into [Valid].[SourceFile]
(
	[SourceFileName],
	[FilePreparationDate],
	[SoftwareSupplier],
	[SoftwarePackage],
	[Release],
	[SerialNo],
	[DateTime]
)
select
	SF.SourceFileName,
	SF.FilePreparationDate,
	SF.SoftwareSupplier,
	SF.SoftwarePackage,
	SF.Release,
	SF.SerialNo,
	SF.DateTime
from
	[Input].[SourceFile] as SF
end
GO
/****** Object:  StoredProcedure [dbo].[WriteLog]    Script Date: 12/11/2018 09:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




/****** Object:  Stored Procedure dbo.WriteLog    Script Date: 10/1/2004 3:16:36 PM ******/

CREATE PROCEDURE [dbo].[WriteLog]
(
	@EventID int, 
	@Priority int, 
	@Severity nvarchar(32), 
	@Title nvarchar(256), 
	@Timestamp datetime,
	@MachineName nvarchar(32), 
	@AppDomainName nvarchar(512),
	@ProcessID nvarchar(256),
	@ProcessName nvarchar(512),
	@ThreadName nvarchar(512),
	@Win32ThreadId nvarchar(128),
	@Message nvarchar(1500),
	@FormattedMessage ntext,
	@LogId int OUTPUT
)
AS 

	INSERT INTO [Log] (
		EventID,
		Priority,
		Severity,
		Title,
		[Timestamp],
		MachineName,
		AppDomainName,
		ProcessID,
		ProcessName,
		ThreadName,
		Win32ThreadId,
		Message,
		FormattedMessage
	)
	VALUES (
		@EventID, 
		@Priority, 
		@Severity, 
		@Title, 
		@Timestamp,
		@MachineName, 
		@AppDomainName,
		@ProcessID,
		@ProcessName,
		@ThreadName,
		@Win32ThreadId,
		@Message,
		@FormattedMessage)

	SET @LogID = @@IDENTITY
	RETURN @LogID




GO
