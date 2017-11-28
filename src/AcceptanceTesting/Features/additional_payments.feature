@AdditionalPayments
Feature: 16 to 18 learner incentives, framework uplifts, level 2 english or maths payments
  
   Scenario:AC1- Payment for a 16-18 DAS learner, levy available, incentives earned
    
    Given levy balance > agreed price for all months\
    And the following commitments exist:
        | ULN       | start date | end date   | agreed price | status |
        | learner a | 01/08/2017 | 01/08/2018 | 15000        | active |

    When an ILR file is submitted with the following data:
        | ULN       | learner type             | agreed price | start date | planned end date | actual end date | completion status |
        | learner a | 16-18 programme only DAS | 15000        | 06/08/2017 | 08/08/2018       |                 | continuing        |
      
    Then the provider earnings and payments break down as follows BUILD SERVER TEST Type Suceeds but Provider Earned Total fails:
        | Type                                | 08/17 | 09/17 | 10/17 | 11/17 | 12/17 | ... | 08/18 | 09/18 |
        | Provider Earned Total               | 900  | 1000  | 1000  | 2000  | 1000  | ... | 1000  | 0     |

