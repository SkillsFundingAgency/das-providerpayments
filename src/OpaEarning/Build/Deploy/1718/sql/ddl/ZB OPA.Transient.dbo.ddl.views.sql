IF OBJECT_ID('ValidLearnerDestinationandProgressions') IS NOT NULL
BEGIN
	DROP VIEW ValidLearnerDestinationandProgressions
END
GO
CREATE VIEW ValidLearnerDestinationandProgressions 
AS
SELECT LDP.LearnerDestinationandProgression_Id
FROM [Input].[LearnerDestinationandProgression] LDP
WHERE NOT EXISTS ( SELECT * FROM [Report].[ValidationError] WHERE LearnRefNumber=LDP.LearnRefNumber AND Severity='E' AND SOURCE IN('Validation DP 17_18.zip','FD Validation DP 17_18.zip','SQL DP'))
AND LDP.LearnRefNumber IS NOT NULL

GO

IF OBJECT_ID('ValidLearners') IS NOT NULL
BEGIN
	DROP VIEW ValidLearners
END
GO
CREATE VIEW ValidLearners 
AS
SELECT L.Learner_Id
FROM [Input].[Learner] L
WHERE NOT EXISTS ( SELECT * FROM [Report].[ValidationError] WHERE LearnRefNumber=L.LearnRefNumber AND Severity='E'  AND Source NOT IN ('Validation DP 17_18.zip','FD Validation DP 17_18.zip','SQL DP'))
AND L.LearnRefNumber IS NOT NULL
