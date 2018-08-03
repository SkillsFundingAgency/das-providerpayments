Feature: Holding back completion payments
 
Scenario: AC1 - 1 learner, levy, co-funding has been used and provider data shows enough employer contribution – pay completion

	Given levy balance = 0 for all months
	
	And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
        | ULN       | start date | end date   | agreed price | status |
        | learner a | 01/06/2017 | 01/06/2018 | 9000         | active |

    When an ILR file is submitted for period R01 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | 
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 	              | continuing        |

	And an ILR file is submitted for period R11 with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status |
        | learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         |
        #| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | Employer contributions on ILR |
        #| learner a | programme only DAS | 9000         | 06/06/2017 | 08/06/2018       | 18/06/2018      | completed         | 720                           |

	#And the following historic employer contributions exist:
	#	| Historic employer contributions |
	#	| 120 						      |

    Then the provider earnings and payments break down as follows:
        | Type                                    | 06/17 | 07/17 | 08/17 | ... | 05/18 | 06/18 | 07/18 |
        | Provider Earned Total                   | 0     | 0     | 600   | ... | 600   | 1800  | 0     |
        | Provider Paid by SFA                    | 0     | 0     | 0     | ... | 540   | 540   | 1620  |
        | Payment due from Employer               | 0     | 0     | 0     | ... | 60    | 60    | 180   |
        | Levy account debited                    | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy employer budget                | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA Levy co-funding budget              | 0     | 0     | 540   | ... | 540   | 1620  | 0     |
        | SFA Levy additional payments budget     | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget          | 0     | 0     | 0     | ... | 0     | 0     | 0     |
        | SFA non-Levy additional payments budget | 0     | 0     | 0     | ... | 0     | 0     | 0     |

    And the transaction types for the payments are:
	    | Payment type | 07/17 | 08/17 | 09/17 | ... | 05/18 | 06/18 | 07/18 |
	    | On-program   | 0     | 0		| 540   | ... | 540   | 540   | 0     |
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
