using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Commitments.Api.Types;

namespace IlrGeneratorApp.Services
{
    public static class CommitmentService
    {
        public static async Task<List<CommitmentListItem>> GetProviderCommitments(long ukprn)
        {
            var client = new SFA.DAS.Commitments.Api.Client.CommitmentsApi(new CommitmentsApiClientConfiguration());
            return await client.GetProviderCommitments(ukprn);
        }
        public static async Task<Commitment> GetProviderCommitment(long ukprn, long commitmentId)
        {
            var client = new SFA.DAS.Commitments.Api.Client.CommitmentsApi(new CommitmentsApiClientConfiguration());
            return await client.GetProviderCommitment(ukprn, commitmentId);
        }
    }
}
