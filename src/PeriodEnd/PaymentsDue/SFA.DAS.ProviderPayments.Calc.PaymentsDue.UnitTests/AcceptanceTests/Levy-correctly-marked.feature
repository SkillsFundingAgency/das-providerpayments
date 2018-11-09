Feature: UseLevyBalance flag features
Background: 
		Given The collection period is 1718-R14

Scenario: Transaction type 1, 2 and 3 for levy earnings should have UseLevyBalance field set
	Given I have the following datalocks for provider 10033440
		| Ukprn    | PriceEpisodeIdentifier | LearnRefNumber | AimSeqNumber | CommitmentId | VersionId   | Period | Payable | TransactionType | TransactionTypesFlag |
		| 10033440 | 3-537-1-29/09/2017     | ITCC30282944   | 2            | 266784       | 4098748-001 | 1      | 1       | 0               | 1                    |
		| 10033440 | 3-537-1-29/09/2017     | ITCC30282944   | 2            | 266784       | 4098748-001 | 1      | 1       | 0               | 4                    |
		
	And I have the following datalock errors for provider 10033440
		| Ukprn    | LearnRefNumber | AimSeqNumber | RuleId   | PriceEpisodeIdentifier |
	
	And I have the following earnings for provider 10033440
		| LearnRefNumber | Ukprn    | AimSeqNumber | PriceEpisodeIdentifier | EpisodeStartDate | EpisodeEffectiveTNPStartDate | Period | Uln        | ProgrammeType | FrameworkCode | PathwayCode | StandardCode | SfaContributionPercentage | FundingLineType                                  | LearnAimRef | LearningStartDate | TransactionType01 | TransactionType02 | TransactionType03 | ApprenticeshipContractType |
		| ITCC30282944   | 10033440 | 2            | 3-537-1-29/09/2017     | 2017-09-29       | 2017-09-29                   | 1      | 1316708828 | 3             | 537           | 1           | 0            | 0.90000                   | 19+ Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 2017-09-29        | 100.00000         | 100.00000         | 100.00000         | 1                          |
		
	And I have the following commitments for provider 10033440
		| Uln        | Ukprn    | CommitmentId | CommitmentVersionId | AccountId | AccountVersionId | StartDate  | EndDate    | AgreedCost | StandardCode | ProgrammeType | FrameworkCode | PathwayCode | PaymentStatus | Priority | EffectiveFrom | EffectiveTo | LegalEntityName | TransferSendingEmployerAccountId | TransferApprovalDate | IsLevyPayer |
		| 1316708828 | 10033440 | 266784       | 4098748-001         | 279       | 20181018         | 2017-09-01 | 2019-03-01 | 1500.00    | 0            | 3             | 537           | 1           | 1             | 15322    | 2017-09-01    | NULL        | British Army    | NULL                             | NULL                 | 1           |

	When I process period end
	Then All required payments should have UseLevyBalance flag set
	And There should be 3 required payments


Scenario: Transaction type 1, 2 and 3 for non-levy earnings should not have UseLevyBalance field set
	Given I have the following datalocks for provider 10033440
		| Ukprn    | PriceEpisodeIdentifier | LearnRefNumber | AimSeqNumber | CommitmentId | VersionId   | Period | Payable | TransactionType | TransactionTypesFlag |
		| 10033440 | 3-537-1-29/09/2017     | ITCC30282944   | 2            | 266784       | 4098748-001 | 1      | 1       | 0               | 1                    |
		| 10033440 | 3-537-1-29/09/2017     | ITCC30282944   | 2            | 266784       | 4098748-001 | 1      | 1       | 0               | 4                    |
		
	And I have the following datalock errors for provider 10033440
		| Ukprn    | LearnRefNumber | AimSeqNumber | RuleId   | PriceEpisodeIdentifier |
	
	And I have the following earnings for provider 10033440
		| LearnRefNumber | Ukprn    | AimSeqNumber | PriceEpisodeIdentifier | EpisodeStartDate | EpisodeEffectiveTNPStartDate | Period | Uln        | ProgrammeType | FrameworkCode | PathwayCode | StandardCode | SfaContributionPercentage | FundingLineType                                  | LearnAimRef | LearningStartDate | TransactionType01 | TransactionType02 | TransactionType03 | ApprenticeshipContractType |
		| ITCC30282944   | 10033440 | 2            | 3-537-1-29/09/2017     | 2017-09-29       | 2017-09-29                   | 1      | 1316708828 | 3             | 537           | 1           | 0            | 0.90000                   | 19+ Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 2017-09-29        | 100.00000         | 100.00000         | 100.00000         | 2                          |
		
	And I have the following commitments for provider 10033440
		| Uln        | Ukprn    | CommitmentId | CommitmentVersionId | AccountId | AccountVersionId | StartDate  | EndDate    | AgreedCost | StandardCode | ProgrammeType | FrameworkCode | PathwayCode | PaymentStatus | Priority | EffectiveFrom | EffectiveTo | LegalEntityName | TransferSendingEmployerAccountId | TransferApprovalDate | IsLevyPayer |
		| 1316708828 | 10033440 | 266784       | 4098748-001         | 279       | 20181018         | 2017-09-01 | 2019-03-01 | 1500.00    | 0            | 3             | 537           | 1           | 1             | 15322    | 2017-09-01    | NULL        | British Army    | NULL                             | NULL                 | 1           |

	When I process period end
	Then All required payments should not have UseLevyBalance flag set
	And There should be 3 required payments


Scenario: Transaction type 1, 2 and 3 for small employer levy earnings should not have UseLevyBalance field set
	Given I have the following datalocks for provider 10033440
		| Ukprn    | PriceEpisodeIdentifier | LearnRefNumber | AimSeqNumber | CommitmentId | VersionId   | Period | Payable | TransactionType | TransactionTypesFlag |
		| 10033440 | 3-537-1-29/09/2017     | ITCC30282944   | 2            | 266784       | 4098748-001 | 1      | 1       | 0               | 1                    |
		| 10033440 | 3-537-1-29/09/2017     | ITCC30282944   | 2            | 266784       | 4098748-001 | 1      | 1       | 0               | 4                    |
		
	And I have the following datalock errors for provider 10033440
		| Ukprn    | LearnRefNumber | AimSeqNumber | RuleId   | PriceEpisodeIdentifier |
	
	And I have the following earnings for provider 10033440
		| LearnRefNumber | Ukprn    | AimSeqNumber | PriceEpisodeIdentifier | EpisodeStartDate | EpisodeEffectiveTNPStartDate | Period | Uln        | ProgrammeType | FrameworkCode | PathwayCode | StandardCode | SfaContributionPercentage | FundingLineType                                  | LearnAimRef | LearningStartDate | TransactionType01 | TransactionType02 | TransactionType03 | ApprenticeshipContractType |
		| ITCC30282944   | 10033440 | 2            | 3-537-1-29/09/2017     | 2017-09-29       | 2017-09-29                   | 1      | 1316708828 | 3             | 537           | 1           | 0            | 1.0                       | 19+ Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 2017-09-29        | 100.00000         | 100.00000         | 100.00000         | 1                          |
		
	And I have the following commitments for provider 10033440
		| Uln        | Ukprn    | CommitmentId | CommitmentVersionId | AccountId | AccountVersionId | StartDate  | EndDate    | AgreedCost | StandardCode | ProgrammeType | FrameworkCode | PathwayCode | PaymentStatus | Priority | EffectiveFrom | EffectiveTo | LegalEntityName | TransferSendingEmployerAccountId | TransferApprovalDate | IsLevyPayer |
		| 1316708828 | 10033440 | 266784       | 4098748-001         | 279       | 20181018         | 2017-09-01 | 2019-03-01 | 1500.00    | 0            | 3             | 537           | 1           | 1             | 15322    | 2017-09-01    | NULL        | British Army    | NULL                             | NULL                 | 1           |

	When I process period end
	Then All required payments should not have UseLevyBalance flag set
	And There should be 3 required payments
	

Scenario: Transaction type 1, 2 and 3 for non-levy earnings should not have UseLevyBalance field set with 95% contribution
	Given I have the following datalocks for provider 10033440
		| Ukprn    | PriceEpisodeIdentifier | LearnRefNumber | AimSeqNumber | CommitmentId | VersionId   | Period | Payable | TransactionType | TransactionTypesFlag |
		| 10033440 | 3-537-1-29/09/2017     | ITCC30282944   | 2            | 266784       | 4098748-001 | 1      | 1       | 0               | 1                    |
		| 10033440 | 3-537-1-29/09/2017     | ITCC30282944   | 2            | 266784       | 4098748-001 | 1      | 1       | 0               | 4                    |
		
	And I have the following datalock errors for provider 10033440
		| Ukprn    | LearnRefNumber | AimSeqNumber | RuleId   | PriceEpisodeIdentifier |
	
	And I have the following earnings for provider 10033440
		| LearnRefNumber | Ukprn    | AimSeqNumber | PriceEpisodeIdentifier | EpisodeStartDate | EpisodeEffectiveTNPStartDate | Period | Uln        | ProgrammeType | FrameworkCode | PathwayCode | StandardCode | SfaContributionPercentage | FundingLineType                                  | LearnAimRef | LearningStartDate | TransactionType01 | TransactionType02 | TransactionType03 | ApprenticeshipContractType |
		| ITCC30282944   | 10033440 | 2            | 3-537-1-29/09/2017     | 2017-09-29       | 2017-09-29                   | 1      | 1316708828 | 3             | 537           | 1           | 0            | 0.95000                   | 19+ Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 2017-09-29        | 100.00000         | 100.00000         | 100.00000         | 2                          |
		
	And I have the following commitments for provider 10033440
		| Uln        | Ukprn    | CommitmentId | CommitmentVersionId | AccountId | AccountVersionId | StartDate  | EndDate    | AgreedCost | StandardCode | ProgrammeType | FrameworkCode | PathwayCode | PaymentStatus | Priority | EffectiveFrom | EffectiveTo | LegalEntityName | TransferSendingEmployerAccountId | TransferApprovalDate | IsLevyPayer |
		| 1316708828 | 10033440 | 266784       | 4098748-001         | 279       | 20181018         | 2017-09-01 | 2019-03-01 | 1500.00    | 0            | 3             | 537           | 1           | 1             | 15322    | 2017-09-01    | NULL        | British Army    | NULL                             | NULL                 | 1           |

	When I process period end
	Then All required payments should not have UseLevyBalance flag set
	And There should be 3 required payments