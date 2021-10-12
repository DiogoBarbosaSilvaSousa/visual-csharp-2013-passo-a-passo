using System;
using System.Windows.Data;

namespace Delegates.Common
{
    /// <summary>
    /// Value converter that converts True to Yes and False to No, for displaying age restrictions.
    /// </summary>
    public sealed class AgeRestrictedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? "Yes" : "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
