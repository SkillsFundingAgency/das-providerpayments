CREATE TABLE Lars.FrameworkAims
(
	FworkCode				int				NOT NULL,
	ProgType				int				NOT NULL,
	PwayCode				int				NOT NULL,
	LearnAimRef				varchar(8)		NOT NULL,
	EffectiveFrom			date			NOT NULL,
	EffectiveTo				date			NULL,
	FrameworkComponentType	int				NULL,
	Created_On				datetime		NOT NULL,
	Created_By				varchar(100)	NOT NULL,
	Modified_On				datetime		NOT NULL,
	Modified_By				varchar(100)	NOT NULL,
	IsCommonComponent		bit				NOT NULL DEFAULT(0),
	IsEnglishAndMathsAim	bit				NOT NULL DEFAULT(0),
	CONSTRAINT [PK_Lars_FrameworkAims] PRIMARY KEY CLUSTERED 
	(
		ProgType,
		FworkCode,
		PwayCode,
		LearnAimRef,
		EffectiveFrom
	)
)
