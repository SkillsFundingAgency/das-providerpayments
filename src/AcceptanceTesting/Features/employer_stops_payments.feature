@EmployerStopsPayments
Feature: Employer stops payments on a commitment

Scenario: Commitment payments are stopped midway through the learning episode
    Given levy balance > agreed price for all months
    And the following commitments exist:
        | commitment Id | version Id | ULN       | start date | end date   | status | agreed price | effective from | effective to |
        | 1             | 1          | learner a | 01/09/2017 | 08/09/2018 | active | 15000        | 01/09/2017     | 31/10/2017   |
        | 1             | 2          | learner a | 01/09/2017 | 08/09/2018 | paused | 15000        | 01/11/2017     |              |
    When an ILR file is submitted every month with the following data:
        | ULN       | agreed price | learner type       | start date | planned end date | completion status |
        | learner a | 15000        | programme only DAS | 01/09/2017 | 08/09/2018       | continuing        |
    Then the provider earnings and payments break down as follows:
        | Type                          | 09/17 | 10/17 | 11/17 | 12/17 | ... | 03/18 |
        | Provider Earned Total         | 1000  | 1000  | 1000  | 1000  | ... | 1000  |
        | Provider Earned from SFA      | 1000  | 1000  | 0     | 0     | ... | 0     |
        | Provider Earned from Employer | 0     | 0     | 0     | 0     | ... | 0     |
        | Provider Paid by SFA          | 0     | 1000  | 1000  | 0     | ... | 0     |
        | Payment due from Employer     | 0     | 0     | 0     | 0     | ... | 0     |
        | Levy account debited          | 0     | 1000  | 1000  | 0     | ... | 0     |
        | SFA Levy employer budget      | 1000  | 1000  | 0     | 0     | ... | 0     |
        | SFA Levy co-funding budget    | 0     | 0     | 0     | 0     | ... | 0     |
        | SFA non-Levy co-funding budget| 0     | 0     | 0     | 0     | ... | 0     |

            
Scenario: The provider submits the first ILR file after the commitment payments have been stopped
    Given levy balance > agreed price for all months
    And the following commitments exist:
        | commitment Id | version Id | ULN       | start date | end date   | status | agreed price | effective from | effective to |
        | 1             | 1          | learner a | 01/09/2017 | 08/09/2018 | active | 15000        | 01/09/2017     | 02/09/2017   |
        | 1             | 2          | learner a | 01/09/2017 | 08/09/2018 | paused | 15000        | 02/09/2017     |              |
    When an ILR file is submitted for the first time on 28/12/17 with the following data:
        | ULN       | agreed price | learner type       | start date | planned end date | completion status |
        | learner a | 15000        | programme only DAS | 01/09/2017 | 08/09/2018       | continuing        |
    Then the provider earnings and payments break down as follows:
        | Type                          | 09/17 | 10/17 | 11/17 | 12/17 | ... | 03/18 |
        | Provider Earned Total         | 1000  | 1000  | 1000  | 1000  | ... | 1000  |
        | Provider Earned from SFA      | 0     | 0     | 0     | 0     | ... | 0     |
        | Provider Earned from Employer | 0     | 0     | 0     | 0     | ... | 0     |
        | Provider Paid by SFA          | 0     | 0     | 0     | 0     | ... | 0     |
        | Payment due from Employer     | 0     | 0     | 0     | 0     | ... | 0     |
        | Levy account debited          | 0     | 0     | 0     | 0     | ... | 0     |
        | SFA Levy employer budget      | 0     | 0     | 0     | 0     | ... | 0     |
        | SFA Levy co-funding budget    | 0     | 0     | 0     | 0     | ... | 0     |
        | SFA non-Levy co-funding budget| 0     | 0     | 0     | 0     | ... | 0     |

@_Minimum_Acceptance_
Scenario:700_AC01 DAS learner, payments are stopped as the employer has never paid levy
 
        Given the employer is not a levy payer
		And the following commitments exist:
            | commitment Id | ULN       | priority | start date | end date   | agreed price | 
            | 1             | learner a | 1        | 01/08/2017 | 01/08/2018 | 15000        | 
		When an ILR file is submitted with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
			| learner a | programme only DAS | 15000        | 05/08/2017 | 20/08/2018       |                 | continuing        |
		Then the data lock status will be as follows:
			| Payment type                   | 08/17           | 09/17           | 10/17           | 11/17           | 12/17           |
			| On-program                     |                 |                 |                 |                 |                 |
			| Completion                     |                 |                 |                 |                 |                 |
			| Employer 16-18 incentive       |                 |                 |                 |                 |                 |
			| Provider 16-18 incentive       |                 |                 |                 |                 |                 |
			| Provider learning support      |                 |                 |                 |                 |                 |
			| English and maths on programme |                 |                 |                 |                 |                 |
			| English and maths Balancing    |                 |                 |                 |                 |                 |     

		And the provider earnings and payments break down as follows:
            | Type                       | 08/17 | 09/17 | 10/17 | 
            | Provider Earned Total      | 1000  | 1000  | 1000  | 
            | Provider Earned from SFA   | 0     | 0     | 0     | 
            | Provider Paid by SFA       | 0     | 0     | 0     | 
            | Levy account debited       | 0     | 0     | 0     | 
            | SFA Levy employer budget   | 0     | 0     | 0     | 
            | SFA Levy co-funding budget | 0     | 0     | 0     | 
            

Scenario:700_AC02 DAS learner, payments are allowed as the employer has previously paid levy
 
    Given levy balance = 0 for all months
	And the following commitments exist:
        | commitment Id | version Id | ULN       | priority | start date | end date   | agreed price |
        | 1             | 1-001      | learner a | 1        | 01/08/2017 | 01/08/2018 | 15000        | 
            
    When an ILR file is submitted with the following data:
        |ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
        |learner a | programme only DAS | 15000        | 05/08/2017 | 20/08/2018       |                 | continuing        |
      
	Then the data lock status will be as follows:
		| Payment type                   | 08/17               | 09/17               | 10/17               | 11/17               | 12/17               |
		| On-program                     | commitment 1 v1-001 | commitment 1 v1-001 | commitment 1 v1-001 | commitment 1 v1-001 | commitment 1 v1-001 |
		| Completion                     |                     |                     |                     |                     |                     |
		| Employer 16-18 incentive       |                     |                     |                     |                     |                     |
		| Provider 16-18 incentive       |                     |                     |                     |                     |                     |
		| Provider learning support      |                     |                     |                     |                     |                     |
		| English and maths on programme |                     |                     |                     |                     |                     |
		| English and maths Balancing    |                     |                     |                     |                     |                     |     

	Then OBSOLETE - the provider earnings and payments break down as follows:
        | Type                          | 08/17 | 09/17 | ... | 01/18 |
        | Provider Earned Total         | 1000  | 1000  | ... | 1000  |
        | Provider Earned from SFA      | 900   | 900   | ... | 900   |
        | Provider Earned from Employer | 100   | 100   | ... | 100   |
        | Provider Paid by SFA          | 0     | 900   | ... | 900   |
        | Payment due from Employer     | 0     | 100   | ... | 100   |
        | Levy account debited          | 0     | 0     | ... | 0     |
        | SFA Levy employer budget      | 0     | 0     | ... | 0     |
        | SFA Levy co-funding budget    | 900   | 900   | ... | 900   |
														 

Scenario: 1647 a Employer Stops After Completion Payment
#Employer stops commitment after the day that the course is completed, but in the same month. Expecting completion payment to be paid
  
  Given levy balance > agreed price for all months
  And the following commitments exist:
        | commitment Id | version Id | ULN       | start date | end date   | status    | agreed price | effective from | effective to | stop effective from |
        | 1             | 1          | learner a | 01/08/2018 | 01/09/2019 | cancelled | 15000        | 01/08/2018     | 01/09/2019   | 15/08/2019          |
            
  When an ILR file is submitted with the following data:
        | ULN       | agreed price | learner type       | start date | planned end date | actual end date   |completion status|
        | learner a | 15000        | programme only DAS | 01/08/2018 | 09/08/2019       | 09/08/2019        |completed        |
  
  Then the provider earnings and payments break down as follows:
        | Type                           | 08/18 | ... | 07/19 | 08/19 | 09/19 |
        | Provider Earned Total          | 1000  | ... | 1000  | 3000  | 0     |
        | Provider Earned from SFA       | 1000  | ... | 1000  | 3000  | 0     |
        | Provider Earned from Employer  | 0     | ... | 0     | 0     | 0     |
        | Provider Paid by SFA           | 0     | ... | 1000  | 1000  | 3000  |
        | Payment due from Employer      | 0     | ... | 0     | 0     | 0     |
        | Levy account debited           | 0     | ... | 1000  | 1000  | 3000  |
        | SFA Levy employer budget       | 1000  | ... | 1000  | 3000  | 0     |
        | SFA Levy co-funding budget     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget | 0     | ... | 0     | 0     | 0     |

	And the transaction types for the payments are:
		| Payment type                 | 09/18 | ... | 08/19 | 09/19 |
		| On-program                   | 1000  | ... | 1000  | 0     |
		| Completion                   | 0     | ... | 0     | 3000  |
		| Balancing                    | 0     | ... | 0     | 0     |
		| Employer 16-18 incentive     | 0     | ... | 0     | 0     |
		| Provider 16-18 incentive     | 0     | ... | 0     | 0     |
		| Framework uplift on-program  | 0     | ... | 0     | 0     |
		| Framework uplift completion  | 0     | ... | 0     | 0     |
		| Framework uplift balancing   | 0     | ... | 0     | 0     |
		| Provider disadvantage uplift | 0     | ... | 0     | 0     |

Scenario: 1647 b Employer Stops On Completion Payment
# Employer stops commitment on the day that the course is completed. Expecting completion payment to be paid

	Given levy balance > agreed price for all months
    And the following commitments exist:
        | commitment Id | version Id | ULN       | start date | end date   | status    | agreed price | effective from | effective to | stop effective from |
        | 1             | 1          | learner a | 01/08/2018 | 01/09/2019 | cancelled | 15000        | 01/08/2018     | 01/09/2019   | 09/08/2019          |
       
	When an ILR file is submitted with the following data:
        | ULN       | agreed price | learner type       | start date | planned end date | actual end date | completion status |
        | learner a | 15000        | programme only DAS | 01/08/2018 | 09/08/2019       | 09/08/2019      | completed         |
       
	Then the provider earnings and payments break down as follows:
        | Type                           | 08/18 | ... | 07/19 | 08/19 | 09/19 |
        | Provider Earned Total          | 1000  | ... | 1000  | 3000  | 0     |
        | Provider Earned from SFA       | 1000  | ... | 1000  | 3000  | 0     |
        | Provider Earned from Employer  | 0     | ... | 0     | 0     | 0     |
        | Provider Paid by SFA           | 0     | ... | 1000  | 1000  | 3000  |
        | Payment due from Employer      | 0     | ... | 0     | 0     | 0     |
        | Levy account debited           | 0     | ... | 1000  | 1000  | 3000  |
        | SFA Levy employer budget       | 1000  | ... | 1000  | 3000  | 0     |
        | SFA Levy co-funding budget     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget | 0     | ... | 0     | 0     | 0     |

    And the transaction types for the payments are:
        | Payment type                 | 09/18 | ... | 08/19 | 09/19 |
        | On-program                   | 1000  | ... | 1000  | 0     |
        | Completion                   | 0     | ... | 0     | 3000  |
        | Balancing                    | 0     | ... | 0     | 0     |
        | Employer 16-18 incentive     | 0     | ... | 0     | 0     |
        | Provider 16-18 incentive     | 0     | ... | 0     | 0     |
        | Framework uplift on-program  | 0     | ... | 0     | 0     |
        | Framework uplift completion  | 0     | ... | 0     | 0     |
        | Framework uplift balancing   | 0     | ... | 0     | 0     |
        | Provider disadvantage uplift | 0     | ... | 0     | 0     |

Scenario: 1647 c Employer Stops Before Completion Payment
# Employer stops commitment before the day that the course is completed, but in the same month. Expect completion to not be paid

    Given levy balance > agreed price for all months
    And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | status    | agreed price | effective from | effective to | stop effective from |
		| 1             | 1          | learner a | 01/08/2018 | 01/09/2019 | cancelled | 15000        | 01/08/2018     | 01/09/2019   | 05/08/2019          |
		
	When an ILR file is submitted with the following data:
        | ULN       | agreed price | learner type       | start date | planned end date | actual end date   |completion status|
        | learner a | 15000        | programme only DAS | 01/08/2018 | 09/08/2019       | 09/08/2019        |completed        |
       
	Then the provider earnings and payments break down as follows:
        | Type                           | 08/18 | ... | 07/19 | 08/19 | 09/19 |
        | Provider Earned Total          | 1000  | ... | 1000  | 3000  | 0     |
        | Provider Earned from SFA       | 1000  | ... | 1000  | 0     | 0     |
        | Provider Earned from Employer  | 0     | ... | 0     | 0     | 0     |
        | Provider Paid by SFA           | 0     | ... | 1000  | 1000  | 0     |
        | Payment due from Employer      | 0     | ... | 0     | 0     | 0     |
        | Levy account debited           | 0     | ... | 1000  | 1000  | 0     |
        | SFA Levy employer budget       | 1000  | ... | 1000  | 0     | 0     |
        | SFA Levy co-funding budget     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget | 0     | ... | 0     | 0     | 0     |

	And the transaction types for the payments are:
        | Payment type                 | 09/18 | ... | 08/19 | 09/19 |
        | On-program                   | 1000  | ... | 1000  | 0     |
        | Completion                   | 0     | ... | 0     | 0     |
        | Balancing                    | 0     | ... | 0     | 0     |
        | Employer 16-18 incentive     | 0     | ... | 0     | 0     |
        | Provider 16-18 incentive     | 0     | ... | 0     | 0     |
        | Framework uplift on-program  | 0     | ... | 0     | 0     |
        | Framework uplift completion  | 0     | ... | 0     | 0     |
        | Framework uplift balancing   | 0     | ... | 0     | 0     |
        | Provider disadvantage uplift | 0     | ... | 0     | 0     |



Scenario: 1649 a Employer Stops After Balancing Payment
#Employer stops commitment after the day that the course is completed, but in the same month. Expecting completion payment to be paid
  
  Given levy balance > agreed price for all months
  And the following commitments exist:
        | commitment Id | version Id | ULN       | start date | end date   | status    | agreed price | effective from | effective to | stop effective from |
        | 1             | 1          | learner a | 01/08/2018 | 01/09/2019 | cancelled | 15000        | 01/08/2018     | 01/09/2019   | 15/07/2019          |
            
  When an ILR file is submitted with the following data:
        | ULN       | agreed price | learner type       | start date | planned end date | actual end date   |completion status|
        | learner a | 15000        | programme only DAS | 01/08/2018 | 09/08/2019       | 09/07/2019        |completed        |
  
  Then the provider earnings and payments break down as follows:
        | Type                           | 08/18 | ... | 07/19 | 08/19 |
        | Provider Earned Total          | 1000  | ... | 4000  | 0     |
        | Provider Earned from SFA       | 1000  | ... | 4000  | 0     |
        | Provider Earned from Employer  | 0     | ... | 0     | 0     |
        | Provider Paid by SFA           | 0     | ... | 1000  | 4000  |
        | Payment due from Employer      | 0     | ... | 0     | 0     |
        | Levy account debited           | 0     | ... | 1000  | 4000  |
        | SFA Levy employer budget       | 1000  | ... | 4000  | 0     |
        | SFA Levy co-funding budget     | 0     | ... | 0     | 0     |
        | SFA non-Levy co-funding budget | 0     | ... | 0     | 0     |

	And the transaction types for the payments are:
		| Payment type                 | 09/18 | ... | 07/19 | 08/19 |
		| On-program                   | 1000  | ... | 1000  | 0     |
		| Completion                   | 0     | ... | 0     | 3000  |
		| Balancing                    | 0     | ... | 0     | 1000  |
		| Employer 16-18 incentive     | 0     | ... | 0     | 0     |
		| Provider 16-18 incentive     | 0     | ... | 0     | 0     |
		| Framework uplift on-program  | 0     | ... | 0     | 0     |
		| Framework uplift completion  | 0     | ... | 0     | 0     |
		| Framework uplift balancing   | 0     | ... | 0     | 0     |
		| Provider disadvantage uplift | 0     | ... | 0     | 0     |


Scenario: 1649 b Employer Stops On Balancing Payment
# Employer stops commitment on the day that the course is completed. Expecting completion payment to be paid

	Given levy balance > agreed price for all months
    And the following commitments exist:
        | commitment Id | version Id | ULN       | start date | end date   | status    | agreed price | effective from | effective to | stop effective from |
        | 1             | 1          | learner a | 01/08/2018 | 01/09/2019 | cancelled | 15000        | 01/08/2018     | 01/09/2019   | 09/07/2019          |
       
	When an ILR file is submitted with the following data:
        | ULN       | agreed price | learner type       | start date | planned end date | actual end date | completion status |
        | learner a | 15000        | programme only DAS | 01/08/2018 | 09/08/2019       | 09/07/2019      | completed         |
       
	Then the provider earnings and payments break down as follows:
        | Type                           | 08/18 | ... | 06/19 | 07/19 | 08/19 |
        | Provider Earned Total          | 1000  | ... | 1000  | 4000  | 0     |
        | Provider Earned from SFA       | 1000  | ... | 1000  | 4000  | 0     |
        | Provider Earned from Employer  | 0     | ... | 0     | 0     | 0     |
        | Provider Paid by SFA           | 0     | ... | 1000  | 1000  | 4000  |
        | Payment due from Employer      | 0     | ... | 0     | 0     | 0     |
        | Levy account debited           | 0     | ... | 1000  | 1000  | 4000  |
        | SFA Levy employer budget       | 1000  | ... | 1000  | 4000  | 0     |
        | SFA Levy co-funding budget     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget | 0     | ... | 0     | 0     | 0     |

    And the transaction types for the payments are:
        | Payment type                 | 09/18 | ... | 07/19 | 08/19 |
        | On-program                   | 1000  | ... | 1000  | 0     |
        | Completion                   | 0     | ... | 0     | 3000  |
        | Balancing                    | 0     | ... | 0     | 1000  |
        | Employer 16-18 incentive     | 0     | ... | 0     | 0     |
        | Provider 16-18 incentive     | 0     | ... | 0     | 0     |
        | Framework uplift on-program  | 0     | ... | 0     | 0     |
        | Framework uplift completion  | 0     | ... | 0     | 0     |
        | Framework uplift balancing   | 0     | ... | 0     | 0     |
        | Provider disadvantage uplift | 0     | ... | 0     | 0     |

Scenario: 1649 c Employer Stops Before Balancing Payment
# Employer stops commitment before the day that the course is completed, but in the same month. Expect completion to not be paid

    Given levy balance > agreed price for all months
    And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | status    | agreed price | effective from | effective to | stop effective from |
		| 1             | 1          | learner a | 01/08/2018 | 01/09/2019 | cancelled | 15000        | 01/08/2018     | 01/09/2019   | 05/07/2019          |
		
	When an ILR file is submitted with the following data:
        | ULN       | agreed price | learner type       | start date | planned end date | actual end date   |completion status|
        | learner a | 15000        | programme only DAS | 01/08/2018 | 09/08/2019       | 09/07/2019        |completed        |
       
	Then the provider earnings and payments break down as follows:
        | Type                           | 08/18 | ... | 06/19 | 07/19 | 08/19 |
        | Provider Earned Total          | 1000  | ... | 1000  | 4000  | 0     |
        | Provider Earned from SFA       | 1000  | ... | 1000  | 0     | 0     |
        | Provider Earned from Employer  | 0     | ... | 0     | 0     | 0     |
        | Provider Paid by SFA           | 0     | ... | 1000  | 1000  | 0     |
        | Payment due from Employer      | 0     | ... | 0     | 0     | 0     |
        | Levy account debited           | 0     | ... | 1000  | 1000  | 0     |
        | SFA Levy employer budget       | 1000  | ... | 1000  | 0     | 0     |
        | SFA Levy co-funding budget     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget | 0     | ... | 0     | 0     | 0     |

	And the transaction types for the payments are:
        | Payment type                 | 09/18 | ... | 07/19 | 08/19 |
        | On-program                   | 1000  | ... | 1000  | 0     |
        | Completion                   | 0     | ... | 0     | 0     |
        | Balancing                    | 0     | ... | 0     | 0     |
        | Employer 16-18 incentive     | 0     | ... | 0     | 0     |
        | Provider 16-18 incentive     | 0     | ... | 0     | 0     |
        | Framework uplift on-program  | 0     | ... | 0     | 0     |
        | Framework uplift completion  | 0     | ... | 0     | 0     |
        | Framework uplift balancing   | 0     | ... | 0     | 0     |
        | Provider disadvantage uplift | 0     | ... | 0     | 0     |