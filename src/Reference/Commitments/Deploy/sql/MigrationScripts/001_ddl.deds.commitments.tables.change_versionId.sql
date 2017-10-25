

Declare @constraintName varchar(4000)
SELECT @constraintname = name
FROM   sys.key_constraints
WHERE  [type] = 'PK'
AND parent_object_id = OBJECT_ID ('DasCommitments')

If @constraintName Is Not NULL
begin
	Declare @sql varchar(4000) = 'Alter Table dbo.DasCommitments drop ' + @constraintName
	Exec(@sql)
end


ALTER TABLE [dbo].[DasCommitments]
ALTER COLUMN VersionId varchar(25) NOT NULL


ALTER TABLE [dbo].[DasCommitments]
ADD CONSTRAINT PK_Commitments_CommitmentId_VersionId PRIMARY KEY (CommitmentId,VersionId)

