using System.Windows;
using IlrGeneratorApp.ViewModels;

namespace IlrGeneratorApp.Dialogs
{
    /// <summary>
    /// Interaction logic for ProviderLookupDialog.xaml
    /// </summary>
    public partial class ProviderLookupDialog : Window
    {
        public ProviderLookupDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ProviderLookupViewModel(Close);
        }

        private void Close(bool success)
        {
            DialogResult = success;
            Close();
        }
    }
}
