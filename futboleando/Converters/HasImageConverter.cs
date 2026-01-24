using System.Globalization;

namespace futboleando.Converters
{
    public class HasImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ? Verificar si es string Base64 y no está vacío
            bool hasImage = false;
            
            if (value is string base64String)
            {
                hasImage = !string.IsNullOrWhiteSpace(base64String);
                System.Diagnostics.Debug.WriteLine($"[HasImageConverter] String evaluado: {hasImage} (longitud: {base64String?.Length ?? 0})");
            }
            else if (value is byte[] imageBytes)
            {
                hasImage = imageBytes.Length > 0;
                System.Diagnostics.Debug.WriteLine($"[HasImageConverter] Byte[] evaluado: {hasImage} (longitud: {imageBytes.Length})");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[HasImageConverter] Tipo no reconocido o null: {value?.GetType().Name ?? "null"}");
            }
            
            // Si el parámetro es "invert", invertir el resultado
            if (parameter?.ToString() == "invert")
            {
                System.Diagnostics.Debug.WriteLine($"[HasImageConverter] Invertido: {!hasImage}");
                return !hasImage;
            }
            
            System.Diagnostics.Debug.WriteLine($"[HasImageConverter] Resultado final: {hasImage}");
            return hasImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
