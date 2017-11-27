
-----------------------------------------------------------------------------------------------------------------------------------------------
-- Earnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Earnings' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP TABLE PaymentsDue.Earnings
END
GO
CREATE TABLE PaymentsDue.Earnings
(
    RequiredPaymentId uniqueidentifier NOT NULL, 
    StartDate datetime NOT NULL,
    PlannedEndDate datetime NOT NULL,
	ActualEnddate datetime,
    CompletionStatus int,
    CompletionAmount decimal(15,5),
	MonthlyInstallment decimal(15,5) NOT NULL,
	TotalInstallments int NOT NULL,
	EndpointAssessorId varchar(7) NULL
)