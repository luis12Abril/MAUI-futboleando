using System.Globalization;

namespace futboleando.Converters
{
    public class EquipoNombreJuegoConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var nombreEquipo = values.Length > 0 ? values[0] as string : null;
            var resultadoEquipo = values.Length > 1 ? values[1] as string : null;
            var estatus = values.Length > 2 ? values[2] as string : null;

            var estatusLimpio = estatus?.Trim();
            var esJugado = string.Equals(estatusLimpio, "JUGADO", StringComparison.OrdinalIgnoreCase);

            var nombreLimpio = string.IsNullOrWhiteSpace(nombreEquipo) ? null : nombreEquipo.Trim();
            var resultadoLimpio = string.IsNullOrWhiteSpace(resultadoEquipo) ? null : resultadoEquipo.Trim();

            if (esJugado)
            {
                return resultadoLimpio ?? nombreLimpio ?? "SIN EQUIPO";
            }

            return nombreLimpio ?? resultadoLimpio ?? "SIN EQUIPO";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
