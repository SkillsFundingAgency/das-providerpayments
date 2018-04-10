@edit_to_small_employer_funding_details
Feature: Edit to small employer funding details

Scenario: DPP-966_01 - 16-18 Non-Levy apprentice, provider retrospectively adds small employer flag in the ILR, previous on-programme payments are refunded and repaid according to latest small employer status

        Given the apprenticeship funding band maximum is 9000

        When an ILR file is submitted for period R01 with the following data:
            | learner reference number | ULN       | learner type                 | agreed price | start date | planned end date | actual end date | aim sequence number | aim type  | completion status | standard code | Employer Employment Status | Employer Employment Status Applies | Employer   | Employer Id |
            | learner a                | learner a | 16-18 programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | 1                   | programme | continuing        | 50            | in paid employment         | 05/08/2017                         | employer 1 | 12345678    |
        
			       
        And an ILR file is submitted for period R03 with the following data:
            | learner reference number | ULN       | learner type                 | agreed price | start date | planned end date | actual end date | aim sequence number | aim type  | completion status | standard code | Employer Employment Status | Employer Employment Status Applies | Employer   | Employer Id | Employer Small Employer |
            | learner a                | learner a | 16-18 programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | 1                   | programme | continuing        | 50            | in paid employment         | 05/08/2017                         | employer 1 | 12345678    | SEM1                    |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17 | 09/17 | 10/17 | 11/17 |
            | Provider Earned Total                   | 600   | 600   | 600   | 1600  |
            | Provider Earned from SFA                | 600   | 600   | 600   | 1600  |
            | Provider Earned from Employer           | 0     | 0     | 0     | 0     |
            | Provider Paid by SFA                    | 0     | 540   | 540   | 1800  |
            | Refund taken by SFA                     | 0     | 0     | 0     | -1080 |
            | Payment due from Employer               | 0     | 60    | 60    | 0     |
            | Refund due to employer                  | 0     | 0     | 0     | 120   |
            | Levy account debited                    | 0     | 0     | 0     | 0     |
            | Levy account credited                   | 0     | 0     | 0     | 0     |
            | SFA Levy employer budget                | 0     | 0     | 0     | 0     |
            | SFA Levy co-funding budget              | 0     | 0     | 0     | 0     |
            | SFA Levy additional payments budget     | 0     | 0     | 0     | 0     |
            | SFA non-Levy co-funding budget          | 600   | 600   | 600   | 600   |
            | SFA non-Levy additional payments budget | 0     | 0     | 0     | 1000  |
			

Scenario: DPP-966_02 - 16-18 Non-Levy apprentice, provider retrospectively removes small employer flag in the ILR, previous on-programme payments are refunded and repaid according to latest small employer status

        Given levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R01 with the following data:
			| learner reference number | ULN       | learner type                 | agreed price | start date | planned end date | actual end date | aim sequence number | aim type  | completion status | standard code | Employer Employment Status | Employer Employment Status Applies | Employer   | Employer Id | Employer Small Employer |
			| learner a                | learner a | 16-18 programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | 1                   | programme | continuing        | 50            | in paid employment         | 05/08/2017                         | employer 1 | 12345678    | SEM1                    |
        
			       
        And an ILR file is submitted for period R03 with the following data:
            | learner reference number | ULN       | learner type                 | agreed price | start date | planned end date | actual end date | aim sequence number | aim type  | completion status | standard code | Employer Employment Status | Employer Employment Status Applies | Employer   | Employer Id |
            | learner a                | learner a | 16-18 programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | 1                   | programme | continuing        | 50            | in paid employment         | 05/08/2017                         | employer 1 | 12345678    |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17 | 09/17 | 10/17 | 11/17 |
            | Provider Earned Total                   | 600   | 600   | 600   | 1600  |
            | Provider Earned from SFA                | 540   | 540   | 540   | 0     |
            | Provider Earned from Employer           | 60    | 60    | 60    | 0     |
            | Provider Paid by SFA                    | 0     | 600   | 600   | 1620  |
            | Refund taken by SFA                     | 0     | 0     | 0     | -1200 |
            | Payment due from Employer               | 0     | 0     | 0     | 180   |
            | Refund due to employer                  | 0     | 0     | 0     | 0     |
            | Levy account debited                    | 0     | 0     | 0     | 0     |
            | Levy account credited                   | 0     | 0     | 0     | 0     |
            | SFA Levy employer budget                | 0     | 0     | 0     | 0     |
            | SFA Levy co-funding budget              | 0     | 0     | 0     | 0     |
            | SFA Levy additional payments budget     | 0     | 0     | 0     | 0     |
            | SFA non-Levy co-funding budget          | 540   | 540   | 540   | 540   |
            | SFA non-Levy additional payments budget | 0     | 0     | 0     | 1000  |


Scenario: DPP-966_03 - 19-24 year old Non-Levy apprentice, small employer flag added, provider retrospectively adds Education Health Care (EHC) plan flag in the ILR, previous on-programme payments are refunded and repaid according to latest EHC plan status

        Given levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000


		When an ILR file is submitted for period R01 with the following data:
			| learner reference number | ULN       | learner type                 | agreed price | start date | planned end date | actual end date | aim sequence number | aim type  | completion status | standard code | Employer Employment Status | Employer Employment Status Applies | Employer   | Employer Id | Employer Small Employer |
			| learner a                | learner a | 19-24 programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | 1                   | programme | continuing        | 50            | in paid employment         | 05/08/2017                         | employer 1 | 12345678    | SEM1                    |
        
			       
        And an ILR file is submitted for period R03 with the following data:
            | learner reference number | ULN       | learner type                 | agreed price | start date | planned end date | actual end date | aim sequence number | aim type  | completion status | standard code | Employer Employment Status | Employer Employment Status Applies | Employer   | Employer Id | Employer Small Employer | LearnDelFam |
            | learner a                | learner a | 19-24 programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | 1                   | programme | continuing        | 50            | in paid employment         | 05/08/2017                         | employer 1 | 12345678    | SEM1                    | EEF2        |

#        When an ILR file is submitted for period R02 with the following data:
#            | ULN       | learner type                 | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
#            | learner a | 19-24 programme only non-DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | No value    |
#
#		       
#        And an ILR file is submitted for period R03 with the following data:
#            | ULN       | learner type                 | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
#            | learner a | 19-24 programme only non-DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | EEF2        |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 1600      | 
            | Provider Earned from SFA                | 600    | 600    | 600    | 0      | 
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 
            | Provider Paid by SFA                    | 0      | 540    | 540    | 1800   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1080  | 
            | Payment due from Employer               | 0      | 60     | 60     | 0      | 
            | Refund due to employer                  | 0      | 0      | 0      | 120    | 
            | Levy account debited                    | 0      | 0      | 0      | 0      | 
            | Levy account credited                   | 0      | 0      | 0      | 0      | 
            | SFA Levy employer budget                | 0      | 0      | 0      | 0      | 
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 
            | SFA non-Levy co-funding budget          | 600    | 600    | 600    | 600      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 1000      |  
			
			
Scenario: DPP-966_04 - 19-24 year old Non-Levy apprentice, small employer flag added, provider retrospectively removes Education Health Care (EHC) plan flag in the ILR, previous on-programme payments are refunded and repaid according to latest EHC plan status

        Given levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

       When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type                 | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only non-DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | EEF2        |
     
		       
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type                 | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only non-DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | No value    |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 540    | 540    | 540    | 0      | 
            | Provider Earned from Employer           | 60     | 60     | 60     | 0      | 
            | Provider Paid by SFA                    | 0      | 600    | 600    | 1620   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1200  | 
            | Payment due from Employer               | 0      | 0      | 0      | 180    | 
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 
            | Levy account debited                    | 0      | 0      | 0      | 0      | 
            | Levy account credited                   | 0      | 0      | 0      | 0      | 
            | SFA Levy employer budget                | 0      | 0      | 0      | 0      | 
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 
            | SFA non-Levy co-funding budget          | 540    | 540    | 540    | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      | 
			
			
Scenario: DPP-966_05 - 19-24 year old Non-Levy apprentice, small employer flag added, provider retrospectively adds care leaver flag in the ILR, previous on-programme payments are refunded and repaid according to latest care leaver status

        Given levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type                 | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only non-DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | No value    |
        
		       
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type                 | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only non-DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | EEF4        |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 600    | 600    | 600    | 0      | 
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 
            | Provider Paid by SFA                    | 0      | 540    | 540    | 1800   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1080  | 
            | Payment due from Employer               | 0      | 60     | 60     | 0      | 
            | Refund due to employer                  | 0      | 0      | 0      | 120    | 
            | Levy account debited                    | 0      | 0      | 0      | 0      | 
            | Levy account credited                   | 0      | 0      | 0      | 0      | 
            | SFA Levy employer budget                | 0      | 0      | 0      | 0      | 
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 
            | SFA non-Levy co-funding budget          | 600    | 600    | 600    | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      |  


Scenario: DPP-966_06 - 19-24 year old Non-Levy apprentice, small employer flag added, provider retrospectively removes care leaver flag in the ILR, previous on-programme payments are refunded and repaid according to latest care leaver status

        Given levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

       When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type                 | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only non-DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | EEF4        |
        
			       
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type                 | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only non-DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    | SEM1           | No value    |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 540    | 540    | 540    | 0      | 
            | Provider Earned from Employer           | 60     | 60     | 60     | 0      | 
            | Provider Paid by SFA                    | 0      | 600    | 600    | 1620   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1200  | 
            | Payment due from Employer               | 0      | 60     | 60     | 60     | 
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 
            | Levy account debited                    | 0      | 0      | 0      | 0      | 
            | Levy account credited                   | 0      | 0      | 0      | 0      | 
            | SFA Levy employer budget                | 0      | 0      | 0      | 0      | 
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 
            | SFA non-Levy co-funding budget          | 540    | 540    | 540    | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      | 
			

Scenario: DPP-966_07 - 16-18 Levy apprentice, provider retrospectively adds small employer flag in the ILR, previous on-programme payments are refunded and repaid according to latest small employer status

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer |
            | learner a | 16-18 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	No value       |
        
	
		And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer |
            | learner a | 16-18 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 600    | 600    | 600    | 0      | 
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 
            | Provider Paid by SFA                    | 0      | 600    | 600    | 1800   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1200  | 
            | Payment due from Employer               | 0      | 0      | 0      | 0      | 
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 
            | Levy account debited                    | 0      | 600    | 600    | 0      | 
            | Levy account credited                   | 0      | 0      | 0      | 1200   | 
            | SFA Levy employer budget                | 0      | 0      | 0      | 0      | 
            | SFA Levy co-funding budget              | 600    | 600    | 600    | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      |  
			

Scenario: DPP-966_08 - 16-18 Levy apprentice, provider retrospectively removes small employer flag in the ILR, previous on-programme payments are refunded and repaid according to latest small employer status

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer |
            | learner a | 16-18 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           |
        
		And the following programme earnings and payments have been made to the provider A for learner a:
            | Type                                | 08/17 | 09/17 | 10/17 | 11/17 |
            | Provider Earned Total               | 600   | 600   | 0     | 0     |
            | Provider Earned from SFA            | 600   | 600   | 0     | 0     |
            | Provider Earned from Employer       | 0     | 0     | 0     | 0     |
            | Provider Paid by SFA                | 0     | 600   | 600   | 0     |
            | Payment due from Employer           | 0     | 0     | 0     | 0     |
            | Levy account debited                | 0     | 0     | 0     | 0     |
            | SFA Levy employer budget            | 0     | 0     | 0     | 0     |
            | SFA Levy co-funding budget          | 600   | 600   | 0     | 0     |
            | SFA Levy additional payments budget | 0     | 0     | 0     | 0     |
            | SFA non-Levy co-funding budget      | 0     | 0     | 0     | 0     | 

		       
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer |
            | learner a | 16-18 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	No value       |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 600    | 600    | 600    | 0      | 
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 
            | Provider Paid by SFA                    | 0      | 600    | 600    | 1800   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1200  | 
            | Payment due from Employer               | 0      | 0      | 0      | 0      |  
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 
            | Levy account debited                    | 0      | 0      | 0      | 1800   | 
            | Levy account credited                   | 0      | 0      | 0      | 0      | 
            | SFA Levy employer budget                | 600    | 600    | 600    | 0      | 
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |  
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      | 


Scenario: DPP-966_09 - 19-24 year old Levy apprentice, small employer flag added, provider retrospectively adds Education Health Care (EHC) plan flag in the ILR, previous on-programme payments are refunded and repaid according to latest EHC plan status

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

        When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | No value    |
		       
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | EEF2        |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 600    | 600    | 600    | 0      | 
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 
            | Provider Paid by SFA                    | 0      | 600    | 600    | 1800   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1200  | 
            | Payment due from Employer               | 0      | 0      | 0      | 0      | 
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 
            | Levy account debited                    | 0      | 600    | 600    | 0      | 
            | Levy account credited                   | 0      | 0      | 0      | 1200   | 
            | SFA Levy employer budget                | 0      | 0      | 0      | 0      | 
            | SFA Levy co-funding budget              | 600    | 600    | 600    | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      |  

		
Scenario: DPP-966_10 - 19-24 year old Levy apprentice, small employer flag added, provider retrospectively removes Education Health Care (EHC) plan flag in the ILR, previous on-programme payments are refunded and repaid according to latest EHC plan status

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | EEF2        |
        
			       
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | No value    |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 600    | 600    | 600    | 0      | 
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 
            | Provider Paid by SFA                    | 0      | 600    | 600    | 1800   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1200  | 
            | Payment due from Employer               | 0      | 0      | 0      | 0      |  
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 
            | Levy account debited                    | 0      | 0      | 0      | 1800   | 
            | Levy account credited                   | 0      | 0      | 0      | 0      | 
            | SFA Levy employer budget                | 600    | 600    | 600    | 0      | 
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |  
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      | 

		
Scenario: DPP-966_11 - 19-24 year old Levy apprentice, small employer flag added, provider retrospectively adds care leaver flag in the ILR, previous on-programme payments are refunded and repaid according to latest care leaver status

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | No value    |
        
			       
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | EEF4        |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 600    | 600    | 600    | 0      | 
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 
            | Provider Paid by SFA                    | 0      | 600    | 600    | 1800   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1200  | 
            | Payment due from Employer               | 0      | 0      | 0      | 0      | 
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 
            | Levy account debited                    | 0      | 600    | 600    | 0      | 
            | Levy account credited                   | 0      | 0      | 0      | 1200   | 
            | SFA Levy employer budget                | 0      | 0      | 0      | 0      | 
            | SFA Levy co-funding budget              | 600    | 600    | 600    | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      |  
			

Scenario: DPP-966_12 - 19-24 year old Levy apprentice, small employer flag added, provider retrospectively removes care leaver flag in the ILR, previous on-programme payments are refunded and repaid according to latest care leaver status

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R02 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | EEF4        |
        	
		       
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type             | start date | aim sequence number | aim type  | completion status | framework code | programme type | pathway code | Employment Status	| Employment Status Applies | Employer Id | Small Employer | LearnDelFam |
            | learner a | 19-24 programme only DAS | 06/08/2017 | 1                   | programme | continuing        | 403            | 2              | 1            | in paid employment	| 05/08/2017	            | 12345678    |	SEM1           | No value    |
        
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 
            | Provider Earned Total                   | 600    | 600    | 600    | 0      | 
            | Provider Earned from SFA                | 600    | 600    | 600    | 0      | 
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 
            | Provider Paid by SFA                    | 0      | 600    | 600    | 1800   | 
            | Refund taken by SFA                     | 0      | 0      | 0      | -1200  | 
            | Payment due from Employer               | 0      | 0      | 0      | 0      |  
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 
            | Levy account debited                    | 0      | 0      | 0      | 1800   | 
            | Levy account credited                   | 0      | 0      | 0      | 0      | 
            | SFA Levy employer budget                | 600    | 600    | 600    | 0      | 
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |  
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0      | 
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      | 
