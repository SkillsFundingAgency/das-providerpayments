INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', 
	'05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 
	'Starting population')

IF EXISTS (SELECT NULL FROM sys.tables WHERE name = 'Deds_AEC_ApprenticeshipPriceEpisode')
BEGIN
	DROP TABLE  Reference.Deds_AEC_ApprenticeshipPriceEpisode
END
GO

SELECT * 
INTO Reference.Deds_AEC_ApprenticeshipPriceEpisode
FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode]

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Deds_AEC_ApprenticeshipPriceEpisode')
GO



IF EXISTS (SELECT NULL FROM sys.tables WHERE name = 'Deds_AEC_ApprenticeshipPriceEpisode_Period')
BEGIN
	DROP TABLE  Reference.Deds_AEC_ApprenticeshipPriceEpisode_Period
END
GO

SELECT * 
INTO Reference.Deds_AEC_ApprenticeshipPriceEpisode_Period
FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Reference.Deds_AEC_ApprenticeshipPriceEpisode_Period')
GO



IF EXISTS (SELECT NULL FROM sys.tables WHERE name = 'Deds_Valid_Learner')
BEGIN
	DROP TABLE  Reference.Deds_Valid_Learner
END
GO

SELECT * 
INTO Reference.Deds_Valid_Learner
FROM ${ILR_Deds.FQ}.[Valid].[Learner]

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Reference.Deds_Valid_Learner')   
GO



IF EXISTS (SELECT NULL FROM sys.tables WHERE name = 'Deds_Valid_LearningDelivery')
BEGIN
	DROP TABLE  Reference.Deds_Valid_LearningDelivery
END
GO


SELECT * 
INTO Reference.Deds_Valid_LearningDelivery
FROM ${ILR_Deds.FQ}.[Valid].[LearningDelivery]

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Reference.Deds_Valid_LearningDelivery')
GO



IF EXISTS (SELECT NULL FROM sys.tables WHERE name = 'Deds_AEC_LearningDelivery')
BEGIN
	DROP TABLE  Reference.Deds_AEC_LearningDelivery
END
GO


SELECT * 
INTO Reference.Deds_AEC_LearningDelivery
FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery]

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Reference.Deds_AEC_LearningDelivery')
GO



IF EXISTS (SELECT NULL FROM sys.tables WHERE name = 'Deds_AEC_LearningDelivery_Period')
BEGIN
	DROP TABLE  Reference.Deds_AEC_LearningDelivery_Period
END
GO

SELECT *
INTO Reference.Deds_AEC_LearningDelivery_Period
FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery_Period] 

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Reference.Deds_AEC_LearningDelivery_Period')
GO


CREATE INDEX IX_Deds_AEC_ApprenticeshipPriceEpisode ON Reference.Deds_AEC_ApprenticeshipPriceEpisode (UKPRN, LearnRefNumber, PriceEpisodeIdentifier, PriceEpisodeAimSeqNumber)
INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Index created for IX_Deds_AEC_ApprenticeshipPriceEpisode')
GO


CREATE INDEX IX_Deds_AEC_ApprenticeshipPriceEpisode_Period ON Reference.Deds_AEC_ApprenticeshipPriceEpisode_Period (UKPRN, LearnRefNumber, PriceEpisodeIdentifier)
INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Index created for IX_Deds_AEC_ApprenticeshipPriceEpisode_Period')
GO


CREATE INDEX IX_Deds_Valid_Learner ON Reference.Deds_Valid_Learner (UKPRN, LearnRefNumber)
INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Index created for IX_Deds_Valid_Learner')
GO


CREATE INDEX IX_Deds_Valid_LearningDelivery ON Reference.Deds_Valid_LearningDelivery (UKPRN, LearnRefNumber, AimSeqNumber)
INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Index created for IX_Deds_Valid_LearningDelivery')
GO


CREATE INDEX IX_Deds_AEC_LearningDelivery ON Reference.Deds_AEC_LearningDelivery (UKPRN, LearnRefNumber, AimSeqNumber)
INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Index created for IX_Deds_AEC_LearningDelivery')
GO


CREATE INDEX IX_Deds_AEC_LearningDelivery_Period ON Reference.Deds_AEC_LearningDelivery_Period (UKPRN, LearnRefNumber, AimSeqNumber)
INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', '05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 'Index created for IX_Deds_AEC_LearningDelivery_Period')
GO   