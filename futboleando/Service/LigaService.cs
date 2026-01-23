using futboleandoEntities.Liga;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace futboleando.Service
{
    public class LigaService
    {
        private readonly HttpClient _httpClient;

        public LigaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<LigaListCLS>> ListarLigas()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<LigaListCLS>>("api/Liga");
                if (response != null)
                {
                    return new ObservableCollection<LigaListCLS>(response);
                }
                return new ObservableCollection<LigaListCLS>();
            }
            catch (Exception)
            {
                return new ObservableCollection<LigaListCLS>();
            }
        }

        public async Task<ObservableCollection<LigaListCLS>> ListarPorMunicipio(int idMunicipio)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<LigaListCLS>>($"api/Liga/PorMunicipio/{idMunicipio}");
                if (response != null)
                {
                    return new ObservableCollection<LigaListCLS>(response);
                }
                return new ObservableCollection<LigaListCLS>();
            }
            catch (Exception)
            {
                return new ObservableCollection<LigaListCLS>();
            }
        }
    }
}
