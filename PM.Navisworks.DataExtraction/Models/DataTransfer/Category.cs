using System.Collections.ObjectModel;
using Newtonsoft.Json;
using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction.Models.DataTransfer
{
    public class Category : BindableBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<Property> _properties;

        [JsonIgnore]
        public ObservableCollection<Property> Properties
        {
            get => _properties;
            set => SetProperty(ref _properties, value);
        }

        public override string ToString() => Name;
    }
}