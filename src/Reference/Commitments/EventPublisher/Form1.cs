using System;
using System.Configuration;
using System.Windows.Forms;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Client.Configuration;
using SFA.DAS.Events.Api.Types;

namespace EventPublisher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                var trainingType = StandardInput.Value > 0 ? TrainingTypes.Standard : TrainingTypes.Framework;
                var trainingId = trainingType == TrainingTypes.Standard
                    ? StandardInput.Value.ToString()
                    : $"{FrameworkInput.Value}-{ProgrammeInput.Value}-{PathwayInput.Value}";

                var config = new EventApiConfiguration();
                var client = new EventsApi(config);

                await client.CreateApprenticeshipEvent(new SFA.DAS.Events.Api.Types.ApprenticeshipEvent
                {
                    ApprenticeshipId = (long)ApprenticeshipIdInput.Value,
                    ProviderId = ProviderInput.Value.ToString(),
                    LearnerId = LearnerInput.Value.ToString(),
                    EmployerAccountId = AccountInput.Text,
                    TrainingStartDate = StartDatePicker.Value,
                    TrainingEndDate = EndDatePicker.Value,
                    TrainingId = trainingId,
                    TrainingType = trainingType,
                    TrainingTotalCost = CostInput.Value,
                    AgreementStatus = AgreementStatus.BothAgreed,
                    PaymentStatus = PaymentStatus.Active,
                    PaymentOrder = Int32.Parse(PriorityInput.Text),
                    Event = "NA"
                });

                MessageBox.Show("Done", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    internal class EventApiConfiguration : IEventsApiClientConfiguration
    {
        public EventApiConfiguration()
        {
            BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            ClientToken = ConfigurationManager.AppSettings["ClientToken"];
        }
        public string BaseUrl { get; set; }
        public string ClientToken { get; set; }
    }
}
