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
  - component\NLog.dll
  - component\SFA.DAS.Payments.Calc.CoInvestedPayments.dll
  - component\SFA.DAS.ProviderPayments.Calc.Common.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - Summarisation.Transient.CoInvestedPayments.DDL.tables.sql:
   - transient database tables that need to be present when the component is executed

 1.3 Test results:
  - test-results\TestResult.SFA.DAS.Payments.Calc.CoInvestedPayments.xml
  - test-results\TestResult.SFA.DAS.ProviderPayments.Calc.Common.xml
   
-------------------------------------------------------------------------------------
2. Expected context properties
-------------------------------------------------------------------------------------
 2.1 Transient database connection string:
  - key: TransientDatabaseConnectionString
  - value: Summarisation transient database connection string
 2.2 Log level:
  - key: LogLevel
  - value: one of the following is valid: Fatal, Error, Warn, Info, Debug, Trace, Off
