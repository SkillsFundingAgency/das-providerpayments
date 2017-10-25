using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Payments.Automation.Infrastructure.PaymentResults;
using SFA.DAS.Provider.Events.Api.Client;

namespace SFA.DAS.Payments.Automation.Infrastructure.Data
{
    public class PaymentsApiClient : IPaymentsClient
    {
        private readonly PaymentsEventsApiConfiguration _config;
        public PaymentsApiClient(string apiBaseUrl, string clientToken)
        {
            _config = new PaymentsEventsApiConfiguration {
                ApiBaseUrl = apiBaseUrl,
                ClientToken = clientToken
            };
        }

        public async Task<List<PaymentResult>> GetPayments(long ukprn)
        {
            var items = new List<PaymentResult>();

            var apiClient = new PaymentsEventsApiClient(_config);
            var result =  await apiClient.GetPayments(ukprn: ukprn).ConfigureAwait(false);
            
            result.Items.ToList().ForEach(x =>
                items.Add(
                    new PaymentResult
                    {
                        Amount = x.Amount,
                        ApprenticeshipId = x.ApprenticeshipId,
                        CollectionPeriodMonth = x.CollectionPeriod.Month,
                        CollectionPeriodYear = x.CollectionPeriod.Year,
                        CalculationPeriod = $"{x.CollectionPeriod.Month:00}/{(x.CollectionPeriod.Year - 2000):00}",
                        DeliveryPeriod = $"{x.DeliveryPeriod.Month:00}/{(x.DeliveryPeriod.Year - 2000):00}",
                        DeliveryMonth = x.DeliveryPeriod.Month,
                        DeliveryYear = x.DeliveryPeriod.Year,
                        EmployerAccountId = x.EmployerAccountId,
                        FundingSource = (FundingSource)x.FundingSource,
                        TransactionType =(TransactionType) x.TransactionType,
                        Ukprn = x.Ukprn,
                        Uln = x.Uln,
                        ContractType =(ContractType) x.ContractType 
                    }
                    )
                );
            
            return items;
        }
    }
}
