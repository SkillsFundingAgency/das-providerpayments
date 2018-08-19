@ChangeInCircumstances
Feature: chanegs in aim sequence or ULN

@AimSequenceOrUlnChange
Scenario:822-AC01- Levy apprentice, provider changes aim sequence numbers in ILR after payments have already occurred

        Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

        And the following commitments exist:
			| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
			| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active    | 01/08/2017     |              |
        
		When an ILR file is submitted for period R01 with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   |          | 403            | 2              | 1            |
        
        And an ILR file is submitted for period R03 with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
			| learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |
  
        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17 | 09/17 | 10/17  | 11/17  |
			| Provider Earned Total                   | 600   | 600   | 639.25 | 639.25 |
			| Provider Earned from SFA                | 600   | 600   | 639.25 | 0      |
			| Provider Earned from Employer           | 0     | 0     | 0      | 0      |
			| Provider Paid by SFA                    | 0     | 600   | 600    | 717.75 |
			| Refund taken by SFA                     | 0     | 0     | 0      | 0      |
			| Payment due from Employer               | 0     | 0     | 0      | 0      |
			| Refund due to employer                  | 0     | 0     | 0      | 0      |
			| Levy account debited                    | 0     | 600   | 600    | 600    |
			| Levy account credited                   | 0     | 0     | 0      | 0      |
			| SFA Levy employer budget                | 600   | 600   | 600    | 600    |
			| SFA Levy co-funding budget              | 0     | 0     | 0      | 0      |
			| SFA Levy additional payments budget     | 39.25 | 39.25 | 39.25  | 39.25  |
			| SFA non-Levy co-funding budget          | 0     | 0     | 0      | 0      |
			| SFA non-Levy additional payments budget | 0     | 0     | 0      | 0      |
        

@AimSequenceOrUlnChange
 Scenario:822-AC02 Non-Levy apprentice, provider changes aim sequence numbers in ILR after payments have already occurred

        Given the apprenticeship funding band maximum is 9000

        When an ILR file is submitted for period R01 with the following data:
			| ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type  | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme | 1                   |          | 403            | 2              | 1            |
        
        And an ILR file is submitted for period R03 with the following data:
			| ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
			| learner a | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |
  	
        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17 | 09/17 | 10/17  | 11/17  |
			| Provider Earned Total                   | 600   | 600   | 639.25 | 639.25 |
			| Provider Earned from SFA                | 540   | 540   | 579.25 | 0      |
			| Provider Earned from Employer           | 60    | 60    | 60     | 0      |
			| Provider Paid by SFA                    | 0     | 540   | 540    | 657.75 |
			| Refund taken by SFA                     | 0     | 0     | 0      | 0      |
			| Payment due from Employer               | 0     | 60    | 60     | 60     |
			| Refund due to employer                  | 0     | 0     | 0      | 0      |
			| Levy account debited                    | 0     | 0     | 0      | 0      |
			| Levy account credited                   | 0     | 0     | 0      | 0      |
			| SFA Levy employer budget                | 0     | 0     | 0      | 0      |
			| SFA Levy co-funding budget              | 0     | 0     | 0      | 0      |
			| SFA Levy additional payments budget     | 0     | 0     | 0      | 0      |
			| SFA non-Levy co-funding budget          | 540   | 540   | 540    | 540    |
			| SFA non-Levy additional payments budget | 39.25 | 39.25 | 39.25  | 39.25  | 

@AimSequenceOrUlnChange
 Scenario:822-AC03  Non-Levy apprentice, provider changes ULN for an apprentice in ILR after payments have already occurred

        Given the apprenticeship funding band maximum is 9000
       
        When an ILR file is submitted for period R01 with the following data:
			| learner reference number | ULN        | learner type           | agreed price | start date | planned end date | actual end date | completion status | framework code | programme type | pathway code |
			| learner a                | 1111111111 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | 403            | 2              | 1            |
        
        And an ILR file is submitted for period R03 with the following data:
			| learner reference number | ULN        | learner type           | agreed price | start date | planned end date | actual end date | completion status | framework code | programme type | pathway code |
			| learner a                | 2222222222 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | 403            | 2              | 1            |
        
        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17 | 09/17 | 10/17 | 11/17 |
			| Provider Earned Total                   | 600   | 600   | 600   | 600   |
			| Provider Earned from SFA                | 540   | 540   | 540   | 540   |
			| Provider Earned from Employer           | 60    | 60    | 60    | 60    |
			| Provider Paid by SFA                    | 0     | 540   | 540   | 540   |
			| Refund taken by SFA                     | 0     | 0     | 0     | 0     |
			| Payment due from Employer               | 0     | 60    | 60    | 60    |
			| Refund due to employer                  | 0     | 0     | 0     | 0     |
			| Levy account debited                    | 0     | 0     | 0     | 0     |
			| Levy account credited                   | 0     | 0     | 0     | 0     |
			| SFA Levy employer budget                | 0     | 0     | 0     | 0     |
			| SFA Levy co-funding budget              | 0     | 0     | 0     | 0     |
			| SFA Levy additional payments budget     | 0     | 0     | 0     | 0     |
			| SFA non-Levy co-funding budget          | 540   | 540   | 540   | 540   |
			| SFA non-Levy additional payments budget | 0     | 0     | 0     | 0     | 
        
@CourseOrAimrefChanges
# Failing because datalock is not picking up correct commitments to use in the match handlers. 
# always has first ver of commitment even if there is a subsequent ver that it should be reading.
Scenario:852-AC01- Levy apprentice, provider changes course details in ILR after payments have already occurred

        Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

         And the following commitments exist:
			| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to | withdrawn on |
			| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 401            | 2              | 1            | 9000         | Cancelled | 01/08/2017     | 30/09/2017   | 01/10/2017   |
			| 2             | 1          | learner a | 01/08/2017 | 01/08/2018 | 404            | 2              | 1            | 9000         | Active    | 01/10/2017     |              |              |
        
        When an ILR file is submitted for period R01 with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   |          | 401            | 2              | 1            |
        
        And an ILR file is submitted for period R03 with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 404            | 2              | 1            |
			| learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 404            | 2              | 1            |
  
        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17 | 09/17 | 10/17  | 11/17   | 12/17  | 01/18  |
			| Provider Earned Total                   | 600   | 600   | 639.25 | 639.25  | 639.25 | 639.25 |
			| Provider Earned from SFA                | 600   | 600   | 639.25 | 639.25  | 639.25 | 639.25 |
			| Provider Earned from Employer           | 0     | 0     | 0      | 0       | 0      | 0      |
			| Provider Paid by SFA                    | 0     | 600   | 600    | 1917.75 | 639.25 | 639.25 |
			| Refund taken by SFA                     | 0     | 0     | 0      | -1200   | 0      | 0      |
			| Payment due from Employer               | 0     | 0     | 0      | 0       | 0      | 0      |
			| Refund due to employer                  | 0     | 0     | 0      | 0       | 0      | 0      |
			| Levy account debited                    | 0     | 600   | 600    | 1800    | 600    | 600    |
			| Levy account credited                   | 0     | 0     | 0      | 1200    | 0      | 0      |
			| SFA Levy employer budget                | 600   | 600   | 600    | 600     | 600    | 600    |
			| SFA Levy co-funding budget              | 0     | 0     | 0      | 0       | 0      | 0      |
			| SFA Levy additional payments budget     | 39.25 | 39.25 | 39.25  | 39.25   | 39.25  | 39.25  |
			| SFA non-Levy co-funding budget          | 0     | 0     | 0      | 0       | 0      | 0      |
			| SFA non-Levy additional payments budget | 0     | 0     | 0      | 0       | 0      | 0      |
     

@CourseOrAimrefChanges
Scenario:852-AC02 Levy apprentice, changes aim reference for English/maths aims and payments are reconciled 

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

        And the following commitments exist:
			| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
			| 1             | 1          | learner a | 01/05/2017 | 01/05/2018 | 403            | 2              | 1            | 9000         | Active    | 01/05/2017     |              |
        
        And following learning has been recorded for previous payments:
            | ULN       | start date | aim sequence number | aim type         | aim reference | framework code | programme type | pathway code | completion status |
            | learner a | 06/05/2017 | 1                   | programme        | ZPROG001      | 403            | 2              | 1            | continuing        |
  
        And the following programme earnings and payments have been made to the provider A for learner a:
            | Type                                | 05/17 | 06/17 | 07/17 | 08/17 |
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

		And following learning has been recorded for previous payments:
            | ULN       | start date | aim sequence number | aim type         | aim reference | framework code | programme type | pathway code | completion status |
            | learner a | 06/05/2017 | 2                   | maths or English | 60001227      | 403            | 2              | 1            | continuing        |
  
        And the following maths or english earnings and payments have been made to the provider A for learner a:
            | Type                                | 05/17 | 06/17 | 07/17 | 08/17 |
            | Provider Earned Total               | 39.25 | 39.25 | 0     | 0     |
            | Provider Earned from SFA            | 39.25 | 39.25 | 0     | 0     |
            | Provider Earned from Employer       | 0     | 0     | 0     | 0     |
            | Provider Paid by SFA                | 0     | 39.25 | 39.25 | 0     |
            | Payment due from Employer           | 0     | 0     | 0     | 0     |
            | Levy account debited                | 0     | 0     | 0     | 0     |
            | SFA Levy employer budget            | 0     | 0     | 0     | 0     |
            | SFA Levy co-funding budget          | 0     | 0     | 0     | 0     |
            | SFA Levy additional payments budget | 39.25 | 39.25 | 0     | 0     |
            | SFA non-Levy co-funding budget      | 0     | 0     | 0     | 0     | 
        
        When an ILR file is submitted for the first time on 31/07/17 with the following data:
            | ULN       | learner type       | aim sequence number | aim type         | aim reference | aim rate | agreed price | start date | planned end date | actual end date | completion status | framework code | programme type | pathway code |
            | learner a | programme only DAS | 2                   | programme        | ZPROG001      |          | 9000         | 06/05/2017 | 20/05/2018       |                 | continuing        | 403            | 2              | 1            |
            | learner a | programme only DAS | 1                   | maths or English | 50086832      | 471      |              | 06/05/2017 | 20/05/2018       |                 | continuing        | 403            | 2              | 1            |
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 05/17  | 06/17  | 07/17  | 08/17   | 09/17  | 10/17  |
            | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25  | 639.25 | 639.25 |
            | Provider Earned from SFA                | 639.25 | 639.25 | 639.25 | 639.25  | 639.25 | 639.25 |
            | Provider Earned from Employer           | 0      | 0      | 0      | 0       | 0      | 0      |
            | Provider Paid by SFA                    | 0      | 639.25 | 639.25 | 717.75  | 639.25 | 639.25 |
            | Refund taken by SFA                     | 0      | 0      | 0      | -78.50  | 0      | 0      |
            | Payment due from Employer               | 0      | 0      | 0      | 0       | 0      | 0      |
            | Refund due to employer                  | 0      | 0      | 0      | 0       | 0      | 0      |
            | Levy account debited                    | 0      | 600    | 600    | 600     | 600    | 600    |
            | Levy account credited                   | 0      | 0      | 0      | 0       | 0      | 0      |
            | SFA Levy employer budget                | 600    | 600    | 600    | 600     | 600    | 600    |
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0       | 0      | 0      |
            | SFA Levy additional payments budget     | 39.25  | 39.25  | 39.25  | 39.25   | 39.25  | 39.25  |
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0       | 0      | 0      |
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0       | 0      | 0      |   

@CourseOrAimrefChanges
	Scenario:852-AC03 Levy apprentice, deleted aim reference for English/maths aims and payments are refunded for the aim 

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

        And the following commitments exist:
			| commitment Id | version Id | ULN       | employer   | provider   | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
			| 1             | 1          | learner a | employer 0 | provider A | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active    | 01/08/2017     |              |
        
        When an ILR file is submitted for period R01 with the following data:
            | ULN       | learner type       | aim sequence number | aim type         | aim reference | aim rate | agreed price | start date | planned end date | actual end date | completion status | framework code | programme type | pathway code |
            | learner a | programme only DAS | 2                   | programme        | ZPROG001      |          | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | 403            | 2              | 1            |
            | learner a | programme only DAS | 1                   | maths or English | 50086832      | 471      |              | 06/08/2017 | 20/08/2018       |                 | continuing        | 403            | 2              | 1            |
  
        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type       | aim sequence number | aim type         | aim reference | aim rate | agreed price | start date | planned end date | actual end date | completion status | framework code | programme type | pathway code |
            | learner a | programme only DAS | 1                   | programme        | ZPROG001      |          | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | 403            | 2              | 1            |
  
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  | 12/17 | 01/18 |
            | Provider Earned Total                   | 639.25 | 639.25 | 600    | 600    | 600   | 600   |
            | Provider Earned from SFA                | 639.25 | 639.25 | 600    | 600    | 600   | 600   |
            | Provider Earned from Employer           | 0      | 0      | 0      | 0      | 0     | 0     |
            | Provider Paid by SFA                    | 0      | 639.25 | 639.25 | 600    | 600   | 600   |
            | Refund taken by SFA                     | 0      | 0      | 0      | -78.50 | 0     | 0     |
            | Payment due from Employer               | 0      | 0      | 0      | 0      | 0     | 0     |
            | Refund due to employer                  | 0      | 0      | 0      | 0      | 0     | 0     |
            | Levy account debited                    | 0      | 600    | 600    | 600    | 600   | 600   |
            | Levy account credited                   | 0      | 0      | 0      | 0      | 0     | 0     |
            | SFA Levy employer budget                | 600    | 600    | 600    | 600    | 600   | 600   |
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0      | 0     | 0     |
            | SFA Levy additional payments budget     | 0      | 0      | 0      | 0      | 0     | 0     |
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0      | 0     | 0     |
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0      | 0     | 0     |


			
@CourseOrAimrefChanges
# Failing because datalock is not picking up correct commitments to use in the match handlers. 
# always has first ver of commitment even if there is a subsequent ver that it should be reading.
Scenario:852-AC04- Levy apprentice, provider changes course details from standard to framework and adds maths/english in ILR after payments have already occurred

        Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

        And the following commitments exist:
			| commitment Id | version Id | ULN       | start date | end date   | framework code | standard code | programme type | pathway code | agreed price | status    | effective from | effective to | withdrawn on |
			| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 |                | 50            | 2              | 1            | 9000         | Active    | 01/08/2017     | 30/09/2017   | 01/10/2017   |
			| 2             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            |               | 2              | 1            | 9000         | Active    | 02/10/2017     |              |              |
			
        When an ILR file is submitted for period R01 with the following data:
            | ULN       | learner type       | aim sequence number | aim type         | aim rate | agreed price | start date | planned end date | actual end date | completion status | standard code | programme type | pathway code |
            | learner a | programme only DAS | 1                   | programme        |          | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | 50            | 2              | 1            |
  

        And an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type       | aim sequence number | aim type         | aim rate | agreed price | start date | planned end date | actual end date | completion status | framework code | programme type | pathway code |
            | learner a | programme only DAS | 2                   | programme        |          | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | 403            | 2              | 1            |
            | learner a | programme only DAS | 1                   | maths or English | 471      |              | 06/08/2017 | 20/08/2018       |                 | continuing        | 403            | 2              | 1            |

        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17 | 09/17 | 10/17  | 11/17   | 12/17  | 01/18  |
			| Provider Earned Total                   | 600   | 600   | 639.25 | 639.25  | 639.25 | 639.25 |
			| Provider Earned from SFA                | 600   | 600   | 639.25 | 639.25  | 639.25 | 639.25 |
			| Provider Earned from Employer           | 0     | 0     | 0      | 0       | 0      | 0      |
			| Provider Paid by SFA                    | 0     | 600   | 600    | 1917.75 | 639.25 | 639.25 |
			| Refund taken by SFA                     | 0     | 0     | 0      | -1200   | 0      | 0      |
			| Payment due from Employer               | 0     | 0     | 0      | 0       | 0      | 0      |
			| Refund due to employer                  | 0     | 0     | 0      | 0       | 0      | 0      |
			| Levy account debited                    | 0     | 600   | 600    | 1800    | 600    | 600    |
			| Levy account credited                   | 0     | 0     | 0      | 1200    | 0      | 0      |
			| SFA Levy employer budget                | 600   | 600   | 600    | 600     | 600    | 600    |
			| SFA Levy co-funding budget              | 0     | 0     | 0      | 0       | 0      | 0      |
			| SFA Levy additional payments budget     | 39.25 | 39.25 | 39.25  | 39.25   | 39.25  | 39.25  |
			| SFA non-Levy co-funding budget          | 0     | 0     | 0      | 0       | 0      | 0      |
			| SFA non-Levy additional payments budget | 0     | 0     | 0      | 0       | 0      | 0      |


						
@StartDateMovedForward
Scenario:865-AC01- Levy apprentice, provider moves forward course start and adds maths/english in ILR after payments have already occurred

        Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		And the following commitments exist:
			| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
			| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active    | 01/08/2017     |              |
        
		When an ILR file is submitted for period R01 with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   |          | 403            | 2              | 1            |
        
        And an ILR file is submitted for period R03 with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only DAS | 9000         | 06/10/2017 | 20/10/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
			| learner a | programme only DAS |              | 06/10/2017 | 20/10/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |
  									   
        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17 | 09/17 | 10/17  | 11/17  | 12/17  | 01/18  |
			| Provider Earned Total                   | 600   | 600   | 639.25 | 639.25 | 639.25 | 639.25 |
			| Provider Earned from SFA                | 600   | 600   | 639.25 | 639.25 | 639.25 | 639.25 |
			| Provider Earned from Employer           | 0     | 0     | 0      | 0      | 0      | 0      |
			| Provider Paid by SFA                    | 0     | 600   | 600    | 639.25 | 639.25 | 639.25 |
			| Refund taken by SFA                     | 0     | 0     | 0      | -1200  | 0      | 0      |
			| Payment due from Employer               | 0     | 0     | 0      | 0      | 0      | 0      |
			| Refund due to employer                  | 0     | 0     | 0      | 0      | 0      | 0      |
			| Levy account debited                    | 0     | 600   | 600    | 600    | 600    | 600    |
			| Levy account credited                   | 0     | 0     | 0      | 1200   | 0      | 0      |
			| SFA Levy employer budget                | 0     | 0     | 600    | 600    | 600    | 600    |
			| SFA Levy co-funding budget              | 0     | 0     | 0      | 0      | 0      | 0      |
			| SFA Levy additional payments budget     | 0     | 0     | 39.25  | 39.25  | 39.25  | 39.25  |
			| SFA non-Levy co-funding budget          | 0     | 0     | 0      | 0      | 0      | 0      |
			| SFA non-Levy additional payments budget | 0     | 0     | 0      | 0      | 0      | 0      |      

@StartDateMovedForward
Scenario:865-AC02 Levy apprentice, learner moves start date forward, on prog payments and english/maths will be refunded

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		And the following commitments exist:
			| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
			| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active    | 01/08/2017     |              |
        
		When an ILR file is submitted for period R01 with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
			| learner a | programme only DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |
			
        And an ILR file is submitted for period R03 with the following data:
			| ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| learner a | programme only DAS | 9000         | 06/10/2017 | 20/10/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
			| learner a | programme only DAS |              | 06/10/2017 | 20/10/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |
  									    
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17    | 12/17  | 01/18  |
            | Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25   | 639.25 | 639.25 |
            | Provider Earned from SFA                | 639.25 | 639.25 | 639.25 | 639.25   | 639.25 | 639.25 |
            | Provider Earned from Employer           | 0      | 0      | 0      | 0        | 0      | 0      |
            | Provider Paid by SFA                    | 0      | 639.25 | 639.25 | 639.25   | 639.25 | 639.25 |
            | Refund taken by SFA                     | 0      | 0      | 0      | -1278.50 | 0      | 0      |
            | Payment due from Employer               | 0      | 0      | 0      | 0        | 0      | 0      |
            | Refund due to employer                  | 0      | 0      | 0      | 0        | 0      | 0      |
            | Levy account debited                    | 0      | 600    | 600    | 600      | 600    | 600    |
            | Levy account credited                   | 0      | 0      | 0      | 1200     | 0      | 0      |
            | SFA Levy employer budget                | 0      | 0      | 600    | 600      | 600    | 600    |
            | SFA Levy co-funding budget              | 0      | 0      | 0      | 0        | 0      | 0      |
            | SFA Levy additional payments budget     | 0      | 0      | 39.25  | 39.25    | 39.25  | 39.25  |
            | SFA non-Levy co-funding budget          | 0      | 0      | 0      | 0        | 0      | 0      |
            | SFA non-Levy additional payments budget | 0      | 0      | 0      | 0        | 0      | 0      |   



			
@LearningSupportDateMovedForward
Scenario:869-AC01 Levy apprentice, learner moves dates for learning support , on learning support payments will be refunded

		Given The learner is programme only DAS
        And levy balance > agreed price for all months
        And the apprenticeship funding band maximum is 9000

		And the following commitments exist:
			| commitment Id | version Id | ULN       | start date | end date   | framework code | programme type | pathway code | agreed price | status    | effective from | effective to |
			| 1             | 1          | learner a | 01/08/2017 | 01/08/2018 | 403            | 2              | 1            | 9000         | Active    | 01/08/2017     |              |
              
        When an ILR file is submitted for period R01 with the following data:
            | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type  | aim sequence number | aim rate | framework code | programme type | pathway code | learning support code | learning support date from | learning support date to |
            | learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme | 1                   |          | 403            | 2              | 1            | 1                     | 06/08/2017                 | 20/08/2018               |  
  
        When an ILR file is submitted for period R03 with the following data:
            | ULN       | learner type       | agreed price | start date | planned end date | actual end date | completion status | aim type  | aim sequence number | aim rate | framework code | programme type | pathway code | learning support code | learning support date from | learning support date to |
            | learner a | programme only DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme | 1                   |          | 403            | 2              | 1            | 1                     | 06/10/2017                 | 20/08/2018               |  
  
        #todo: confirm line "SFA Levy additional payments budget" is meant to be 0 for first 2 months
        Then the provider earnings and payments break down as follows:
            | Type                                    | 08/17 | 09/17 | 10/17 | 11/17 | 12/17 | 01/18 |
            | Provider Earned Total                   | 750   | 750   | 750   | 750   | 750   | 750   |
            | Provider Earned from SFA                | 750   | 750   | 750   | 750   | 750   | 750   |
            | Provider Earned from Employer           | 0     | 0     | 0     | 0     | 0     | 0     |
            | Provider Paid by SFA                    | 0     | 750   | 750   | 750   | 750   | 750   |
            | Refund taken by SFA                     | 0     | 0     | 0     | -300  | 0     | 0     |
            | Payment due from Employer               | 0     | 0     | 0     | 0     | 0     | 0     |
            | Refund due to employer                  | 0     | 0     | 0     | 0     | 0     | 0     |
            | Levy account debited                    | 0     | 600   | 600   | 600   | 600   | 600   |
            | Levy account credited                   | 0     | 0     | 0     | 0     | 0     | 0     |
            | SFA Levy employer budget                | 600   | 600   | 600   | 600   | 600   | 600   |
            | SFA Levy co-funding budget              | 0     | 0     | 0     | 0     | 0     | 0     |
            | SFA Levy additional payments budget     | 0     | 0     | 150   | 150   | 150   | 150   |
            | SFA non-Levy co-funding budget          | 0     | 0     | 0     | 0     | 0     | 0     |
            | SFA non-Levy additional payments budget | 0     | 0     | 0     | 0     | 0     | 0     | 
