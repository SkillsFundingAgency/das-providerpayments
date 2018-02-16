UPDATE rp
SET rp.LearnAimRef = IsNull(ld.LearnAimRef, 'ZPROG001'),
    rp.LearningStartDate = IsNull(ld.LearnStartDate, '2017-05-01')
FROM PaymentsDue.RequiredPayments rp
LEFT JOIN OPENQUERY(${DS_SILR1718_Collection.servername}, '
		SELECT 
			UKPRN, 
			LearnRefNumber, 
			StdCode, 
			FworkCode, 
			PwayCode, 
			ProgType, 
			FundModel, 
			LearnAimRef, 
			LearnStartDate  
		FROM
			${DS_SILR1718_Collection.databasename}.Valid.LearningDelivery'
    ) AS ld ON rp.Ukprn = ld.UKPRN
    AND rp.LearnRefNumber = ld.LearnRefNumber
    AND rp.LearnRefNumber = ld.LearnRefNumber
    AND IsNull(rp.StandardCode, 0) = IsNull(ld.StdCode, 0)
    AND IsNull(rp.FrameworkCode, 0) = IsNull(ld.FworkCode, 0)
    AND IsNull(rp.PathwayCode, 0) = IsNull(ld.PwayCode, 0)
    AND (
        IsNull(rp.ProgrammeType, 0) = IsNull(ld.ProgType, 0)
        OR IsNull(rp.StandardCode, 0) > 0
        )
WHERE ld.LearnAimRef = 'ZPROG001'
    AND ld.FundModel = 36
    AND TransactionType NOT IN (
        13,
        14
        )


UPDATE rp
SET rp.LearnAimRef = IsNull(ld.LearnAimRef, ''),
    rp.LearningStartDate = IsNull(ld.LearnStartDate, '2017-05-01')
FROM PaymentsDue.RequiredPayments rp
LEFT JOIN OPENQUERY(${DS_SILR1718_Collection.servername}, '
		select
			UKPRN,
			LearnRefNumber,
			StdCode,
			FworkCode,
			PwayCode,
			ProgType,
			LearnAimRef,
			FundModel,
			LearnStartDate
		from ${DS_SILR1718_Collection.databasename}.Valid.LearningDelivery ld'
    ) AS ld ON rp.Ukprn = ld.UKPRN
    AND rp.LearnRefNumber = ld.LearnRefNumber
    AND rp.LearnRefNumber = ld.LearnRefNumber
    AND IsNull(rp.StandardCode, 0) = IsNull(ld.StdCode, 0)
    AND IsNull(rp.FrameworkCode, 0) = IsNull(ld.FworkCode, 0)
    AND IsNull(rp.PathwayCode, 0) = IsNull(ld.PwayCode, 0)
    AND (
        IsNull(rp.ProgrammeType, 0) = IsNull(ld.ProgType, 0)
        OR IsNull(rp.StandardCode, 0) > 0
        )
WHERE ld.LearnAimRef != 'ZPROG001'
    AND ld.FundModel = 36
    AND TransactionType IN (
        13,
        14
        )



Update PaymentsDue.RequiredPayments 
Set LearnAimRef = '',
LearningStartDate = '2017-05-01'
Where LearnAimRef is null and  LearningStartDate is null
