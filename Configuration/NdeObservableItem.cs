using NavisDataExtraction.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NavisDataExtraction.Configuration
{
    public class NdeObservableItem : INotifyPropertyChanged
    {
        // Constructors
        public NdeObservableItem()
        {

        }

        //Properties
        public event PropertyChangedEventHandler PropertyChanged;

        //Methods
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
