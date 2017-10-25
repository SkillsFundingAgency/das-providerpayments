CREATE TABLE Lars.StandardAims
(
	StandardCode			int				NOT NULL,
	LearnAimRef				varchar(8)		NOT NULL,
	EffectiveFrom			date			NOT NULL,
	EffectiveTo				date			NULL,
	StandardComponentType	int				NULL,
	Created_On				datetime		NOT NULL,
	Created_By				varchar(100)	NOT NULL,
	Modified_On				datetime		NOT NULL,
	Modified_By				varchar(100)	NOT NULL,
	IsCommonComponent		bit				NOT NULL DEFAULT(0),
	IsEnglishAndMathsAim	bit				NOT NULL DEFAULT(0),
	CONSTRAINT [PK_Lars_StandardAims] PRIMARY KEY CLUSTERED 
	(
		StandardCode,
		LearnAimRef,
		EffectiveFrom
	)
)
