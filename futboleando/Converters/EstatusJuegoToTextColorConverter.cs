using System.Globalization;

namespace futboleando.Converters
{
    public class EstatusJuegoToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string estatusNombre)
            {
                var estatusLimpio = estatusNombre.Trim().ToUpperInvariant();

                if (estatusLimpio == "JUGADO")
                {
                    return Color.FromArgb("#166534");
                }

                return Color.FromArgb("#1E3A8A");
            }

            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
