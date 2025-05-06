using System;
using System.Globalization;
using System.Windows.Data;

namespace MangerTest.Converter
{
 
    
  public class MaskedTextToDoubleConverter : IValueConverter
    {
        private readonly CultureInfo germanCulture = new CultureInfo("de-DE");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value?.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            if (double.TryParse(stringValue, NumberStyles.Any, germanCulture, out double result))
                return result;

            return null;
        }
    }
}
