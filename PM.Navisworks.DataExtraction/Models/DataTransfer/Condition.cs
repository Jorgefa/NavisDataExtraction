using System;
using System.Text;
using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction.Models.DataTransfer
{
    public class Condition : BindableBase
    {
        private Category _category;

        public Category Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private Property _property;

        public Property Property
        {
            get => _property;
            set => SetProperty(ref _property, value);
        }

        private ConditionComparer _comparer;

        public ConditionComparer Comparer
        {
            get => _comparer;
            set => SetProperty(ref _comparer, value);
        }

        private bool _boolValue;

        public bool BoolValue
        {
            get => _boolValue;
            set => SetProperty(ref _boolValue, value);
        }

        private string _stringValue;

        public string StringValue
        {
            get => _stringValue;
            set => SetProperty(ref _stringValue, value);
        }

        private double _doubleValue;

        public double DoubleValue
        {
            get => _doubleValue;
            set => SetProperty(ref _doubleValue, value);
        }

        private int _integerValue;

        public int IntegerValue
        {
            get => _integerValue;
            set => SetProperty(ref _integerValue, value);
        }

        private DateTime _dateTimeValue;

        public DateTime DateTimeValue
        {
            get => _dateTimeValue;
            set => SetProperty(ref _dateTimeValue, value);
        }

        private string _displayName;

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        public void SetDisplayName(Type type)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Category:");
            stringBuilder.Append(Category?.Name);

            if (Property != null)
            {
                stringBuilder.Append(" Property:");
                stringBuilder.Append(Property?.Name);
            }

            stringBuilder.Append(" ");

            if (Comparer == ConditionComparer.Exists || type == null)
            {
                stringBuilder.Append("Exists");
            }
            else
            {
                stringBuilder.Append(Comparer.ToString());
                stringBuilder.Append(" ");

                if (type == typeof(bool)) stringBuilder.Append(BoolValue ? "Yes" : "No");
                if (type == typeof(string)) stringBuilder.Append(StringValue);
                if (type == typeof(double)) stringBuilder.Append(DoubleValue);
                if (type == typeof(int)) stringBuilder.Append(IntegerValue);
                if (type == typeof(DateTime)) stringBuilder.Append(DateTimeValue.ToString("d"));
            }

            DisplayName = stringBuilder.ToString();
        }
    }
}