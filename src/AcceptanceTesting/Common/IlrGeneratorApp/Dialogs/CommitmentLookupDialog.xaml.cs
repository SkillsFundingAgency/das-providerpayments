using System.Windows;
using IlrGeneratorApp.ViewModels;

namespace IlrGeneratorApp.Dialogs
{
    /// <summary>
    /// Interaction logic for CommitmentLookupDialog.xaml
    /// </summary>
    public partial class CommitmentLookupDialog : Window
    {
        public CommitmentLookupDialog(long ukprn)
        {
            InitializeComponent();

            DataContext = new CommitmentLookupViewModel(ukprn, Close);
        }

        private void Close(bool success)
        {
            DialogResult = success;
            Close();
        }

    }
}
