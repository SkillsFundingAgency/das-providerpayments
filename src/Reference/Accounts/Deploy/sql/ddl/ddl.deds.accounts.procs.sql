IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='DeleteAccounts' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP PROCEDURE dbo.DeleteAccounts
END
GO
-- =============================================
CREATE PROCEDURE dbo.DeleteAccounts 
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM [dbo].[DasAccounts]

END
GO
