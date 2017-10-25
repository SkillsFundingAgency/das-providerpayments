using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Microsoft.Azure;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    //TODO: Abstract this to application/domain/infra level
    public class AuthenticationService : IAuthenticationService
    {
        public bool AuthenticateUser(string emailAddress, string password)
        {
            var account = GetAccount(emailAddress);
            if (account == null)
            {
                return false;
            }

            var hashedPassword = HashPassword(password, account.Salt);
            return hashedPassword == account.PasswordHash;
        }

        private AccountEntity GetAccount(string emailAddress)
        {
            using (var connection = new SqlConnection(CloudConfigurationManager.GetSetting("ConnectionString")))
            {
                return connection.Query<AccountEntity>("SELECT * FROM Config.Account WHERE EmailAddress = @emailAddress", new { emailAddress }).SingleOrDefault();
            }
        }
        private string HashPassword(string plainText, string salt)
        {
            var saltedPassword = Convert.FromBase64String(salt).Concat(Encoding.Unicode.GetBytes(plainText)).ToArray();

            HMAC hasher = new HMACSHA256(Convert.FromBase64String(CloudConfigurationManager.GetSetting("PasswordKey")));
            var hash = hasher.ComputeHash(saltedPassword);
            var pbkdf2 = new Rfc2898DeriveBytes(Convert.ToBase64String(hash), Convert.FromBase64String(salt), 1000);
            hash = pbkdf2.GetBytes(hash.Length);
            return Convert.ToBase64String(hash);
        }

        private class AccountEntity
        {
            public long Id { get; set; }
            public string EmailAddress { get; set; }
            public string PasswordHash { get; set; }
            public string Salt { get; set; }
            public bool IsActive { get; set; }
        }
    }
}