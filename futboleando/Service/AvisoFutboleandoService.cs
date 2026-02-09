using System.Net.Http.Json;
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
                var telefono = await _httpClient.GetFromJsonAsync<string>("api/AvisoFutboleando/Telefono");
                return string.IsNullOrWhiteSpace(telefono) ? null : telefono;
            }
            catch
            {
                return null;
            }
        }
    }
}
