IF NOT EXISTS(SELECT Id FROM Config.Account WHERE EmailAddress = 'dev@dev.local')
	BEGIN
		INSERT INTO Config.Account
		(EmailAddress, PasswordHash, Salt, IsActive)
		VALUES
		('dev@dev.local', '6ZzgMsxqrCAg66xW1LojIiCkSpn7XS7BznoDkHqwTQI=', 'b3dHanhQR3lYJi1zYSxnJEBxOTVxcMKjbTU1ZUJFQA==', 1)
	END
