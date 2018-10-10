ALTER TABLE ProviderAdjustments.Payments
DROP CONSTRAINT PK_ProviderAdjustmentsPayments;

ALTER TABLE ProviderAdjustments.Payments
ADD PRIMARY KEY (Ukprn, SubmissionId, SubmissionCollectionPeriod, SubmissionAcademicYear, PaymentType, CollectionPeriodName);