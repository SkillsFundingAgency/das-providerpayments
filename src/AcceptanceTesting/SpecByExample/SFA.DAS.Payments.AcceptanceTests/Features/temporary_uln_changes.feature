@TemporaryULNChanges
Feature: Where a temporary ULN changes to a proper one, payments should align

Scenario:902-AC01 - Non-Levy apprentice, provider changes ULN value in ILR after payments have already occurred

        Given the apprenticeship funding band maximum is 9000
 	#	And following learning has been recorded for previous payments:
		#	| learner reference number | Employer   | ULN       | learner type           | start date | aim sequence number | aim type         | framework code | programme type | pathway code | completion status |
		#	| 123                      | employer 0 | 999999999 | programme only non-DAS | 06/08/2017 | 1                   | programme        | 403            | 2              | 1            | continuing        |
		#	| 123                      | employer 0 | 999999999 | programme only non-DAS | 06/08/2017 | 2                   | maths or english | 403            | 2              | 1            | continuing        |
  #
  
		When an ILR file is submitted for period R01 with the following data:
		| learner reference number | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
		| 123                      | 999999999 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
		| 123                      | 999999999 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |  

		And an ILR file is submitted for period R03 with the following data:
		| learner reference number | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
		| 123                      | 100000000 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
		| 123                      | 100000000 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |  

#		And the following earnings and payments have been made to the provider A for 123:
#
#            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
#			| Provider Earned Total                   | 639.25 | 639.25 | 0      | 0      |
#			| Provider Earned from SFA                | 579.25 | 579.25 | 0      | 0      |
#			| Provider Earned from Employer           | 60     | 60     | 0      | 0      |
#			| Provider Paid by SFA                    | 0      | 540    | 540    | 0      |
#			| Refund taken by SFA                     | 0      | 0      | 0      | 0      |
#			| Payment due from Employer               | 0      | 60     | 60     | 0      |
#			| Refund due to employer                  | 0      | 0      | 0      | 0      |
#			| Levy account debited                    | 0      | 0      | 0      | 0      |
#			| Levy account credited                   | 0      | 0      | 0      | 0      |
#			| SFA Levy employer budget                | 0      | 0      | 0      | 0      |
#			| SFA Levy co-funding budget              | 0      | 0      | 0      | 0      |
#			| SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |
#			| SFA non-Levy co-funding budget          | 540    | 540    | 0      | 0      |
#			| SFA non-Levy additional payments budget | 39.25  | 39.25  | 0      | 0      | 			
#        
#        When an ILR file is submitted for the first time on 31/10/17 with the following data:
#			| learner reference number | Employer   | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
#			| 123                      | employer 0 | 100000000 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   |          | 403            | 2              | 1            |
#			| 123                      | employer 0 | 100000000 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 2                   | 471      | 403            | 2              | 1            |
#  	
        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
			| Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25 |
			| Provider Earned from SFA                | 579.25 | 579.25 | 579.25 | 579.25 |
			| Provider Earned from Employer           | 60     | 60     | 60     | 60     |
			| Provider Paid by SFA                    | 0      | 540    | 540    | 657.75 |
			| Refund taken by SFA                     | 0      | 0      | 0      | 0      |
			| Payment due from Employer               | 0      | 60     | 60     | 60     |
			| Refund due to employer                  | 0      | 0      | 0      | 0      |
			| Levy account debited                    | 0      | 0      | 0      | 0      |
			| Levy account credited                   | 0      | 0      | 0      | 0      |
			| SFA Levy employer budget                | 0      | 0      | 0      | 0      |
			| SFA Levy co-funding budget              | 0      | 0      | 0      | 0      |
			| SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |
			| SFA non-Levy co-funding budget          | 540    | 540    | 540    | 540    |
			| SFA non-Levy additional payments budget | 39.25  | 39.25  | 39.25  | 39.25  | 
			
@_Minimum_Acceptance_		
Scenario:902-AC02 - Non-Levy apprentice, provider changes learner reference number in ILR after payments have already occurred

        Given levy balance > agreed price for all months
		And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R01 with the following data:
		| learner reference number | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
		| 123                      | 999999999 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
		| 123                      | 999999999 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |  

		And an ILR file is submitted for period R03 with the following data:
		| learner reference number | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
		| 456                      | 999999999 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
		| 456                      | 999999999 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |  

#
# 		And following learning has been recorded for previous payments:
#			| learner reference number | ULN       | learner type          | start date | aim sequence number | aim type          | framework code | programme type | pathway code | completion status |
#			| 123                      | 999999999 | programme only non-DAS| 06/08/2017 | 1                   | programme         | 403            | 2              | 1            | continuing        |
#			| 123                      | 999999999 | programme only non-DAS| 06/08/2017 | 2                   | maths or english  | 403            | 2              | 1            | continuing        |
#  
#		And the following earnings and payments have been made to the provider A for 123:
#
#            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
#			| Provider Earned Total                   | 639.25 | 639.25 | 0      | 0      |
#			| Provider Earned from SFA                | 579.25 | 579.25 | 0      | 0      |
#			| Provider Earned from Employer           | 60     | 60     | 0      | 0      |
#			| Provider Paid by SFA                    | 0      | 579.25 | 579.25 | 0      |
#			| Refund taken by SFA                     | 0      | 0      | 0      | 0      |
#			| Payment due from Employer               | 0      | 60     | 60     | 0      |
#			| Refund due to employer                  | 0      | 0      | 0      | 0      |
#			| Levy account debited                    | 0      | 0      | 0      | 0      |
#			| Levy account credited                   | 0      | 0      | 0      | 0      |
#			| SFA Levy employer budget                | 0      | 0      | 0      | 0      |
#			| SFA Levy co-funding budget              | 0      | 0      | 0      | 0      |
#			| SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |
#			| SFA non-Levy co-funding budget          | 540    | 540    | 0      | 0      |
#			| SFA non-Levy additional payments budget | 39.25  | 39.25  | 0      | 0      | 			
#        
#        When an ILR file is submitted for the first time on 31/10/17 with the following data:
#			| learner reference number | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
#			| 456                      | 999999999 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   |          | 403            | 2              | 1            |
#			| 456                      | 999999999 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 2                   | 471      | 403            | 2              | 1            |
#  	
        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
			| Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25 |
			| Provider Earned from SFA                | 579.25 | 579.25 | 579.25 | 0      |
		#	| Provider Earned from Employer           | 60     | 60     | 60     | 0      |
			| Provider Paid by SFA                    | 0      | 540    | 540    | 1737.75|
			| Refund taken by SFA                     | 0      | 0      | 0      | 1158.50|
			| Payment due from Employer               | 0      | 60     | 60     | 180    |
			| Refund due to employer                  | 0      | 0      | 0      | 120    |
			| Levy account debited                    | 0      | 0      | 0      | 0      |
			| Levy account credited                   | 0      | 0      | 0      | 0      |
			| SFA Levy employer budget                | 0      | 0      | 0      | 0      |
			| SFA Levy co-funding budget              | 0      | 0      | 0      | 0      |
			| SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |
			| SFA non-Levy co-funding budget          | 1080   | 1080   | 540    | 540    |
			| SFA non-Levy additional payments budget | 39.25  | 39.25  | 39.25  | 39.25  | 

Scenario:902-AC03 - Non-Levy apprentice, apprentice removed from ILR after payments have been paid to provider

        Given the apprenticeship funding band maximum is 9000
 		And following learning has been recorded for previous payments:
			| learner reference number | ULN       | learner type          | start date | aim sequence number | aim type          | framework code | programme type | pathway code | completion status |
			| 123                      | 123	   | programme only non-DAS| 06/08/2017 | 1                   | programme         | 403            | 2              | 1            | continuing        |
			| 123                      | 123	   | programme only non-DAS| 06/08/2017 | 2                   | maths or english  | 403            | 2              | 1            | continuing        |
			| 456                      | 456	   | programme only non-DAS| 06/08/2017 | 1                   | programme         | 403            | 2              | 1            | continuing        |
			| 456                      | 456	   | programme only non-DAS| 06/08/2017 | 2                   | maths or english  | 403            | 2              | 1            | continuing        |
  
		And the following earnings and payments have been made to the provider A for 123:

            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
			| Provider Earned Total                   | 639.25 | 639.25 | 0      | 0      |
			| Provider Earned from SFA                | 579.25 | 579.25 | 0      | 0      |
			| Provider Earned from Employer           | 60     | 60     | 0      | 0      |
			| Provider Paid by SFA                    | 0      | 540    | 540    | 0      |
			| Refund taken by SFA                     | 0      | 0      | 0      | 0      |
			| Payment due from Employer               | 0      | 60     | 60     | 0      |
			| Refund due to employer                  | 0      | 0      | 0      | 0      |
			| Levy account debited                    | 0      | 0      | 0      | 0      |
			| Levy account credited                   | 0      | 0      | 0      | 0      |
			| SFA Levy employer budget                | 0      | 0      | 0      | 0      |
			| SFA Levy co-funding budget              | 0      | 0      | 0      | 0      |
			| SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |
			| SFA non-Levy co-funding budget          | 540    | 540    | 0      | 0      |
			| SFA non-Levy additional payments budget | 39.25  | 39.25  | 0      | 0      | 	
			
		And the following earnings and payments have been made to the provider A for 456:

            | Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
			| Provider Earned Total                   | 639.25 | 639.25 | 0      | 0      |
			| Provider Earned from SFA                | 579.25 | 579.25 | 0      | 0      |
			| Provider Earned from Employer           | 60     | 60     | 0      | 0      |
			| Provider Paid by SFA                    | 0      | 540    | 540    | 0      |
			| Refund taken by SFA                     | 0      | 0      | 0      | 0      |
			| Payment due from Employer               | 0      | 60     | 60     | 0      |
			| Refund due to employer                  | 0      | 0      | 0      | 0      |
			| Levy account debited                    | 0      | 0      | 0      | 0      |
			| Levy account credited                   | 0      | 0      | 0      | 0      |
			| SFA Levy employer budget                | 0      | 0      | 0      | 0      |
			| SFA Levy co-funding budget              | 0      | 0      | 0      | 0      |
			| SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |
			| SFA non-Levy co-funding budget          | 540    | 540    | 0      | 0      |
			| SFA non-Levy additional payments budget | 39.25  | 39.25  | 0      | 0      | 	
        
        When an ILR file is submitted for the first time on 31/10/17 with the following data:
			| learner reference number | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
			| 123                      | 123	   | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 1                   |          | 403            | 2              | 1            |
			| 123                      | 123	   | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 2                   | 471      | 403            | 2              | 1            |
  	
        Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
			| Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25 |
			| Provider Earned from SFA                | 579.25 | 579.25 | 579.25 | 579.25 |
			| Provider Earned from Employer           | 60     | 120    | 120    | 120    |
			| Provider Paid by SFA                    | 0      | 1080   | 1080   | 657.75 |
			| Refund taken by SFA                     | 0      | 0      | 0      | 1080   |
			| Payment due from Employer               | 0      | 120    | 120    | 60     |
			| Refund due to employer                  | 0      | 0      | 0      | 120    |
			| Levy account debited                    | 0      | 0      | 0      | 0      |
			| Levy account credited                   | 0      | 0      | 0      | 0      |
			| SFA Levy employer budget                | 0      | 0      | 0      | 0      |
			| SFA Levy co-funding budget              | 0      | 0      | 0      | 0      |
			| SFA Levy additional payments budget     | 0      | 0      | 0      | 0      |
			| SFA non-Levy co-funding budget          | 469.35 | 469.35 | 540	 | 540	  |
			| SFA non-Levy additional payments budget | 39.25  | 39.25  | 39.25  | 39.25  | 