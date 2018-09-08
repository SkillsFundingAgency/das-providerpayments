Feature: Holding back completion payments
 
Scenario: AC1 - 1 learner, levy, co-funding has been used and provider data shows enough employer contribution – pay completion

	Given levy balance = 0 for all months
	
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
        | ULN       | start date | end date   | agreed price | status |
        | learner a | 01/06/2017 | 01/06/2018 | 9000         | active |

    When an ILR file is submitted for academic year 1617 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
		| learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       |                 | continuing        |

    And an ILR file is submitted for academic year 1718 in period R01 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
		| learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       |                 | continuing        |

    And an ILR file is submitted for academic year 1718 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 720                   |

    Then the provider earnings and payments break down as follows:
        | Type                                    | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18 | 07/18 |
        | Provider Earned Total                   | 600   | 600   | 600   | ... | 600   | 1800  | 0     |
        | Provider Paid by SFA                    | 0     | 540   | 540   | ... | 540   | 540   | 1620  |
        | Payment due from Employer               | 0     | 60    | 60    | ... | 60    | 60    | 180   |
        | Levy account debited                    | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy employer budget                | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy co-funding budget              | 540   | 540   | 540   | ... | 540   | 1620  | 0     |
        | SFA Levy additional payments budget     | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget          | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy additional payments budget | 0     | 0     | 0     | ... | 0     | 0     | 0     |

    And the transaction types for the payments are:
	    | Payment type | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18 |
	    | On-program   | 540   | 540   | 540   | ... | 540   | 540   | 0     |
	    | Completion   | 0     | 0     | 0     | ... | 0     | 0     | 1620  |
	    | Balancing    | 0     | 0     | 0     | ... | 0     | 0     | 0     |

#Maths.
#Price x 0.20 = £7,200
#£7,200 x 0.90 = £720 = Employer Contribution
#£720/12 = £60 
#ILR says submission made at R11, therefore contributions = £660
#
#Completion payment workings:
#£9000 x 0.20 = £1,800
#£1800 x 0.90 (for co-funded) = £1620 = SFA, & £180 Employer contribution.

Scenario: AC2 - 1 learner, non levy, non-Das, co-funding has been used and provider data shows enough employer contribution – pay completion

	Given the apprenticeship funding band maximum is 9000 

    When an ILR file is submitted for academic year 1617 in period R11 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |

    And an ILR file is submitted for academic year 1718 in period R01 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |

    And an ILR file is submitted for academic year 1718 in period R11 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 720					  |
                                                                         
    Then the provider earnings and payments break down as follows:       
        | Type                                	  | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18 | 07/18 |
        | Provider Earned Total               	  | 600   | 600   | 600   | ... | 600   | 1800  | 0	    |
        | Provider Paid by SFA                	  | 0     | 540   | 540   | ... | 540   | 540   | 1620  | 
	    | Payment due from Employer	      	      | 0	  | 60    | 60    | ... | 60    | 60    | 180   |
        | Levy account debited                	  | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy employer budget            	  | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy co-funding budget          	  | 0     | 0     | 0     | ... | 0     | 0	    | 0     |
        | SFA Levy additional payments budget 	  | 0     | 0     | 0     | ... | 0     | 0	    | 0     |
	    | SFA non-Levy co-funding budget          | 540   | 540   | 540   | ... | 540   | 1620  | 0		| 
	    | SFA non-Levy additional payments budget | 0     | 0     | 0     | ... | 0     | 0     | 0     |

    And the transaction types for the payments are:
	    | Payment type                            | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18 |
        | On-program                              | 540   | 540   | 540   | ... | 540   | 540   | 0	    |
        | Completion                              | 0     | 0     | 0     | ... | 0     | 0     | 1620  |
        | Balancing                               | 0     | 0     | 0     | ... | 0     | 0     | 0	    |
		
#Maths.
#Price x 0.20 = £7,200
#£7,200 x 0.90 = £720 = Employer Contribution
#£720/12 = £60

#Completion payment workings:
#£9000 x 0.20 = £1,800
#£1800 x 0.90 (for co-funded) = £1620 = SFA, & £180 Employer contribution.

#We expect the employer contributions to total 720 in order for the completion payment to be released to the training provider.

Scenario: AC3 - 1 learner, levy, co-funding has been used and provider data shows not enough employer contribution – don’t pay completion

    Given levy balance = 0 for all months
	
	And the apprenticeship funding band maximum is 9000
	
	And the following commitments exist:
        | ULN       | start date | end date   | agreed price | status |
        | learner a | 01/06/2017 | 01/06/2018 | 9000         | active |

    When an ILR file is submitted for academic year 1617 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |
    And an ILR file is submitted for academic year 1718 in period R01 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |

    When an ILR file is submitted for academic year 1718 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 719					  |

    Then the provider earnings and payments break down as follows:
        | Type                                    | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18 | 07/18 |
        | Provider Earned Total                   | 600   | 600   | 600   | ... | 600   | 1800  | 0     |
        | Provider Paid by SFA                    | 0     | 540   | 540   | ... | 540   | 540   | 0     |
        | Payment due from Employer               | 0     | 60    | 60    | ... | 60    | 60    | 0     |
        | Levy account debited                    | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy employer budget                | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy co-funding budget              | 540   | 540   | 540   | ... | 540   | 0     | 0     |
        | SFA Levy additional payments budget     | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget          | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy additional payments budget | 0     | 0     | 0     | ... | 0     | 0     | 0     |
	
    And the transaction types for the payments are:
	    | Payment type                        | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18 |
        | On-program                          | 540   | 540   | 540   | ... | 540   | 540   | 0	    |
        | Completion                          | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | Balancing                           | 0     | 0     | 0     | ... | 0     | 0     | 0	    |
		
#Maths.
#Price x 0.20 = £7,200
#£7,200 x 0.90 = £720 = Employer Contribution
#£720/12 = £60 

#Completion payment workings:
#£9000 x 0.20 = £1,800
#£1800 x 0.90 (for co-funded) = £1620 = SFA, & £180 Employer contribution.
#We expect the employer contributions to total 620 in order for the completion payment to be released to the training provider.

Scenario: AC4 - 1 learner, non levy, co-funding has been used and provider data shows not enough employer contribution – don't pay completion

	Given the apprenticeship funding band maximum is 9000

    When an ILR file is submitted for academic year 1617 in period R11 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |
		
	And an ILR file is submitted for academic year 1718 in period R01 with the following data:	
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |		
		
	And an ILR file is submitted for academic year 1718 in period R11 with the following data:

        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 719				      |

    Then the provider earnings and payments break down as follows:
        | Type                                	  | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18 | 07/18 |
        | Provider Earned Total               	  | 600   | 600   | 600   | ... | 600   | 1800  | 0	    |
        | Provider Paid by SFA                	  | 0     | 540   | 540   | ... | 540   | 540   | 0     | 
	    | Payment due from Employer	      	      | 0     | 60    | 60    | ... | 60    | 60    | 0     |
        | Levy account debited                	  | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy employer budget            	  | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy co-funding budget          	  | 0     | 0     | 0     | ... | 0     | 0	    | 0     |
        | SFA Levy additional payments budget 	  | 0     | 0     | 0     | ... | 0     | 0     | 0     |
	    | SFA non-Levy co-funding budget          | 540   | 540   | 540   | ... | 540   | 0     | 0     |
	    | SFA non-Levy additional payments budget | 0     | 0     | 0     | ... | 0     | 0     | 0     |

   	And the transaction types for the payments are:
	    | Payment type                            | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18 |
        | On-program                              | 540   | 540   | 540   | ... | 540   | 540   | 0	    |
        | Completion                              | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | Balancing                               | 0     | 0     | 0     | ... | 0     | 0     | 0	    |
		
#Maths.
#Price x 0.20 = £7,200
#£7,200 x 0.90 = £720 = Employer Contribution
#£720/12 = £60

#Completion payment workings:
#£9000 x 0.20 = £1,800
#£1800 x 0.90 (for co-funded) = £1620 = SFA, & £180 Employer contribution.

#We expect the employer contributions to total 720 in order for the completion payment to be released to the training provider.

Scenario: AC5 - 1 learner, levy, co-funding has been used and provider data shows not enough employer contribution at the time completion is recorded, 
but then another contribution is evidenced later – don’t pay completion initially, but then release completion payment when extra contribution is recorded

	Given levy balance = 0 for all months
	
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
        | ULN       | start date | end date   | agreed price | status |
        | learner a | 01/06/2017 | 01/06/2018 | 9000         | active |
		
	When an ILR file is submitted for academic year 1617 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |
		
	And an ILR file is submitted for academic year 1718 in period R01 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |
		
    And an ILR file is submitted for academic year 1718 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 719					   |

    And an ILR file is submitted for academic year 1718 in period R12 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 720					   |
	
	Then the provider earnings and payments break down as follows:
        | Type                                       | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18 | 07/18 | 08/18 |
        | Provider Earned Total                      | 600   | 600   | 600   | ... | 600   | 1800  | 0     | 0     |
        | Provider Paid by SFA                       | 0     | 540   | 540   | ... | 540   | 540   | 0     | 1620  |
	    | Payment due from Employer                  | 0     | 60    | 60    | ... | 60    | 60    | 0     | 180   |
        | Levy account debited                       | 0     | 0     | 0     | ... | 0     | 0     | 0     | 0     |
        | SFA Levy employer budget                   | 0     | 0     | 0     | ... | 0     | 0     | 0     | 0     |
        | SFA Levy co-funding budget                 | 540   | 540   | 540   | ... | 540   | 1620  | 0	   | 0     |
        | SFA Levy additional payments budget        | 0     | 0     | 0     | ... | 0     | 0	   | 0     | 0     |
		| SFA non-Levy co-funding budget             | 0     | 0     | 0     | ... | 0     | 0     | 0     | 0     |
	    | SFA non-Levy additional payments budget    | 0     | 0     | 0     | ... | 0     | 0     | 0     | 0     |

    And the transaction types for the payments are:
	    | Payment type                               | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18 | 08/18 |
        | On-program                                 | 540   | 540   | 540   | ... | 540   | 540   | 0	   | 0     |
        | Completion                                 | 0     | 0     | 0     | ... | 0     | 0     | 1620  | 0     |
        | Balancing                                  | 0     | 0     | 0     | ... | 0     | 0     | 0	   | 0	   |
			
#Maths.
#Price x 0.20 = £7,200
#£7,200 x 0.90 = £720 = Employer Contribution
#£720/12 = £60 
#
#Completion payment workings:
#£9000 x 0.20 = £1,800
#£1800 x 0.90 (for co-funded) = £1620 = SFA, & £180 Employer contribution.
#
#We expect the employer contributions to total 720 in order for the completion payment to be released to the training provider.

Scenario: AC6 - 1 learner, non-levy, co-funding has been used and provider data shows not enough employer contribution at the time completion is recorded, 
but then another contribution is evidenced later – don’t pay completion initially, but then release completion payment when extra contribution is recorded

	Given the apprenticeship funding band maximum is 9000

    When an ILR file is submitted for academic year 1617 in period R11 with the following data:
		| ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | 
		| learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |

	And an ILR file is submitted for academic year 1718 in period R01 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |	
	
	And an ILR file is submitted for academic year 1718 in period R11 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 719					  |
		
    And an ILR file is submitted for academic year 1718 in period R12 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only non-DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 720					  |

	Then the provider earnings and payments break down as follows:
        | Type                                       | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18 | 07/18 | 08/18 |
        | Provider Earned Total                      | 600   | 600   | 600   | ... | 600   | 1800  | 0     | 0     |
        | Provider Paid by SFA                       | 0     | 540   | 540   | ... | 540   | 540   | 0     | 1620  |
	    | Payment due from Employer                  | 0     | 60    | 60    | ... | 60    | 60    | 0     | 180   |
        | Levy account debited                       | 0     | 0     | 0     | ... | 0     | 0     | 0     | 0     |
        | SFA Levy employer budget                   | 0     | 0     | 0     | ... | 0     | 0     | 0     | 0     |
        | SFA Levy co-funding budget                 | 0     | 0     | 0     | ... | 0     | 0     | 0     | 0     |
        | SFA Levy additional payments budget        | 0     | 0     | 0     | ... | 0     | 0	   | 0     | 0     |
	    | SFA non-Levy co-funding budget             | 540   | 540   | 540   | ... | 540   | 1620  | 0     | 0     |
	    | SFA non-Levy additional payments budget    | 0     | 0     | 0     | ... | 0     | 0     | 0     | 0     |

    And the transaction types for the payments are:
	    | Payment type                               | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18 | 08/18 |
        | On-program                                 | 540   | 540   | 540   | ... | 540   | 540   | 0	   | 0     |
        | Completion                                 | 0     | 0     | 0     | ... | 0     | 0     | 1620  | 0     |
        | Balancing                                  | 0     | 0     | 0     | ... | 0     | 0     | 0	   | 0	   |
			
#Maths.
#Price x 0.20 = £7,200
#£7,200 x 0.90 = £720 = Employer Contribution
#£720/12 = £60 
#
#Completion payment workings:
#£9000 x 0.20 = £1,800
#£1800 x 0.90 (for co-funded) = £1620 = SFA, & £180 Employer contribution.
#
#We expect the employer contributions to total 720 in order for the completion payment to be released to the training provider.

Scenario: AC7 - 1 learner, levy, planned end date is last day of month, co-funding has been used and provider data shows enough employer contribution – pay completion

	Given levy balance = 0 for all months
	
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
        | ULN       | start date | end date   | agreed price | status |
        | learner a | 01/06/2017 | 01/06/2018 | 8125         | active |

    When an ILR file is submitted for academic year 1617 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only DAS | 8125         | 06/06/2017 | 30/06/2018       | 	              | continuing        |
		
	And an ILR file is submitted for academic year 1718 in period R01 with the following data:	
		
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only DAS | 8125         | 06/06/2017 | 30/06/2018       | 	              | continuing        |	
		
	And an ILR file is submitted for academic year 1718 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only DAS | 8125         | 06/06/2017 | 30/06/2018       | 30/06/2018      | completed         | 600				       |

    Then the provider earnings and payments break down as follows:
        | Type                                    | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18   | 07/18   |
        | Provider Earned Total                   | 500   | 500   | 500   | ... | 500   | 2125    | 0       |
        | Provider Paid by SFA                    | 0     | 450   | 450   | ... | 450   | 450     | 1912.50 |
        | Payment due from Employer               | 0     | 50    | 50    | ... | 50    | 50      | 212.50  |
        | Levy account debited                    | 0     | 0     | 0     | ... | 0     | 0       | 0       |
        | SFA Levy employer budget                | 0     | 0     | 0     | ... | 0     | 0       | 0       |
        | SFA Levy co-funding budget              | 450   | 450   | 450   | ... | 450   | 1912.50 | 0       |
        | SFA Levy additional payments budget     | 0     | 0     | 0     | ... | 0     | 0       | 0       |
        | SFA non-Levy co-funding budget          | 0     | 0     | 0     | ... | 0     | 0       | 0       |
        | SFA non-Levy additional payments budget | 0     | 0     | 0     | ... | 0     | 0       | 0       |

    And the transaction types for the payments are:
	    | Payment type                            | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18   |
        | On-program                              | 450   | 450   | 450   | ... | 450   | 450   | 450     | 
        | Completion                              | 0     | 0     | 0     | ... | 0     | 0     | 1462.50 |
        | Balancing                               | 0     | 0     | 0     | ... | 0     | 0     | 0	      |

#Maths.
#Agreed price: 8125
#Completion payment: 20% of agreed price = 1625
#
#6500 on program payments (13 installments of 500)
#450 paid by ESFA and 50 due from employer (per month)
#Expected employer contributions is 600
#
#90% of completion payment (ESFA) = 1462.50
#10% of completion payment (Employer) = 162.50
#
#We expect the employer contributions to total 600 in order for the completion payment to be released to the training provider.

		
#Maths.
#Price x 0.20 = £7,200
#£7,200 x 0.90 = £720 = Employer Contribution
#£720/12 = £60 

#Completion payment workings:
#£9000 x 0.20 = £1,800
#£1800 x 0.90 (for co-funded) = £1620 = SFA, & £180 Employer contribution.
#We expect the employer contributions to total 620 in order for the completion payment to be released to the training provider.

Scenario: AC8 - 1 learner, non-levy, planned end date is last day of month, co-funding has been used and provider data shows enough employer contribution – pay completion

	Given the apprenticeship funding band maximum is 9000

    When an ILR file is submitted for academic year 1617 in period R11 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only non-DAS | 8125         | 06/06/2017 | 30/06/2018       | 	              | continuing        |
		
	And an ILR file is submitted for academic year 1718 in period R01 with the following data:	
		| ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only non-DAS | 8125         | 06/06/2017 | 30/06/2018       | 	              | continuing        |
		
	And an ILR file is submitted for academic year 1718 in period R11 with the following data:
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | employer contribution |
        | learner a | programme only non-DAS | 8125         | 06/06/2017 | 30/06/2018       | 30/06/2018      | completed         | 600				      |

    Then the provider earnings and payments break down as follows:
        | Type                                    | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18   | 07/18   |
        | Provider Earned Total                   | 500   | 500   | 500   | ... | 500   | 2125    | 0       |
        | Provider Paid by SFA                    | 0     | 450   | 450   | ... | 450   | 450     | 1912.50 |
        | Payment due from Employer               | 0     | 50    | 50    | ... | 50    | 50      | 212.50  |
        | Levy account debited                    | 0     | 0     | 0     | ... | 0     | 0       | 0       |
        | SFA Levy employer budget                | 0     | 0     | 0     | ... | 0     | 0       | 0       |
        | SFA Levy co-funding budget              | 0     | 0     | 0     | ... | 0     | 0       | 0       |
        | SFA Levy additional payments budget     | 0     | 0     | 0     | ... | 0     | 0       | 0       |
        | SFA non-Levy co-funding budget          | 450   | 450   | 450   | ... | 450   | 1912.50 | 0       |
        | SFA non-Levy additional payments budget | 0     | 0     | 0     | ... | 0     | 0       | 0       |

    And the transaction types for the payments are:
	    | Payment type                            | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18   |
        | On-program                              | 450   | 450   | 450   | ... | 450   | 450   | 450	  | 
        | Completion                              | 0     | 0     | 0     | ... | 0     | 0     | 1462.50 |
        | Balancing                               | 0     | 0     | 0     | ... | 0     | 0     | 0	      |

#Maths.
#Agreed price: 8125
#Completion payment: 20% of agreed price = 1625
#
#6500 on program payments (13 installments of 500)
#450 paid by ESFA and 50 due from employer (per month)
#Expected employer contributions is 600
#
#90% of completion payment (ESFA) = 1462.50
#10% of completion payment (Employer) = 162.50
#
#We expect the employer contributions to total 600 in order for the completion payment to be released to the training provider.

Scenario: AC9 - 1 learner, levy, LDM code 356 used, co-funding has been used and provider data shows enough employer contribution – pay completion

	Given levy balance = 0 for all months
	
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
        | ULN       | start date | end date   | agreed price | status |
        | learner a | 01/06/2017 | 01/06/2018 | 9000         | active |

    When an ILR file is submitted for academic year 1617 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | LearnDelFAM |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       |                 | continuing        | LDM356      |
		
	And an ILR file is submitted for academic year 1718 in period R01 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | LearnDelFAM |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        | LDM356      |

	And an ILR file is submitted for academic year 1718 in period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | employer contribution | LearnDelFAM | 
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 720					  | LDM356      |

    Then the provider earnings and payments break down as follows:

        | Type                                    | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18 | 07/18 |
        | Provider Earned Total                   | 600   | 600   | 600   | ... | 600   | 1800  | 0	    |
        | Provider Paid by SFA                    | 0     | 540   | 540   | ... | 540   | 540   | 1620  | 
	    | Payment due from Employer               | 0     | 60    | 60    | ... | 60    | 60    | 180   |
        | Levy account debited                    | 0     | 0     | 0     | ... | 0     | 0     | 0     | 
        | SFA Levy employer budget                | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy co-funding budget              | 540   | 540   | 540   | ... | 540   | 1620  | 0  |
        | SFA Levy additional payments budget     | 0     | 0     | 0     | ... | 0     | 0	    | 0     |
		| SFA non-Levy co-funding budget          | 0     | 0     | 0     | ... | 0     | 0     | 0     | 
	    | SFA non-Levy additional payments budget | 0     | 0     | 0     | ... | 0     | 0     | 0     |

    And the transaction types for the payments are:

	    | Payment type                            | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18 |
        | On-program                              | 540   | 540   | 540   | ... | 540   | 540   | 0	    |
        | Completion                              | 0     | 0     | 0     | ... | 0     | 0     | 1620  |
        | Balancing                               | 0     | 0     | 0     | ... | 0     | 0     | 0	    |

#Maths.
#Price x 0.20 = £7,200
#£7,200 x 0.90 = £720 = Employer Contribution
#£720/12 = £60
#
#Completion payment workings:
#£9000 x 0.20 = £1,800
#£1800 x 0.90 (for co-funded) = £1620 = SFA, & £180 Employer contribution.
#
#We expect the employer contributions to total 720 in order for the completion payment to be released to the training provider.
#
#Completion Payment always released when LDM code 356 has been submitted in ILR.
#
