
BEGIN TRANSACTION

declare @collectionPeriodName varchar(8) = '1718-R14'
declare @ukprn1 bigint = 1;
declare @ukprn2 bigint = 2;

PRINT 'Delete [DataLock].[ValidationError]'
DELETE 
FROM [DataLock].[ValidationError]
WHERE [CollectionPeriodName]= @CollectionPeriodName AND [Ukprn] IN (@ukprn1, @ukprn2);

PRINT 'Delete [DataLock].[PriceEpisodeMatch]'
DELETE 
FROM [DataLock].[ValidationErrorByPeriod]
WHERE [CollectionPeriodName]= @CollectionPeriodName AND [Ukprn] IN (@ukprn1, @ukprn2);

PRINT 'Delete [DataLock].[PriceEpisodeMatch]'
DELETE 
FROM [DataLock].[PriceEpisodeMatch]
WHERE [CollectionPeriodName]= @CollectionPeriodName AND [Ukprn] IN (@ukprn1, @ukprn2);

PRINT 'Delete [DataLock].[PriceEpisodePeriodMatch]'
DELETE 
FROM [DataLock].[PriceEpisodePeriodMatch]
WHERE [CollectionPeriodName]= @CollectionPeriodName AND [Ukprn] IN (@ukprn1, @ukprn2);

-- data lock events

--DELETE FROM [DataLock].[DataLockEventPeriods]
--WHERE DataLockEventId in (SELECT DataLockEventId FROM [DataLock].[DataLockEvents] WHERE UKPRN IN (@ukprn1, @ukprn2))
--	AND CollectionPeriodName = @collectionPeriodName;

-- DataLockEvents?
-- DataLockEventErrors?
-- DataLockEventCommitmentVersions?


-- manual adjustments
PRINT 'Update Adjustments.ManualAdjustments'
UPDATE Adjustments.ManualAdjustments
SET RequiredPaymentIdForReversal = NULL
WHERE RequiredPaymentIdForReversal IN 
	(
		SELECT [Id] 
		FROM [PaymentsDue].[RequiredPayments] 
		WHERE [Ukprn] IN (@ukprn1, @ukprn2) 
			AND CollectionPeriodName = @collectionPeriodName
	);
	 
PRINT 'Delete Adjustments.ManualAdjustments'
DELETE
FROM Adjustments.ManualAdjustments
WHERE RequiredPaymentIdToReverse IN 
	(
		SELECT [Id] 
		FROM [PaymentsDue].[RequiredPayments] 
		WHERE [Ukprn] IN (@ukprn1, @ukprn2) 
			AND CollectionPeriodName = @collectionPeriodName
	);

-- provider adjustments
PRINT 'Delete [ProviderAdjustments].[Payments]'
DELETE
FROM [ProviderAdjustments].[Payments]
WHERE CollectionPeriodName = @collectionPeriodName AND Ukprn IN (@ukprn1, @ukprn2);

-- payments
PRINT 'Delete [Payments].[Payments]'
DELETE 
FROM [Payments].[Payments]
WHERE [RequiredPaymentId] IN (SELECT [Id] FROM [PaymentsDue].[RequiredPayments] WHERE [Ukprn] IN (@ukprn1, @ukprn2))
	AND [CollectionPeriodName] = @collectionPeriodName;

PRINT 'Delete [PaymentsDue].[Earnings]'
DELETE E 
FROM [PaymentsDue].[Earnings] E
WHERE EXISTS (
	SELECT Id 
	FROM PaymentsDue.RequiredPayments R
	WHERE [CollectionPeriodName] = @collectionPeriodName
		AND E.RequiredPaymentId = R.Id
		AND R.[Ukprn] IN (@ukprn1, @ukprn2)
);

PRINT 'Delete [PaymentsDue].[RequiredPayments]'
DELETE 
FROM [PaymentsDue].[RequiredPayments]
WHERE [CollectionPeriodName] = @collectionPeriodName
	AND [Ukprn] IN (@ukprn1, @ukprn2);

PRINT 'Delete [PaymentsDue].[NonPayableEarnings]'
DELETE 
FROM [PaymentsDue].[NonPayableEarnings]
WHERE [CollectionPeriodName] = @collectionPeriodName
	AND [Ukprn] IN (@ukprn1, @ukprn2);



--ROLLBACK TRANSACTION 
COMMIT TRANSACTION 




