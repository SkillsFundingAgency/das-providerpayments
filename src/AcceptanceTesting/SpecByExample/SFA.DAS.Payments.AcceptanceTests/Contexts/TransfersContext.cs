using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Contexts
{
    public class TransfersContext
    {
        public TransfersContext()
        {
            TransfersBreakdown = new TransfersBreakdown();
        }

        public TransfersBreakdown TransfersBreakdown { get; set; }
    }
}