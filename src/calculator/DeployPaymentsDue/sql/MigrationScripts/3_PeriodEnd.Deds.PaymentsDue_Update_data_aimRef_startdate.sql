 Update rp
 Set rp.LearnAimRef = ld.LearnAimRef,
 rp.LearningStartDate = ld.LearnStartDate 
from PaymentsDue.RequiredPayments rp 
join valid.LearningDelivery ld
on rp.Ukprn = ld.UKPRN
	and rp.LearnRefNumber = ld.LearnRefNumber
	and rp.StandardCode = ld.StdCode
	and rp.FrameworkCode = ld.FworkCode
	and rp.PathwayCode = ld.PwayCode
	and rp.ProgrammeType = ld.ProgType
Where ld.LearnAimRef = 'ZPROG001' and ld.FundModel=36
and TransactionType NOT IN (13,14)


 Update rp
 Set rp.LearnAimRef = ld.LearnAimRef,
 rp.LearningStartDate = ld.LearnStartDate 
 
from PaymentsDue.RequiredPayments rp 
join valid.LearningDelivery ld
on rp.Ukprn = ld.UKPRN
	and rp.LearnRefNumber = ld.LearnRefNumber
	and rp.StandardCode = ld.StdCode
	and rp.FrameworkCode = ld.FworkCode
	and rp.PathwayCode = ld.PwayCode
	and rp.ProgrammeType = ld.ProgType
Where ld.LearnAimRef != 'ZPROG001' and ld.FundModel=36
	and TransactionType IN (13,14)