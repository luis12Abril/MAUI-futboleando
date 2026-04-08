using futboleandoEntities.Estado;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace futboleando.Service
{
    public class EstadoService
    {
        private readonly HttpClient _httpClient;

        public EstadoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<EstadoListCLS>> ListarEstados()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[EstadoService] Llamando: {_httpClient.BaseAddress}api/Estado");
                var response = await _httpClient.GetFromJsonAsync<List<EstadoListCLS>>("api/Estado");
                if (response != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[EstadoService] OK - {response.Count} estados recibidos");
                    return new ObservableCollection<EstadoListCLS>(response);
                }
                System.Diagnostics.Debug.WriteLine("[EstadoService] Respuesta nula del servidor");
                return new ObservableCollection<EstadoListCLS>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EstadoService] Error de red: {ex.Message} | StatusCode: {ex.StatusCode}");
                return new ObservableCollection<EstadoListCLS>();
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EstadoService] Timeout: {ex.Message}");
                return new ObservableCollection<EstadoListCLS>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EstadoService] Error inesperado: {ex.GetType().Name}: {ex.Message}");
                return new ObservableCollection<EstadoListCLS>();
            }
        }

        public async Task<EstadoListCLS?> ObtenerPorId(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<EstadoListCLS>($"api/Estado/{id}");
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
