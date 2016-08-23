-------------------------------------------------------------------------------------
DAS Levy Calculator Component
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
 1.1 DLLs:
  - component\CS.Common.External.dll
  - component\NLog.dll
  - component\SFA.DAS.ProviderPayments.Calculator.LevyPayments.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\Summarisation.Transient.LevyPayments.DDL.sql:
   - database tables that need to be present when the component is executed
  
-------------------------------------------------------------------------------------
2. Expected context properties
-------------------------------------------------------------------------------------
 2.1 Transient database connection string:
  - key: TransientDatabaseConnectionString
  - value: Summarisation transient database connection string
 2.2 Log level:
  - key: LogLevel
  - value: one of the following is valid: Fatal, Error, Warn, Info, Debug, Trace, Off