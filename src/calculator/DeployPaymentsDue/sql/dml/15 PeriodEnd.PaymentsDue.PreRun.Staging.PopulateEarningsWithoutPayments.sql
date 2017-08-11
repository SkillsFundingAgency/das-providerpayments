TRUNCATE TABLE Staging.EarningsWithoutPayments
GO

INSERT INTO Staging.EarningsWithoutPayments
Select ph.*

FROM Reference.RequiredPaymentsHistory ph

JOIN (

	SELECT SUM(AmountDue) Amount, LearnRefNumber,Ukprn,DeliveryMonth,DeliveryYear, TransactionType
	From Reference.RequiredPaymentsHistory 
	Group By LearnRefNumber,Ukprn,DeliveryMonth,DeliveryYear,TransactionType
	having Sum(AmountDue) <>0
	) phTotal

	On ph.LearnRefNumber = phTotal.LearnRefNumber
	And ph.Ukprn = phTotal.Ukprn
	And ph.DeliveryMonth = phTotal.DeliveryMonth
	And ph.DeliveryYear = phTotal.DeliveryYear
	and ph.TransactionType = phTotal.TransactionType

LEFT JOIN PaymentsDue.vw_ApprenticeshipEarning e
	
ON ph.Ukprn = e.Ukprn
	And ph.LearnAimref = e.LearnAimref
	AND ph.LearnRefNumber = e.LearnRefNumber
       and IsNull(ph.StandardCode,0) = IsNull(e.StandardCode,0)
       and IsNull(ph.FrameworkCode,0) = IsNull(e.FrameworkCode,0)
       and IsNull(ph.PathwayCode ,0)= IsNull(e.PathwayCode,0)
       and (IsNull(ph.ProgrammeType,0) = IsNull(e.ProgrammeType,0) OR  IsNull(ph.StandardCode,0) > 0)

WHERE (e.LearnRefNumber IS NULL or e.LearnAimRef is null)
AND ph.CollectionPeriodName LIKE '${YearOfCollection}-%'

