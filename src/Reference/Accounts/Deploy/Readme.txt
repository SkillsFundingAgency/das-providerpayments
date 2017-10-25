-------------------------------------------------------------------------------------
DAS Accounts Reference Data Component
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
  - component\SFA.DAS.Account.Api.Client.dll
  - component\SFA.DAS.Payments.DCFS.dll
  - component\SFA.DAS.Payments.DCFS.StructureMap.dll
  - component\SFA.DAS.Payments.Reference.Accounts.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\ddl.transient.accounts.tables:
   - transient database tables that need to be present when the component is executed

  - sql\ddl\ddl.deds.accounts.tables.sql:
   - deds database tables
  - sql\dml\dml.accounts.cleanup.deds.sql:
   - deds database cleanup script that needs to be executed before copying from the transient database to the deds database
   
 1.3 Copy to transient mapping xml:
  - copy mappings\DasAccountsCopyToTransientMapping.xml:
   - sql bulk copy binary task configuration file that copies das accounts from deds to transient
   - SourceConnectionString: deds das accounts connection string
   - DestinationConnectionString: transient connection string
  
 1.4 Copy to deds mapping xml:
  - copy mappings\DasAccountsCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies das accounts from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das accounts connection string

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
 2.3 Accounts Api base url:
  - key: DAS.Payments.Accounts.BaseUrl
  - value: base url for the accounts api
 2.4 Accounts Api client id:
  - key: DAS.Payments.Accounts.ClientId
  - value: client id for the accounts api
 2.5 Accounts Api client secret:
  - key: DAS.Payments.Accounts.ClientSecret
  - value: client secret for the accounts api
 2.6 Accounts Api identifier uri:
  - key: DAS.Payments.Accounts.IdentifierUri
  - value: identifier uri for the accounts api
 2.7 Accounts Api tenant:
  - key: DAS.Payments.Accounts.Tenant
  - value: tenant for the accounts api
  
-------------------------------------------------------------------------------------
3. Expected data set keys
-------------------------------------------------------------------------------------
 3.1 DAS Accounts Reference Data Collection: ${DAS_Accounts.FQ}
 
-------------------------------------------------------------------------------------
4. Expected manifest steps for the process that reads the accounts from the api
-------------------------------------------------------------------------------------
 4.1 Build the transient database.
 4.2 Bulk copy accounts from deds to transient
 4.3 Execute the 'DAS Accounts Reference Data Component' component
 4.4 Cleanup the deds accounts database using the 'dml.accounts.cleanup.deds.sql' sql script
 4.5 Bulk copy the accounts from transient to deds
