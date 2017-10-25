
  -- ALTER for [DataLock].[ValidationError]
  ALTER TABLE [DataLock].[ValidationError]
  Alter column [LearnRefNumber] varchar(12)
  GO

  ALTER TABLE [DataLock].[ValidationError]
  Alter column AimSeqNumber int
  GO

  -- ALTER for DataLock.PriceEpisodeMatch
  ALTER TABLE DataLock.PriceEpisodeMatch
  Alter column [LearnRefNumber] varchar(12)
  GO

  ALTER TABLE DataLock.PriceEpisodeMatch
  Alter column AimSeqNumber int
  GO

  ALTER TABLE DataLock.PriceEpisodeMatch
  Alter column CommitmentId bigint
  GO

  
  -- ALTER for DataLock.PriceEpisodePeriodMatch
  ALTER TABLE DataLock.PriceEpisodePeriodMatch
  Alter column [LearnRefNumber] varchar(12)
  GO


  ALTER TABLE DataLock.PriceEpisodePeriodMatch
  Alter column AimSeqNumber int
  GO