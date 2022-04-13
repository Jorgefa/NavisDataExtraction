using System.Collections.ObjectModel;
using System.Linq;

namespace NavisDataExtraction.DataClasses
{
    public class NavisExtractionTypeCollection : NavisObservableItem
    {
        //Constructors
        public NavisExtractionTypeCollection()
        {
        }

        public NavisExtractionTypeCollection(string name)
        {
            Name = name;
        }

        public NavisExtractionTypeCollection(string name, ObservableCollection<NavisExtractionType> types)
        {
            Name = name;
            Types = types;
        }

        //Properties
        private ObservableCollection<NavisExtractionType> _types;

        public ObservableCollection<NavisExtractionType> Types
        {
            get { return _types; }
            set
            {
                _types = value;
                OnPropertyChanged();
            }
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

        //Methods
        public string TypesValidation()
        {
            if (Types == null)
            {
                return null;
            }

            var typeNames = Types.ToList().Select(x => x.Name).ToList();

            if (typeNames.Count != typeNames.Distinct().Count())
            {
                return "duplicates";
            }
            return "ok";
        }
    }
}