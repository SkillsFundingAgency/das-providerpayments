Feature: commitments_overlapping_month_end
	Test what happens when a learner withdrawns from one course in the middle of the month and then starts a new course at the start of the same month.


Scenario: Learner Completes and progresses - commitment is stopped - no datalocks
	Given I have the following on programme earnings
		| PriceEpisodeIdentifier | EpisodeStartDate | TNPStartDate | AgreedPrice | Period | Prog | Pathway | Standard | Framework | TT01   | TT02 | TT03   | TT04 | TT05 | TT06 | TT07 | TT08  | TT09 | TT10 | TT11 | TT12 | TT15 | End Date   | 2nd Incentive Date |
		| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 11     | 3    | 1       | 0        | 490       | 0      | 390  | 222.85 | 0    | 0    | 500  | 500  | 0     | 10   | 20   | 0    | 0    | 0    | 06/06/2018 | 03/06/2018         |
		| 2-490-1-18/06/2018     | 2018-06-18       | 2018-06-18   | 2450.00     | 11     | 2    | 1       | 0        | 490       | 115.29 | 0    | 0      | 0    | 0    | 0    | 0    | 23.52 | 0    | 0    | 0    | 0    | 0    |            |                    |

	And I have the following commitments
		| CommitmentId | VersionId   | AccountId | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | PaymentStatus | PaymentStatusDescription | EffectiveFromDate | EffectiveToDate | WithdrawnOnDate |
		| 12526        | 2957434-001 | 13577     | 2017-06-01 | 2018-07-01 | 1950.00    | 0        | 3    | 490       | 1       | 3             | Withdrawn                | 2017-06-01        | NULL            | 2018-06-18      |
		| 277697       | 2600007-001 | 13577     | 2018-06-01 | 2019-10-01 | 2450.00    | 0        | 2    | 490       | 1       | 1             | Active                   | 2018-06-01        | NULL            | NULL            |

	When I call the service ValidataDatalockForProvider

	Then I get 0 validation errors in the DataLockValidationResult 


Scenario: Learner Completes and progresses - commitment is not stopped - no datalocks
	Given I have the following on programme earnings
		| PriceEpisodeIdentifier | EpisodeStartDate | TNPStartDate | AgreedPrice | Period | Prog | Pathway | Standard | Framework | TT01   | TT02 | TT03   | TT06 | TT07 | TT08  | TT09 | TT10 | End Date   | 2nd Incentive Date |
		| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 11     | 3    | 1       | 0        | 490       | 0      | 390  | 222.85 | 500  | 500  | 0     | 10   | 20   | 06/06/2018 | 03/06/2018         |
		| 2-490-1-18/06/2018     | 2018-06-18       | 2018-06-18   | 2450.00     | 11     | 2    | 1       | 0        | 490       | 115.29 | 0    | 0      | 0    | 0    | 23.52 | 0    | 0    |            |                    |

	And I have the following commitments
		| CommitmentId | VersionId   | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | PaymentStatus | PaymentStatusDescription | 
		| 12526        | 2957434-001 | 2017-06-01 | 2018-06-01 | 1950.00    | 0        | 3    | 490       | 1       | 1             | Active                   | 
		| 277697       | 2600007-001 | 2018-06-01 | 2019-10-01 | 2450.00    | 0        | 2    | 490       | 1       | 1             | Active                   | 

	When I call the service ValidataDatalockForProvider

	Then I get 0 validation errors in the DataLockValidationResult 
