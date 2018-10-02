Feature: commitments_overlapping_month_end
	Test what happens when a learner withdrawns from one course in the middle of the month and then starts a new course at the start of the same month.


Scenario: Datalock doesn't occur when overlapping commitments

Given I have the following on programme earnings
| PriceEpisodeIdentifier | EpisodeStartDate | TNPStartDate | AgreedPrice | Period | Prog | Pathway | Standard | Framework | TT01   | TT02 | TT03   | TT04 | TT05 | TT06 | TT07 | TT08  | TT09 | TT10  | TT11 | TT12 | TT15 |
| 3-490-1-01/08/2017     | 2017-08-01       | 2018-06-18   | 1950.00     | 1      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2018-06-18   | 1950.00     | 2      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 500  | 500  | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2018-06-18   | 1950.00     | 3      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 4      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 5      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 6      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 7      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 8      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 9      | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 10     | 3    | 1       | 0        | 490       | 111.42 | 0    | 0      | 0    | 0    | 0    | 0    | 22.85 | 0    | 0     | 0    | 0    | 0    |
| 3-490-1-01/08/2017     | 2017-08-01       | 2017-06-13   | 1950.00     | 11     | 3    | 1       | 0        | 490       | 0      | 390  | 222.85 | 0    | 0    | 500  | 500  | 0     | 80   | 45.71 | 0    | 0    | 0    |
| 2-490-1-18/06/2018     | 2018-06-18       | 2018-06-18   | 2450.00     | 11     | 2    | 1       | 0        | 490       | 115.29 | 0    | 0      | 0    | 0    | 0    | 0    | 23.52 | 0    | 0     | 0    | 0    | 0    |
| 2-490-1-18/06/2018     | 2018-06-18       | 2018-06-18   | 2450.00     | 12     | 2    | 1       | 0        | 490       | 115.29 | 0    | 0      | 0    | 0    | 0    | 0    | 23.52 | 0    | 0     | 0    | 0    | 0    |

And I have the following commitments
| CommitmentId | VersionId   | AccountId | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | PaymentStatus | PaymentStatusDescription | EffectiveFromDate | EffectiveToDate | WithdrawnOnDate |
| 12526        | 2957434-001 | 13577     | 2017-06-01 | 2018-07-01 | 1950.00    | 0        | 3    | 490       | 1       | 3             | Withdrawn                | 2017-06-01        | NULL            | 2018-06-18      |
| 277697       | 2600007-001 | 13577     | 2018-06-01 | 2019-10-01 | 2450.00    | 0        | 2    | 490       | 1       | 1             | Active                   | 2018-06-01        | NULL            | NULL            |

When I call the service ValidataDatalockForProvider

Then I get 3 validation errors in the DataLockValidationResult 
