Update rp
Set rp.LearnAimRef = IsNull(ld.LearnAimRef,'ZPROG001'),
rp.LearningStartDate = IsNull(ld.LearnStartDate,'2017-05-01') 
from PaymentsDue.RequiredPayments rp 
LEFT JOIN ${ILR_Deds.FQ}.Valid.LearningDelivery ld
on rp.Ukprn = ld.UKPRN
       and rp.LearnRefNumber = ld.LearnRefNumber
       and rp.LearnRefNumber = ld.LearnRefNumber
       and IsNull(rp.StandardCode,0) = IsNull(ld.StdCode,0)
       and IsNull(rp.FrameworkCode,0) = IsNull(ld.FworkCode,0)
       and IsNull(rp.PathwayCode ,0)= IsNull(ld.PwayCode,0)
       and (IsNull(rp.ProgrammeType,0) = IsNull(ld.ProgType,0) OR  IsNull(rp.StandardCode,0) > 0)
Where ld.LearnAimRef = 'ZPROG001' and ld.FundModel=36
and TransactionType NOT IN (13,14)


Update rp
Set rp.LearnAimRef = IsNull(ld.LearnAimRef,''),
rp.LearningStartDate = IsNull(ld.LearnStartDate,'2017-05-01') 
 
from PaymentsDue.RequiredPayments rp 
LEFT JOIN ${ILR_Deds.FQ}.Valid.LearningDelivery ld

on rp.Ukprn = ld.UKPRN
       and rp.LearnRefNumber = ld.LearnRefNumber
       and rp.LearnRefNumber = ld.LearnRefNumber
and IsNull(rp.StandardCode,0) = IsNull(ld.StdCode,0)
       and IsNull(rp.FrameworkCode,0) = IsNull(ld.FworkCode,0)
       and IsNull(rp.PathwayCode ,0)= IsNull(ld.PwayCode,0)
       and (IsNull(rp.ProgrammeType,0) = IsNull(ld.ProgType,0) OR  IsNull(rp.StandardCode,0) > 0)
Where ld.LearnAimRef != 'ZPROG001' and ld.FundModel=36
       and TransactionType IN (13,14)



Update PaymentsDue.RequiredPayments 
Set LearnAimRef = '',
LearningStartDate = '2017-05-01'
Where LearnAimRef is null and  LearningStartDate is null
