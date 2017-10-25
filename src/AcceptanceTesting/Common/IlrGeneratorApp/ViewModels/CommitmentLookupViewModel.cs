using System;
using System.Collections.ObjectModel;
using IlrGeneratorApp.Commands;
using IlrGeneratorApp.Services;
using SFA.DAS.Commitments.Api.Types;

namespace IlrGeneratorApp.ViewModels
{
    public class CommitmentLookupViewModel : GeneratorViewModel
    {
        private readonly Action<bool> _closeCallback;
        private CommitmentListItem _selectedCommitment;
        private bool _isLoading;

        public CommitmentLookupViewModel(long ukprn, Action<bool> closeCallback)
        {
            _closeCallback = closeCallback;
            Ukprn = ukprn;
            Commitments = new ObservableCollection<CommitmentListItem>();

            SelectCommand = new DelegateCommand(SelectCommitment, CanSelectCommitment);
            CancelCommand = new DelegateCommand(Cancel);

            LoadCommitments();
        }


        public long Ukprn { get; }
        public ObservableCollection<CommitmentListItem> Commitments { get; }
        public CommitmentListItem SelectedCommitment
        {
            get { return _selectedCommitment; }
            set
            {
                if (_selectedCommitment != value)
                {
                    _selectedCommitment = value;
                    OnPropertyChanged();
                    SelectCommand.NotifyCanExecuteChanged();
                }
            }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }



        private bool CanSelectCommitment(object parameter = null)
        {
            return SelectedCommitment != null;
        }
        private void SelectCommitment(object parameter = null)
        {
            ReferenceDataService.NotifyCommitmentSelected(SelectedCommitment);
            _closeCallback.Invoke(true);
        }
        private void Cancel(object parameter = null)
        {
            _closeCallback.Invoke(false);
        }

        private async void LoadCommitments()
        {
            try
            {
                IsLoading = true;

                var commitments = await CommitmentService.GetProviderCommitments(Ukprn);

                Commitments.Clear();
                foreach (var commitment in commitments)
                {
                    Commitments.Add(commitment);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
    
}
