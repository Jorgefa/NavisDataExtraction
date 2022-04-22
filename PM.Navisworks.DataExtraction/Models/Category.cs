using System.Collections.Generic;
using System.Collections.ObjectModel;
using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction.Models
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

        public ObservableCollection<Property> Properties
        {
            get => _properties;
            set => SetProperty(ref _properties, value);
        }

        public override string ToString() => Name;
    }
}