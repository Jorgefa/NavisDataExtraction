using System;
using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction.Models
{
    public class Property : BindableBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Type _valueType;

        public Type ValueType
        {
            get => _valueType;
            set => SetProperty(ref _valueType, value);
        }
        
        public override string ToString() => Name + ":" + ValueType.Name;
    }
}