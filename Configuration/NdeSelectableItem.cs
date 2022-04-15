using NavisDataExtraction.Annotations;
using NavisDataExtraction.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
