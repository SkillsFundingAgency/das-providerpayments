using System;
using System.Collections.ObjectModel;
using IlrGeneratorApp.Commands;
using IlrGeneratorApp.DataSources;
using IlrGeneratorApp.DataSources.Provider;
using IlrGeneratorApp.Services;

namespace IlrGeneratorApp.ViewModels
{
    public class ProviderLookupViewModel : GeneratorViewModel
    {
        private readonly Action<bool> _closeCallback;

        private string _searchCriteria;
        private IProviderDataSource _selectedDataSource;
        private Provider _selectedProvider;

        public ProviderLookupViewModel(Action<bool> closeCallback)
        {
            _closeCallback = closeCallback;

            DataSources = new ObservableCollection<IProviderDataSource>(DataSourceFactory.GetProviderDataSources());
            SelectedDataSource = DataSources[0];
            SearchCommand = new DelegateCommand(SearchForProvider);
            Providers = new ObservableCollection<Provider>();
            SelectProviderCommand = new DelegateCommand(SelectProvider, CanSelectProvider);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public string SearchCriteria
        {
            get { return _searchCriteria; }
            set
            {
                if (_searchCriteria != value)
                {
                    _searchCriteria = value;
                    OnPropertyChanged();
                }
            }
        }
        public DelegateCommand SearchCommand { get; }
        public ObservableCollection<IProviderDataSource> DataSources { get; }
        public IProviderDataSource SelectedDataSource
        {
            get { return _selectedDataSource; }
            set
            {
                if (_selectedDataSource != value)
                {
                    _selectedDataSource = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<Provider> Providers { get; }
        public Provider SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                if (_selectedProvider != value)
                {
                    _selectedProvider = value;
                    OnPropertyChanged();

                    SelectProviderCommand.NotifyCanExecuteChanged();
                }
            }
        }
        public DelegateCommand SelectProviderCommand { get; }
        public DelegateCommand CancelCommand { get; }


        private void SearchForProvider(object parameter = null)
        {
            Providers.Clear();
            foreach (var provider in SelectedDataSource.SearchForProvider(SearchCriteria))
            {
                Providers.Add(provider);
            }
        }

        private void SelectProvider(object parameter = null)
        {
            ReferenceDataService.NotifyProviderSelected(SelectedProvider.Ukprn);
            _closeCallback.Invoke(true);
        }
        private bool CanSelectProvider(object parameter = null)
        {
            return SelectedProvider != null;
        }

        private void Cancel(object parameter = null)
        {
            _closeCallback.Invoke(false);
        }
    }
}
