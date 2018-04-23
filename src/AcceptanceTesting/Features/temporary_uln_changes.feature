@TemporaryULNChanges
Feature: Where a temporary ULN changes to a proper one, payments should align

Scenario:902-AC01 - Non-Levy apprentice, provider changes ULN value in ILR after payments have already occurred

        Given the apprenticeship funding band maximum is 9000
 	
		When an ILR file is submitted for period R01 with the following data:
		| learner reference number | Employer   | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
		| 123                      | employer 0 | 999999999 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
		| 123                      | employer 0 | 999999999 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |  

		And an ILR file is submitted for period R03 with the following data:
		| learner reference number | Employer   | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
		| 123                      | employer 0 | 100000000 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
		| 123                      | employer 0 | 100000000 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |  

       Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17  | 09/17  | 10/17  | 11/17  |
			| Provider Earned Total                   | 639.25 | 639.25 | 639.25 | 639.25 |
			| Provider Earned from SFA                | 579.25 | 579.25 | 579.25 | 579.25 |
			| Provider Earned from Employer           | 60     | 60     | 60     | 60     |
			| Provider Paid by SFA                    | 0      | 579.25 | 579.25 | 579.25 |
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

Scenario:902-AC02 - Non-Levy apprentice, provider changes learner reference number in ILR after payments have already occurred

        Given levy balance > agreed price for all months
		And the apprenticeship funding band maximum is 9000

		When an ILR file is submitted for period R01 with the following data:
		| learner reference number | Employer   | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
		| 123                      | employer 0 | 999999999 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
		| 123                      | employer 0 | 999999999 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |  

		And an ILR file is submitted for period R03 with the following data:
		| learner reference number | Employer   | ULN       | learner type           | agreed price | start date | planned end date | actual end date | completion status | aim type         | aim sequence number | aim rate | framework code | programme type | pathway code |
		| 456                      | employer 0 | 999999999 | programme only non-DAS | 9000         | 06/08/2017 | 20/08/2018       |                 | continuing        | programme        | 2                   |          | 403            | 2              | 1            |
		| 456                      | employer 0 | 999999999 | programme only non-DAS |              | 06/08/2017 | 20/08/2018       |                 | continuing        | maths or english | 1                   | 471      | 403            | 2              | 1            |  

       Then the provider earnings and payments break down as follows:
			| Type                                    | 08/17   | 09/17   | 10/17  | 11/17    |
			| Provider Earned Total                   | 1278.50 | 1278.50 | 639.25 | 639.25   |
			| Provider Earned from SFA                | 1158.50 | 1158.50 | 579.25 | 579.25   |
			| Provider Earned from Employer           | 60      | 60      | 60     | 0        |
			| Provider Paid by SFA                    | 0       | 579.25  | 579.25 | 1737.75  |
			| Refund taken by SFA                     | 0       | 0       | 0      | -1158.50 |
			| Payment due from Employer               | 0       | 60      | 60     | 60       |
			| Refund due to employer                  | 0       | 0       | 0      | 120      |
			| Levy account debited                    | 0       | 0       | 0      | 0        |
			| Levy account credited                   | 0       | 0       | 0      | 0        |
			| SFA Levy employer budget                | 0       | 0       | 0      | 0        |
			| SFA Levy co-funding budget              | 0       | 0       | 0      | 0        |
			| SFA Levy additional payments budget     | 0       | 0       | 0      | 0        |
			| SFA non-Levy co-funding budget          | 540     | 540     | 540    | 540      |
			| SFA non-Levy additional payments budget | 39.25   | 39.25   | 39.25  | 39.25    |