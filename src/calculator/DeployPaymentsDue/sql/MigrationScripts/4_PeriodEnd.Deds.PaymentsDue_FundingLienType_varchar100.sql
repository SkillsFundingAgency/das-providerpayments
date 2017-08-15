IF (SELECT max_length FROM sys.columns WHERE [object_id] = OBJECT_ID('PaymentsDue.RequiredPayments') AND [name] = 'FundingLineType') <> 100
	BEGIN
		ALTER TABLE PaymentsDue.RequiredPayments
		ALTER COLUMN FundingLineType varchar(100)
	END