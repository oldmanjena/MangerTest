using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;

namespace MangerTest.Converter
{
    public class FlowDocumentToStringConverter : IValueConverter
    {
        // Convert string to FlowDocument (used for initial binding)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                // Convert string to FlowDocument
                var flowDoc = new FlowDocument(new Paragraph(new Run(text)));
                return flowDoc;
            }
            return new FlowDocument();
        }

        // Convert FlowDocument to string (used for two-way binding)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FlowDocument flowDocument)
            {
                // Convert FlowDocument to string
                TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                return textRange.Text;
            }
            return string.Empty;
        }
    }
}
