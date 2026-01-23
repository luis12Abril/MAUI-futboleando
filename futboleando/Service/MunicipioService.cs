using futboleandoEntities.Municipio;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace futboleando.Service
{
    public class MunicipioService
    {
        private readonly HttpClient _httpClient;

        public MunicipioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<MunicipioListCLS>> ListarMunicipios()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<MunicipioListCLS>>("api/Municipio");
                if (response != null)
                {
                    return new ObservableCollection<MunicipioListCLS>(response);
                }
                return new ObservableCollection<MunicipioListCLS>();
            }
            catch (Exception)
            {
                return new ObservableCollection<MunicipioListCLS>();
            }
        }

        public async Task<ObservableCollection<MunicipioListCLS>> ListarPorEstado(int idEstado)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<MunicipioListCLS>>($"api/Municipio/PorEstado/{idEstado}");
                if (response != null)
                {
                    return new ObservableCollection<MunicipioListCLS>(response);
                }
                return new ObservableCollection<MunicipioListCLS>();
            }
            catch (Exception)
            {
                return new ObservableCollection<MunicipioListCLS>();
            }
        }
    }
}
