using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments
{
    public class TransfersProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ContextWrapper _context;

        public TransfersProcessor(ILogger logger, IMediator mediator, ContextWrapper context)
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }

        protected TransfersProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Transfers Processor.");

            // Get a list of accounts with transfers

            // In parallel, process each sending employer

            _logger.Info("Finished Transfers Processor.");
        }

        class TransferPaymentSet
        {
            public void AddPayment(RequiredTransferPayment requiredPayment, decimal amount)
            {
                var payment = new TransferLevyPayment(requiredPayment, amount);
                var transfer = new AccountLevyTransfer(requiredPayment, amount);

                TransferLevyPayments.Add(payment);
                AccountLevyTransfers.Add(transfer);
            }

            private List<TransferLevyPayment> TransferLevyPayments { get; } = new List<TransferLevyPayment>();
            private List<AccountLevyTransfer> AccountLevyTransfers { get; } = new List<AccountLevyTransfer>();


            public IReadOnlyList<TransferLevyPayment> TransferPayments
            {
                get { return TransferLevyPayments; }
            }

            public IReadOnlyList<AccountLevyTransfer> AccountTransfers
            {
                get { return AccountLevyTransfers; }
            }
        }

        TransferPaymentSet ProcessSendingAccount(DasAccount account)
        {
            var result = new TransferPaymentSet();
            


            return result;
        }
    }
}
