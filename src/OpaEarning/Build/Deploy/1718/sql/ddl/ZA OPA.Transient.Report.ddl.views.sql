IF NOT EXISTS (
SELECT  schema_name
FROM    information_schema.schemata
WHERE   schema_name = 'Report' ) 

BEGIN
EXEC sp_executesql N'CREATE SCHEMA Report'
END


IF OBJECT_ID('Report.ValidationError') IS NOT NULL
BEGIN
	DROP VIEW Report.ValidationError
END
GO

CREATE VIEW [Report].[ValidationError]
AS
WITH CTE_SQLValidationError AS
(
SELECT [VE].[Source] AS [Source] ,  [LD].[LearnAimRef] AS [LearnAimRef], '' AS [AimSeqNum], [LD].[SWSupAimId] AS [SWSupAimID], [EMLU].[ErrorMessage] AS [ErrorMessage],
[VE].[FieldValues] AS [FieldValues], [VE].[LearnRefNumber] AS [LearnRefNumber], [VE].[RuleName]	AS [RuleName], [EMLU].[Severity] AS [Severity],
[VE].[FileLevelError] AS [FileLevelError]
FROM 
SQLValidationError VE
LEFT JOIN [Static].[ErrorMessageLookUp] EMLU ON VE.RuleName = EMLU.RuleName 
LEFT JOIN [Input].[LearningDelivery] LD ON VE.AimSeqNum = LD.AimSeqNumber AND VE.LearnRefNumber = LD.LearnRefNumber
WHERE ve.RuleName NOT LIKE 'Schema'
)
, CTE_RulebaseErrorOutput AS
( 
SELECT 'FD Validation 17_18.zip' AS [Source], [AimSeqNumber], [ErrorString], [FieldValues], [LearnRefNumber], [RuleId] FROM [Rulebase].[VALFD_ValidationError] WHERE [RuleId] NOT LIKE 'FD[_]DP[_]%'
UNION ALL
SELECT 'FD Validation DP 17_18.zip' AS [Source], [AimSeqNumber], [ErrorString], [FieldValues], [LearnRefNumber], [RuleId] FROM [Rulebase].[VALFD_ValidationError] WHERE [RuleId] LIKE 'FD[_]DP[_]%'
UNION ALL
SELECT 'Validation DP 17_18.zip' AS [Source], NULL, [ErrorString], [FieldValues], [LearnRefNumber], [RuleId] FROM [Rulebase].[VALDP_ValidationError]
UNION ALL
SELECT 'Validation 17_18.zip' AS [Source], [AimSeqNumber], [ErrorString], [FieldValues], [LearnRefNumber], [RuleId] FROM [Rulebase].[VAL_ValidationError]
UNION ALL
SELECT 'ESF Validation 17_18.zip' AS [Source], [AimSeqNumber], [ErrorString], [FieldValues], [LearnRefNumber], [RuleId] FROM [Rulebase].[ESFVAL_ValidationError]
)
, CTE_RulebaseValidationError AS
(
SELECT [VE].[Source] AS [Source] ,  [LD].[LearnAimRef] AS [LearnAimRef], [VE].[AimSeqNumber] AS [AimSeqNum], [LD].[SWSupAimId] AS [SWSupAimID], [EMLU].[ErrorMessage] AS [ErrorMessage],
[VE].[FieldValues] AS [FieldValues], [VE].[LearnRefNumber] AS [LearnRefNumber], [VE].[RuleId]	AS [RuleName], [EMLU].[Severity] AS [Severity],
'' AS [FileLevelError]
FROM 
CTE_RulebaseErrorOutput VE
LEFT JOIN [Static].[ErrorMessageLookUp] EMLU ON VE.[RuleId] = EMLU.RuleName 
LEFT JOIN [Input].[LearningDelivery] LD ON VE.AimSeqNumber = LD.AimSeqNumber AND VE.LearnRefNumber = LD.LearnRefNumber
WHERE [VE].[RuleId] NOT LIKE 'Schema'
)
,CTE_SchemaError AS
(
SELECT 'Schema' AS [Source],'' AS [LearnAimRef],'' AS [AimSeqNum], '' [SWSupAimID], [ErrorMessage] AS [ErrorMessage],
[FieldValues] AS [FieldValues], [LearnRefNumber] AS [LearnRefNumber],[RuleName]	AS [RuleName], [Severity] AS [Severity],
'' AS [FileLevelError]
FROM [dbo].[SchemaValidationError]
)
SELECT * FROM CTE_SQLValidationError
UNION ALL
SELECT * FROM CTE_RulebaseValidationError
UNION ALL
SELECT * FROM CTE_SchemaError
