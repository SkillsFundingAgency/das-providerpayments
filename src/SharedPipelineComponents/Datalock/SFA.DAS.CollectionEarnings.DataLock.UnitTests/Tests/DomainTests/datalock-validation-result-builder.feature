Feature: datalock-validation-result-builder tests
	In order to produce the correct datalock output 
	My builder must ensure that there is exactl

Scenario: Multiple earnings for a commitment that was stopped in the previous academic year
	Given I have the following earnings
		| PriceEpisodeIdentifier | CommitmentId | Period | LearnRefNumber | AimSeqNumber | Ukprn    | Prog | Framework | Pathway | SfaContributionPercentage | FundingLineType                                    | LearnAimRef | ApprenticeshipContractType | TT01     |
		| 2-454-1-01/01/2018     | 105898       | 1      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0                         | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 0        |
		| 2-454-1-01/08/2018     | 105898       | 1      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 2      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 3      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 4      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 5      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 6      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 7      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 8      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 9      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 10     | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 11     | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2018     | 105898       | 12     | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 |
		| 2-454-1-01/08/2019     | 105898       | 1      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0                         | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 0        |
	
	And I have the following commitments
		| CommitmentId | VersionId   | AccountId | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | PaymentStatus | PaymentStatusDescription | EffectiveFromDate | EffectiveToDate | WithdrawnOnDate |
		| 105898       | 2957434-001 | 001       | 2018-01-01 | 2020-01-01 | 1950.00    | 0        | 3    | 490       | 1       | 3             | Withdrawn                | 2017-06-01        | NULL            | 2018-07-31      |
	
	And I build using DatalockValidationResultBuilder
		| PriceEpisodeIdentifier | RuleId   |
		| 2-454-1-01/01/2018     |          |
		| 2-454-1-01/08/2018     | DLOCK_10 |
		| 2-454-1-01/08/2019     | DLOCK_10 |

	When I call Build

	Then I get 2 validation errors in the DataLockValidationResult 
	And The DatalockValidatioResult contains DLOCK_10



