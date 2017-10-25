using System.Configuration;
using SFA.DAS.Commitments.Api.Client.Configuration;

namespace IlrGeneratorApp.Services
{
    public class CommitmentsApiClientConfiguration : ICommitmentsApiClientConfiguration
    {
        public CommitmentsApiClientConfiguration()
        {
            BaseUrl = ConfigurationManager.AppSettings["Commitments:BaseUrl"];
            ClientToken = ConfigurationManager.AppSettings["Commitments:ClientToken"];
        }
        public string BaseUrl { get; set; }
        public string ClientToken { get; set; }
    }
}