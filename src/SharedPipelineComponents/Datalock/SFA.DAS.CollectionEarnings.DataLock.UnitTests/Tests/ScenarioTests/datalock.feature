Feature: Datalock Features


Scenario: The commitment has both a DLOCK_09 and a DLOCK_07 - only DLOCK_09 in results
	Given I have the following earnings
		| PriceEpisodeIdentifier | EpisodeStartDate | TNPStartDate | AgreedPrice | Period | Prog | Pathway | Standard | Framework | TT01 | 
		| 2-490-1-01/06/2017     | 2017-06-01       | 2017-06-01   | 1950.00     | 11     | 3    | 1       | 0        | 490       | 100  | 
		| 2-490-1-01/06/2017     | 2017-06-01       | 2017-06-01   | 1950.00     | 11     | 3    | 1       | 0        | 490       | 100  | 

	And I have the following commitments
		| CommitmentId | VersionId   | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | Status | StatusDescription |
		| 12526        | 2957434-001 | 2017-06-10 | 2018-06-01 | 2000.00    | 0        | 3    | 490       | 1       | 1      | Active            |  

	When I call the service ValidataDatalockForProvider

	Then I get 1 validation errors in the DataLockValidationResult 
	And The DatalockValidatioResult contains DLOCK_09


Scenario: The course extends past the end date of the commitment - no datalock errors expected
	Given I have the following earnings
		| PriceEpisodeIdentifier | EpisodeStartDate | TNPStartDate | AgreedPrice | Period | Prog | Pathway | Standard | Framework | TT01 | 
		| 2-490-1-01/06/2017     | 2017-06-01       | 2017-06-01   | 2000.00     | 11     | 3    | 1       | 0        | 490       | 100  | 
		| 2-490-1-01/06/2017     | 2017-06-01       | 2017-06-01   | 2000.00     | 11     | 3    | 1       | 0        | 490       | 100  | 

	And I have the following commitments
		| CommitmentId | VersionId   | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | Status | StatusDescription |
		| 12526        | 2957434-001 | 2017-06-01 | 2017-06-02 | 2000.00    | 0        | 3    | 490       | 1       | 1      | Active            |

	When I call the service ValidataDatalockForProvider

	Then I get 0 validation errors in the DataLockValidationResult 


Scenario: The commitment is paused - DLOCK_12 expected
	Given I have the following earnings
		| PriceEpisodeIdentifier | EpisodeStartDate | TNPStartDate | AgreedPrice | Period | Prog | Pathway | Standard | Framework | TT01 | 
		| 2-490-1-01/06/2017     | 2017-06-01       | 2017-06-01   | 2000.00     | 11     | 3    | 1       | 0        | 490       | 100  | 
		| 2-490-1-01/06/2017     | 2017-06-01       | 2017-06-01   | 2000.00     | 11     | 3    | 1       | 0        | 490       | 100  | 

	And I have the following commitments
		| CommitmentId | VersionId   | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | Status | StatusDescription |
		| 12526        | 2957434-001 | 2017-06-01 | 2017-06-02 | 2000.00    | 0        | 3    | 490       | 1       | 2      | Paused            |

	When I call the service ValidataDatalockForProvider

	Then I get 1 validation errors in the DataLockValidationResult 
	And The DatalockValidatioResult contains DLOCK_12
	And There are 0 payable datalocks for price episode 2-490-1-01/06/2017
	

Scenario: The commitment has versions and is stopped during first version - should pay correctly
	Given I have the following earnings
		| PriceEpisodeIdentifier | EpisodeStartDate | TNPStartDate | AgreedPrice | Period | Framework | Prog | Pathway | Standard | TT01 |
		| 2-490-1-01/08/2017     | 2017-08-01       | 2017-08-01   | 2000.00     | 4      | 490       | 3    | 1       | 0        | 100  |
		| 2-490-1-01/08/2017     | 2017-08-01       | 2017-08-01   | 2000.00     | 5      | 490       | 3    | 1       | 0        | 100  |
		| 2-490-1-15/01/2018     | 2018-01-15       | 2018-01-15   | 3000.00     | 6      | 490       | 3    | 1       | 0        | 100  | 

	And I have the following commitments
		| CommitmentId | VersionId   | StartDate  | EndDate    | AgreedCost | EffectiveFrom | EffectiveTo | WithdrawnOnDate | Framework | Prog | Pathway | Standard | Status | StatusDescription |
		| 12526        | 2957434-001 | 2017-08-01 | 2018-08-01 | 2000.00    | 2017-08-01    | 2018-01-15  | 2017-12-15      | 490       | 3    | 1       | 0        | 3      | Cancelled         |
		| 12526        | 2957434-002 | 2017-08-01 | 2018-08-01 | 3000.00    | 2018-01-15    | 2018-06-15  | 2017-12-15      | 490       | 3    | 1       | 0        | 3      | Cancelled         |

	When I call the service ValidataDatalockForProvider

	Then There are 1 payable datalocks for price episode 2-490-1-01/08/2017
	And There are 1 non-payable datalocks for price episode 2-490-1-01/08/2017
	And There are 1 non-payable datalocks for price episode 2-490-1-15/01/2018


Scenario: The commitment is stopped in the previous academic year
	Given I have the following earnings
		| PriceEpisodeIdentifier | CommitmentId | Period | LearnRefNumber | AimSeqNumber | Ukprn    | Prog | Framework | Pathway | SfaContributionPercentage | FundingLineType                                    | LearnAimRef | ApprenticeshipContractType | TT01     | EpisodeStartDate | TNPStartDate | AgreedPrice |
		| 2-454-1-01/08/2019     | 105898       | 1      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0                         | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 0        | 2019-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 1      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 2      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 3      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 4      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 5      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 6      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 7      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 8      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 9      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 10     | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 11     | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/08/2018     | 105898       | 12     | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0.9                       | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 13.33333 | 2018-08-01       | 2018-01-01   | 1950.00     |
		| 2-454-1-01/01/2018     | 105898       | 1      | DPP1668S07     | 2            | 10038368 | 2    | 454       | 1       | 0                         | 16-18 Apprenticeship (From May 2017) Levy Contract | ZPROG001    | 1                          | 0        | 2018-01-01       | 2018-01-01   | 1950.00     |
	
	And I have the following commitments
		| CommitmentId | VersionId   | AccountId | StartDate  | EndDate    | AgreedCost | Standard | Prog | Framework | Pathway | Status | StatusDescription | EffectiveFrom | EffectiveTo | WithdrawnOnDate |
		| 105898       | 2957434-001 | 001       | 2018-01-01 | 2020-01-01 | 1950.00    | 0        | 2    | 454       | 1       | 3      | Withdrawn         | 2017-06-01    | NULL        | 2018-07-31      |  
	
	When I call the service ValidataDatalockForProvider

	Then There are 0 payable datalocks for price episode 2-454-1-01/08/2018
	And I get 2 validation errors in the DataLockValidationResult 
	And The DatalockValidatioResult contains DLOCK_10
