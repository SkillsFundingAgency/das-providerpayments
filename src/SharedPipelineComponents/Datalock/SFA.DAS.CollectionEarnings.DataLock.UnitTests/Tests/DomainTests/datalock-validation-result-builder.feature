Feature: datalock-validation-result-builder tests
	In order to produce the correct datalock output 
	My builder must ensure that there is exactl

Scenario: Multiple earnings for a commitment that was stopped in the previous academic year
	Given I have the following earnings
		| PriceEpisodeIdentifier | CommitmentId | Period | LearnRefNumber | AimSeqNumber | Ukprn |
		| 2-454-1-01/01/2018     | 105898       | 1      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 1      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 2      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 3      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 4      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 5      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 6      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 7      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 8      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 9      | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 10     | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 11     | 123            | 2            | 321   |
		| 2-454-1-01/08/2018     | 105898       | 12     | 123            | 2            | 321   |
		| 2-454-1-01/08/2019     | 105898       | 1      | 123            | 2            | 321   |
	
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



