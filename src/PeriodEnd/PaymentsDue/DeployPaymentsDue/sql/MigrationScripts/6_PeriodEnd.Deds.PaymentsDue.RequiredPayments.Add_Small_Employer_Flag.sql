IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='RequiredPayments'
	AND s.name='PaymentsDue'
	AND c.name='IsSmallEmployer'
)
BEGIN
ALTER TABLE  PaymentsDue.RequiredPayments
  Add IsSmallEmployer bit NOT NULL DEFAULT 0
END