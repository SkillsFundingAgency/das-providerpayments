-------------------------------------------------------------------------------------
DAS Data Lock Component - DAS Period End
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
    
 1.3 Copy to deds mapping xml:
  - copy mappings\DasPeriodEndDataLockCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies data lock results (validation errors and found matches) from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das period end connection string
 
 1.4 Test results:
  - test-results\TestResult.xml
  - test-results\TestResult-Integration.xml
 
 1.5 Validation messages CSV file
  - DataLock.ValidationMessages.csv
  
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
3. Expected data set keys / properties in the manifest that runs the component
-------------------------------------------------------------------------------------
 3.1 Current ILR Collection: ${ILR_Deds.FQ}
 3.1 Current ILR Summarisation Collection: ${ILR_Summarisation.FQ}
 3.3 DAS Period End Collection: ${DAS_PeriodEnd.FQ}
 3.4 DAS Commitments Reference Data Collection: ${DAS_Commitments.FQ}
 3.5 Academic year of current ILR Collection: ${YearOfCollection}

-------------------------------------------------------------------------------------
4. Expected manifest steps for the das period end process - data lock period end
-------------------------------------------------------------------------------------
 4.1 Build the transient database.
 4.2 Copy reference data from deds to transient using the provided scripts in the 01 - 05 order
 4.3 Execute the 'DAS Data Lock Component - DAS Period End' component
 4.4 Cleanup the deds data lock results using the 'PeriodEnd.DataLock.Cleanup.Deds.DML.sql' sql script
 4.5 Bulk copy the data lock results from transient to deds
