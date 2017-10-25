using System.ComponentModel;
using System.Runtime.CompilerServices;
using IlrGeneratorApp.Properties;

namespace IlrGeneratorApp.ViewModels
{
    public abstract class GeneratorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
