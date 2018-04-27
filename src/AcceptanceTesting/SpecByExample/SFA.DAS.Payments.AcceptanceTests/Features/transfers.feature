Feature: transfers

Scenario: testing new rule in earnings and payments break down
    Given the employer 1 has a levy balance > agreed price for all months
    And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status | effective from | effective to | employer   | employer paying for training |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active | 01/08/2017     |              | employer 1 | employer 2                   |
        
    When an ILR file is submitted for period R01 with the following data:
		| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from | contract type date to | Employer Employment Status | Employer Employment Status Applies | Employer   | Employer Id |
		| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | DAS           | 06/08/2017              | 20/08/2018            | in paid employment         | 05/08/2017                         | employer 1 | 12345678    |
		| learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |                       |                            |                                    |            |             |

    And an ILR file is submitted for period R03 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code | contract type | contract type date from | contract type date to | Employer Employment Status | Employer Employment Status Applies | Employer   | Employer Id |
        | learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            | Non-DAS       | 06/08/2017              | 20/08/2018            | in paid employment         | 05/08/2017                         | employer 1 | 12345678    |
        | learner a | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |               |                         |                       |                            |                                    |            |             |
  
    Then the provider earnings and payments break down as follows:
        | Type                                    | 08/17  | 09/17  | 10/17  | 11/17    |
        | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25   |
        | Provider Earned from SFA                | 579.25 | 579.25 | 579.25 | 579.25   |
        | Provider Earned from Employer           | 60     | 60     | 0      | 0        |
        | Provider Paid by SFA                    | 0      | 639.25 | 639.25 | 1737.75  |
        | Refund taken by SFA                     | 0      | 0      | 0      | -1278.50 |
        | Payment due from Employer               | 0      | 0      | 0      | 180      |
        | Refund due to employer                  | 0      | 0      | 0      | 0        |
        | employer 1 Levy account debited         | 0      | 600    | 600    | 0        |
        | employer 1 Levy account credited        | 0      | 0      | 0      | 1200     |
        | SFA Levy employer budget                | 0      | 0      | 0      | 0        |
        | SFA Levy co-funding budget              | 0      | 0      | 0      | 0        |
        | SFA Levy additional payments budget     | 0      | 0      | 0      | 0        |
        | SFA non-Levy co-funding budget          | 540    | 540    | 540    | 540      |
        | SFA non-Levy additional payments budget | 39.25  | 39.25  | 39.25  | 39.25    |

	And the following transfers from employer 2 exist
		| Recipient  | 05/18 | 06/18 | 07/18 | 08/18 |
		| Employer 1 | 0     | 100   | 100   | 100   |
