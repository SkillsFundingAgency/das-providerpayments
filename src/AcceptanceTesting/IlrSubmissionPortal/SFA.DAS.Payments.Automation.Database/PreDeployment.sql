/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='FrameworkAims' AND TABLE_SCHEMA='Lars')
--    BEGIN
--        TRUNCATE TABLE Lars.FrameworkAims;
--    END
--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='StandardAims' AND TABLE_SCHEMA='Lars')
--    BEGIN
--        TRUNCATE TABLE Lars.StandardAims;
--    END
