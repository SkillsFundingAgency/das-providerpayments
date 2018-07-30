declare @collectionPeriodName varchar(8) = (
	SELECT [Collection_Period_Name] 
	FROM ${DAS_PeriodEnd.FQ}.dbo.Collection_Period_Mapping 
	WHERE [Collection_Open] = 1
);

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
