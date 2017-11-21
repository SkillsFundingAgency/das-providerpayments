@EAS1718
Feature: Provider adjustments (EAS) payments

#    Scenario: Payments when current and previous provider adjustments exist
#        Given that the previous EAS entries for a provider are as follows:
#            | Type                                                                    | 05/17  | 06/17 | year to date total |
#            | 16-18 levy additional provider payments: audit adjustments              | 143.52 | 13.59 | 157.11             |
#            | 16-18 non-levy additional provider payments: training authorised claims | 17.57  | 11.89 | 29.46              |
#            | Adult levy training: authorised claims                                  | 501.02 | 98.14 | 599.16             |
#            | Adult non-levy additional employer payments: audit adjustments          | 305.25 | 5.23  | 310.48             |
#        When the following EAS entries are submitted:
#            | Type                                                                    | 05/17  | 06/17 | 07/17  | year to date total |
#            | 16-18 levy additional provider payments: audit adjustments              | 195.17 | 4.98  | 63.42  | 263.57             |
#            | 16-18 non-levy additional provider payments: training authorised claims | 17.57  | 2.89  | 2.45   | 22.91              |
#            | Adult levy training: authorised claims                                  | 475.34 | 98.14 | 65.49  | 638.97             |
#            | Adult levy additional provider payments: audit adjustments              | 0      | 18.65 | 1.63   | 20.28              |
#            | Adult non-levy additional employer payments: audit adjustments          | 341.25 | 5.23  | 159.34 | 505.82             |
#        Then the following adjustments will be generated:
#            | Type                                                                    | 05/17  | 06/17 | 07/17  | payments year to date |
#            | 16-18 levy additional provider payments: audit adjustments              | 51.65  | -8.61 | 63.42  | 106.46                |
#            | 16-18 non-levy additional provider payments: training authorised claims | 0      | -9.00 | 2.45   | -6.55                 |
#            | Adult levy training: authorised claims                                  | -25.68 | 0     | 65.49  | 39.81                 |
#            | Adult levy additional provider payments: audit adjustments              | 0      | 18.65 | 1.63   | 20.28                 |
#            | Adult non-levy additional employer payments: audit adjustments          | 36.00  | 0     | 159.34 | 195.34                |
#
#
#    Scenario: Payments when only current provider adjustments exist
#        Given that the previous EAS entries for a provider are as follows:
#            | Type                                                                    | 05/17 | 06/17 | year to date total |
#            | 16-18 levy additional provider payments: audit adjustments              | 0     | 0     | 0                  |
#            | 16-18 non-levy additional provider payments: training authorised claims | 0     | 0     | 0                  |
#            | Adult levy training: authorised claims                                  | 0     | 0     | 0                  |
#            | Adult non-levy additional employer payments: audit adjustments          | 0     | 0     | 0                  |
#        When the following EAS entries are submitted:
#            | Type                                                                    | 05/17  | 06/17 | 07/17  | year to date total |
#            | 16-18 levy additional provider payments: audit adjustments              | 195.17 | 4.98  | 63.42  | 263.57             |
#            | 16-18 non-levy additional provider payments: training authorised claims | 17.57  | 2.89  | 2.45   | 22.91              |
#            | Adult levy training: authorised claims                                  | 475.34 | 98.14 | 65.49  | 638.97             |
#            | Adult levy additional provider payments: audit adjustments              | 0      | 18.65 | 1.63   | 20.28              |
#            | Adult non-levy additional employer payments: audit adjustments          | 341.25 | 5.23  | 159.34 | 505.82             |
#        Then the following adjustments will be generated:
#            | Type                                                                    | 05/17  | 06/17 | 07/17  | year to date total |
#            | 16-18 levy additional provider payments: audit adjustments              | 195.17 | 4.98  | 63.42  | 263.57             |
#            | 16-18 non-levy additional provider payments: training authorised claims | 17.57  | 2.89  | 2.45   | 22.91              |
#            | Adult levy training: authorised claims                                  | 475.34 | 98.14 | 65.49  | 638.97             |
#            | Adult levy additional provider payments: audit adjustments              | 0      | 18.65 | 1.63   | 20.28              |
#            | Adult non-levy additional employer payments: audit adjustments          | 341.25 | 5.23  | 159.34 | 505.82             |




#Feature: 1.SUBMISSION IN CURRENT MONTH 

#1a. Submission nothing claimed (blank vanilla submission)
#1b. Submission for every claim line
#1c. Submission for mixture of claim lines (some entered some not)

	Scenario: 1a. Submission nothing claimed (blank vanilla submission)

		Given the EAS collection period is 08/17
		When the following EAS form is submitted:
			| Type                                        | 08/17 |
			| 16-18 Levy Employer Audit Adjustments       | 0     |
			| 16-18 Levy Provider Audit Adjustments       | 0     |
			| 16-18 Levy Training Audit Adjustments       | 0     |
			#
			| Adult Levy Employer Audit Adjustments       | 0     |
			| Adult Levy Provider Audit Adjustments       | 0     |
			| Adult Levy Training Audit Adjustments       | 0     |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     |
			| 16-18 Levy Provider Authorised Claims       | 0     |
			| 16-18 Levy Training Authorised Claims       | 0     |
			#
			| Adult Levy Employer Authorised Claims       | 0     |
			| Adult Levy Provider Authorised Claims       | 0     |
			| Adult Levy Training Authorised Claims       | 0     |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     |
			| Adult Non-Levy Provider Audit Adjustments   | 0     |
			| Adult Non-Levy Training Audit Adjustments   | 0     |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     |
			| 16-18 Non Levy Provider Authorised Claims   | 0     |
			| 16-18 Non Levy Training Authorised Claims   | 0     |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     |
			| Adult Non-Levy Provider Authorised Claims   | 0     |
			| Adult Non-Levy Training Authorised Claims   | 0     |
#
			| 16-18 Levy Excess Learning Support          | 0     |
			| 16-18 Non-Levey Excess Learning Support     | 0     |
			| Adult Levy Excess Learning Support          | 0     |
			| Adult Non-Levy Excess Learning Support      | 0     |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     |
			| 16-18 Non-Levy Exceptional Learning Support | 0     |
			| Adult Levy Exceptional Learning Support     | 0     |
			| Adult Non-Levy Exceptional Learning Support | 0     |
	
	
		Then the EAS payments are:
			| type                                        | 08/17 | 09/17 |
			| 16-18 Levy Employer Audit Adjustments       | 0     | 0     |
			| 16-18 Levy Provider Audit Adjustments       | 0     | 0     |
			| 16-18 Levy Training Audit Adjustments       | 0     | 0     |
			#
			| Adult Levy Employer Audit Adjustments       | 0     | 0     |
			| Adult Levy Provider Audit Adjustments       | 0     | 0     |
			| Adult Levy Training Audit Adjustments       | 0     | 0     |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     | 0     |
			| 16-18 Levy Provider Authorised Claims       | 0     | 0     |
			| 16-18 Levy Training Authorised Claims       | 0     | 0     |
			#
			| Adult Levy Employer Authorised Claims       | 0     | 0     |
			| Adult Levy Provider Authorised Claims       | 0     | 0     |
			| Adult Levy Training Authorised Claims       | 0     | 0     |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     | 0     |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     | 0     |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     | 0     |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     | 0     |
			| Adult Non-Levy Provider Audit Adjustments   | 0     | 0     |
			| Adult Non-Levy Training Audit Adjustments   | 0     | 0     |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     | 0     |
			| 16-18 Non Levy Provider Authorised Claims   | 0     | 0     |
			| 16-18 Non Levy Training Authorised Claims   | 0     | 0     |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     | 0     |
			| Adult Non-Levy Provider Authorised Claims   | 0     | 0     |
			| Adult Non-Levy Training Authorised Claims   | 0     | 0     |
#
			| 16-18 Levy Excess Learning Support          | 0     | 0     |
			| 16-18 Non-Levey Excess Learning Support     | 0     | 0     |
			| Adult Levy Excess Learning Support          | 0     | 0     |
			| Adult Non-Levy Excess Learning Support      | 0     | 0     |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     |
			| 16-18 Non-Levy Exceptional Learning Support | 0     | 0     |
			| Adult Levy Exceptional Learning Support     | 0     | 0     |
			| Adult Non-Levy Exceptional Learning Support | 0     | 0     |



	Scenario: 1b. Submission for all claim lines

		Given the EAS collection period is 08/17
		When the following EAS form is submitted:
			| Type                                        | 08/17  |
			| 16-18 Levy Employer Audit Adjustments       | 10.01  |
			| 16-18 Levy Provider Audit Adjustments       | 20.02  |
			| 16-18 Levy Training Audit Adjustments       | 30.03  |
			#
			| Adult Levy Employer Audit Adjustments       | 40.04  |
			| Adult Levy Provider Audit Adjustments       | 50.05  |
			| Adult Levy Training Audit Adjustments       | 60.06  |
			#
			| 16-18 Levy Employer Authorised Claims       | 70.07  |
			| 16-18 Levy Provider Authorised Claims       | 80.08  |
			| 16-18 Levy Training Authorised Claims       | 90.09  |
			#
			| Adult Levy Employer Authorised Claims       | 100.10 |
			| Adult Levy Provider Authorised Claims       | 110.11 |
			| Adult Levy Training Authorised Claims       | 120.12 |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 130.13 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 140.14 |
			| 16-18 Non-Levy Training Audit Adjustments   | 150.15 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 160.16 |
			| Adult Non-Levy Provider Audit Adjustments   | 170.17 |
			| Adult Non-Levy Training Audit Adjustments   | 180.18 |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 190.19 |
			| 16-18 Non Levy Provider Authorised Claims   | 200.20 |
			| 16-18 Non Levy Training Authorised Claims   | 210.21 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 220.22 |
			| Adult Non-Levy Provider Authorised Claims   | 230.23 |
			| Adult Non-Levy Training Authorised Claims   | 240.14 |
#
			| 16-18 Levy Excess Learning Support          | 250.25 |
			| 16-18 Non-Levey Excess Learning Support     | 260.26 |
			| Adult Levy Excess Learning Support          | 270.27 |
			| Adult Non-Levy Excess Learning Support      | 280.28 |
			#
			| 16-18 Levy Exceptional Learning Support     | 290.29 |
			| 16-18 Non-Levy Exceptional Learning Support | 300.30 |
			| Adult Levy Exceptional Learning Support     | 310.31 |
			| Adult Non-Levy Exceptional Learning Support | 320.32 |



		Then the EAS payments are:
			| type                                        | 08/17 | 09/17  |
			| 16-18 Levy Employer Audit Adjustments       | 0     | 10.01  |
			| 16-18 Levy Provider Audit Adjustments       | 0     | 20.02  |
			| 16-18 Levy Training Audit Adjustments       | 0     | 30.03  |
			#
			| Adult Levy Employer Audit Adjustments       | 0     | 40.04  |
			| Adult Levy Provider Audit Adjustments       | 0     | 50.05  |
			| Adult Levy Training Audit Adjustments       | 0     | 60.06  |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     | 70.07  |
			| 16-18 Levy Provider Authorised Claims       | 0     | 80.08  |
			| 16-18 Levy Training Authorised Claims       | 0     | 90.09  |
			#
			| Adult Levy Employer Authorised Claims       | 0     | 100.10 |
			| Adult Levy Provider Authorised Claims       | 0     | 110.11 |
			| Adult Levy Training Authorised Claims       | 0     | 120.12 |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     | 130.13 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     | 140.14 |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     | 150.15 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     | 160.16 |
			| Adult Non-Levy Provider Audit Adjustments   | 0     | 170.17 |
			| Adult Non-Levy Training Audit Adjustments   | 0     | 180.18 |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     | 190.19 |
			| 16-18 Non Levy Provider Authorised Claims   | 0     | 200.20 |
			| 16-18 Non Levy Training Authorised Claims   | 0     | 210.21 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     | 220.22 |
			| Adult Non-Levy Provider Authorised Claims   | 0     | 230.23 |
			| Adult Non-Levy Training Authorised Claims   | 0     | 240.14 |
#
			| 16-18 Levy Excess Learning Support          | 0     | 250.25 |
			| 16-18 Non-Levey Excess Learning Support     | 0     | 260.26 |
			| Adult Levy Excess Learning Support          | 0     | 270.27 |
			| Adult Non-Levy Excess Learning Support      | 0     | 280.28 |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     | 290.29 |
			| 16-18 Non-Levy Exceptional Learning Support | 0     | 300.30 |
			| Adult Levy Exceptional Learning Support     | 0     | 310.31 |
			| Adult Non-Levy Exceptional Learning Support | 0     | 320.32 |

		

	Scenario: 1c. Submission for not all claim lines

		Given the EAS collection period is 08/17
		When the following EAS form is submitted:
			| Type                                        | 08/17  |
			| 16-18 Levy Employer Audit Adjustments       | 10.01  |
			| 16-18 Levy Provider Audit Adjustments       | 0      |
			| 16-18 Levy Training Audit Adjustments       | 30.03  |
			#
			| Adult Levy Employer Audit Adjustments       | 0      |
			| Adult Levy Provider Audit Adjustments       | 50.05  |
			| Adult Levy Training Audit Adjustments       | 0      |
			#
			| 16-18 Levy Employer Authorised Claims       | 70.07  |
			| 16-18 Levy Provider Authorised Claims       | 0      |
			| 16-18 Levy Training Authorised Claims       | 90.09  |
			#
			| Adult Levy Employer Authorised Claims       | 0      |
			| Adult Levy Provider Authorised Claims       | 110.11 |
			| Adult Levy Training Authorised Claims       | 0      |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 130.13 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0      |
			| 16-18 Non-Levy Training Audit Adjustments   | 150.15 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0      |
			| Adult Non-Levy Provider Audit Adjustments   | 170.17 |
			| Adult Non-Levy Training Audit Adjustments   | 0      |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 190.19 |
			| 16-18 Non Levy Provider Authorised Claims   | 0      |
			| 16-18 Non Levy Training Authorised Claims   | 210.21 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0      |
			| Adult Non-Levy Provider Authorised Claims   | 230.23 |
			| Adult Non-Levy Training Authorised Claims   | 0      |
#
			| 16-18 Levy Excess Learning Support          | 250.25 |
			| 16-18 Non-Levey Excess Learning Support     | 0      |
			| Adult Levy Excess Learning Support          | 270.27 |
			| Adult Non-Levy Excess Learning Support      | 0      |
			#
			| 16-18 Levy Exceptional Learning Support     | 290.29 |
			| 16-18 Non-Levy Exceptional Learning Support | 0      |
			| Adult Levy Exceptional Learning Support     | 310.31 |
			| Adult Non-Levy Exceptional Learning Support | 0      |


		Then the EAS payments are:
			| type                                        | 08/17 | 09/17  |
			| 16-18 Levy Employer Audit Adjustments       | 0     | 10.01  |
			| 16-18 Levy Provider Audit Adjustments       | 0     | 0      |
			| 16-18 Levy Training Audit Adjustments       | 0     | 30.03  |
			#
			| Adult Levy Employer Audit Adjustments       | 0     | 0      |
			| Adult Levy Provider Audit Adjustments       | 0     | 50.05  |
			| Adult Levy Training Audit Adjustments       | 0     | 0      |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     | 70.07  |
			| 16-18 Levy Provider Authorised Claims       | 0     | 0      |
			| 16-18 Levy Training Authorised Claims       | 0     | 90.09  |
			#
			| Adult Levy Employer Authorised Claims       | 0     | 0      |
			| Adult Levy Provider Authorised Claims       | 0     | 110.11 |
			| Adult Levy Training Authorised Claims       | 0     | 0      |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     | 130.13 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     | 0      |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     | 150.15 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     | 0      |
			| Adult Non-Levy Provider Audit Adjustments   | 0     | 170.17 |
			| Adult Non-Levy Training Audit Adjustments   | 0     | 0      |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     | 190.19 |
			| 16-18 Non Levy Provider Authorised Claims   | 0     | 0      |
			| 16-18 Non Levy Training Authorised Claims   | 0     | 210.21 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     | 0      |
			| Adult Non-Levy Provider Authorised Claims   | 0     | 230.23 |
			| Adult Non-Levy Training Authorised Claims   | 0     | 0      |
#
			| 16-18 Levy Excess Learning Support          | 0     | 250.25 |
			| 16-18 Non-Levey Excess Learning Support     | 0     | 0      |
			| Adult Levy Excess Learning Support          | 0     | 270.27 |
			| Adult Non-Levy Excess Learning Support      | 0     | 0      |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     | 290.29 |
			| 16-18 Non-Levy Exceptional Learning Support | 0     | 0      |
			| Adult Levy Exceptional Learning Support     | 0     | 310.31 |
			| Adult Non-Levy Exceptional Learning Support | 0     | 0      |


	# Feature: 2.SUBMISSION FOR A PREVIOUS MONTH - NO PREVIOUS SUBMISSIONS
	#Note: Do we need some ILR attributes here to determine it is a retrospective claim submission or will the system not just assume it is always the active collection period?
	#(AW - the form is only available for the current month and previous months; future months are not available. So, if the latest month on the form is November, that means it is currently November)

	#2a. Submission for every claim line
	#2b. Submission for mixture of claim lines (some entered some not)



	Scenario: 2a. Submission for all claim lines

		Given the EAS collection period is 09/17
		When the following EAS form is submitted:
			| Type                                        | 08/17  | 09/17 |
			| 16-18 Levy Employer Audit Adjustments       | 10.01  | 0     |
			| 16-18 Levy Provider Audit Adjustments       | 20.02  | 0     |
			| 16-18 Levy Training Audit Adjustments       | 30.03  | 0     |
			#
			| Adult Levy Employer Audit Adjustments       | 40.04  | 0     |
			| Adult Levy Provider Audit Adjustments       | 50.05  | 0     |
			| Adult Levy Training Audit Adjustments       | 60.06  | 0     |
			#
			| 16-18 Levy Employer Authorised Claims       | 70.07  | 0     |
			| 16-18 Levy Provider Authorised Claims       | 80.08  | 0     |
			| 16-18 Levy Training Authorised Claims       | 90.09  | 0     |
			#
			| Adult Levy Employer Authorised Claims       | 100.10 | 0     |
			| Adult Levy Provider Authorised Claims       | 110.11 | 0     |
			| Adult Levy Training Authorised Claims       | 120.12 | 0     |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 130.13 | 0     |
			| 16-18 Non-Levy Provider Audit Adjustments   | 140.14 | 0     |
			| 16-18 Non-Levy Training Audit Adjustments   | 150.15 | 0     |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 160.16 | 0     |
			| Adult Non-Levy Provider Audit Adjustments   | 170.17 | 0     |
			| Adult Non-Levy Training Audit Adjustments   | 180.18 | 0     |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 190.19 | 0     |
			| 16-18 Non Levy Provider Authorised Claims   | 200.20 | 0     |
			| 16-18 Non Levy Training Authorised Claims   | 210.21 | 0     |
			#
			| Adult Non-Levy Employer Authorised Claims   | 220.22 | 0     |
			| Adult Non-Levy Provider Authorised Claims   | 230.23 | 0     |
			| Adult Non-Levy Training Authorised Claims   | 240.14 | 0     |
#
			| 16-18 Levy Excess Learning Support          | 250.25 | 0     |
			| 16-18 Non-Levey Excess Learning Support     | 260.26 | 0     |
			| Adult Levy Excess Learning Support          | 270.27 | 0     |
			| Adult Non-Levy Excess Learning Support      | 280.28 | 0     |
			#
			| 16-18 Levy Exceptional Learning Support     | 290.29 | 0     |
			| 16-18 Non-Levy Exceptional Learning Support | 300.30 | 0     |
			| Adult Levy Exceptional Learning Support     | 310.31 | 0     |
			| Adult Non-Levy Exceptional Learning Support | 320.32 | 0     |
		

		Then the EAS payments are:
			| type                                        | 08/17 | 09/17 | 10/17  |
			| 16-18 Levy Employer Audit Adjustments       | 0     | 0     | 10.01  |
			| 16-18 Levy Provider Audit Adjustments       | 0     | 0     | 20.02  |
			| 16-18 Levy Training Audit Adjustments       | 0     | 0     | 30.03  |
			#
			| Adult Levy Employer Audit Adjustments       | 0     | 0     | 40.04  |
			| Adult Levy Provider Audit Adjustments       | 0     | 0     | 50.05  |
			| Adult Levy Training Audit Adjustments       | 0     | 0     | 60.06  |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     | 0     | 70.07  |
			| 16-18 Levy Provider Authorised Claims       | 0     | 0     | 80.08  |
			| 16-18 Levy Training Authorised Claims       | 0     | 0     | 90.09  |
			#
			| Adult Levy Employer Authorised Claims       | 0     | 0     | 100.10 |
			| Adult Levy Provider Authorised Claims       | 0     | 0     | 110.11 |
			| Adult Levy Training Authorised Claims       | 0     | 0     | 120.12 |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     | 0     | 130.13 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     | 0     | 140.14 |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     | 0     | 150.15 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     | 0     | 160.16 |
			| Adult Non-Levy Provider Audit Adjustments   | 0     | 0     | 170.17 |
			| Adult Non-Levy Training Audit Adjustments   | 0     | 0     | 180.18 |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     | 0     | 190.19 |
			| 16-18 Non Levy Provider Authorised Claims   | 0     | 0     | 200.20 |
			| 16-18 Non Levy Training Authorised Claims   | 0     | 0     | 210.21 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     | 0     | 220.22 |
			| Adult Non-Levy Provider Authorised Claims   | 0     | 0     | 230.23 |
			| Adult Non-Levy Training Authorised Claims   | 0     | 0     | 240.14 |
#
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 250.25 |
			| 16-18 Non-Levey Excess Learning Support     | 0     | 0     | 260.26 |
			| Adult Levy Excess Learning Support          | 0     | 0     | 270.27 |
			| Adult Non-Levy Excess Learning Support      | 0     | 0     | 280.28 |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 290.29 |
			| 16-18 Non-Levy Exceptional Learning Support | 0     | 0     | 300.30 |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 310.31 |
			| Adult Non-Levy Exceptional Learning Support | 0     | 0     | 320.32 |



	Scenario: 2b. Submission for not all claim lines

		Given the EAS collection period is 09/17
		When the following EAS form is submitted:
			| Type                                        | 08/17  | 09/17 |
			| 16-18 Levy Employer Audit Adjustments       | 10.01  | 0     |
			| 16-18 Levy Provider Audit Adjustments       | 0      | 0     |
			| 16-18 Levy Training Audit Adjustments       | 30.03  | 0     |
			#
			| Adult Levy Employer Audit Adjustments       | 0      | 0     |
			| Adult Levy Provider Audit Adjustments       | 50.05  | 0     |
			| Adult Levy Training Audit Adjustments       | 0      | 0     |
			#
			| 16-18 Levy Employer Authorised Claims       | 70.07  | 0     |
			| 16-18 Levy Provider Authorised Claims       | 0      | 0     |
			| 16-18 Levy Training Authorised Claims       | 90.09  | 0     |
			#
			| Adult Levy Employer Authorised Claims       | 0      | 0     |
			| Adult Levy Provider Authorised Claims       | 110.11 | 0     |
			| Adult Levy Training Authorised Claims       | 0      | 0     |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 130.13 | 0     |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0      | 0     |
			| 16-18 Non-Levy Training Audit Adjustments   | 150.15 | 0     |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0      | 0     |
			| Adult Non-Levy Provider Audit Adjustments   | 170.17 | 0     |
			| Adult Non-Levy Training Audit Adjustments   | 0      | 0     |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 190.19 | 0     |
			| 16-18 Non Levy Provider Authorised Claims   | 0      | 0     |
			| 16-18 Non Levy Training Authorised Claims   | 210.21 | 0     |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0      | 0     |
			| Adult Non-Levy Provider Authorised Claims   | 230.23 | 0     |
			| Adult Non-Levy Training Authorised Claims   | 0      | 0     |
#
			| 16-18 Levy Excess Learning Support          | 250.25 | 0     |
			| 16-18 Non-Levey Excess Learning Support     | 0      | 0     |
			| Adult Levy Excess Learning Support          | 270.27 | 0     |
			| Adult Non-Levy Excess Learning Support      | 0      | 0     |
			#
			| 16-18 Levy Exceptional Learning Support     | 290.29 | 0     |
			| 16-18 Non-Levy Exceptional Learning Support | 0      | 0     |
			| Adult Levy Exceptional Learning Support     | 310.31 | 0     |
			| Adult Non-Levy Exceptional Learning Support | 0      | 0     |


		Then the EAS payments are:
			| type                                        | 08/17 | 09/17 | 10/17  |
			| 16-18 Levy Employer Audit Adjustments       | 0     | 0     | 10.01  |
			| 16-18 Levy Provider Audit Adjustments       | 0     | 0     | 0      |
			| 16-18 Levy Training Audit Adjustments       | 0     | 0     | 30.03  |
			#
			| Adult Levy Employer Audit Adjustments       | 0     | 0     | 0      |
			| Adult Levy Provider Audit Adjustments       | 0     | 0     | 50.05  |
			| Adult Levy Training Audit Adjustments       | 0     | 0     | 0      |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     | 0     | 70.07  |
			| 16-18 Levy Provider Authorised Claims       | 0     | 0     | 0      |
			| 16-18 Levy Training Authorised Claims       | 0     | 0     | 90.09  |
			#
			| Adult Levy Employer Authorised Claims       | 0     | 0     | 0      |
			| Adult Levy Provider Authorised Claims       | 0     | 0     | 110.11 |
			| Adult Levy Training Authorised Claims       | 0     | 0     | 0      |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     | 0     | 130.13 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     | 0     | 0      |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     | 0     | 150.15 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     | 0     | 0      |
			| Adult Non-Levy Provider Audit Adjustments   | 0     | 0     | 170.17 |
			| Adult Non-Levy Training Audit Adjustments   | 0     | 0     | 0      |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     | 0     | 190.19 |
			| 16-18 Non Levy Provider Authorised Claims   | 0     | 0     | 0      |
			| 16-18 Non Levy Training Authorised Claims   | 0     | 0     | 210.21 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     | 0     | 0      |
			| Adult Non-Levy Provider Authorised Claims   | 0     | 0     | 230.23 |
			| Adult Non-Levy Training Authorised Claims   | 0     | 0     | 0      |
#
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 250.25 |
			| 16-18 Non-Levey Excess Learning Support     | 0     | 0     | 0      |
			| Adult Levy Excess Learning Support          | 0     | 0     | 270.27 |
			| Adult Non-Levy Excess Learning Support      | 0     | 0     | 0      |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 290.29 |
			| 16-18 Non-Levy Exceptional Learning Support | 0     | 0     | 0      |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 310.31 |
			| Adult Non-Levy Exceptional Learning Support | 0     | 0     | 0      |


	# Feature: 3.SUBMISSION FOR ALL MONTHS - NO PREVIOUS SUBMISSIONS
	#3a. Submission for every claim line
	#3b. Submission for mixture of claim lines (some entered some not)



	Scenario: 3a. Submission for all claim lines

		Given the EAS collection period is 09/17
		When the following EAS form is submitted:
			| Type                                        | 08/17  | 09/17  |
			| 16-18 Levy Employer Audit Adjustments       | 10.01  | 9.00   |
			| 16-18 Levy Provider Audit Adjustments       | 20.02  | 19.01  |
			| 16-18 Levy Training Audit Adjustments       | 30.03  | 29.02  |
			#
			| Adult Levy Employer Audit Adjustments       | 40.04  | 39.03  |
			| Adult Levy Provider Audit Adjustments       | 50.05  | 49.04  |
			| Adult Levy Training Audit Adjustments       | 60.06  | 59.05  |
			#
			| 16-18 Levy Employer Authorised Claims       | 70.07  | 69.06  |
			| 16-18 Levy Provider Authorised Claims       | 80.08  | 79.07  |
			| 16-18 Levy Training Authorised Claims       | 90.09  | 89.08  |
			#
			| Adult Levy Employer Authorised Claims       | 100.10 | 99.09  |
			| Adult Levy Provider Authorised Claims       | 110.11 | 109.10 |
			| Adult Levy Training Authorised Claims       | 120.12 | 119.11 |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 130.13 | 129.12 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 140.14 | 139.13 |
			| 16-18 Non-Levy Training Audit Adjustments   | 150.15 | 149.14 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 160.16 | 159.15 |
			| Adult Non-Levy Provider Audit Adjustments   | 170.17 | 169.16 |
			| Adult Non-Levy Training Audit Adjustments   | 180.18 | 179.17 |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 190.19 | 189.18 |
			| 16-18 Non Levy Provider Authorised Claims   | 200.20 | 199.19 |
			| 16-18 Non Levy Training Authorised Claims   | 210.21 | 209.20 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 220.22 | 219.21 |
			| Adult Non-Levy Provider Authorised Claims   | 230.23 | 229.22 |
			| Adult Non-Levy Training Authorised Claims   | 240.24 | 239.23 |
#
			| 16-18 Levy Excess Learning Support          | 250.25 | 249.24 |
			| 16-18 Non-Levey Excess Learning Support     | 260.26 | 259.25 |
			| Adult Levy Excess Learning Support          | 270.27 | 269.26 |
			| Adult Non-Levy Excess Learning Support      | 280.28 | 279.27 |
			#
			| 16-18 Levy Exceptional Learning Support     | 290.29 | 289.28 |
			| 16-18 Non-Levy Exceptional Learning Support | 300.30 | 299.29 |
			| Adult Levy Exceptional Learning Support     | 310.31 | 309.30 |
			| Adult Non-Levy Exceptional Learning Support | 320.32 | 319.31 |
		

		Then the EAS payments are:
			| type                                        | 08/17 | 09/17 | 10/17  |
			| 16-18 Levy Employer Audit Adjustments       | 0     | 0     | 19.01  |
			| 16-18 Levy Provider Audit Adjustments       | 0     | 0     | 39.03  |
			| 16-18 Levy Training Audit Adjustments       | 0     | 0     | 59.05  |
			#
			| Adult Levy Employer Audit Adjustments       | 0     | 0     | 79.07  |
			| Adult Levy Provider Audit Adjustments       | 0     | 0     | 99.09  |
			| Adult Levy Training Audit Adjustments       | 0     | 0     | 119.11 |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     | 0     | 139.13 |
			| 16-18 Levy Provider Authorised Claims       | 0     | 0     | 159.15 |
			| 16-18 Levy Training Authorised Claims       | 0     | 0     | 179.17 |
			#
			| Adult Levy Employer Authorised Claims       | 0     | 0     | 199.19 |
			| Adult Levy Provider Authorised Claims       | 0     | 0     | 219.21 |
			| Adult Levy Training Authorised Claims       | 0     | 0     | 239.23 |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     | 0     | 259.25 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     | 0     | 279.27 |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     | 0     | 299.29 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     | 0     | 319.31 |
			| Adult Non-Levy Provider Audit Adjustments   | 0     | 0     | 339.33 |
			| Adult Non-Levy Training Audit Adjustments   | 0     | 0     | 359.35 |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     | 0     | 379.37 |
			| 16-18 Non Levy Provider Authorised Claims   | 0     | 0     | 399.39 |
			| 16-18 Non Levy Training Authorised Claims   | 0     | 0     | 419.41 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     | 0     | 439.43 |
			| Adult Non-Levy Provider Authorised Claims   | 0     | 0     | 459.45 |
			| Adult Non-Levy Training Authorised Claims   | 0     | 0     | 479.47 |
#
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 499.49 |
			| 16-18 Non-Levey Excess Learning Support     | 0     | 0     | 519.51 |
			| Adult Levy Excess Learning Support          | 0     | 0     | 539.53 |
			| Adult Non-Levy Excess Learning Support      | 0     | 0     | 559.55 |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 579.57 |
			| 16-18 Non-Levy Exceptional Learning Support | 0     | 0     | 599.59 |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 619.61 |
			| Adult Non-Levy Exceptional Learning Support | 0     | 0     | 639.63 |



	Scenario: 3b.  Submission for mixture of claim lines (some entered some not)

		Given the EAS collection period is 09/17
		When the following EAS form is submitted:
			| Type                                        | 08/17  | 09/17  |
			| 16-18 Levy Employer Audit Adjustments       | 10.01  | 9.00   |
			| 16-18 Levy Provider Audit Adjustments       | 0      | 0      |
			| 16-18 Levy Training Audit Adjustments       | 30.03  | 29.02  |
			#
			| Adult Levy Employer Audit Adjustments       | 0      | 0      |
			| Adult Levy Provider Audit Adjustments       | 50.05  | 49.04  |
			| Adult Levy Training Audit Adjustments       | 0      | 0      |
			#
			| 16-18 Levy Employer Authorised Claims       | 70.07  | 69.06  |
			| 16-18 Levy Provider Authorised Claims       | 0      | 0      |
			| 16-18 Levy Training Authorised Claims       | 90.09  | 89.08  |
			#
			| Adult Levy Employer Authorised Claims       | 0      | 0      |
			| Adult Levy Provider Authorised Claims       | 110.11 | 109.10 |
			| Adult Levy Training Authorised Claims       | 0      | 0      |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 130.13 | 129.12 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0      | 0      |
			| 16-18 Non-Levy Training Audit Adjustments   | 150.15 | 149.14 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0      | 0      |
			| Adult Non-Levy Provider Audit Adjustments   | 170.17 | 169.16 |
			| Adult Non-Levy Training Audit Adjustments   | 0      | 0      |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 190.19 | 189.18 |
			| 16-18 Non Levy Provider Authorised Claims   | 0      | 0      |
			| 16-18 Non Levy Training Authorised Claims   | 210.21 | 209.20 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0      | 0      |
			| Adult Non-Levy Provider Authorised Claims   | 230.23 | 229.22 |
			| Adult Non-Levy Training Authorised Claims   | 0      | 0      |
#
			| 16-18 Levy Excess Learning Support          | 250.25 | 249.24 |
			| 16-18 Non-Levey Excess Learning Support     | 0      | 0      |
			| Adult Levy Excess Learning Support          | 270.27 | 269.26 |
			| Adult Non-Levy Excess Learning Support      | 0      | 0      |
			#
			| 16-18 Levy Exceptional Learning Support     | 290.29 | 289.28 |
			| 16-18 Non-Levy Exceptional Learning Support | 0      | 0      |
			| Adult Levy Exceptional Learning Support     | 310.31 | 309.30 |
			| Adult Non-Levy Exceptional Learning Support | 0      | 0      |


		Then the EAS payments are:
			| type                                        | 08/17 | 09/17 | 10/17  |
			| 16-18 Levy Employer Audit Adjustments       | 0     | 0     | 19.01  |
			| 16-18 Levy Provider Audit Adjustments       | 0     | 0     | 0      |
			| 16-18 Levy Training Audit Adjustments       | 0     | 0     | 59.05  |
			#
			| Adult Levy Employer Audit Adjustments       | 0     | 0     | 0      |
			| Adult Levy Provider Audit Adjustments       | 0     | 0     | 99.09  |
			| Adult Levy Training Audit Adjustments       | 0     | 0     | 0      |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     | 0     | 139.13 |
			| 16-18 Levy Provider Authorised Claims       | 0     | 0     | 0      |
			| 16-18 Levy Training Authorised Claims       | 0     | 0     | 179.17 |
			#
			| Adult Levy Employer Authorised Claims       | 0     | 0     | 0      |
			| Adult Levy Provider Authorised Claims       | 0     | 0     | 219.21 |
			| Adult Levy Training Authorised Claims       | 0     | 0     | 0      |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     | 0     | 259.25 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     | 0     | 0      |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     | 0     | 299.29 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     | 0     | 0      |
			| Adult Non-Levy Provider Audit Adjustments   | 0     | 0     | 339.33 |
			| Adult Non-Levy Training Audit Adjustments   | 0     | 0     | 0      |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     | 0     | 379.37 |
			| 16-18 Non Levy Provider Authorised Claims   | 0     | 0     | 0      |
			| 16-18 Non Levy Training Authorised Claims   | 0     | 0     | 419.41 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     | 0     | 0      |
			| Adult Non-Levy Provider Authorised Claims   | 0     | 0     | 459.45 |
			| Adult Non-Levy Training Authorised Claims   | 0     | 0     | 0      |
#
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 499.49 |
			| 16-18 Non-Levey Excess Learning Support     | 0     | 0     | 0      |
			| Adult Levy Excess Learning Support          | 0     | 0     | 539.53 |
			| Adult Non-Levy Excess Learning Support      | 0     | 0     | 0      |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 579.57 |
			| 16-18 Non-Levy Exceptional Learning Support | 0     | 0     | 0      |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 619.61 |
			| Adult Non-Levy Exceptional Learning Support | 0     | 0     | 0      |



	#Feature: 4. SUBMISSION FOR A PREVIOUS MONTH - UPDATE TO PREVIOUS SUBMISSIONS
	#4a. Submission for every claim line


	Scenario: 4a. Submission for all claim lines

		Given the EAS collection period is 10/17
		And the following EAS form is submitted in 09/17:
			| Type                                        | 08/17  | 09/17  |
			| 16-18 Levy Employer Audit Adjustments       | 10.01  | 9.00   |
			| 16-18 Levy Provider Audit Adjustments       | 20.02  | 19.01  |
			| 16-18 Levy Training Audit Adjustments       | 30.03  | 29.02  |
			#
			| Adult Levy Employer Audit Adjustments       | 40.04  | 39.03  |
			| Adult Levy Provider Audit Adjustments       | 50.05  | 49.04  |
			| Adult Levy Training Audit Adjustments       | 60.06  | 59.05  |
			#
			| 16-18 Levy Employer Authorised Claims       | 70.07  | 69.06  |
			| 16-18 Levy Provider Authorised Claims       | 80.08  | 79.07  |
			| 16-18 Levy Training Authorised Claims       | 90.09  | 89.08  |
			#
			| Adult Levy Employer Authorised Claims       | 100.10 | 99.09  |
			| Adult Levy Provider Authorised Claims       | 110.11 | 109.10 |
			| Adult Levy Training Authorised Claims       | 120.12 | 119.11 |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 130.13 | 129.12 |
			| 16-18 Non-Levy Provider Audit Adjustments   | 140.14 | 139.13 |
			| 16-18 Non-Levy Training Audit Adjustments   | 150.15 | 149.14 |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 160.16 | 159.15 |
			| Adult Non-Levy Provider Audit Adjustments   | 170.17 | 169.16 |
			| Adult Non-Levy Training Audit Adjustments   | 180.18 | 179.17 |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 190.19 | 189.18 |
			| 16-18 Non Levy Provider Authorised Claims   | 200.20 | 199.19 |
			| 16-18 Non Levy Training Authorised Claims   | 210.21 | 209.20 |
			#
			| Adult Non-Levy Employer Authorised Claims   | 220.22 | 219.21 |
			| Adult Non-Levy Provider Authorised Claims   | 230.23 | 229.22 |
			| Adult Non-Levy Training Authorised Claims   | 240.24 | 239.23 |
#
			| 16-18 Levy Excess Learning Support          | 250.25 | 249.24 |
			| 16-18 Non-Levey Excess Learning Support     | 260.26 | 259.25 |
			| Adult Levy Excess Learning Support          | 270.27 | 269.26 |
			| Adult Non-Levy Excess Learning Support      | 280.28 | 279.27 |
			#
			| 16-18 Levy Exceptional Learning Support     | 290.29 | 289.28 |
			| 16-18 Non-Levy Exceptional Learning Support | 300.30 | 299.29 |
			| Adult Levy Exceptional Learning Support     | 310.31 | 309.30 |
			| Adult Non-Levy Exceptional Learning Support | 320.32 | 319.31 |


		When the following EAS form is submitted:
			| Type                                        | 08/17   | 09/17  | 10/17 |
			| 16-18 Levy Employer Audit Adjustments       | 0.01    | 9.00   | 0     |
			| 16-18 Levy Provider Audit Adjustments       | 0.02    | 19.01  | 0     |
			| 16-18 Levy Training Audit Adjustments       | 0.03    | 29.02  | 0     |
			#
			| Adult Levy Employer Audit Adjustments       | 0.04    | 39.03  | 0     |
			| Adult Levy Provider Audit Adjustments       | 0.05    | 49.04  | 0     |
			| Adult Levy Training Audit Adjustments       | 0.06    | 59.05  | 0     |
			#
			| 16-18 Levy Employer Authorised Claims       | 0.07    | 69.06  | 0     |
			| 16-18 Levy Provider Authorised Claims       | 0.08    | 79.07  | 0     |
			| 16-18 Levy Training Authorised Claims       | 0.09    | 89.08  | 0     |
			#
			| Adult Levy Employer Authorised Claims       | 00.10   | 99.09  | 0     |
			| Adult Levy Provider Authorised Claims       | 0.11    | 109.10 | 0     |
			| Adult Levy Training Authorised Claims       | 0.12    | 119.11 | 0     |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0.13    | 129.12 | 0     |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0.14    | 139.13 | 0     |
			| 16-18 Non-Levy Training Audit Adjustments   | 0.15    | 149.14 | 0     |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0.16    | 159.15 | 0     |
			| Adult Non-Levy Provider Audit Adjustments   | 0.17    | 169.16 | 0     |
			| Adult Non-Levy Training Audit Adjustments   | 0.18    | 179.17 | 0     |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0.19    | 189.18 | 0     |
			| 16-18 Non Levy Provider Authorised Claims   | 0.20    | 199.19 | 0     |
			| 16-18 Non Levy Training Authorised Claims   | 0.21    | 209.20 | 0     |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0.22    | 219.21 | 0     |
			| Adult Non-Levy Provider Authorised Claims   | 0.23    | 229.22 | 0     |
			| Adult Non-Levy Training Authorised Claims   | 0.24    | 239.23 | 0     |
#
			| 16-18 Levy Excess Learning Support          | 1250.25 | 249.24 | 0     |
			| 16-18 Non-Levey Excess Learning Support     | 1260.26 | 259.25 | 0     |
			| Adult Levy Excess Learning Support          | 1270.27 | 269.26 | 0     |
			| Adult Non-Levy Excess Learning Support      | 1280.28 | 279.27 | 0     |
			#
			| 16-18 Levy Exceptional Learning Support     | 1290.29 | 289.28 | 0     |
			| 16-18 Non-Levy Exceptional Learning Support | 1300.30 | 299.29 | 0     |
			| Adult Levy Exceptional Learning Support     | 1310.31 | 309.30 | 0     |
			| Adult Non-Levy Exceptional Learning Support | 1320.32 | 319.31 | 0     |
		

		Then the EAS payments are:
			| type                                        | 08/17 | 09/17 | 10/17  | 11/17 |
			| 16-18 Levy Employer Audit Adjustments       | 0     | 0     | 19.01  | -10   |
			| 16-18 Levy Provider Audit Adjustments       | 0     | 0     | 39.03  | -20   |
			| 16-18 Levy Training Audit Adjustments       | 0     | 0     | 59.05  | -30   |
			#
			| Adult Levy Employer Audit Adjustments       | 0     | 0     | 79.07  | -40   |
			| Adult Levy Provider Audit Adjustments       | 0     | 0     | 99.09  | -50   |
			| Adult Levy Training Audit Adjustments       | 0     | 0     | 119.11 | -60   |
			#
			| 16-18 Levy Employer Authorised Claims       | 0     | 0     | 139.13 | -70   |
			| 16-18 Levy Provider Authorised Claims       | 0     | 0     | 159.15 | -80   |
			| 16-18 Levy Training Authorised Claims       | 0     | 0     | 179.17 | -90   |
			#
			| Adult Levy Employer Authorised Claims       | 0     | 0     | 199.19 | -100  |
			| Adult Levy Provider Authorised Claims       | 0     | 0     | 219.21 | -110  |
			| Adult Levy Training Authorised Claims       | 0     | 0     | 239.23 | -120  |
#
#			
			| 16-18 Non-Levy Employer Audit Adjustments   | 0     | 0     | 259.25 | -130  |
			| 16-18 Non-Levy Provider Audit Adjustments   | 0     | 0     | 279.27 | -140  |
			| 16-18 Non-Levy Training Audit Adjustments   | 0     | 0     | 299.29 | -150  |
			#
			| Adult Non-Levy Employer Audit Adjustments   | 0     | 0     | 319.31 | -160  |
			| Adult Non-Levy Provider Audit Adjustments   | 0     | 0     | 339.33 | -170  |
			| Adult Non-Levy Training Audit Adjustments   | 0     | 0     | 359.35 | -180  |
			#
			| 16-18 Non-Levy Employer Authorised Claims   | 0     | 0     | 379.37 | -190  |
			| 16-18 Non Levy Provider Authorised Claims   | 0     | 0     | 399.39 | -200  |
			| 16-18 Non Levy Training Authorised Claims   | 0     | 0     | 419.41 | -210  |
			#
			| Adult Non-Levy Employer Authorised Claims   | 0     | 0     | 439.43 | -220  |
			| Adult Non-Levy Provider Authorised Claims   | 0     | 0     | 459.45 | -230  |
			| Adult Non-Levy Training Authorised Claims   | 0     | 0     | 479.47 | -240  |
#
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 499.49 | 1000  |
			| 16-18 Non-Levey Excess Learning Support     | 0     | 0     | 519.51 | 1000  |
			| Adult Levy Excess Learning Support          | 0     | 0     | 539.53 | 1000  |
			| Adult Non-Levy Excess Learning Support      | 0     | 0     | 559.55 | 1000  |
			#
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 579.57 | 1000  |
			| 16-18 Non-Levy Exceptional Learning Support | 0     | 0     | 599.59 | 1000  |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 619.61 | 1000  |
			| Adult Non-Levy Exceptional Learning Support | 0     | 0     | 639.63 | 1000  |