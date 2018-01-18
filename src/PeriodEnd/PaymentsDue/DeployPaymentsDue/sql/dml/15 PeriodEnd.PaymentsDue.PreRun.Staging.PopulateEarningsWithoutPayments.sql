TRUNCATE TABLE Staging.EarningsWithoutPayments
GO

INSERT INTO Staging.EarningsWithoutPayments

Select ph.*

FROM Reference.RequiredPaymentsHistory ph --previous ILR data
LEFT JOIN PaymentsDue.vw_ApprenticeshipEarning e --current ILR data
ON ph.Ukprn = e.Ukprn
	And ph.LearnAimref = e.LearnAimref
	AND ph.LearnRefNumber = e.LearnRefNumber
       and IsNull(ph.StandardCode,0) = IsNull(e.StandardCode,0)
       and IsNull(ph.FrameworkCode,0) = IsNull(e.FrameworkCode,0)
       and IsNull(ph.PathwayCode ,0)= IsNull(e.PathwayCode,0)
       and (IsNull(ph.ProgrammeType,0) = IsNull(e.ProgrammeType,0) OR  IsNull(ph.StandardCode,0) > 0)
	   and DATEADD(MONTH, DATEDIFF(MONTH, 0, ph.LearningStartDate), 0) = DATEADD(MONTH, DATEDIFF(MONTH, 0, e.LearningStartDate), 0)
       AND case When DeliveryMonth between 1 and 7 Then DeliveryMonth + 5 Else DeliveryMonth - 7 END =  e.Period  
	  AND ph.TransactionType = e.TransactionType
WHERE 
(e.LearnRefNumber IS NULL or e.LearnAimRef is null ) 
AND
EXISTS (Select top 1 * from Staging.CollectionPeriods cp Where [Open] = 1 And DATEFROMPARTS(ph.DeliveryYear,ph.DeliveryMonth,1) between cp.FirstDayOfAcademicYear and DATEADD(day,364,cp.FirstDayOfAcademicYear)) 
AND ph.CollectionPeriodName LIKE '${YearOfCollection}-%' 

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
	and isNull(ph.ApprenticeshipContractType,0) = isNull(p.ApprenticeshipContractType,0)
	and IsNull(ph.CommitmentId,0) = IsNull(p.CommitmentId,0)
	And IsNull(ph.FundingLineType,'') = IsNull(p.FundingLineType,'')
	and ph.AimSeqNumber = p.AimSeqNumber
	and IsNull(ph.AccountId,0) = IsNull(p.AccountId,0)
	and IsNull(ph.SfaContributionPercentage,0) = IsNull(p.SfaContributionPercentage,0)
	and IsNull(ph.PriceEpisodeIdentifier,'') = IsNull(p.PriceEpisodeIdentifier,'')
	and IsNull(ph.UseLevyBalance ,0)= IsNull(p.UseLevyBalance,0)
	and IsNull(ph.CommitmentVersionId,'') = IsNull(p.CommitmentVersionId,'')
	And IsNull(ph.LearnAimRef,'') = IsNull(p.LearnAimRef,'')
	And IsNull(ph.LearningStartDate,'') = IsNull(p.LearningStartDate,'')
	And ph.AmountDue *-1  = p.AmountDue
	And p.Id <> ph.Id
)

--idea of this query is to refund any courses which have been superceeded (e.g. by a new identical course with a different start date, it is not a new course, the old course has been effectively superceeded by the change in start date)
