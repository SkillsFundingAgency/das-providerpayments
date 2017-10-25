using System;

namespace SFA.DAS.Payments.DCFS.Application
{
    public abstract class Response
    {
        public bool IsValid { get; set; }
        public Exception Exception { get; set; }
    }
}
