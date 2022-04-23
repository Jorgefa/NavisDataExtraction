using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction.Models.DataTransfer
{
    public class CategoryPropertyPair : BindableBase
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
    }
}