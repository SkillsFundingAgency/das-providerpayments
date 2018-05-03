Feature: transfers

Scenario: Levy apprentice, paid for by a different employer through a transfer 

    Given The learner is programme only DAS
	#And a transfer agreement has been set up between employer 1 and employer 2
	And the employer 1 has a levy balance > agreed price for all months
	And the employer 2 has a levy balance > agreed price for all months
	And the employer 2 has a transfer allowance > agreed price for all months
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| employer of apprentice | employer paying for training | commitment Id | version Id | ULN       | start date | end date   | standard code | agreed price | status | effective from | effective to |
		| employer 1             | employer 2                   | 1             | 1          | learner a | 01/05/2018 | 01/05/2019 | 50            | 1500         | Active | 01/05/2018     |              |
        
	When an ILR file is submitted for period R10 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | standard code | contract type | contract type date from | employer   | employer employment status | employer employment status applies |
        | learner a | programme only DAS | 1500         | 06/05/2018 | 20/05/2019       |                 | continuing        | programme        | 2                   |          | 50            | DAS           | 06/05/2018              | employer 1 | in paid employment         | 06/05/2018                         |
        | learner a | programme only DAS |              | 06/05/2018 | 20/05/2019       |                 | continuing        | maths or english | 1                   | 471      | 50            |               |                         |            |                            |                                    |
        
   
	Then the provider earnings and payments break down as follows:
        | Type                                         | 05/18  | 06/18  | 07/18  |
        | Provider Earned Total                        | 139.25 | 139.25 | 139.25 |
        | Provider Earned from SFA                     | 139.25 | 139.25 | 139.25 |
        | Provider Earned from employer 1              | 0      | 0      | 0      |
        | Provider Earned from employer 2              | 0      | 0      | 0      |
        | Provider Paid by SFA                         | 0      | 139.25 | 139.25 |
        | Refund taken by SFA                          | 0      | 0      | 0      |
        | Payment due from employer 1                  | 0      | 0      | 0      |
        | Payment due from employer 2                  | 0      | 0      | 0      |
        | Refund due to employer 1                     | 0      | 0      | 0      |
        | Refund due to employer 2                     | 0      | 0      | 0      |
        | employer 1 levy account debited              | 0      | 0      | 0      |
        | employer 2 levy account debited via transfer | 0      | 100    | 100    |
        | SFA Levy employer budget                     | 100    | 100    | 100    |
        | SFA Levy co-funding budget                   | 0      | 0      | 0      |
        | SFA Levy additional payments budget          | 39.25  | 39.25  | 39.25  |
        | SFA non-Levy co-funding budget               | 0      | 0      | 0      |
        | SFA non-Levy additional payments budget      | 0      | 0      | 0      |

	And the following transfers from employer 2 exist for the given months of earnings activity:
		| Recipient  | 05/18 | 06/18 | 07/18 |
		| Employer 1 | 100   | 100   | 100   |


Scenario: Levy apprentice, transfer set up but no funds available from sender, receiving employer uses their own levy to fund apprentice  

    Given The learner is programme only DAS
	#And a transfer agreement has been set up between employer 1 and employer 2
	And the employer 2 has a levy balance = 0 for all months
	And the employer 2 has a transfer allowance = 0 for all months
	And the employer 1 has a levy balance > agreed price for all months
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| employer of apprentice | employer paying for training | commitment Id | version Id | ULN       | start date | end date   | standard code | agreed price | status | effective from | effective to |
		| employer 1             | employer 2                   | 1             | 1          | learner a | 01/05/2018 | 01/05/2019 | 50            | 3000         | Active | 01/05/2018     |              |
        
	When an ILR file is submitted for period R10 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | standard code | contract type | contract type date from | employer employment status | employer employment status applies |
        | learner a | programme only DAS     | 3000         | 06/05/2018 | 20/05/2019       |                 | continuing        | programme        | 2                   |          | 50            | DAS           | 06/05/2018              | in paid employment         | 06/05/2018                         |
        | learner a | programme only DAS     |              | 06/05/2018 | 20/05/2019       |                 | continuing        | maths or english | 1                   | 471      | 50            |               |                         |                            |                                    |
        
   
	Then the provider earnings and payments break down as follows:
        | Type                                         | 05/18  | 06/18  | 07/18  |
        | Provider Earned Total                        | 239.25 | 239.25 | 239.25 |
        | Provider Earned from SFA                     | 239.25 | 239.25 | 239.25 |
        | Provider Earned from employer 1              | 0      | 0      | 0      |
        | Provider Earned from employer 2              | 0      | 0      | 0      |
        | Provider Paid by SFA                         | 0      | 239.25 | 239.25 |
        | Refund taken by SFA                          | 0      | 0      | 0      |
        | Payment due from employer 1                  | 0      | 0      | 0      |
        | Payment due from employer 2                  | 0      | 0      | 0      |
        | Refund due to employer 1                     | 0      | 0      | 0      |
        | Refund due to employer 2                     | 0      | 0      | 0      |
        | employer 1 levy account debited              | 0      | 200    | 200    |
        | employer 2 levy account debited via transfer | 0      | 0      | 0      |
        | SFA Levy employer budget                     | 200    | 200    | 200    |
        | SFA Levy co-funding budget                   | 0      | 0      | 0      |
        | SFA Levy additional payments budget          | 39.25  | 39.25  | 39.25  |
        | SFA non-Levy co-funding budget               | 0      | 0      | 0      |
        | SFA non-Levy additional payments budget      | 0      | 0      | 0      |
		
	And the following transfers from employer 2 exist for the given months of earnings activity:
		| Recipient  | 05/18 | 06/18 | 07/18 |
		| Employer 1 | 0     | 0     | 0     |

#Scenario: Levy apprentice, transfer set up but no funds available from sender, receiving employer uses co-funding to fund apprentice  
#
#    Given The learner is programme only DAS
#	And a transfer agreement has been set up between employer 1 and employer 2 
#	And employer b's levy balance = 0 for all months
#	And employer b's transfer allowance = 0 for all months
#	And employer a's levy balance = 0
#	And the apprenticeship funding band maximum is 9000
#
#	And the following commitments exist:
#		| employer of apprentice | employer paying for training | commitment Id | version Id | ULN       | start date | end date   | standard code | agreed price | status | effective from | effective to |
#		| employer a             | employer b                   | 1             | 1          | learner a | 01/05/2018 | 01/05/2019 | 50            | 3000         | Active | 01/05/2018     |              |
#        
#	When an ILR file is submitted for period R10 with the following data:
#        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | standard code | contract type | contract type date from |
#        | learner a | programme only DAS     | 3000         | 06/05/2018 | 20/05/2019       |                 | continuing        | programme        | 2                   |          | 50            | DAS           | 06/05/2018              |
#        | learner a | programme only DAS     |              | 06/05/2018 | 20/05/2019       |                 | continuing        | maths or english | 1                   | 471      | 50            |               |                         |
#        
#   
#	Then the provider earnings and payments break down as follows:
#        | Type                                    | 05/18  | 06/18  | 07/18  | 08/18    |
#        | Provider Earned Total                   | 239.25 | 239.25 | 239.25 | 239.25   |
#        | Provider Earned from SFA                | 219.25 | 219.25 | 219.25 | 219.25   |
#        | Provider Earned from employer a         | 20     | 20     | 20     | 20       |
#		| Provider Earned from employer b         | 0      | 0      | 0      | 0        |
#        | Provider Paid by SFA                    | 0      | 219.25 | 219.25 | 219.25   |
#        | Refund taken by SFA                     | 0      | 0      | 0      | 0        |
#        | Payment due from employer a             | 0      | 20     | 20     | 20       |
#		| Payment due from employer b             | 0      | 0      | 0      | 0        |
#        | Refund due to employer a                | 0      | 0      | 0      | 0        |
#		| Refund due to employer b                | 0      | 0      | 0      | 0        |
#        | Levy account for employer a debited     | 0      | 0      | 0      | 0        |
#		| Levy account for employer b debited     | 0      | 0      | 0      | 0        |
#        | Levy account for employer a credited    | 0      | 0      | 0      | 0        |
#		| Levy account for employer b credited    | 0      | 0      | 0      | 0        |
#        | SFA Levy employer budget                | 0      | 0      | 0      | 0        |
#        | SFA Levy co-funding budget              | 180    | 180    | 180    | 180      |
#        | SFA Levy additional payments budget     | 39.25  | 39.25  | 39.25  | 39.25    |
#        | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0        |
#        | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0        |
#		
#AND the following transfers from employer b exist
#|Recipient|05/18|06/18|07/18|08/18|
#|Employer b|0|0|0|0|