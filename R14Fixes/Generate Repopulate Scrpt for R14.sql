declare @collectionPeriodName varchar(8) = '1718-R14'
declare @ukprn1 bigint = 1;
declare @ukprn2 bigint = 2;

set nocount on;

PRINT 'BEGIN TRANSACTION'

SELECT 'INSERT INTO [PaymentsDue].[RequiredPayments](
[Id]
,[CommitmentId]
,[CommitmentVersionId]
,[AccountId]
,[AccountVersionId]
,[Uln]
,[LearnRefNumber]
,[AimSeqNumber]
,[Ukprn]
,[IlrSubmissionDateTime]
,[PriceEpisodeIdentifier]
,[StandardCode]
,[ProgrammeType]
,[FrameworkCode]
,[PathwayCode]
,[ApprenticeshipContractType]
,[DeliveryMonth]
,[DeliveryYear]
,[CollectionPeriodName]
,[CollectionPeriodMonth]
,[CollectionPeriodYear]
,[TransactionType]
,[AmountDue]
,[SfaContributionPercentage]
,[FundingLineType]
,[UseLevyBalance]
,[LearnAimRef]
,[LearningStartDate]
) VALUES (	
' + ISNULL('''' + cast([Id] as sysname) + '''' , 'NULL')  + '	
,' + ISNULL(cast([CommitmentId] as sysname), 'NULL')  + '	
,' + ISNULL('''' + [CommitmentVersionId] + '''' , 'NULL')  + '	
,' + ISNULL(cast([AccountId] as sysname), 'NULL')  + '	
,' + ISNULL('''' + [AccountVersionId] + '''' , 'NULL')  + '	
,' + ISNULL(cast([Uln] as sysname), 'NULL')  + '	
,' + ISNULL('''' + [LearnRefNumber] + '''' , 'NULL')  + '	
,' + ISNULL(cast([AimSeqNumber] as sysname), 'NULL')  + '	
,' + ISNULL(cast([Ukprn] as sysname), 'NULL')  + '	
,' + ISNULL('''' + convert(sysname, [IlrSubmissionDateTime], 120) + '''', 'NULL')  + '	
,' + ISNULL('''' + [PriceEpisodeIdentifier] + '''' , 'NULL')  + '	
,' + ISNULL(cast([StandardCode] as sysname), 'NULL')  + '	
,' + ISNULL(cast([ProgrammeType] as sysname), 'NULL')  + '	
,' + ISNULL(cast([FrameworkCode] as sysname), 'NULL')  + '	
,' + ISNULL(cast([PathwayCode] as sysname), 'NULL')  + '	
,' + ISNULL(cast([ApprenticeshipContractType] as sysname), 'NULL')  + '	
,' + ISNULL(cast([DeliveryMonth] as sysname), 'NULL')  + '	
,' + ISNULL(cast([DeliveryYear] as sysname), 'NULL')  + '	
,' + ISNULL('''' + [CollectionPeriodName] + '''' , 'NULL')  + '	
,' + ISNULL(cast([CollectionPeriodMonth] as sysname), 'NULL')  + '	
,' + ISNULL(cast([CollectionPeriodYear] as sysname), 'NULL')  + '	
,' + ISNULL(cast([TransactionType] as sysname), 'NULL')  + '	
,' + ISNULL(cast([AmountDue] as sysname), 'NULL')  + '	
,' + ISNULL(cast([SfaContributionPercentage] as sysname), 'NULL')  + '	
,' + ISNULL('''' + [FundingLineType] + '''' , 'NULL')  + '	
,' + ISNULL(cast([UseLevyBalance] as sysname), 'NULL')  + '	
,' + ISNULL('''' + [LearnAimRef] + '''' , 'NULL')  + '	
,' + ISNULL('''' + convert(sysname, [LearningStartDate], 120) + '''', 'NULL')  + '	
	
)' FROM [PaymentsDue].[RequiredPayments]	
WHERE [CollectionPeriodName] = @collectionPeriodName	
	AND [Ukprn] IN (@ukprn1, @ukprn2);




SELECT 'INSERT INTO [PaymentsDue].[NonPayableEarnings](
[Id]
,[CommitmentId]
,[CommitmentVersionId]
,[AccountId]
,[AccountVersionId]
,[Uln]
,[LearnRefNumber]
,[AimSeqNumber]
,[Ukprn]
,[IlrSubmissionDateTime]
,[PriceEpisodeIdentifier]
,[StandardCode]
,[ProgrammeType]
,[FrameworkCode]
,[PathwayCode]
,[ApprenticeshipContractType]
,[DeliveryMonth]
,[DeliveryYear]
,[CollectionPeriodName]
,[CollectionPeriodMonth]
,[CollectionPeriodYear]
,[TransactionType]
,[AmountDue]
,[SfaContributionPercentage]
,[FundingLineType]
,[UseLevyBalance]
,[LearnAimRef]
,[LearningStartDate]
,[PaymentFailureMessage]
,[PaymentFailureReason]
) VALUES (		
' + ISNULL('''' + cast([Id] as sysname) + '''' , 'NULL')  + '		
,' + ISNULL(cast([CommitmentId] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [CommitmentVersionId] + '''' , 'NULL')  + '		
,' + ISNULL(cast([AccountId] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [AccountVersionId] + '''' , 'NULL')  + '		
,' + ISNULL(cast([Uln] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [LearnRefNumber] + '''' , 'NULL')  + '		
,' + ISNULL(cast([AimSeqNumber] as sysname), 'NULL')  + '		
,' + ISNULL(cast([Ukprn] as sysname), 'NULL')  + '		
,' + ISNULL('''' + convert(sysname, [IlrSubmissionDateTime], 120) + '''', 'NULL')  + '		
,' + ISNULL('''' + [PriceEpisodeIdentifier] + '''' , 'NULL')  + '		
,' + ISNULL(cast([StandardCode] as sysname), 'NULL')  + '		
,' + ISNULL(cast([ProgrammeType] as sysname), 'NULL')  + '		
,' + ISNULL(cast([FrameworkCode] as sysname), 'NULL')  + '		
,' + ISNULL(cast([PathwayCode] as sysname), 'NULL')  + '		
,' + ISNULL(cast([ApprenticeshipContractType] as sysname), 'NULL')  + '		
,' + ISNULL(cast([DeliveryMonth] as sysname), 'NULL')  + '		
,' + ISNULL(cast([DeliveryYear] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [CollectionPeriodName] + '''' , 'NULL')  + '		
,' + ISNULL(cast([CollectionPeriodMonth] as sysname), 'NULL')  + '		
,' + ISNULL(cast([CollectionPeriodYear] as sysname), 'NULL')  + '		
,' + ISNULL(cast([TransactionType] as sysname), 'NULL')  + '		
,' + ISNULL(cast([AmountDue] as sysname), 'NULL')  + '		
,' + ISNULL(cast([SfaContributionPercentage] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [FundingLineType] + '''' , 'NULL')  + '		
,' + ISNULL(cast([UseLevyBalance] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [LearnAimRef] + '''' , 'NULL')  + '		
,' + ISNULL('''' + convert(sysname, [LearningStartDate], 120) + '''', 'NULL')  + '		
,' + ISNULL('''' + [PaymentFailureMessage] + '''' , 'NULL')  + '		
,' + ISNULL(cast([PaymentFailureReason] as sysname), 'NULL')  + '		
		
)' FROM [PaymentsDue].[NonPayableEarnings]		
WHERE [CollectionPeriodName] = @collectionPeriodName		
	AND [Ukprn] IN (@ukprn1, @ukprn2);	




SELECT 'INSERT INTO [PaymentsDue].[Earnings](
[RequiredPaymentId]
,[StartDate]
,[PlannedEndDate]
,[ActualEnddate]
,[CompletionStatus]
,[CompletionAmount]
,[MonthlyInstallment]
,[TotalInstallments]
,[EndpointAssessorId]
) VALUES (		
' + ISNULL('''' + cast([RequiredPaymentId] as sysname) + '''' , 'NULL')  + '		
,' + ISNULL('''' + convert(sysname, [StartDate], 120) + '''', 'NULL')  + '		
,' + ISNULL('''' + convert(sysname, [PlannedEndDate], 120) + '''', 'NULL')  + '		
,' + ISNULL('''' + convert(sysname, [ActualEnddate], 120) + '''', 'NULL')  + '		
,' + ISNULL(cast([CompletionStatus] as sysname), 'NULL')  + '		
,' + ISNULL(cast([CompletionAmount] as sysname), 'NULL')  + '		
,' + ISNULL(cast([MonthlyInstallment] as sysname), 'NULL')  + '		
,' + ISNULL(cast([TotalInstallments] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [EndpointAssessorId] + '''' , 'NULL')  + '		
		
)' FROM [PaymentsDue].[Earnings] E 		
WHERE EXISTS (		
	SELECT Id 	
	FROM PaymentsDue.RequiredPayments R	
	WHERE [CollectionPeriodName] = @collectionPeriodName	
		AND E.RequiredPaymentId = R.Id
		AND R.[Ukprn] IN (@ukprn1, @ukprn2)
);		
		




SELECT 'INSERT INTO [Payments].[Payments](
[PaymentId]
,[RequiredPaymentId]
,[DeliveryMonth]
,[DeliveryYear]
,[CollectionPeriodName]
,[CollectionPeriodMonth]
,[CollectionPeriodYear]
,[FundingSource]
,[TransactionType]
,[Amount]
) VALUES (		
' + ISNULL('''' + cast([PaymentId] as sysname) + '''' , 'NULL')  + '		
,' + ISNULL('''' + cast([RequiredPaymentId] as sysname) + '''' , 'NULL')  + '		
,' + ISNULL(cast([DeliveryMonth] as sysname), 'NULL')  + '		
,' + ISNULL(cast([DeliveryYear] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [CollectionPeriodName] + '''' , 'NULL')  + '		
,' + ISNULL(cast([CollectionPeriodMonth] as sysname), 'NULL')  + '		
,' + ISNULL(cast([CollectionPeriodYear] as sysname), 'NULL')  + '		
,' + ISNULL(cast([FundingSource] as sysname), 'NULL')  + '		
,' + ISNULL(cast([TransactionType] as sysname), 'NULL')  + '		
,' + ISNULL(cast([Amount] as sysname), 'NULL')  + '		
		
)' FROM [Payments].[Payments]		
WHERE [RequiredPaymentId] IN (SELECT [Id] FROM [PaymentsDue].[RequiredPayments] WHERE [Ukprn] IN (@ukprn1, @ukprn2))		
	AND [CollectionPeriodName] = @collectionPeriodName;	



SELECT 'INSERT INTO [ProviderAdjustments].[Payments](
[Ukprn]
,[SubmissionId]
,[SubmissionCollectionPeriod]
,[SubmissionAcademicYear]
,[PaymentType]
,[PaymentTypeName]
,[Amount]
,[CollectionPeriodName]
,[CollectionPeriodMonth]
,[CollectionPeriodYear]
) VALUES (		
' + ISNULL(cast([Ukprn] as sysname), 'NULL')  + '		
,' + ISNULL('''' + cast([SubmissionId] as sysname) + '''' , 'NULL')  + '		
,' + ISNULL(cast([SubmissionCollectionPeriod] as sysname), 'NULL')  + '		
,' + ISNULL(cast([SubmissionAcademicYear] as sysname), 'NULL')  + '		
,' + ISNULL(cast([PaymentType] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [PaymentTypeName] + '''' , 'NULL')  + '		
,' + ISNULL(cast([Amount] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [CollectionPeriodName] + '''' , 'NULL')  + '		
,' + ISNULL(cast([CollectionPeriodMonth] as sysname), 'NULL')  + '		
,' + ISNULL(cast([CollectionPeriodYear] as sysname), 'NULL')  + '		
		
)' FROM [ProviderAdjustments].[Payments]		
WHERE CollectionPeriodName = @collectionPeriodName AND Ukprn IN (@ukprn1, @ukprn2);		



SELECT 'INSERT INTO Adjustments.ManualAdjustments(
[RequiredPaymentIdToReverse]
,[ReasonForReversal]
,[RequestorName]
,[DateUploaded]
,[RequiredPaymentIdForReversal]
) VALUES (			
' + ISNULL('''' + cast([RequiredPaymentIdToReverse] as sysname) + '''' , 'NULL')  + '			
,' + ISNULL('''' + [ReasonForReversal] + '''' , 'NULL')  + '			
,' + ISNULL('''' + [RequestorName] + '''' , 'NULL')  + '			
,' + ISNULL('''' + convert(sysname, [DateUploaded], 120) + '''', 'NULL')  + '			
,' + ISNULL('''' + cast([RequiredPaymentIdForReversal] as sysname) + '''' , 'NULL')  + '			
			
)' FROM Adjustments.ManualAdjustments			
WHERE RequiredPaymentIdToReverse IN 			
	(		
		SELECT [Id] 	
		FROM [PaymentsDue].[RequiredPayments] 	
		WHERE [Ukprn] IN (@ukprn1, @ukprn2) 	
			AND CollectionPeriodName = @collectionPeriodName
	);		




SELECT 'INSERT INTO [DataLock].[PriceEpisodePeriodMatch](
[Ukprn]
,[PriceEpisodeIdentifier]
,[LearnRefNumber]
,[AimSeqNumber]
,[CommitmentId]
,[VersionId]
,[Period]
,[Payable]
,[TransactionType]
,[CollectionPeriodName]
,[CollectionPeriodMonth]
,[CollectionPeriodYear]
,[TransactionTypesFlag]
) VALUES (			
' + ISNULL(cast([Ukprn] as sysname), 'NULL')  + '			
,' + ISNULL('''' + [PriceEpisodeIdentifier] + '''' , 'NULL')  + '			
,' + ISNULL('''' + [LearnRefNumber] + '''' , 'NULL')  + '			
,' + ISNULL(cast([AimSeqNumber] as sysname), 'NULL')  + '			
,' + ISNULL(cast([CommitmentId] as sysname), 'NULL')  + '			
,' + ISNULL('''' + [VersionId] + '''' , 'NULL')  + '			
,' + ISNULL(cast([Period] as sysname), 'NULL')  + '			
,' + ISNULL(cast([Payable] as sysname), 'NULL')  + '			
,' + ISNULL(cast([TransactionType] as sysname), 'NULL')  + '			
,' + ISNULL('''' + [CollectionPeriodName] + '''' , 'NULL')  + '			
,' + ISNULL(cast([CollectionPeriodMonth] as sysname), 'NULL')  + '			
,' + ISNULL(cast([CollectionPeriodYear] as sysname), 'NULL')  + '			
,' + ISNULL(cast([TransactionTypesFlag] as sysname), 'NULL')  + '			
			
)' FROM [DataLock].[PriceEpisodePeriodMatch]			
WHERE [CollectionPeriodName]= @CollectionPeriodName AND [Ukprn] IN (@ukprn1, @ukprn2);			



SELECT 'INSERT INTO [DataLock].[PriceEpisodeMatch](
[Ukprn]
,[PriceEpisodeIdentifier]
,[LearnRefNumber]
,[AimSeqNumber]
,[CommitmentId]
,[CollectionPeriodName]
,[CollectionPeriodMonth]
,[CollectionPeriodYear]
,[IsSuccess]
) VALUES (		
' + ISNULL(cast([Ukprn] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [PriceEpisodeIdentifier] + '''' , 'NULL')  + '		
,' + ISNULL('''' + [LearnRefNumber] + '''' , 'NULL')  + '		
,' + ISNULL(cast([AimSeqNumber] as sysname), 'NULL')  + '		
,' + ISNULL(cast([CommitmentId] as sysname), 'NULL')  + '		
,' + ISNULL('''' + [CollectionPeriodName] + '''' , 'NULL')  + '		
,' + ISNULL(cast([CollectionPeriodMonth] as sysname), 'NULL')  + '		
,' + ISNULL(cast([CollectionPeriodYear] as sysname), 'NULL')  + '		
,' + ISNULL(cast([IsSuccess] as sysname), 'NULL')  + '		
		
)' FROM [DataLock].[PriceEpisodeMatch]		
WHERE [CollectionPeriodName]= @CollectionPeriodName AND [Ukprn] IN (@ukprn1, @ukprn2);		



SELECT 'INSERT INTO [DataLock].[ValidationErrorByPeriod](
[Ukprn]
,[LearnRefNumber]
,[AimSeqNumber]
,[RuleId]
,[PriceEpisodeIdentifier]
,[Period]
,[CollectionPeriodName]
,[CollectionPeriodMonth]
,[CollectionPeriodYear]
) VALUES (			
' + ISNULL(cast([Ukprn] as sysname), 'NULL')  + '			
,' + ISNULL('''' + [LearnRefNumber] + '''' , 'NULL')  + '			
,' + ISNULL(cast([AimSeqNumber] as sysname), 'NULL')  + '			
,' + ISNULL('''' + [RuleId] + '''' , 'NULL')  + '			
,' + ISNULL('''' + [PriceEpisodeIdentifier] + '''' , 'NULL')  + '			
,' + ISNULL(cast([Period] as sysname), 'NULL')  + '			
,' + ISNULL('''' + [CollectionPeriodName] + '''' , 'NULL')  + '			
,' + ISNULL(cast([CollectionPeriodMonth] as sysname), 'NULL')  + '			
,' + ISNULL(cast([CollectionPeriodYear] as sysname), 'NULL')  + '			
			
)' FROM [DataLock].[ValidationErrorByPeriod]			
WHERE [CollectionPeriodName]= @CollectionPeriodName AND [Ukprn] IN (@ukprn1, @ukprn2);			




SELECT 'INSERT INTO [DataLock].[ValidationError](
[Ukprn]
,[LearnRefNumber]
,[AimSeqNumber]
,[RuleId]
,[PriceEpisodeIdentifier]
,[CollectionPeriodName]
,[CollectionPeriodMonth]
,[CollectionPeriodYear]
) VALUES (			
' + ISNULL(cast([Ukprn] as sysname), 'NULL')  + '			
,' + ISNULL('''' + [LearnRefNumber] + '''' , 'NULL')  + '			
,' + ISNULL(cast([AimSeqNumber] as sysname), 'NULL')  + '			
,' + ISNULL('''' + [RuleId] + '''' , 'NULL')  + '			
,' + ISNULL('''' + [PriceEpisodeIdentifier] + '''' , 'NULL')  + '			
,' + ISNULL('''' + [CollectionPeriodName] + '''' , 'NULL')  + '			
,' + ISNULL(cast([CollectionPeriodMonth] as sysname), 'NULL')  + '			
,' + ISNULL(cast([CollectionPeriodYear] as sysname), 'NULL')  + '			
			
)' FROM [DataLock].[ValidationError]			
WHERE [CollectionPeriodName]= @CollectionPeriodName AND [Ukprn] IN (@ukprn1, @ukprn2);			


PRINT 'ROLLBACK TRANSACTION'
PRINT '--COMMIT TRANSACTION'


