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
  - component\SFA.DAS.Payments.DCFS.dll
  - component\SFA.DAS.Payments.DCFS.StructureMap.dll
  - component\SFA.DAS.ProviderPayments.Calc.Common.dll
  - component\SFA.DAS.ProviderPayments.Calc.PaymentsDue.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\PeriodEnd.Transient.PaymentsDue.DDL.tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.PaymentsDue.DDL.views.sql:
   - transient database views that need to be present when the component is executed

  - sql\ddl\PeriodEnd.Transient.PaymentsDue.Reference.DDL.tables.sql:
   - transient database reference tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.Reference.Providers.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.Reference.Accounts.ddl.tables.sql:
   - transient database reference tables that need to be present when the component is executed

  - sql\ddl\PeriodEnd.Deds.PaymentsDue.DDL.tables.sql:
   - deds database tables that need to be present when the component is executed
  - sql\dml\PeriodEnd.PaymentsDue.Cleanup.Deds.DML.sql:
   - deds database cleanup script that needs to be executed before copying from the transient database to the deds database

  - sql\dml\01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql:
   - populate collection periods mapping reference data (from deds to transient) needed to run payments due
  - sql\dml\02 PeriodEnd.Populate.Reference.Providers.dml.sql:
   - populate learning providers reference data (from deds to transient) needed to run payments due
  - sql\dml\03 PeriodEnd.Populate.Reference.Commitments.dml.sql:
   - populate commitments reference data (from deds to transient) needed to run levy payments
  - sql\dml\04 PeriodEnd.Populate.Reference.Accounts.dml.sql:
   - populate accounts reference data (from deds to transient) needed to run levy payments
  - sql\dml\05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql:
   - populate apprenticeship earnings reference data (from deds to transient) needed to run payments due
  - sql\dml\06 PeriodEnd.PaymentsDue.Populate.Reference.RequiredPaymentsHistory.dml.sql:
   - populate required payments history reference data (from deds to transient) needed to run payments due
   
 1.3 Copy to deds mapping xml:
  - copy mappings\DasPaymentsDueCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies payments due results from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das period end connection string
 
 1.4 Test results:
  - test-results\TestResult.SFA.DAS.ProviderPayments.Calc.Common.xml
  - test-results\TestResult.SFA.DAS.ProviderPayments.Calc.PaymentsDue.xml
  - test-results\TestResult-Integration.SFA.DAS.ProviderPayments.Calc.PaymentsDue.xml
   
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
3. Expected data set keys / properties in the manifest that runs the component
-------------------------------------------------------------------------------------
 3.1 Current ILR Collection: ${ILR_Deds.FQ}
 3.2 Current DC Summarisation Collection: ${ILR_Summarisation.FQ}
 3.3 DAS Period End Collection: ${DAS_PeriodEnd.FQ}
 3.4 DAS Commitments Reference Data Collection: ${DAS_Commitments.FQ}
 3.5 DAS Accounts Reference Data Collection: ${DAS_Accounts.FQ}
 3.6 Academic year of current ILR Collection: ${YearOfCollection}

-------------------------------------------------------------------------------------
4. Expected manifest steps for the das period end process - payments due
-------------------------------------------------------------------------------------
 4.1 Build the transient database
 4.2 Copy reference data from deds to transient using the provided scripts in the 01 - 06 order
 4.3 Execute the 'DAS Payments Due' component
 4.4 Cleanup the deds payments due results using the 'PeriodEnd.PaymentsDue.Cleanup.Deds.DML.sql' sql script
 4.5 Bulk copy the payments due results from transient to deds
