TRUNCATE TABLE Staging.EarningsWithoutPayments
GO

INSERT INTO Staging.EarningsWithoutPayments
Select ph.*

FROM Reference.RequiredPaymentsHistory ph
LEFT JOIN PaymentsDue.vw_ApprenticeshipEarning e
ON ph.Ukprn = e.Ukprn
	And ph.LearnAimref = e.LearnAimref
	AND ph.LearnRefNumber = e.LearnRefNumber
       and IsNull(ph.StandardCode,0) = IsNull(e.StandardCode,0)
       and IsNull(ph.FrameworkCode,0) = IsNull(e.FrameworkCode,0)
       and IsNull(ph.PathwayCode ,0)= IsNull(e.PathwayCode,0)
       and (IsNull(ph.ProgrammeType,0) = IsNull(e.ProgrammeType,0) OR  IsNull(ph.StandardCode,0) > 0)
       and e.Period =  case When DeliveryMonth between 1 and 7 Then DeliveryMonth + 5
						Else DeliveryMonth - 7 END	 
WHERE 
(e.LearnRefNumber IS NULL or e.LearnAimRef is null ) 
AND
EXISTS (Select top 1 * from Staging.CollectionPeriods cp Where [Open] = 1 And DATEFROMPARTS(ph.DeliveryYear,ph.DeliveryMonth,1) between cp.FirstDayOfAcademicYear and DATEADD(day,364,cp.FirstDayOfAcademicYear)) 
AND 
ph.CollectionPeriodName LIKE '${YearOfCollection}-%' 

AND NOT EXISTS(
Select 1 from Reference.RequiredPaymentsHistory p Where 
	ph.LearnRefNumber = p.LearnRefNumber
	And ph.Ukprn = p.Ukprn
	And ph.DeliveryMonth = p.DeliveryMonth
	And ph.DeliveryYear = p.DeliveryYear
	and ph.TransactionType = p.TransactionType
	And IsNull(ph.StandardCode,0) = IsNull(p.StandardCode,0)
	And IsNull(ph.FrameworkCode,0) = IsNull(p.FrameworkCode,0)
	And IsNull(ph.ProgrammeType,0) = IsNull(p.ProgrammeType,0)
	And IsNull(ph.PathwayCode,0) = IsNull(p.PathwayCode,0)
	And ph.LearnAimRef = p.LearnAimRef
	And ph.AmountDue *-1  = p.AmountDue
	And p.Id <> ph.Id
)
