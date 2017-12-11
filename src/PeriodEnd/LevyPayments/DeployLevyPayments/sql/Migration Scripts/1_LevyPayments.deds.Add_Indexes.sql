--IF IndexProperty(Object_Id('${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping]'), 'IX_PeriodMapping_YearOfCollection', 'IndexId') IS NULL
--BEGIN
--	CREATE INDEX IX_PeriodMapping_YearOfCollection ON ${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping] (YearOfCollection);
--END
--GO






