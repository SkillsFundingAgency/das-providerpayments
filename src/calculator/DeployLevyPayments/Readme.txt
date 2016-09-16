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
  - sql\ddl\Summarisation.Transient.LevyPayments.DDL.tables.sql:
   - transient database tables that need to be present when the component is executed
  - Summarisation.Transient.LevyPayments.DDL.views.sql:
   - transient database views that need to be present when the component is executed
  - Summarisation.Transient.LevyPayments.DDL.sprocs.sql:
   - transient database stored procedures that need to be present when the component is executed

 1.3 Test results:
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
 3.3 Current ILR Collection: ${ILR_Current.FQ}