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
                var response = await _httpClient.GetFromJsonAsync<List<EstadoListCLS>>("api/Estado");
                if (response != null)
                {
                    return new ObservableCollection<EstadoListCLS>(response);
                }
                return new ObservableCollection<EstadoListCLS>();
            }
            catch (Exception)
            {
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
