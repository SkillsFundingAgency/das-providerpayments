IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Refunds')
BEGIN
    EXEC('CREATE SCHEMA Refunds')
END
GO

