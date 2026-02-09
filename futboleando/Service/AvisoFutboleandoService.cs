using System.Net.Http.Json;
using System.Text.Json;
using futboleandoEntities.Aviso;

namespace futboleando.Service
{
    public class AvisoFutboleandoService
    {
        private readonly HttpClient _httpClient;

        public AvisoFutboleandoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AvisoFutboleandoCLS?> ObtenerAvisoAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<AvisoFutboleandoCLS>("api/AvisoFutboleando");
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> ObtenerTelefonoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/AvisoFutboleando/Telefono");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var telefono = NormalizarTelefonoRespuesta(content);
                    if (!string.IsNullOrWhiteSpace(telefono))
                    {
                        return telefono;
                    }
                }
            }
            catch
            {
            }

            var aviso = await ObtenerAvisoAsync();
            return string.IsNullOrWhiteSpace(aviso?.titulomensaje) ? null : aviso.titulomensaje;
        }

        private static string? NormalizarTelefonoRespuesta(string? contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido))
            {
                return null;
            }

            try
            {
                var valor = JsonSerializer.Deserialize<string>(contenido);
                return string.IsNullOrWhiteSpace(valor) ? null : valor;
            }
            catch
            {
                var recortado = contenido.Trim().Trim('"');
                return string.IsNullOrWhiteSpace(recortado) ? null : recortado;
            }
        }
    }
}
