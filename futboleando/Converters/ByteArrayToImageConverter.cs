using System.Collections.Concurrent;
using System.Globalization;

namespace futboleando.Converters
{
    public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ? Manejar string Base64
            if (value is string base64String && !string.IsNullOrWhiteSpace(base64String))
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] Intentando convertir Base64 string (longitud: {base64String.Length})");
                    
                    // Limpiar espacios en blanco
                    base64String = base64String.Trim();
                    
                    // Verificar si tiene prefijo data:image
                    if (base64String.StartsWith("data:image/"))
                    {
                        var indexComa = base64String.IndexOf(",");
                        if (indexComa > 0)
                        {
                            base64String = base64String.Substring(indexComa + 1);
                            System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] Prefijo eliminado. Nueva longitud: {base64String.Length}");
                        }
                    }
                    
                    byte[] bytes = System.Convert.FromBase64String(base64String);
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] ? Conversión exitosa a {bytes.Length} bytes");
                    
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] ? ImageSource creado exitosamente");
                    
                    return imageSource;
                }
                catch (FormatException fex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] ? Error de formato Base64: {fex.Message}");
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] Primeros 50 caracteres: {base64String.Substring(0, Math.Min(50, base64String.Length))}");
                    return null;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] ? Error converting Base64 string to image: {ex.Message}");
                    return null;
                }
            }
            
            // ? También mantener compatibilidad con byte[] por si acaso
            if (value is byte[] imageBytes && imageBytes.Length > 0)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] Convirtiendo byte[] ({imageBytes.Length} bytes)");
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] ? byte[] convertido exitosamente");
                    return imageSource;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] ? Error converting byte[] to image: {ex.Message}");
                    return null;
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"[ByteArrayToImageConverter] ?? Valor null o vacío recibido. Tipo: {value?.GetType().Name ?? "null"}");
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
