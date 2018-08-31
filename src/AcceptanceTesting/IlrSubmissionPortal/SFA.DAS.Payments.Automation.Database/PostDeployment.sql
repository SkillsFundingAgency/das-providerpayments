/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/*:r .\SeedData\Config.Account.sql
:r .\SeedData\Lars.FrameworkAims.sql
:r .\SeedData\PopulateULNs.sql
:r .\SeedData\Lars.StandardAims.sql */


IF '$(DeployMode)'='Release'
	BEGIN
		DELETE FROM Config.Account WHERE EmailAddress='dev@dev.local'
	END
