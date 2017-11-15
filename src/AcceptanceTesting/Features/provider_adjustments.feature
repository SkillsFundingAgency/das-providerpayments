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
			| 16-18 Levy Excess Learning Support          | 0     |
			| 16-18 Levy Exceptional Learning Support     | 0     |
			| 16-18 Levy Audit Adjustments                | 0     |
			| 16-18 Levy Authorised Claims                | 0     |
			| 16-18 Levy Authorised Claims                | 0     |
			| 16-18 Non Levy Excess Learning Support      | 0     |
			| 16-18 Non Levy Exceptional Learning Support | 0     |
			| 16-18 Non Levy Audit Adjustments            | 0     |
			| 16-18 Non Levy Authorised Claims            | 0     |
			| Adult Levy Excess Learning Support          | 0     |
			| Adult Levy Exceptional Learning Support     | 0     |
			| Adult Levy Audit Adjustments                | 0     |
			| Adult Levy Authorised Claims                | 0     |
			| Adult Non Levy Excess Learning Support      | 0     |
			| Adult Non Levy Exceptional Learning Support | 0     |
			| Adult Non Levy Audit Adjustments            | 0     |
			| Adult Non Levy Authorised Claims            | 0     |  
		
		Then the EAS payments are:
			| Type                                        | 08/17 | 09/17 |
			| 16-18 Levy Excess Learning Support          | 0     | 0     |
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     |
			| 16-18 Levy Audit Adjustments                | 0     | 0     |
			| 16-18 Levy Authorised Claims                | 0     | 0     |
			| 16-18 Levy Authorised Claims                | 0     | 0     |
			| 16-18 Non Levy Excess Learning Support      | 0     | 0     |
			| 16-18 Non Levy Exceptional Learning Support | 0     | 0     |
			| 16-18 Non Levy Audit Adjustments            | 0     | 0     |
			| 16-18 Non Levy Authorised Claims            | 0     | 0     |
			| Adult Levy Excess Learning Support          | 0     | 0     |
			| Adult Levy Exceptional Learning Support     | 0     | 0     |
			| Adult Levy Audit Adjustments                | 0     | 0     |
			| Adult Levy Authorised Claims                | 0     | 0     |
			| Adult Non Levy Excess Learning Support      | 0     | 0     |
			| Adult Non Levy Exceptional Learning Support | 0     | 0     |
			| Adult Non Levy Audit Adjustments            | 0     | 0     |
			| Adult Non Levy Authorised Claims            | 0     | 0     | 


	Scenario: 1b. Submission for all claim lines

		Given the EAS collection period is 08/17
		When the following EAS form is submitted:
			| Type                                        | 08/17 |
			| 16-18 Levy Excess Learning Support          | 10.01 |
			| 16-18 Levy Exceptional Learning Support     | 11.02 |
			| 16-18 Levy Audit Adjustments                | 12.03 |
			| 16-18 Levy Authorised Claims                | 13.04 |
			| 16-18 Levy Authorised Claims                | 14.05 |
			| 16-18 Non Levy Excess Learning Support      | 15.06 |
			| 16-18 Non Levy Exceptional Learning Support | 16.07 |
			| 16-18 Non Levy Audit Adjustments            | 17.08 |
			| 16-18 Non Levy Authorised Claims            | 18.09 |
			| Adult Levy Excess Learning Support          | 20.10 |
			| Adult Levy Exceptional Learning Support     | 21.11 |
			| Adult Levy Audit Adjustments                | 22.12 |
			| Adult Levy Authorised Claims                | 23.13 |
			| Adult Non Levy Excess Learning Support      | 24.14 |
			| Adult Non Levy Exceptional Learning Support | 25.15 |
			| Adult Non Levy Audit Adjustments            | 26.16 |
			| Adult Non Levy Authorised Claims            | 27.17 | 
		
		Then the EAS payments are:
			| Type                                        | 08/17 | 09/17 |
			| 16-18 Levy Excess Learning Support          | 0     | 10.01 |
			| 16-18 Levy Exceptional Learning Support     | 0     | 11.02 |
			| 16-18 Levy Audit Adjustments                | 0     | 12.03 |
			| 16-18 Levy Authorised Claims                | 0     | 13.04 |
			| 16-18 Levy Authorised Claims                | 0     | 14.05 |
			| 16-18 Non Levy Excess Learning Support      | 0     | 15.06 |
			| 16-18 Non Levy Exceptional Learning Support | 0     | 16.07 |
			| 16-18 Non Levy Audit Adjustments            | 0     | 17.08 |
			| 16-18 Non Levy Authorised Claims            | 0     | 18.09 |
			| Adult Levy Excess Learning Support          | 0     | 20.10 |
			| Adult Levy Exceptional Learning Support     | 0     | 21.11 |
			| Adult Levy Audit Adjustments                | 0     | 22.12 |
			| Adult Levy Authorised Claims                | 0     | 23.13 |
			| Adult Non Levy Excess Learning Support      | 0     | 24.14 |
			| Adult Non Levy Exceptional Learning Support | 0     | 25.15 |
			| Adult Non Levy Audit Adjustments            | 0     | 26.16 |
			| Adult Non Levy Authorised Claims            | 0     | 27.17 |

		
	Scenario: 1c. Submission for not all claim lines

		Given the EAS collection period is 08/17
		When the following EAS form is submitted:
			| Type                                        | 08/17 |
			| 16-18 Levy Excess Learning Support          | 10.01 |
			| 16-18 Levy Exceptional Learning Support     | 0     |
			| 16-18 Levy Audit Adjustments                | 12.03 |
			| 16-18 Levy Authorised Claims                | 0     |
			| 16-18 Levy Authorised Claims                | 14.05 |
			| 16-18 Non Levy Excess Learning Support      | 0     |
			| 16-18 Non Levy Exceptional Learning Support | 16.07 |
			| 16-18 Non Levy Audit Adjustments            | 0     |
			| 16-18 Non Levy Authorised Claims            | 18.09 |
			| Adult Levy Excess Learning Support          | 0     |
			| Adult Levy Exceptional Learning Support     | 21.11 |
			| Adult Levy Audit Adjustments                | 0     |
			| Adult Levy Authorised Claims                | 23.13 |
			| Adult Non Levy Excess Learning Support      | 0     |
			| Adult Non Levy Exceptional Learning Support | 25.15 |
			| Adult Non Levy Audit Adjustments            | 0     |
			| Adult Non Levy Authorised Claims            | 27.17 | 

		Then the EAS payments are:
			| Type                                        | 08/17 | 09/17 |
			| 16-18 Levy Excess Learning Support          | 0     | 10.01 |
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     |
			| 16-18 Levy Audit Adjustments                | 0     | 12.03 |
			| 16-18 Levy Authorised Claims                | 0     | 0     |
			| 16-18 Levy Authorised Claims                | 0     | 14.05 |
			| 16-18 Non Levy Excess Learning Support      | 0     | 0     |
			| 16-18 Non Levy Exceptional Learning Support | 0     | 16.07 |
			| 16-18 Non Levy Audit Adjustments            | 0     | 0     |
			| 16-18 Non Levy Authorised Claims            | 0     | 18.09 |
			| Adult Levy Excess Learning Support          | 0     | 0     |
			| Adult Levy Exceptional Learning Support     | 0     | 21.11 |
			| Adult Levy Audit Adjustments                | 0     | 0     |
			| Adult Levy Authorised Claims                | 0     | 23.13 |
			| Adult Non Levy Excess Learning Support      | 0     | 0     |
			| Adult Non Levy Exceptional Learning Support | 0     | 25.15 |
			| Adult Non Levy Audit Adjustments            | 0     | 0     |
			| Adult Non Levy Authorised Claims            | 0     | 27.17 |


	# Feature: 2.SUBMISSION FOR A PREVIOUS MONTH - NO PREVIOUS SUBMISSIONS
	#Note: Do we need some ILR attributes here to determine it is a retrospective claim submission or will the system not just assume it is always the active collection period?
	#(AW - the form is only available for the current month and previous months; future months are not available. So, if the latest month on the form is November, that means it is currently November)

	#2a. Submission for every claim line
	#2b. Submission for mixture of claim lines (some entered some not)


	Scenario: 2a. Submission for all claim lines

		Given the EAS collection period is 09/17
		When the following EAS form is submitted:
			| Type                                        | 08/17 | 09/17 |
			| 16-18 Levy Excess Learning Support          | 10.01 | 0     |
			| 16-18 Levy Exceptional Learning Support     | 11.02 | 0     |
			| 16-18 Levy Audit Adjustments                | 12.03 | 0     |
			| 16-18 Levy Authorised Claims                | 13.04 | 0     |
			| 16-18 Levy Authorised Claims                | 14.05 | 0     |
			| 16-18 Non Levy Excess Learning Support      | 15.06 | 0     |
			| 16-18 Non Levy Exceptional Learning Support | 16.07 | 0     |
			| 16-18 Non Levy Audit Adjustments            | 17.08 | 0     |
			| 16-18 Non Levy Authorised Claims            | 18.09 | 0     |
			| Adult Levy Excess Learning Support          | 20.10 | 0     |
			| Adult Levy Exceptional Learning Support     | 21.11 | 0     |
			| Adult Levy Audit Adjustments                | 22.12 | 0     |
			| Adult Levy Authorised Claims                | 23.13 | 0     |
			| Adult Non Levy Excess Learning Support      | 24.14 | 0     |
			| Adult Non Levy Exceptional Learning Support | 25.15 | 0     |
			| Adult Non Levy Audit Adjustments            | 26.16 | 0     |
			| Adult Non Levy Authorised Claims            | 27.17 | 0     | 
		
		Then the EAS payments are:
			| Type                                        | 08/17 | 09/17 | 10/17 |
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 10.01 |
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 11.02 |
			| 16-18 Levy Audit Adjustments                | 0     | 0     | 12.03 |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 13.04 |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 14.05 |
			| 16-18 Non Levy Excess Learning Support      | 0     | 0     | 15.06 |
			| 16-18 Non Levy Exceptional Learning Support | 0     | 0     | 16.07 |
			| 16-18 Non Levy Audit Adjustments            | 0     | 0     | 17.08 |
			| 16-18 Non Levy Authorised Claims            | 0     | 0     | 18.09 |
			| Adult Levy Excess Learning Support          | 0     | 0     | 20.10 |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 21.11 |
			| Adult Levy Audit Adjustments                | 0     | 0     | 22.12 |
			| Adult Levy Authorised Claims                | 0     | 0     | 23.13 |
			| Adult Non Levy Excess Learning Support      | 0     | 0     | 24.14 |
			| Adult Non Levy Exceptional Learning Support | 0     | 0     | 25.15 |
			| Adult Non Levy Audit Adjustments            | 0     | 0     | 26.16 |
			| Adult Non Levy Authorised Claims            | 0     | 0     | 27.17 | 


	Scenario: 2b. Submission for not all claim lines

		Given the EAS collection period is 09/17
		When the following EAS form is submitted:
			| Type                                        | 08/17 | 09/17 |
			| 16-18 Levy Excess Learning Support          | 10.01 | 0     |
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     |
			| 16-18 Levy Audit Adjustments                | 12.03 | 0     |
			| 16-18 Levy Authorised Claims                | 0     | 0     |
			| 16-18 Levy Authorised Claims                | 14.05 | 0     |
			| 16-18 Non Levy Excess Learning Support      | 0     | 0     |
			| 16-18 Non Levy Exceptional Learning Support | 16.07 | 0     |
			| 16-18 Non Levy Audit Adjustments            | 0     | 0     |
			| 16-18 Non Levy Authorised Claims            | 18.09 | 0     |
			| Adult Levy Excess Learning Support          | 0     | 0     |
			| Adult Levy Exceptional Learning Support     | 21.11 | 0     |
			| Adult Levy Audit Adjustments                | 0     | 0     |
			| Adult Levy Authorised Claims                | 23.13 | 0     |
			| Adult Non Levy Excess Learning Support      | 0     | 0     |
			| Adult Non Levy Exceptional Learning Support | 25.15 | 0     |
			| Adult Non Levy Audit Adjustments            | 0     | 0     |
			| Adult Non Levy Authorised Claims            | 27.17 | 0     | 

		Then the EAS payments are:
			| Type                                        | 08/17 | 09/17 | 10/17 |
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 10.01 |
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 0     |
			| 16-18 Levy Audit Adjustments                | 0     | 0     | 12.03 |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 0     |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 14.05 |
			| 16-18 Non Levy Excess Learning Support      | 0     | 0     | 0     |
			| 16-18 Non Levy Exceptional Learning Support | 0     | 0     | 16.07 |
			| 16-18 Non Levy Audit Adjustments            | 0     | 0     | 0     |
			| 16-18 Non Levy Authorised Claims            | 0     | 0     | 18.09 |
			| Adult Levy Excess Learning Support          | 0     | 0     | 0     |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 21.11 |
			| Adult Levy Audit Adjustments                | 0     | 0     | 0     |
			| Adult Levy Authorised Claims                | 0     | 0     | 23.13 |
			| Adult Non Levy Excess Learning Support      | 0     | 0     | 0     |
			| Adult Non Levy Exceptional Learning Support | 0     | 0     | 25.15 |
			| Adult Non Levy Audit Adjustments            | 0     | 0     | 0     |
			| Adult Non Levy Authorised Claims            | 0     | 0     | 27.17 | 


	# Feature: 3.SUBMISSION FOR ALL MONTHS - NO PREVIOUS SUBMISSIONS
	#3a. Submission for every claim line
	#3b. Submission for mixture of claim lines (some entered some not)


	Scenario: 3a. Submission for all claim lines

		Given the EAS collection period is 09/17
		When the following EAS form is submitted:
			| Type                                        | 08/17 | 09/17 |
			| 16-18 Levy Excess Learning Support          | 10.01 | 28.18 |
			| 16-18 Levy Exceptional Learning Support     | 11.02 | 29.19 |
			| 16-18 Levy Audit Adjustments                | 12.03 | 30.20 |
			| 16-18 Levy Authorised Claims                | 13.04 | 31.21 |
			| 16-18 Levy Authorised Claims                | 14.05 | 32.22 |
			| 16-18 Non Levy Excess Learning Support      | 15.06 | 33.23 |
			| 16-18 Non Levy Exceptional Learning Support | 16.07 | 34.23 |
			| 16-18 Non Levy Audit Adjustments            | 17.08 | 35.24 |
			| 16-18 Non Levy Authorised Claims            | 18.09 | 36.25 |
			| Adult Levy Excess Learning Support          | 20.10 | 37.26 |
			| Adult Levy Exceptional Learning Support     | 21.11 | 38.27 |
			| Adult Levy Audit Adjustments                | 22.12 | 39.28 |
			| Adult Levy Authorised Claims                | 23.13 | 40.29 |
			| Adult Non Levy Excess Learning Support      | 24.14 | 41.30 |
			| Adult Non Levy Exceptional Learning Support | 25.15 | 42.31 |
			| Adult Non Levy Audit Adjustments            | 26.16 | 43.32 |
			| Adult Non Levy Authorised Claims            | 27.17 | 44.33 | 
		
		Then the EAS payments are:
			| Type                                        | 08/17 | 09/17 | 10/17 |
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 38.19 |
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 40.21 |
			| 16-18 Levy Audit Adjustments                | 0     | 0     | 42.23 |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 44.25 |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 46.27 |
			| 16-18 Non Levy Excess Learning Support      | 0     | 0     | 48.29 |
			| 16-18 Non Levy Exceptional Learning Support | 0     | 0     | 50.3  |
			| 16-18 Non Levy Audit Adjustments            | 0     | 0     | 52.32 |
			| 16-18 Non Levy Authorised Claims            | 0     | 0     | 54.34 |
			| Adult Levy Excess Learning Support          | 0     | 0     | 57.36 |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 59.38 |
			| Adult Levy Audit Adjustments                | 0     | 0     | 61.40 |
			| Adult Levy Authorised Claims                | 0     | 0     | 63.42 |
			| Adult Non Levy Excess Learning Support      | 0     | 0     | 65.44 |
			| Adult Non Levy Exceptional Learning Support | 0     | 0     | 67.46 |
			| Adult Non Levy Audit Adjustments            | 0     | 0     | 69.48 |
			| Adult Non Levy Authorised Claims            | 0     | 0     | 71.50 |


	Scenario: 3b.  Submission for mixture of claim lines (some entered some not)

		Given the EAS collection period is 09/17
		When the following EAS form is submitted:
			| Type                                        | 08/17 | 09/17 |
			| 16-18 Levy Excess Learning Support          | 10.01 | 0     |
			| 16-18 Levy Exceptional Learning Support     | 0     | 29.19 |
			| 16-18 Levy Audit Adjustments                | 12.03 | 0     |
			| 16-18 Levy Authorised Claims                | 0     | 31.21 |
			| 16-18 Levy Authorised Claims                | 14.05 | 0     |
			| 16-18 Non Levy Excess Learning Support      | 0     | 33.23 |
			| 16-18 Non Levy Exceptional Learning Support | 16.07 | 0     |
			| 16-18 Non Levy Audit Adjustments            | 0     | 35.25 |
			| 16-18 Non Levy Authorised Claims            | 18.09 | 0     |
			| Adult Levy Excess Learning Support          | 0     | 37.27 |
			| Adult Levy Exceptional Learning Support     | 21.11 | 0     |
			| Adult Levy Audit Adjustments                | 0     | 39.29 |
			| Adult Levy Authorised Claims                | 23.13 | 0     |
			| Adult Non Levy Excess Learning Support      | 0     | 41.31 |
			| Adult Non Levy Exceptional Learning Support | 25.15 | 0     |
			| Adult Non Levy Audit Adjustments            | 0     | 43.33 |
			| Adult Non Levy Authorised Claims            | 27.17 | 0     | 

		Then the EAS payments are:
			| Type                                        | 08/17 | 09/17 | 10/17 |
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 10.01 |
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 29.19 |
			| 16-18 Levy Audit Adjustments                | 0     | 0     | 12.03 |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 31.21 |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 14.05 |
			| 16-18 Non Levy Excess Learning Support      | 0     | 0     | 33.23 |
			| 16-18 Non Levy Exceptional Learning Support | 0     | 0     | 16.07 |
			| 16-18 Non Levy Audit Adjustments            | 0     | 0     | 35.25 |
			| 16-18 Non Levy Authorised Claims            | 0     | 0     | 18.09 |
			| Adult Levy Excess Learning Support          | 0     | 0     | 37.27 |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 21.11 |
			| Adult Levy Audit Adjustments                | 0     | 0     | 39.29 |
			| Adult Levy Authorised Claims                | 0     | 0     | 23.13 |
			| Adult Non Levy Excess Learning Support      | 0     | 0     | 41.31 |
			| Adult Non Levy Exceptional Learning Support | 0     | 0     | 25.15 |
			| Adult Non Levy Audit Adjustments            | 0     | 0     | 43.33 |
			| Adult Non Levy Authorised Claims            | 0     | 0     | 27.17 |



	#Feature: 4. SUBMISSION FOR A PREVIOUS MONTH - UPDATE TO PREVIOUS SUBMISSIONS
	#4a. Submission for every claim line

	Scenario: 4a. Submission for all claim lines

		Given the EAS collection period is 10/17
		And the following EAS form is submitted in 09/17:
			| Type                                        | 08/17 | 09/17 |
			| 16-18 Levy Excess Learning Support          | 10.01 | 28.18 |
			| 16-18 Levy Exceptional Learning Support     | 11.02 | 29.19 |
			| 16-18 Levy Audit Adjustments                | 12.03 | 30.20 |
			| 16-18 Levy Authorised Claims                | 13.04 | 31.21 |
			| 16-18 Levy Authorised Claims                | 14.05 | 32.22 |
			| 16-18 Non Levy Excess Learning Support      | 15.06 | 33.23 |
			| 16-18 Non Levy Exceptional Learning Support | 16.07 | 34.23 |
			| 16-18 Non Levy Audit Adjustments            | 17.08 | 35.24 |
			| 16-18 Non Levy Authorised Claims            | 18.09 | 36.25 |
			| Adult Levy Excess Learning Support          | 20.10 | 37.26 |
			| Adult Levy Exceptional Learning Support     | 21.11 | 38.27 |
			| Adult Levy Audit Adjustments                | 22.12 | 39.28 |
			| Adult Levy Authorised Claims                | 23.13 | 40.29 |
			| Adult Non Levy Excess Learning Support      | 24.14 | 41.30 |
			| Adult Non Levy Exceptional Learning Support | 25.15 | 42.31 |
			| Adult Non Levy Audit Adjustments            | 26.16 | 43.32 |
			| Adult Non Levy Authorised Claims            | 27.17 | 44.33 | 

		When the following EAS form is submitted:
			| Type                                        | 08/17 | 09/17 | 10/17 |
			| 16-18 Levy Exceptional Learning Support     | 0.01  | 28.18 | 0     |
			| 16-18 Levy Audit Adjustments                | 21.02 | 29.19 | 0     |
			| 16-18 Levy Excess Learning Support          | 2.03  | 30.20 | 0     |
			| 16-18 Levy Authorised Claims                | 3.04  | 31.21 | 0     |
			| 16-18 Levy Authorised Claims                | 24.05 | 32.22 | 0     |
			| 16-18 Non Levy Excess Learning Support      | 25.06 | 33.23 | 0     |
			| 16-18 Non Levy Exceptional Learning Support | 26.07 | 34.23 | 0     |
			| 16-18 Non Levy Audit Adjustments            | 27.08 | 35.24 | 0     |
			| 16-18 Non Levy Authorised Claims            | 18.09 | 46.25 | 0     |
			| Adult Levy Excess Learning Support          | 20.10 | 47.26 | 0     |
			| Adult Levy Exceptional Learning Support     | 21.11 | 48.27 | 0     |
			| Adult Levy Audit Adjustments                | 22.12 | 49.28 | 0     |
			| Adult Levy Authorised Claims                | 13.13 | 30.29 | 0     |
			| Adult Non Levy Excess Learning Support      | 14.14 | 31.30 | 0     |
			| Adult Non Levy Exceptional Learning Support | 15.15 | 32.31 | 0     |
			| Adult Non Levy Audit Adjustments            | 16.16 | 33.32 | 0     |
			| Adult Non Levy Authorised Claims            | 17.17 | 34.33 | 0     |
		
		Then the EAS payments are:
			| Type                                        | 08/17 | 09/17 | 10/17 | 11/17 |
			| 16-18 Levy Excess Learning Support          | 0     | 0     | 38.19 | -10   |
			| 16-18 Levy Exceptional Learning Support     | 0     | 0     | 40.21 | 10    |
			| 16-18 Levy Audit Adjustments                | 0     | 0     | 42.23 | -10   |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 44.25 | -10   |
			| 16-18 Levy Authorised Claims                | 0     | 0     | 46.27 | 10    |
			| 16-18 Non Levy Excess Learning Support      | 0     | 0     | 48.29 | 10    |
			| 16-18 Non Levy Exceptional Learning Support | 0     | 0     | 50.30 | 10    |
			| 16-18 Non Levy Audit Adjustments            | 0     | 0     | 52.32 | 10    |
			| 16-18 Non Levy Authorised Claims            | 0     | 0     | 54.34 | 10    |
			| Adult Levy Excess Learning Support          | 0     | 0     | 57.36 | 10    |
			| Adult Levy Exceptional Learning Support     | 0     | 0     | 59.38 | 10    |
			| Adult Levy Audit Adjustments                | 0     | 0     | 61.40 | 10    |
			| Adult Levy Authorised Claims                | 0     | 0     | 63.42 | -20   |
			| Adult Non Levy Excess Learning Support      | 0     | 0     | 65.44 | -20   |
			| Adult Non Levy Exceptional Learning Support | 0     | 0     | 67.46 | -20   |
			| Adult Non Levy Audit Adjustments            | 0     | 0     | 69.48 | -20   |
			| Adult Non Levy Authorised Claims            | 0     | 0     | 71.50 | -20   |