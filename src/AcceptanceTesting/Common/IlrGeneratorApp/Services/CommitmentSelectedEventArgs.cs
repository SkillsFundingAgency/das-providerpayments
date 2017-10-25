using System;
using SFA.DAS.Commitments.Api.Types;

namespace IlrGeneratorApp.Services
{
    public class CommitmentSelectedEventArgs : EventArgs
    {
        public CommitmentSelectedEventArgs(CommitmentListItem commitment)
        {
            Commitment = commitment;
        }
        public CommitmentListItem Commitment { get; }
    }
}