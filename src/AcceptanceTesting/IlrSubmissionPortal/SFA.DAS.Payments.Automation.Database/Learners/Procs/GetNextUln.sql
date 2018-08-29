
CREATE PROCEDURE [Learners].[GetNextUln]
	@LearnRefNumber nvarchar(12),
	@ScenarioName nvarchar(max),
	@Ukprn bigint
AS
	Declare @ULN bigint = (SELECT Top 1 ULN FROM Learners.ULNs
							WHERE Ukprn = @Ukprn AND ScenarioName = @ScenarioName AND LearnRefNumber = @LearnRefNumber And Used = 1)

	If ( @ULN Is Null)
		Begin
			Set @ULN = (SELECT Top 1 ULN FROM Learners.ULNs 
						WHERE IsNull(USED,0) = 0 ORDER BY Id)
	
			Update Learners.ULNs Set Used = 1, 
			ScenarioName = @ScenarioName, 
			LearnRefNumber = @LearnRefNumber,
			Ukprn = @Ukprn
			WHERE ULN = @ULN
		End

	SELECT @ULN
GO

