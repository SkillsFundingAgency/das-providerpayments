IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
    EXEC('CREATE SCHEMA PaymentsDue')
END
GO
