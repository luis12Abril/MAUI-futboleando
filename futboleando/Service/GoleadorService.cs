using futboleandoEntities.Goleador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class GoleadorService
    {
        private readonly HttpClient _httpClient;

        public GoleadorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ? Método para listar goleadores por torneo (optimizado)
        public async Task<List<GoleadorCLS>> ListarGoleadoresPorTorneo(int idTorneo)
        {
            try
            {
                var startTime = DateTime.Now;
                System.Diagnostics.Debug.WriteLine($"[GOLEADOR SERVICE] Iniciando petición API...");
                
                // Usar GetFromJsonAsync es más eficiente que GetAsync + ReadFromJsonAsync
                var goleadores = await _httpClient.GetFromJsonAsync<List<GoleadorCLS>>($"api/Jugador/Goleadores/{idTorneo}");
                
                var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine($"[GOLEADOR SERVICE] API respondió en {elapsed}ms con {goleadores?.Count ?? 0} goleadores");
                
                return goleadores ?? new List<GoleadorCLS>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error al obtener goleadores: {ex.Message}");
                return new List<GoleadorCLS>();
            }
        }
    }
}
