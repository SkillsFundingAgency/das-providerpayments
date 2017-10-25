-------------------------------------------------------------------------------------
DAS Provider Adjustments Calculator Component
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
  - component\SFA.DAS.Payments.Calc.ProviderAdjustments.dll
  - component\SFA.DAS.Payments.DCFS.dll
  - component\SFA.DAS.Payments.DCFS.StructureMap.dll
  - component\SFA.DAS.ProviderPayments.Calc.Common.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\PeriodEnd.Transient.ProviderAdjustments.ddl.tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.ProviderAdjustments.ddl.views.sql:
   - transient database views that need to be present when the component is executed

  - sql\ddl\PeriodEnd.Transient.ProviderAdjustments.Reference.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed

  - sql\ddl\PeriodEnd.Deds.ProviderAdjustments.ddl.tables.sql:
   - deds database tables that need to be present when the component is executed

  - sql\dml\01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql:
   - populate collection periods mapping reference data (from deds to transient) needed to run the component
  - sql\dml\05 PeriodEnd.ProviderAdjustments.Populate.Reference.Providers.dml.sql:
   - populate learning providers reference data (from deds to transient) needed to run the component
  - sql\dml\06 PeriodEnd.ProviderAdjustments.Populate.Reference.Current.dml.sql:
   - populate current provider adjustments (EAS) data (from deds to transient) needed to run the component
  - sql\dml\07 PeriodEnd.ProviderAdjustments.Populate.Reference.History.dml.sql:
   - populate previous provider adjustments payments (from deds to transient) needed to run the component
   
 1.3 Copy to deds mapping xml:
  - copy mappings\DasProviderAdjustmentsCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies provider adjustments payments results from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das period end connection string

 1.4 Test results:
  - test-results\TestResult.SFA.DAS.ProviderPayments.Calc.Common.xml
  - test-results\TestResult.SFA.DAS.Payments.Calc.ProviderAdjustments.xml
  - test-results\TestResult-Integration.SFA.DAS.Payments.Calc.ProviderAdjustments.xml

-------------------------------------------------------------------------------------
2. Expected context properties
-------------------------------------------------------------------------------------
 2.1 Transient database connection string:
  - key: TransientDatabaseConnectionString
  - value: Summarisation transient database connection string
 2.2 Log level:
  - key: LogLevel
  - value: one of the following is valid: Fatal, Error, Warn, Info, Debug, Trace, Off
 2.3 ILR Collection Year:
  - key: YearOfCollection
  - value: 4 characters long string representing the academic year of the ILR connection: 1617, 1718, etc.

-------------------------------------------------------------------------------------
3. Expected data set keys / properties in the manifest that runs the component
-------------------------------------------------------------------------------------
 3.1 Current EAS Collection: ${EAS_Deds.FQ}
 3.2 Current DC Summarisation Collection: ${ILR_Summarisation.FQ}
 3.3 DAS Period End Collection: ${DAS_PeriodEnd.FQ}

-------------------------------------------------------------------------------------
4. Expected manifest steps for the das period end process - payments due
-------------------------------------------------------------------------------------
 4.1 Build the transient database
 4.2 Copy reference data from deds to transient using the provided scripts in the 01 - 07 order
 4.3 Execute the 'DAS rovider Adjustments Calculator' component
 4.4 Bulk copy the results from transient to deds
