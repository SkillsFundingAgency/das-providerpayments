using System;

namespace SFA.DAS.Payments.Automation.Application
{
    public abstract class ApplicationResponse
    {
        public Exception Error { get; set; }
        public virtual bool IsSuccess => Error == null;
    }
}
