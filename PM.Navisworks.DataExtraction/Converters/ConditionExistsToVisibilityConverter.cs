using System;
using System.Globalization;
using System.Windows.Data;
using PM.Navisworks.DataExtraction.Models.DataTransfer;

namespace PM.Navisworks.DataExtraction.Converters
{
    public class ConditionExistsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return System.Windows.Visibility.Collapsed;

            if(value.GetType() != typeof(ConditionComparer))
                return System.Windows.Visibility.Collapsed;
            
            if (((ConditionComparer)value) == ConditionComparer.Exists)
                return System.Windows.Visibility.Collapsed;
            
            return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}