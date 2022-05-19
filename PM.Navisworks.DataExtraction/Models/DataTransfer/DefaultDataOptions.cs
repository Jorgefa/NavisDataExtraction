using PM.Navisworks.DataExtraction.Utilities;

namespace PM.Navisworks.DataExtraction.Models.DataTransfer
{
    public class DefaultDataOptions : BindableBase
    {
        private bool _modelSource;

        public bool ModelSource
        {
            get => _modelSource;
            set => SetProperty(ref _modelSource, value);
        }

        private bool _coordinates;

        public bool Coordinates
        {
            get => _coordinates;
            set => SetProperty(ref _coordinates, value);
        }
    }
}