using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using IlrGenerator;
using IlrGeneratorApp.Commands;
using IlrGeneratorApp.Services;
using Microsoft.Win32;
using SFA.DAS.Commitments.Api.Types;

namespace IlrGeneratorApp.ViewModels
{
    public class MainViewModel : GeneratorViewModel
    {

        public MainViewModel()
        {
            Learners = new ObservableCollection<LearnerViewModel>();
            Learners.CollectionChanged += OnLearnersChanged;
            AddLearner();
            SelectedLearner = Learners[0];

            NewLearnerCommand = new DelegateCommand(AddLearner);
            DeleteLearnerCommand = new DelegateCommand(RemoveLearner, CanRemoveLearner);
            SaveIlrFileCommand = new DelegateCommand(SaveIlrFile);

            ReferenceDataService.ProviderSelected += ReferenceDataService_ProviderSelected;
            ReferenceDataService.CommitmentSelected += ReferenceDataService_CommitmentSelected;

            Ukprn = 10003867;
        }

        private long _ukprn;
        public long Ukprn
        {
            get { return _ukprn; }
            set
            {
                if (_ukprn != value)
                {
                    _ukprn = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<LearnerViewModel> Learners { get; }

        private LearnerViewModel _selectedLearner;
        public LearnerViewModel SelectedLearner
        {
            get { return _selectedLearner; }
            set
            {
                if (_selectedLearner != value)
                {
                    _selectedLearner = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand NewLearnerCommand { get; }
        public ICommand DeleteLearnerCommand { get; }

        public ICommand SaveIlrFileCommand { get; }



        private void ReferenceDataService_ProviderSelected(object sender, ProviderSelectedEventArgs e)
        {
            Ukprn = e.Ukprn;
        }
        private void ReferenceDataService_CommitmentSelected(object sender, CommitmentSelectedEventArgs e)
        {
            BuildModelFromCommitment(e.Commitment);
        }


        private void AddLearner(object parameter = null)
        {
            Learners.Add(new LearnerViewModel());
        }
        private void OnLearnersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (var i = 0; i < Learners.Count; i++)
            {
                Learners[i].Title = $"Learner {i}";
            }
        }

        private void RemoveLearner(object parameter = null)
        {
            Learners.Remove(SelectedLearner);
        }
        private bool CanRemoveLearner(object parameter = null)
        {
            return SelectedLearner != null;
        }

        private void SaveIlrFile(object parameter = null)
        {
            try
            {
                var dlg = new SaveFileDialog();
                dlg.Filter = "XML Files|*.xml|All Files|*.*";
                var result = dlg.ShowDialog();
                if (!result.HasValue || !result.Value)
                {
                    return;
                }

                var path = dlg.FileName;
                var ilrDocument = GenerateIlrDocument();
                ilrDocument.Save(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private XDocument GenerateIlrDocument()
        {
            var submission = GetIlrSubmission();
            var builder = new IlrBuilder();
            return builder.MakeIlrDocument(submission);
        }
        private IlrSubmission GetIlrSubmission()
        {
            var learners = Learners.Select(ConvertLearner).ToArray();
            return new IlrSubmission
            {
                Ukprn = Ukprn,
                Learners = learners
            };
        }
        private Learner ConvertLearner(LearnerViewModel learnerVm)
        {
            return new Learner
            {
                Uln = learnerVm.Uln,
                LearningDeliveries = new[]
                {
                    new LearningDelivery
                    {
                        StandardCode = learnerVm.StandardCode,
                        FrameworkCode = learnerVm.FrameworkCode,
                        ProgrammeType = learnerVm.ProgrammeType,
                        PathwayCode = learnerVm.PathwayCode,
                        ActualStartDate = learnerVm.StartDate,
                        PlannedEndDate = learnerVm.PlannedEndDate,
                        ActualEndDate = learnerVm.ActualEndDate,
                        TrainingCost = learnerVm.TrainingCost,
                        EndpointAssesmentCost = learnerVm.EndpointAssesmentCost,
                        ActFamCodeValue = learnerVm.ContractStatus
                    }
                }
            };
        }


        private async void BuildModelFromCommitment(CommitmentListItem commitmentHeader)
        {
            var commitment = await CommitmentService.GetProviderCommitment(commitmentHeader.ProviderId.Value, commitmentHeader.Id);

            Learners.Clear();
            foreach (var apprenticeship in commitment.Apprenticeships)
            {
                var learner = new LearnerViewModel
                {
                    Uln = apprenticeship.ULN == null ? 0 : long.Parse(apprenticeship.ULN)
                };

                if (apprenticeship.TrainingType == TrainingType.Standard)
                {
                    learner.StandardCode = long.Parse(apprenticeship.TrainingCode);
                }
                else if (apprenticeship.TrainingType == TrainingType.Framework)
                {
                    var parts = apprenticeship.TrainingCode.Split('-');
                    learner.FrameworkCode = int.Parse(parts[0]);
                    learner.ProgrammeType = int.Parse(parts[1]);
                    learner.PathwayCode = int.Parse(parts[2]);
                }

                if (apprenticeship.StartDate.HasValue)
                {
                    learner.StartDate = apprenticeship.StartDate.Value;
                }
                if (apprenticeship.EndDate.HasValue)
                {
                    learner.PlannedEndDate = apprenticeship.EndDate.Value;
                }

                if (apprenticeship.Cost.HasValue)
                {
                    learner.EndpointAssesmentCost = apprenticeship.Cost.Value * 0.2m;
                    learner.TrainingCost = apprenticeship.Cost.Value - learner.EndpointAssesmentCost;
                }

                Learners.Add(learner);
            }
        }
    }
}
