-------------------------------------------------------------------------------------
DAS Payments Due Component
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
 1.1 SQL scripts:
  - sql\ddl\PeriodEnd.Deds.Payments.ddl.tables.sql:
   - deds database tables required for month end collection
  - sql\dml\01 PeriodEnd.Populate.Payments.Periods.dml.sql:
   - populates deds tables for payments
   
-------------------------------------------------------------------------------------
2. Expected data set keys in the manifest that runs the component
-------------------------------------------------------------------------------------
 2.1 Academic year of current ILR Collection: ${ILR_AcademicYear}
 2.2 Das account collection: ${DAS_Accounts.FQ}
 2.3 Das commitment collection: ${DAS_Commitments.FQ}

-------------------------------------------------------------------------------------
3. Expected manifest steps for the das period end process - month end scripts
-------------------------------------------------------------------------------------
 3.1 Run all other month end steps
 3.2 Run DML scripts in order
