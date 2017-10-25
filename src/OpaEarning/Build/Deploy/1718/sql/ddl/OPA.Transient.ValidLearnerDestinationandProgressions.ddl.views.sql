IF OBJECT_ID('ValidLearnerDestinationandProgressions') IS NOT NULL
BEGIN
    DROP VIEW ValidLearnerDestinationandProgressions
END
GO
CREATE VIEW ValidLearnerDestinationandProgressions 
AS
SELECT LDP.LearnerDestinationandProgression_Id
FROM [Input].[LearnerDestinationandProgression] LDP
--WHERE NOT EXISTS ( SELECT * FROM [Report].[ValidationError] WHERE 
--LearnRefNumber=LDP.LearnRefNumber AND Severity='E' AND 
--SOURCE IN('Validation DP 17_18.zip','FD Validation DP 17_18.zip','SQL DP'))
--AND LDP.LearnRefNumber IS NOT NULL