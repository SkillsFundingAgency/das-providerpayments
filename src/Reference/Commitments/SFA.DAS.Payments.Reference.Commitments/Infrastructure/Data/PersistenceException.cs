using System;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data
{
    public class PersistenceException : Exception
    {
        public PersistenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
