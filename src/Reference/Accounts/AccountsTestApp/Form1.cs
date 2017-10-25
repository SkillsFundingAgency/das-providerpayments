using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CS.Common.External.Interfaces;

namespace AccountsTestApp
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource _cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            try
            {
                var task = new SFA.DAS.Payments.Reference.Accounts.ImportAccountsTask();
                var context = new TestAppExecutionContext(BaseUrl.Text, ClientId.Text, ClientSecret.Text,
                    IdentifierUri.Text, Tenant.Text);

                task.Execute(context);

                MessageBox.Show("Completed without Error", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }


    public class TestAppExecutionContext : IExternalContext
    {
        public TestAppExecutionContext(string baseUrl, string clientId, string clientSecret, string identifierUri, string tentant)
        {
            Properties = new Dictionary<string, string>
            {
                {SFA.DAS.Payments.DCFS.Context.ContextPropertyKeys.TransientDatabaseConnectionString,"server=.;database=ReferenceAccounts_Transient;trusted_connection=true;"},
                {SFA.DAS.Payments.DCFS.Context.ContextPropertyKeys.LogLevel,"DEBUG"},
                {SFA.DAS.Payments.Reference.Accounts.Context.KnownContextKeys.AccountsApiBaseUrl, baseUrl},
                {SFA.DAS.Payments.Reference.Accounts.Context.KnownContextKeys.AccountsApiClientId, clientId},
                {SFA.DAS.Payments.Reference.Accounts.Context.KnownContextKeys.AccountsApiClientSecret, clientSecret},
                {SFA.DAS.Payments.Reference.Accounts.Context.KnownContextKeys.AccountsApiIdentifierUri, identifierUri},
                {SFA.DAS.Payments.Reference.Accounts.Context.KnownContextKeys.AccountsApiTenant, tentant}
            };
        }
        public IDictionary<string, string> Properties { get; set; }
    }
}
