 Update rp
 Set rp.LearnAimRef = IsNull(ld.LearnAimRef,'ZPROG001'),
 rp.LearningStartDate = IsNull(ld.LearnStartDate,'2017-05-01') 
from PaymentsDue.RequiredPayments rp 
LEFT JOIN valid.LearningDelivery ld
on rp.Ukprn = ld.UKPRN
	and rp.LearnRefNumber = ld.LearnRefNumber
	and rp.StandardCode = ld.StdCode
	and rp.FrameworkCode = ld.FworkCode
	and rp.PathwayCode = ld.PwayCode
	and rp.ProgrammeType = ld.ProgType
Where ld.LearnAimRef = 'ZPROG001' and ld.FundModel=36
and TransactionType NOT IN (13,14)


 Update rp
 Set rp.LearnAimRef = IsNull(ld.LearnAimRef,''),
 rp.LearningStartDate = IsNull(ld.LearnStartDate,'2017-05-01') 
 
from PaymentsDue.RequiredPayments rp 
LEFT JOIN valid.LearningDelivery ld
on rp.Ukprn = ld.UKPRN
	and rp.LearnRefNumber = ld.LearnRefNumber
	and rp.StandardCode = ld.StdCode
	and rp.FrameworkCode = ld.FworkCode
	and rp.PathwayCode = ld.PwayCode
	and rp.ProgrammeType = ld.ProgType
Where ld.LearnAimRef != 'ZPROG001' and ld.FundModel=36
	and TransactionType IN (13,14)