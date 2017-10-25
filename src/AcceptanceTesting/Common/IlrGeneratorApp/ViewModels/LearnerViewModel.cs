using System;

namespace IlrGeneratorApp.ViewModels
{
    public class LearnerViewModel : GeneratorViewModel
    {
        public LearnerViewModel()
        {
            StartDate = new DateTime(2017, 4, 5);
            PlannedEndDate = new DateTime(2018, 4, 20);
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private long _uln;
        public long Uln
        {
            get { return _uln; }
            set
            {
                if (_uln != value)
                {
                    _uln = value;
                    OnPropertyChanged();
                }
            }
        }

        private long _standardCode;
        public long StandardCode
        {
            get { return _standardCode; }
            set
            {
                if (_standardCode != value)
                {
                    _standardCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _frameworkCode;
        public int FrameworkCode
        {
            get { return _frameworkCode; }
            set
            {
                if (_frameworkCode != value)
                {
                    _frameworkCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _programmeType;
        public int ProgrammeType
        {
            get { return _programmeType; }
            set
            {
                if (_programmeType != value)
                {
                    _programmeType = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _pathwayCode;
        public int PathwayCode
        {
            get { return _pathwayCode; }
            set
            {
                if (_pathwayCode != value)
                {
                    _pathwayCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _plannedEndDate;
        public DateTime PlannedEndDate
        {
            get { return _plannedEndDate; }
            set
            {
                if (_plannedEndDate != value)
                {
                    _plannedEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? _actualEndDate;
        public DateTime? ActualEndDate
        {
            get { return _actualEndDate; }
            set
            {
                if (_actualEndDate != value)
                {
                    _actualEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _trainingCost;
        public decimal TrainingCost
        {
            get { return _trainingCost; }
            set
            {
                if (_trainingCost != value)
                {
                    _trainingCost = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _endpointAssesmentCost;
        public decimal EndpointAssesmentCost
        {
            get { return _endpointAssesmentCost; }
            set
            {
                if (_endpointAssesmentCost != value)
                {
                    _endpointAssesmentCost = value;
                    OnPropertyChanged();
                }
            }
        }

        private short _contractStatus;
        public short ContractStatus
        {
            get { return _contractStatus; }
            set
            {
                if (_contractStatus != value)
                {
                    _contractStatus = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
