using System;
using System.Globalization;
using System.Windows.Data;

namespace MangerTest.Converter
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                // Formatierte Ausgabe als hh:mm
                return timeSpan.ToString(@"hh\:mm");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;  // Keine Rückumwandlung notwendig
        }
    }
}
