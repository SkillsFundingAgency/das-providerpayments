-------------------------------------------------------------------------------------
DAS Apprenticeship Earnings Funding Calculation Component
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
 1.1 DLLs:
  - component\CS.Common.External.dll
  - component\Dapper.Contrib.dll
  - component\Dapper.dll
  - component\FastMember.dll
  - component\MediatR.dll
  - component\NLog.dll
  - component\SFA.DAS.CollectionEarnings.Calculator.dll
  - component\SFA.DAS.Payments.DCFS.dll
  - component\SFA.DAS.Payments.DCFS.StructureMap.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\Ilr.Deds.Earnings.DDL.Tables.sql:
   - database tables to store the component's output in the ILR Deds database
  - sql\ddl\Ilr.Transient.Earnings.DDL.Tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\Ilr.Transient.Earnings.DDL.Views.sql:
   - transient database views that need to be present when the component is executed
  - sql\dml\Ilr.Earnings.Cleanup.Deds.DML.sql:
   - deds database cleanup script that needs to be executed before copying from the transient database to the deds database
 
 1.3 Copy to deds mapping xml:
  - copy mappings\DasEarningsCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies earnings results from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds ilr connection string
 
 1.4 Test results:
  - test-results\TestResult.xml
  - test-results\TestResult-Integration.xml
 
-------------------------------------------------------------------------------------
2. Expected context properties
-------------------------------------------------------------------------------------
 2.1 Transient database connection string:
  - key: TransientDatabaseConnectionString
  - value: ILR transient database connection string
 2.2 Log level:
  - key: LogLevel
  - value: one of the following is valid: Fatal, Error, Warn, Info, Debug, Trace, Off
 2.3 ILR Collection Year:
  - key: YearOfCollection
  - value: 4 characters long string representing the academic year of the ILR connection: 1617, 1718, etc.
  
-------------------------------------------------------------------------------------
3. Expected data set keys in the manifest that runs the component
-------------------------------------------------------------------------------------
 3.1 Current ILR Collection: ${ILR_Deds.FQ}

-------------------------------------------------------------------------------------
4. Expected manifest das steps for the ilr submission process
-------------------------------------------------------------------------------------
 4.1 Build the transient database.
 4.2 Execute the 'DAS Apprenticeship Earnings Funding Calculation' component
 4.4 Cleanup the deds earnings results using the 'Ilr.Earnings.Cleanup.Deds.DML.sql' sql script
 4.5 Bulk copy the earnings results from transient to deds

