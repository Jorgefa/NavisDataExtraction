namespace NavisDataExtraction.Configuration
{
    public class NdeSelectableItem : NdeObservableItem
    {
        // Constructors
        public NdeSelectableItem()
        {

        }

        //Properties
        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }


    }
}
