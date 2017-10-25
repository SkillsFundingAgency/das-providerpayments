using System;
using System.Windows;
using System.Windows.Input;
using IlrGeneratorApp.Commands;
using IlrGeneratorApp.Dialogs;
using SFA.DAS.Commitments.Api.Types;

namespace IlrGeneratorApp.Services
{
    public static class ReferenceDataService
    {
        static ReferenceDataService()
        {
            LookupProvider = new DelegateCommand(ShowProviderLookupDialog);
            LookupCommitment = new DelegateCommand(ShowCommitmentLookupDialog);
        }

        // Events
        public static event EventHandler<ProviderSelectedEventArgs> ProviderSelected;
        public static event EventHandler<CommitmentSelectedEventArgs> CommitmentSelected;

        // Commands
        public static ICommand LookupProvider { get; }
        public static ICommand LookupCommitment { get; }

        // Command handlers
        private static void ShowProviderLookupDialog(object parameter = null)
        {
            OpenDialog<ProviderLookupDialog>();
        }
        private static void ShowCommitmentLookupDialog(object parameter)
        {
            if (!(parameter is long))
            {
                throw new Exception("Need a UKPRN");
            }
            var ukprn = (long) parameter;
            OpenDialog(new CommitmentLookupDialog(ukprn));
        }

        // Event invokers
        public static void NotifyProviderSelected(long ukprn)
        {
            ProviderSelected?.Invoke(null, new ProviderSelectedEventArgs { Ukprn = ukprn });
        }
        public static void NotifyCommitmentSelected(CommitmentListItem commitment)
        {
            CommitmentSelected?.Invoke(null, new CommitmentSelectedEventArgs(commitment));
        }

        // Helper methods
        private static void OpenDialog<T>(T instance = null)
            where T : Window
        {
            var dlg = instance ?? Activator.CreateInstance<T>();
            dlg.Owner = App.Current.MainWindow;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlg.ResizeMode = ResizeMode.NoResize;
            dlg.ShowDialog();
        }
    }
}
