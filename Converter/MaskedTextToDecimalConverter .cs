using System;
using System.Globalization;
using System.Windows.Data;

namespace MangerTest.Converter
{
    public class MaskedTextToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                // Rückgabe der Zahl mit 2 Dezimalstellen
                return decimalValue.ToString("0.00", culture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(text))
                return null;

            // Komma durch Punkt ersetzen, falls im deutschen Kontext Komma verwendet wurde
            text = text.Replace(",", ".");

            // Wenn die Eingabe unvollständig ist, z. B. "12.", zurückgeben
            if (text.EndsWith("."))
                return null;

            // Versuchen, die Eingabe als decimal zu parsen
            if (decimal.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            // Bei ungültiger Eingabe null zurückgeben
            return null;
        }
    }
}
