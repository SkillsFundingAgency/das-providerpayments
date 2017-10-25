-------------------------------------------------------------------------------------
DAS Manual Adjustments Component
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
  - component\SFA.DAS.ProviderPayments.Calc.ManualAdjustments.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\PeriodEnd.Deds.ManualAdjustments.ddl.tables:
   - deds database tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.ManualAdjustments.ddl.tables:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Adjustments.Populate.ManualAdjustments:
   - script to copy data from DEDS manual adjustments table		
 
 1.3 Test results:

  - test-results\TestResult.SFA.DAS.ProviderPayments.Calc.ManualAdjustments.xml
  - test-results\TestResult-Integration.SFA.DAS.ManualAdjustments.Calc.PaymentsDue.xml
   
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
 3.2 Academic year of current ILR Collection: ${YearOfCollection}

-------------------------------------------------------------------------------------
4. Expected manifest steps for the das period end process - payments due
-------------------------------------------------------------------------------------
 4.1 Build the transient database
 4.2 Copy reference data from deds to transient using the provided scripts in the DML folder
 4.3 Execute the 'DAS Manual Adjustments' component
 4.4 Data will be populated in the Requiredpayments and PaymentsDue tables and will be pushed to DEDS
