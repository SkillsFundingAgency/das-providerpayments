using ProviderPayments.TestStack.Domain;

namespace ProviderPayments.TestStack.UI.Models
{
    public class IlrLearnerModel : IlrLearner
    {
        private string _selectedCourse;

        public string SelectedCourse
        {
            get
            {
                return _selectedCourse;
            }
            set
            {
                _selectedCourse = value;

                var courseParts = _selectedCourse.Split('-');
                if (courseParts.Length == 3)
                {
                    PathwayCode = int.Parse(courseParts[0]);
                    FrameworkCode = int.Parse(courseParts[1]);
                    ProgrammeType = int.Parse(courseParts[2]);
                }
                else
                {
                    StandardCode = long.Parse(courseParts[0]);
                }
            }
        }
    }
}