using Autodesk.Navisworks.Api;
using System.Collections.ObjectModel;

namespace NavisDataExtraction.Configuration
{
    public class NdeModelItemGroup : NdeObservableItem
    {
        public NdeModelItemGroup()
        {
        }

        private NdeCollection _collection;

        public NdeCollection Collection
        {
            get { return _collection; }
            set { _collection = value; OnPropertyChanged(); }
        }

        private NdeType _type;

        public NdeType Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ModelItem> _modelItemCollection;

        public ObservableCollection<ModelItem> ModelItemCollection
        {
            get { return _modelItemCollection; }
            set { _modelItemCollection = value; OnPropertyChanged(); }
        }
    }
}