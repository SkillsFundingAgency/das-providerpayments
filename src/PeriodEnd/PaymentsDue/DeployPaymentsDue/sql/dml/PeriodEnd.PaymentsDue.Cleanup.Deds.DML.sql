DECLARE @CollectionPeriodName varchar(8) = (SELECT [AcademicYear] FROM Reference.CollectionPeriods WHERE [Open] = 1) + '-' + (SELECT [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1)


DELETE E 
FROM ${DAS_PeriodEnd.FQ}.[PaymentsDue].[Earnings] E
WHERE EXISTS (
	SELECT Id 
	FROM PaymentsDue.RequiredPayments R
	WHERE [CollectionPeriodName] = @collectionPeriodName
	AND E.RequiredPaymentId = R.Id
);

DELETE 
FROM ${DAS_PeriodEnd.FQ}.[PaymentsDue].[RequiredPayments]
WHERE [CollectionPeriodName] = @collectionPeriodName;

DELETE 
FROM ${DAS_PeriodEnd.FQ}.[PaymentsDue].[NonPayableEarnings]
WHERE [CollectionPeriodName] = @collectionPeriodName;

GO
