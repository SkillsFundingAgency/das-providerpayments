@learner_changes_contract_type
Feature: Learner changes contract type

Scenario: DPP_965_01 - Levy apprentice, provider edits contract type (ACT) in the ILR, previous on-programme and English/math payments are refunded and repaid according to latest contract type
    Given The learner is programme only DAS
	And levy balance > agreed price for all months
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active | 01/08/2017     |              |
        
	When an ILR file is submitted for period R01 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | Non-DAS       | 06/08/2017              |
        | learner a | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |
        
    And an ILR file is submitted for period R03 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | DAS           | 06/08/2017              |
        | learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |

	Then the provider earnings and payments break down as follows:
        | Type                                    | 08/17  | 09/17  | 10/17  | 11/17    |
        | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25   |
        | Provider Earned from SFA                | 639.25 | 639.25 | 639.25 | 639.25   |
        | Provider Earned from Employer           | 0      | 0      | 0      | 0        |
        | Provider Paid by SFA                    | 0      | 579.25 | 579.25 | 1917.75  |
        | Refund taken by SFA                     | 0      | 0      | 0      | -1158.50 |
        | Payment due from Employer               | 0      | 60     | 60     | 0        |
        | Refund due to employer                  | 0      | 0      | 0      | 120      |
        | Levy account debited                    | 0      | 0      | 0      | 1800     |
        | Levy account credited                   | 0      | 0      | 0      | 0        |
        | SFA Levy employer budget                | 600    | 600    | 600    | 600      |
        | SFA Levy co-funding budget              | 0      | 0      | 0      | 0        |
        | SFA Levy additional payments budget     | 39.25  | 39.25  | 39.25  | 39.25    |
        | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0        |
        | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0        |


Scenario: DPP_965_02 - Non-Levy apprentice, provider edits contract type (ACT) in the ILR, previous on-programme and English/math payments are refunded and repaid according to latest contract type		
    Given levy balance > agreed price for all months
    And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active    | 01/08/2017     |              |
        
    When an ILR file is submitted for period R01 with the following data:
		| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from | contract type date to |
		| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | DAS           | 06/08/2017              | 20/08/2018            |
		| learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |                       |


    And an ILR file is submitted for period R03 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from | contract type date to |
        | learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | Non-DAS       | 06/08/2017              | 20/08/2018            |
        | learner a | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |                       |
  
    Then the provider earnings and payments break down as follows:
        | Type                                    | 08/17  | 09/17  | 10/17  | 11/17    |
        | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25   |
        | Provider Earned from SFA                | 579.25 | 579.25 | 579.25 | 579.25   |
        | Provider Earned from Employer           | 60     | 60     | 0      | 0        |
        | Provider Paid by SFA                    | 0      | 639.25 | 639.25 | 1737.75  |
        | Refund taken by SFA                     | 0      | 0      | 0      | -1278.50 |
        | Payment due from Employer               | 0      | 0      | 0      | 180      |
        | Refund due to employer                  | 0      | 0      | 0      | 0        |
        | Levy account debited                    | 0      | 600    | 600    | 0        |
        | Levy account credited                   | 0      | 0      | 0      | 1200     |
        | SFA Levy employer budget                | 0      | 0      | 0      | 0        |
        | SFA Levy co-funding budget              | 0      | 0      | 0      | 0        |
        | SFA Levy additional payments budget     | 0      | 0      | 0      | 0        |
        | SFA non-Levy co-funding budget          | 540    | 540    | 540    | 540      |
        | SFA non-Levy additional payments budget | 39.25  | 39.25  | 39.25  | 39.25    |

Scenario: DPP_965_03 - repeat of DPP-965-01 over shorter period
    Given The learner is programme only DAS
	And levy balance > agreed price for all months
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active | 01/08/2017     |              |
        
	When an ILR file is submitted for period R01 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | Non-DAS       | 06/08/2017              |
        | learner a | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |

        
    And an ILR file is submitted for period R02 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | DAS           | 06/08/2017              |
        | learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   |          | 403            | 2              | 1            |               |                         |

	Then the provider earnings and payments break down as follows:
        | Type                                    | 08/17  | 09/17  | 10/17   |
        | Provider Earned Total                   | 639.25 | 639.25 | 639.25  |
        | Provider Earned from SFA                | 639.25 | 639.25 | 639.25  |
        | Provider Earned from Employer           | 0      | 0      | 0       |
        | Provider Paid by SFA                    | 0      | 579.25 | 1278.50 |
        | Refund taken by SFA                     | 0      | 0      | -579.25 |
        | Payment due from Employer               | 0      | 60     | 0       |
        | Refund due to employer                  | 0      | 0      | 60      |
        | Levy account debited                    | 0      | 0      | 1200    |
        | Levy account credited                   | 0      | 0      | 0       |
        | SFA Levy employer budget                | 600    | 600    | 600     |
        | SFA Levy co-funding budget              | 0      | 0      | 0       |
        | SFA Levy additional payments budget     | 39.25  | 39.25  | 39.25   |
        | SFA non-Levy co-funding budget          | 0      | 0      | 0       |
        | SFA non-Levy additional payments budget | 0      | 0      | 0       | 
		
Scenario: DPP_965_04 - Levy apprentice, provider edits contract type (ACT) in the ILR, first ILR has one contract type for duration of learning and second ILR shows a change between two contract types - change made in submission month 
    Given levy balance > agreed price for all months
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active | 01/08/2017     |              |
        
	When an ILR file is submitted for period R01 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | Non-DAS       | 06/08/2017              |
        | learner a | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |
        
    And an ILR file is submitted for period R02 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from | contract type date to |
        | learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            | Non-DAS       | 06/08/2017              | 05/09/2017            |
        |           |                    |              |            |                  |                 |                   |                  |                     |          |                |                |              | DAS           | 06/09/2017              |                       |
        | learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |               |                         |                       |
        |           |                    |              |            |                  |                 |                   |                  |                     |          |                |                |              |               |                         |                       |
		
	Then the provider earnings and payments break down as follows:
        | Type                                    | 08/17  | 09/17  | 10/17  | 11/17    |
        | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25   |
        | Provider Earned from SFA                | 579.25 | 639.25 | 639.25 | 639.25   |
        | Provider Earned from Employer           | 60     | 0      | 0      | 0        |
        | Provider Paid by SFA                    | 0      | 579.25 | 639.25 | 639.25   |
        | Refund taken by SFA                     | 0      | 0      | 0      | 0        |
        | Payment due from Employer               | 0      | 60     | 0      | 0        |
        | Refund due to employer                  | 0      | 0      | 0      | 0        |
        | Levy account debited                    | 0      | 0      | 600    | 600      |
        | Levy account credited                   | 0      | 0      | 0      | 0        |
        | SFA Levy employer budget                | 0      | 600    | 600    | 600      |
        | SFA Levy co-funding budget              | 0      | 0      | 0      | 0        |
        | SFA Levy additional payments budget     | 0      | 39.25  | 39.25  | 39.25    |
        | SFA non-Levy co-funding budget          | 540    | 0      | 0      | 0        |
        | SFA non-Levy additional payments budget | 39.25  | 0      | 0      | 0        |


Scenario: DPP_965_05 - Non-Levy apprentice, provider edits contract type (ACT) in the ILR, first ILR has one contract type for duration of learning and second ILR shows a change between two contract types - change made in submission month		
    Given levy balance > agreed price for all months
    And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active | 01/08/2017     |              |
        
    When an ILR file is submitted for period R01 with the following data:
		| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from | employer   | employer employment status | employer employment status applies |
		| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | DAS           | 06/08/2017              | employer 0 | in paid employment         | 05/08/2017                         |
		| learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |            |                            |                                    |


    And an ILR file is submitted for period R02 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from | contract type date to | employer   | employer employment status | employer employment status applies |
        | learner a | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            | DAS           | 06/08/2017              | 05/09/2017            | employer 0 | in paid employment         | 05/08/2017                         |
        |           |                        |              |            |                  |                 |                   |                  |                     |          |                |                |              | Non-DAS       | 06/09/2017              |                       |            |                            |                                    |
        | learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |               |                         |                       |            |                            |                                    |
        |           |                        |              |            |                  |                 |                   |                  |                     |          |                |                |              |               |                         |                       |            |                            |                                    |
		
    Then the provider earnings and payments break down as follows:
        | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
        | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25 |
        | Provider Earned from SFA                | 639.25 | 579.25 | 579.25 | 579.25 |
        | Provider Earned from Employer           | 0      | 60     | 60     | 60     |
        | Provider Paid by SFA                    | 0      | 639.25 | 579.25 | 579.25 |
        | Refund taken by SFA                     | 0      | 0      | 0      | 0      |
        | Payment due from Employer               | 0      | 0      | 60     | 60     |
        | Refund due to employer                  | 0      | 0      | 0      | 0      |
        | Levy account debited                    | 0      | 600    | 0      | 0      |
        | Levy account credited                   | 0      | 0      | 0      | 0      |
        | SFA Levy employer budget                | 600    | 0      | 0      | 0      |
        | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      |
        | SFA Levy additional payments budget     | 39.25  | 0      | 0      | 0      |
        | SFA non-Levy co-funding budget          | 0      | 540    | 540    | 540    |
        | SFA non-Levy additional payments budget | 0      | 39.25  | 39.25  | 39.25  |


Scenario: DPP_1283_01 - Levy apprentice, provider submits first ILR in R08 showing ACT2 from the start of learning and then a second ILR in R09 showing ACT1 from the beginning. Correct Maths or English refunds are processed
    Given levy balance > agreed price for all months
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active | 01/08/2017     |              |
        
	When an ILR file is submitted for period R08 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | Non-DAS       | 06/08/2017              |
        | learner a | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |
        
    And an ILR file is submitted for period R09 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            | DAS           | 06/08/2017              |
        | learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |               |                         |
		
	Then the provider earnings and payments break down as follows:
        | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 12/17  | 01/18  | 02/18  | 03/18  | 04/18   | 05/18  |
        | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25  | 639.25 |
        | Provider Earned from SFA                | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25  | 639.25 |
        | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | Provider Paid by SFA                    | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 4634.00 | 639.25 |
        | Refund taken by SFA                     | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | Payment due from Employer               | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 480     | 0      |
        | Refund due to employer                  | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | Levy account debited                    | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 600    |
        | Levy account credited                   | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | SFA Levy employer budget                | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 600     | 600    |
        | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 39.25   | 39.25  |
        | SFA non-Levy co-funding budget          | 540    | 540    | 540    | 540    | 540    | 540    | 540    | 540    | 0       | 0      |
        | SFA non-Levy additional payments budget | 39.25  | 39.25  | 39.25  | 39.25  | 39.25  | 39.25  | 39.25  | 39.25  | 0       | 0      |

Scenario: DPP_1283_02 - Levy apprentice, provider submits first ILR in R08 showing ACT1 from the start of learning and then a second ILR in R09 showing ACT2 from the beginning. Correct Maths or English refunds are processed
    Given levy balance > agreed price for all months
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active | 01/08/2017     |              |
        
	When an ILR file is submitted for period R08 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | DAS           | 06/08/2017              |
        | learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |
        
    And an ILR file is submitted for period R09 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from |
        | learner a | programme only Non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            | Non-DAS       | 06/08/2017              |
        | learner a | programme only Non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |               |                         |
		
	Then the provider earnings and payments break down as follows:
        | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 12/17  | 01/18  | 02/18  | 03/18  | 04/18   | 05/18  |
        | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25  | 639.25 |
        | Provider Earned from SFA                | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25 | 639.25  | 639.25 |
        | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | Provider Paid by SFA                    | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 5114.00 | 579.25 |
        | Refund taken by SFA                     | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | Payment due from Employer               | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 60     |
        | Refund due to employer                  | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | Levy account debited                    | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 4800    | 0      |
        | Levy account credited                   | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | SFA Levy employer budget                | 600    | 600    | 600    | 600    | 600    | 600    | 600    | 600    | 0       | 0      |
        | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0       | 0      |
        | SFA Levy additional payments budget     | 39.25  | 39.25  | 39.25  | 39.25  | 39.25  | 39.25  | 39.25  | 39.25  | 0       | 0      |
        | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 540     | 540    |
        | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 0      | 39.25   | 39.25  |