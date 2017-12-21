@learner_changes_start_date
Feature: Learner changes start date

Scenario: DPP-964_01 - DAS Learner - Change to start date within calendar month, forward in month
    Given The learner is programme only DAS
       
	And levy balance > agreed price for all months
    And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active    | 01/08/2017     |              |
        
	When an ILR file is submitted with the following data:
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type  | aim sequence number | framework code | programme type | pathway code | submission period |
        | learner a | programme only DAS | 9000         | 05/08/2017 | 20/08/2018       |                 | continuing        | programme | 1                   | 403            | 2              | 1            | R01               |
        | learner a | programme only DAS | 9000         | 15/08/2017 | 20/08/2018       |                 | continuing        | programme | 1                   | 403            | 2              | 1            | R03               |
  
	Then the provider earnings and payments break down as follows:
		| Type                                | 08/17 | 09/17 | 10/17 | 11/17 |
        | Provider Earned Total               | 600   | 600   | 600   | 0     |
        | Provider Earned from SFA            | 0     | 600   | 600   | 0     |
        | Provider Earned from Employer       | 0     | 0     | 0     | 0     |
        | Provider Paid by SFA                | 0     | 600   | 600   | 600   |
        | Payment due from Employer           | 0     | 0     | 0     | 0     |
        | Levy account debited                | 0     | 600   | 600   | 600   |
        | SFA Levy employer budget            | 600   | 600   | 600   | 0     |
        | SFA Levy co-funding budget          | 0     | 0     | 0     | 0     |
        | SFA Levy additional payments budget | 0     | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget      | 0     | 0     | 0     | 0     | 


Scenario: DPP-964_02 - DAS Learner - Change to start date within calendar month, backwards in month		
  
    Given The learner is programme only DAS
        
	And levy balance > agreed price for all months
    And the apprenticeship funding band maximum is 9000

	And the following commitments exist:
		
		| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
		| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active    | 01/08/2017     |              |
        
    And the ILR has been submitted on 30/09/2017 with the following data:
        | ULN       | start date | planned end date    |aim sequence number | aim type  | framework code | programme type | pathway code | completion status |
        | learner a | 05/8/2017  | 20/08/2018          | 1                  | programme | 403            | 2              | 1            | continuing        |
  
    And the following programme earnings and payments have been made to the provider:
		
        | Type                                | 08/17 | 09/17 | 10/17 | 11/17 |
        | Provider Earned Total               | 600   | 600   | 0     | 0     |
        | Provider Earned from SFA            | 600   | 600   | 0     | 0     |
        | Provider Earned from Employer       | 0     | 0     | 0     | 0     |
        | Provider Paid by SFA                | 0     | 600   | 600   | 0     |
        | Payment due from Employer           | 0     | 0     | 0     | 0     |
        | Levy account debited                | 0     | 600   | 600   | 0     |
        | SFA Levy employer budget            | 600   | 600   | 0     | 0     |
        | SFA Levy co-funding budget          | 0     | 0     | 0     | 0     |
        | SFA Levy additional payments budget | 0     | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget      | 0     | 0     | 0     | 0     | 
			
	When an ILR is submitted on 30/10/2017 with the following data:	
        
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | framework code | programme type | pathway code |
        | learner a | programme only DAS | 9000         | 04/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   | 403            | 2              | 1            |     
		
	Then the earnings & payments break down as follows:
		
        | Type                                | 08/17 | 09/17 | 10/17 | 11/17 |
        | Provider Earned Total               | 600   | 600   | 600   | 0     |
        | Provider Earned from SFA            | 0     | 600   | 600   | 0     |
        | Provider Earned from Employer       | 0     | 0     | 0     | 0     |
        | Provider Paid by SFA                | 0     | 600   | 600   | 600   |
        | Payment due from Employer           | 0     | 0     | 0     | 0     |
        | Levy account debited                | 0     | 600   | 600   | 600   |
        | SFA Levy employer budget            | 600   | 600   | 600   | 0     |
        | SFA Levy co-funding budget          | 0     | 0     | 0     | 0     |
        | SFA Levy additional payments budget | 0     | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget      | 0     | 0     | 0     | 0     |  


Scenario: DPP-964_03 - non-DAS Learner - Change to start date within calendar month, forward in month

	Given The learner is programme only non-DAS
       
	And levy balance > agreed price for all months
    And the apprenticeship funding band maximum is 9000
	        
    And the ILR has been submitted on 30/09/2017 with the following data:
		
        | ULN       | start date  | planned end date    |aim sequence number | aim type  | framework code | programme type | pathway code | completion status |
        | learner a | 05/08/2017  | 20/08/2018          | 1                  | programme | 403            | 2              | 1            | continuing        |
  
    And the following programme earnings and payments have been made to the provider:
		
        | Type                                | 08/17 | 09/17 | 10/17 | 11/17 |
        | Provider Earned Total               | 600   | 600   | 0     | 0     |
        | Provider Earned from SFA            | 540   | 540   | 0     | 0     |
        | Provider Earned from Employer       | 60    | 60    | 0     | 0     |
        | Provider Paid by SFA                | 0     | 540   | 540   | 0     |
        | Payment due from Employer           | 0     | 60    | 60    | 0     |
        | Levy account debited                | 0     | 0     | 0     | 0     |
        | SFA Levy employer budget            | 0     | 0     | 0     | 0     |
        | SFA Levy co-funding budget          | 0     | 0     | 0     | 0     |
        | SFA Levy additional payments budget | 0     | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget      | 540   | 540   | 0     | 0     | 
		
	When an ILR is submitted on 30/10/2017 with the following data:
        
        | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | framework code | programme type | pathway code |
        | learner a | programme only non-DAS | 9000         | 15/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   | 403            | 2              | 1            |     
		
	Then the earnings & payments break down as follows:
		
        | Type                                | 08/17 | 09/17 | 10/17 | 11/17 |
        | Provider Earned Total               | 600   | 600   | 600   | 0     |
        | Provider Earned from SFA            | 540   | 540   | 540   | 0     |
        | Provider Earned from Employer       | 60    | 60    | 60    | 0     |
        | Provider Paid by SFA                | 0     | 540   | 540   | 540   |
        | Payment due from Employer           | 0     | 60    | 60    | 60    |
        | Levy account debited                | 0     |       | 0     | 0     |
        | SFA Levy employer budget            | 0     | 0     | 0     | 0     |
        | SFA Levy co-funding budget          | 0     | 0     | 0     | 0     |
        | SFA Levy additional payments budget | 0     | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget      | 540   | 540   | 540   | 0     |


Scenario: DPP-964_04 non-DAS Learner - Change to start date within calendar month, backwards in month
    
  Given The learner is programme only non-DAS
       
	And levy balance > agreed price for all months
    And the apprenticeship funding band maximum is 9000
			       
    And the ILR has been submitted on 30/09/2017 with the following data:
		
        | ULN       | start date  | planned end date    |aim sequence number | aim type  | framework code | programme type | pathway code | completion status |
        | learner a | 05/08/2017  | 20/08/2018          | 1                  | programme | 403            | 2              | 1            | continuing        |
  
    And the following programme earnings and payments have been made to the provider:
		
		| Type                                | 08/17 | 09/17 | 10/17 | 11/17 |
        | Provider Earned Total               | 600   | 600   | 0     | 0     |
        | Provider Earned from SFA            | 540   | 540   | 0     | 0     |
        | Provider Earned from Employer       | 60    | 60    | 0     | 0     |
        | Provider Paid by SFA                | 0     | 0     | 540   | 0     |
        | Payment due from Employer           | 0     | 0     | 0     | 0     |
        | Levy account debited                | 0     | 0     | 0     | 0     |
        | SFA Levy employer budget            | 0     | 0     | 0     | 0     |
        | SFA Levy co-funding budget          | 0     | 0     | 0     | 0     |
        | SFA Levy additional payments budget | 0     | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget      | 540   | 540   | 0     | 0     | 
		
            
	When an ILR is submitted on 30/10/2017 with the following data:
        
        | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | framework code | programme type | pathway code |
        | learner a | programme only DAS | 9000         | 04/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   | 403            | 2              | 1            |     
		
	Then the earnings & payments break down as follows:
		 
        | Type                                | 08/17 | 09/17 | 10/17 | 11/17 |
        | Provider Earned Total               | 600   | 600   | 600   | 0     |
        | Provider Earned from SFA            | 540   | 540   | 540   | 0     |
        | Provider Earned from Employer       | 60    | 60    | 60    | 0     |
        | Provider Paid by SFA                | 0     | 540   | 540   | 540   |
        | Payment due from Employer           | 0     | 0     | 0     | 0     |
        | Levy account debited                | 0     |       | 0     | 0     |
        | SFA Levy employer budget            | 0     | 0     | 0     | 0     |
        | SFA Levy co-funding budget          | 0     | 0     | 0     | 0     |
        | SFA Levy additional payments budget | 0     | 0     | 0     | 0     |
        | SFA non-Levy co-funding budget      | 540   |540    |540    | 0     |  