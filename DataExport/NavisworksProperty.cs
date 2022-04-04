using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Navisworks.Api;
using NavisDataExtraction.Annotations;

namespace NavisDataExtraction.DataExport
{
    public class NavisworksProperty : INotifyPropertyChanged
    {
        public PropertyCategory Category { get; set; }
        public DataProperty Property { get; set; }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}