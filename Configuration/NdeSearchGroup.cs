using System.Collections.ObjectModel;

namespace NavisDataExtraction.Configuration
{
    public class NdeSearchGroup : NdeObservableItem
    {
        public NdeSearchGroup()
        {
        }

        public NdeSearchGroup(string name)
        {
            Name = name;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NdeSearcher> _searchers;

        public ObservableCollection<NdeSearcher> Searchers
        {
            get { return _searchers; }
            set
            {
                _searchers = value;
                OnPropertyChanged();
            }
        }
    }
}