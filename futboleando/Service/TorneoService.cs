using futboleandoEntities.Torneo;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace futboleando.Service
{
    public class TorneoService
    {
        private readonly HttpClient _httpClient;

        public TorneoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<TorneoListCLS>> ListarTorneos()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<TorneoListCLS>>("api/Torneo");
                if (response != null)
                {
                    return new ObservableCollection<TorneoListCLS>(response);
                }
                return new ObservableCollection<TorneoListCLS>();
            }
            catch (Exception)
            {
                return new ObservableCollection<TorneoListCLS>();
            }
        }

        public async Task<ObservableCollection<TorneoListCLS>> ListarPorLiga(int idLiga)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<TorneoListCLS>>($"api/Torneo/PorLiga/{idLiga}");
                if (response != null)
                {
                    return new ObservableCollection<TorneoListCLS>(response);
                }
                return new ObservableCollection<TorneoListCLS>();
            }
            catch (Exception)
            {
                return new ObservableCollection<TorneoListCLS>();
            }
        }

        public async Task<TorneoListCLS?> ObtenerPorId(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<TorneoListCLS>($"api/Torneo/{id}");
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
