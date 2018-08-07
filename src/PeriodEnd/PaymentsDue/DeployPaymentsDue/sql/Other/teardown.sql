IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RawEarnings' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_RawEarnings
END
GO

IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RawEarningsMathsEnglish' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_RawEarningsMathsEnglish
END
GO

IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_IlrBreakdown' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_IlrBreakdown
END
GO