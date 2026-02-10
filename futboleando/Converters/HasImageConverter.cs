using System.Globalization;

namespace futboleando.Converters
{
    public class HasImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasImage = value switch
            {
                string base64String => !string.IsNullOrWhiteSpace(base64String),
                byte[] imageBytes => imageBytes.Length > 0,
                _ => false
            };

            if (parameter?.ToString() == "invert")
            {
                return !hasImage;
            }

            return hasImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
