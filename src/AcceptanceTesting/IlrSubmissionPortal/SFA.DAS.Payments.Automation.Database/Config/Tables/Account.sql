CREATE TABLE Config.Account
(
	Id				bigint			PRIMARY KEY IDENTITY(1,1),
	EmailAddress	varchar(255)	NOT NULL UNIQUE,
	PasswordHash	varchar(2048)	NOT NULL,
	Salt			varchar(2048)	NOT NULL,
	IsActive		bit				NOT NULL DEFAULT(1)
)
