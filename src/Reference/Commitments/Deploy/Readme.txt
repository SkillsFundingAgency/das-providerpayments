-------------------------------------------------------------------------------------
DAS Commitments Reference Data Component
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
 1.1 DLLs:
  - component\CS.Common.External.dll
  - component\Dapper.dll
  - component\FastMember.dll
  - component\MediatR.dll
  - component\Newtonsoft.Json.dll
  - component\NLog.dll
  - component\SFA.DAS.Events.Api.Client.dll
  - component\SFA.DAS.Events.Api.Types.dll
  - component\SFA.DAS.Payments.DCFS.dll
  - component\SFA.DAS.Payments.DCFS.StructureMap.dll
  - component\SFA.DAS.Payments.Reference.Commitments.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\ddl.transient.commitments.tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\ddl.transient.commitments.views.sql:
   - transient database views that need to be present when the component is executed
  - sql\ddl\ddl.deds.commitments.tables.sql:
   - deds database tables
  - sql\ddl\ddl.deds.commitments.views.sql:
   - deds database views
  - sql\dml\dml.commitments.cleanup.deds.sql:
   - deds database cleanup script that needs to be executed before copying from the transient database to the deds database
 
 1.3 Copy to transient mapping xml:
  - copy mappings\DasCommitmentsCopyToTransientMapping.xml:
   - sql bulk copy binary task configuration file that copies das commitments from deds to transient
   - SourceConnectionString: deds das commitments connection string
   - DestinationConnectionString: transient connection string
  
 1.4 Copy to deds mapping xml:
  - copy mappings\DasCommitmentsCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies das commitments from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das commitments connection string
 
 1.5 Test results:
  - test-results\TestResult.xml
 
-------------------------------------------------------------------------------------
2. Expected context properties
-------------------------------------------------------------------------------------
 2.1 Transient database connection string:
  - key: TransientDatabaseConnectionString
  - value: ILR transient database connection string
 2.2 Log level:
  - key: LogLevel
  - value: one of the following is valid: Fatal, Error, Warn, Info, Debug, Trace, Off
 2.3 Commitment Api base url:
  - key: DAS.Payments.Commitments.BaseUrl
  - value: base url for the commitments api
 2.4 Commitment Api client token:
  - key: DAS.Payments.Commitments.ClientToken
  - value: client token for the commitments api

-------------------------------------------------------------------------------------
3. Expected data set keys
-------------------------------------------------------------------------------------
 3.1 DAS Commitments Reference Data Collection: ${DAS_Commitments.FQ}
 
-------------------------------------------------------------------------------------
4. Expected manifest steps for the process that reads the commitments from the api
-------------------------------------------------------------------------------------
 4.1 Build the transient database.
 4.2 Bulk copy commitments from deds to transient
 4.3 Execute the 'DAS Commitments Reference Data Component' component
 4.4 Cleanup the deds commitments database using the 'dml.commitments.cleanup.deds.sql' sql script
 4.5 Bulk copy the commitments from transient to deds
 4.6 If one or more records are found in [dbo].[ProcessError], then fail the process with the error details from those records
