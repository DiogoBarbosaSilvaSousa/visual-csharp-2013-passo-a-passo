using System;
using System.Windows.Data;

namespace Delegates.Common
{
    /// <summary>
    /// Value converter that converts prices to strings using the local currency.
    /// </summary>
    public sealed class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return String.Format("{0:C}", value);
            }
            else
            {
                return "";
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
