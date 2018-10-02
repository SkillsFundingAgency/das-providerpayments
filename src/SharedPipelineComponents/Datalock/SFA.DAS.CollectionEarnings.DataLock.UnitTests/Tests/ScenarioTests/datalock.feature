Feature: Datalock Features


Scenario: The commitment has both a DLOCK_09 and a DLOCK_07 - 
	Given I have the following on programme earnings
		| PriceEpisodeIdentifier | EpisodeStartDate | TNPStartDate | AgreedPrice | Period | Prog | Pathway | Standard | Framework | TT01 | 
		| 2-490-1-01/06/2017     | 2017-06-01       | 2017-06-01   | 1950.00     | 11     | 3    | 1       | 0        | 490       | 100  | 
		| 2-490-1-01/06/2017     | 2017-06-01       | 2017-06-01   | 1950.00     | 11     | 3    | 1       | 0        | 490       | 100  | 

	And I have the following commitments
		| CommitmentId | VersionId   | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | PaymentStatus | PaymentStatusDescription | 
		| 12526        | 2957434-001 | 2017-06-10 | 2018-06-01 | 2000.00    | 0        | 3    | 490       | 1       | 1             | Active                   | 

	When I call the service ValidataDatalockForProvider

	Then I get 1 validation errors in the DataLockValidationResult 
	And The DatalockValidatioResult contains DLOCK_09
