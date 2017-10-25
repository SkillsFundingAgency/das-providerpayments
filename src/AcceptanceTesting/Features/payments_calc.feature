Feature: Calculate the payments 


Scenario Outline: Payment type breakdown
Given an employer levy balance of <employer levy balance>
When a payment of <due amount> is due
Then the employer levy account is debited by <levy account debit>
And the provider is paid <paid by SFA> by the SFA
And the provider is due <payment due from employer> from the employer

Examples:
| employer levy balance | due amount | levy account debit | paid by SFA | payment due from employer |
| 99999                 | 1000       | 1000               | 0           | 0                         |
| 500                   | 1000       | 500                | 450         | 50                        |
| 0                     | 1000       | 0                  | 900         | 100                       |

