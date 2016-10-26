-------------------------------------------------------------------------------------
DAS Levy Calculator Component
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
 1.1 DLLs:
  - component\CS.Common.External.dll
  - component\Dapper.dll
  - component\FastMember.dll
  - component\MediatR.dll
  - component\NLog.dll
  - component\SFA.DAS.ProviderPayments.Calc.Common.dll
  - component\SFA.DAS.ProviderPayments.Calc.LevyPayments.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\PeriodEnd.Transient.LevyPayments.ddl.tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.LevyPayments.ddl.views.sql:
   - transient database views that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.LevyPayments.ddl.sprocs.sql:
   - transient database stored procedures that need to be present when the component is executed
  
  - sql\ddl\PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.Reference.Accounts.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed
  
  - sql\ddl\PeriodEnd.Deds.LevyPayments.DDL.tables.sql:
   - deds database tables that need to be present when the component is executed
  - sql\dml\PeriodEnd.LevyPayments.Cleanup.Deds.DML.sql:
   - deds database cleanup script that needs to be executed before copying from the transient database to the deds database

  - sql\dml\01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql:
   - populate collection periods mapping reference data (from deds to transient) needed to run levy payments
  - sql\dml\03 PeriodEnd.Populate.Reference.Commitments.dml.sql:
   - populate commitments reference data (from deds to transient) needed to run levy payments
  - sql\dml\04 PeriodEnd.Populate.Reference.Accounts.dml.sql:
   - populate accounts reference data (from deds to transient) needed to run levy payments

 1.3 Copy to deds mapping xml:
  - copy mappings\DasLevyPaymentsCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies levy payments results from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das period end connection string

 1.4 Test results:
  - test-results\TestResult.SFA.DAS.ProviderPayments.Calc.Common.xml
  - test-results\TestResult.SFA.DAS.ProviderPayments.Calc.LevyPayments.xml
  - test-results\TestResult-Integration.SFA.DAS.ProviderPayments.Calc.LevyPayments.xml
   
-------------------------------------------------------------------------------------
2. Expected context properties
-------------------------------------------------------------------------------------
 2.1 Transient database connection string:
  - key: TransientDatabaseConnectionString
  - value: Summarisation transient database connection string
 2.2 Log level:
  - key: LogLevel
  - value: one of the following is valid: Fatal, Error, Warn, Info, Debug, Trace, Off

-------------------------------------------------------------------------------------
3. Expected data set keys
-------------------------------------------------------------------------------------
 3.1 DAS Accounts Reference Data Collection: ${DAS_Accounts.FQ}
 3.2 DAS Commitments Reference Data Collection: ${DAS_Commitments.FQ}
 3.3 Current DC Summarisation Collection: ${ILR_Summarisation.FQ}
 3.4 DAS Period End Collection: ${DAS_PeriodEnd.FQ}

-------------------------------------------------------------------------------------
4. Expected manifest steps for the das period end process - levy payments calculator
-------------------------------------------------------------------------------------
 4.1 Build the transient database.
 4.2 Copy reference data from deds to transient using the provided scripts in the 01 - 04 order
 4.3 Execute the 'DAS Levy Calculator' component
 4.4 Cleanup the deds levy payments results using the 'PeriodEnd.LevyPayments.Cleanup.Deds.DML.sql' sql script
 4.5 Bulk copy the levy payments results from transient to deds
