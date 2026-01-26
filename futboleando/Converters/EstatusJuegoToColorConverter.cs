using System.Globalization;
using System.Diagnostics;

namespace futboleando.Converters
{
    public class EstatusJuegoToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string estatusNombre)
            {
                // ? Debug: Ver qué valor llega
                Debug.WriteLine($"[EstatusConverter] Valor recibido: '{estatusNombre}'");

                // ? Limpiar espacios y convertir a mayúsculas para comparación segura
                string estatusLimpio = estatusNombre.Trim().ToUpper();
                
                Debug.WriteLine($"[EstatusConverter] Valor limpio: '{estatusLimpio}'");

                // ? Si el estatus es "JUGADO"
                if (estatusLimpio == "JUGADO")
                {
                    Debug.WriteLine("[EstatusConverter] ? Aplicando color VERDE (#E8F5E9)");
                    // ? Color verde suave para juegos jugados
                    return Color.FromArgb("#E8F5E9");  // Verde muy claro
                }
                else
                {
                    Debug.WriteLine($"[EstatusConverter] ? Aplicando color AZUL (#E3F2FD) para '{estatusLimpio}'");
                    // ? Color azul suave para juegos pendientes/programados
                    return Color.FromArgb("#E3F2FD");  // Azul muy claro
                }
            }

            Debug.WriteLine("[EstatusConverter] ?? Valor no es string, usando blanco");
            // ? Color por defecto (blanco)
            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
