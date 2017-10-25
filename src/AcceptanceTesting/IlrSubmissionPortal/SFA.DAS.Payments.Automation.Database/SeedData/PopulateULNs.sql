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

If NOT Exists (Select top 1 * from [Learners].[ULNs])
BEGIN
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2500,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2501,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2502,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2503,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2504,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2505,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2506,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2507,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2508,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2509,0)
	INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2510,0)

	DECLARE @Counter int = 0
	WHILE @Counter < 5000
		BEGIN
			INSERT INTO [Learners].[ULNs]([ULN],[Used]) VALUES (2511 + @Counter,0)

			SET @Counter = @Counter + 1
		END
END