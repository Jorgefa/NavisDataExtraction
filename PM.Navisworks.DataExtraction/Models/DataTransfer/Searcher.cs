using System.Collections.ObjectModel;
using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction.Models.DataTransfer
{
    public class Searcher : BindableBase
    {
        public Searcher()
        {
            Conditions = new ObservableCollection<Condition>();
            Pairs = new ObservableCollection<CategoryPropertyPair>();
        }
        
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _pruneBelow;

        public bool PruneBelow
        {
            get => _pruneBelow;
            set => SetProperty(ref _pruneBelow, value);
        }

        private ObservableCollection<Condition> _conditions;

        public ObservableCollection<Condition> Conditions
        {
            get => _conditions;
            set => SetProperty(ref _conditions, value);
        }

        private ObservableCollection<CategoryPropertyPair> _pairs;

        public ObservableCollection<CategoryPropertyPair> Pairs
        {
            get => _pairs;
            set => SetProperty(ref _pairs, value);
        }

        public override string ToString() => Name;
    }
}