Feature: transfers_multi_learner

Scenario: 2 learners, only enough transfer levy to cover 1 learner - 1 learner paid for by receiver's levy

    Given Two learners are programme only DAS
	#Note: a transfer agreement has been set up between employer 1 and employer 2 
	And the employer 1 has a levy balance of:
        | 06/18 | 07/18 | 08/18 |
        | 100   | 100   | 100   |
	
	And the employer 2 has a levy balance of:
        | 06/18 | 07/18 | 08/18 |
        | 100   | 100   | 100   |

    And the employer 2 has a transfer allowance of:
        | 06/18 | 07/18 | 08/18 |
        | 2500  | 2400  | 2300  |
   
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| employer of apprentice | employer paying for training | transfer approval date  | commitment Id | version Id | ULN       | start date | end date   | standard code | agreed price | status | effective from | effective to |
		| employer 1             | employer 2                   | 18/05/2018              | 1             | 1          | 11111     | 01/05/2018 | 01/05/2019 | 50            | 1500         | Active | 01/05/2018     |              |
		| employer 1             | employer 2                   | 18/05/2018              | 2             | 1          | 22222     | 01/05/2018 | 01/05/2019 | 50            | 1500         | Active | 01/05/2018     |              |
        
	When an ILR file is submitted for period R10 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | standard code | contract type | contract type date from |
        | 11111     | programme only DAS     | 1500         | 06/05/2018 | 20/05/2019       |                 | continuing        | programme        | 1                   |          | 50            | DAS           | 06/05/2018              |
        | 22222     | programme only DAS     | 1500         | 06/05/2018 | 20/05/2019       |                 | continuing        | programme        | 1                   |          | 50            | DAS           | 06/05/2018              |
		
   
	Then the provider earnings and payments break down as follows:
        | Type                                                       | 05/18 | 06/18 | 07/18 |
        | Provider Earned Total                                      | 200   | 200   | 200   |
        | Provider Earned from SFA                                   | 200   | 200   | 200   |
        | Provider Earned from employer 1                            | 0     | 0     | 0     |
        | Provider Earned from employer 2                            | 0     | 0     | 0     |
        | Provider Paid by SFA                                       | 0     | 200   | 200   |
        | Provider Paid by SFA for ULN 11111                         | 0     | 100   | 100   |
        | Provider Paid by SFA for ULN 22222			             | 0     | 100   | 100   |
        | Refund taken by SFA                                        | 0     | 0     | 0     |
        | Payment due from employer 1                                | 0     | 0     | 0     |
        | Payment due from employer 2                                | 0     | 0     | 0     |
        | Refund due to employer 1                                   | 0     | 0     | 0     |
        | Refund due to employer 2                                   | 0     | 0     | 0     |
        | Employer 1 Levy account debited for ULN 22222              | 0     | 100   | 100   |
        | Employer 2 Levy account debited for ULN 11111              | 0     | 0     | 0     |
        | Employer 1 Levy account debited for ULN 22222 via transfer | 0     | 0     | 0     |
        | Employer 2 Levy account debited for ULN 11111 via transfer | 0     | 100   | 100   |
        | SFA Levy employer budget                                   | 200   | 200   | 200   |
        | SFA Levy co-funding budget                                 | 0     | 0     | 0     |
        | SFA Levy additional payments budget                        | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget                             | 0     | 0     | 0     |
        | SFA non-Levy additional payments budget                    | 0     | 0     | 0     |
		
   
    And the following transfers from employer 2 exist for the given months of earnings activity:
        | Recipient    | 05/18  | 06/18 | 07/18 | 
        | Employer 1   | 100    | 100   | 100   | 


Scenario: 2 learners, only enough transfer levy to cover 1 learner - 1 learner paid for by co-funding

    Given Two learners are programme only DAS
	#Note: a transfer agreement has been set up between employer 1 and employer 2 
	And the employer 1 has a levy balance of:
        | 06/18 | 07/18 |
        | 0     | 0     |
	
	And the employer 2 has a levy balance of:
        | 06/18 | 07/18 |
        | 100   | 100   |

    And the employer 2 has a transfer allowance of:
        | 06/18 | 07/18 |
        | 2500  | 2400  |
   
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| employer of apprentice | employer paying for training | transfer approval date | commitment Id | version Id | ULN       | start date | end date   | standard code | agreed price | status | effective from | effective to |
		| employer 1             | employer 2                   | 18/05/2018              | 1             | 1          | 11111     | 01/05/2018 | 01/05/2019 | 50            | 1500         | Active | 01/05/2018     |              |
		| employer 1             | employer 2                   | 18/05/2018              | 2             | 1          | 22222     | 01/05/2018 | 01/05/2019 | 50            | 1500         | Active | 01/05/2018     |              |
        
	When an ILR file is submitted for period R10 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | standard code | contract type | contract type date from |
        | 11111     | programme only DAS     | 1500         | 06/05/2018 | 20/05/2019       |                 | continuing        | programme        | 1                   |          | 50            | DAS           | 06/05/2018              |
        | 22222     | programme only DAS     | 1500         | 06/05/2018 | 20/05/2019       |                 | continuing        | programme        | 1                   |          | 50            | DAS           | 06/05/2018              |
		
   
	Then the provider earnings and payments break down as follows:
        | Type                                          | 05/18 | 06/18 | 07/18 |
        | Provider Earned Total                         | 200   | 200   | 200   |
        | Provider Earned from SFA                      | 190   | 190   | 190   |
        | Provider Earned from employer 1               | 10    | 10    | 10    |
        | Provider Earned from employer 2               | 0     | 0     | 0     |
        | Provider Paid by SFA                          | 0     | 190   | 190   |
        | Provider Paid by SFA for ULN 11111            | 0     | 100   | 100   |
        | Provider Paid by SFA for ULN 22222            | 0     | 90    | 90    |
        | Refund taken by SFA                           | 0     | 0     | 0     |
        | Payment due from employer 1                   | 0     | 10    | 10    |
        | Payment due from employer 2                   | 0     | 0     | 0     |
        | Refund due to employer 1                      | 0     | 0     | 0     |
        | Refund due to employer 2                      | 0     | 0     | 0     |
        | Employer 1 Levy account debited               | 0     | 0     | 0     |
        | Employer 2 Levy account debited via transfer  | 0     | 100   | 100   |
        | Employer 1 Levy account credited              | 0     | 0     | 0     |
        | Employer 2 Levy account credited via transfer | 0     | 0     | 0     |
        | SFA Levy employer budget                      | 100   | 100   | 100   |
        | SFA Levy co-funding budget                    | 90    | 90    | 90    |
        | SFA Levy additional payments budget           | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget                | 0     | 0     | 0     |
        | SFA non-Levy additional payments budget       | 0     | 0     | 0     |

    And the following transfers from employer 2 exist for the given months of earnings activity:
        | Recipient    | 05/18  | 06/18  | 07/18 |
        | Employer 1   | 100    | 100    | 100   |




