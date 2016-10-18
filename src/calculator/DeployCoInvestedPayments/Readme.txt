-------------------------------------------------------------------------------------
DAS Co Invested Payments Component
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
  - component\SFA.DAS.Payments.Calc.CoInvestedPayments.dll
  - component\SFA.DAS.ProviderPayments.Calc.Common.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\Summarisation.Transient.CoInvestedPayments.DDL.tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\Summarisation.Transient.CoInvestedPayments.DDL.views.sql:
   - transient database views that need to be present when the component is executed
  - sql\ddl\Summarisation.Transient.CoInvestedPayments.DDL.sprocs.sql:
   - transient database stored procedures that need to be present when the component is executed
  - sql\ddl\Summarisation.Deds.CoInvestedPayments.DDL.tables.sql:
   - deds database tables that need to be present when the component is executed
  - sql\dml\PeriodEnd.CoInvestedPayments.Cleanup.Deds.DML.sql:
   - deds database cleanup script that needs to be executed before copying from the transient database to the deds database

 1.3 Copy to deds mapping xml:
  - copy mappings\DasCoInvestedPaymentsCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies co-invested payments results from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das period end connection string

 1.4 Test results:
  - test-results\TestResult.SFA.DAS.Payments.Calc.CoInvestedPayments.xml
  - test-results\TestResult.SFA.DAS.ProviderPayments.Calc.Common.xml
  - test-results\TestResult-Integration.SFA.DAS.Payments.Calc.CoInvestedPayments.xml
   
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
 3.1 Current DC Summarisation Collection: ${ILR_Summarisation.FQ}
 3.2 DAS Period End Collection: ${DAS_PeriodEnd.FQ}

-------------------------------------------------------------------------------------
4. Expected manifest steps for the das period end process - co-invested payments calculator
-------------------------------------------------------------------------------------
 4.1 Build the transient database.
 4.2 Execute the 'DAS Co Invested Payments' component
 4.3 Cleanup the deds co-invested payments results using the 'PeriodEnd.CoInvestedPayments.Cleanup.Deds.DML.sql' sql script
 4.4 Bulk copy the co-invested payments results from transient to deds

