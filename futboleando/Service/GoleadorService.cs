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

        // ? Método para listar goleadores por torneo
        public async Task<List<GoleadorCLS>> ListarGoleadoresPorTorneo(int idTorneo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Jugador/Goleadores/{idTorneo}");
                
                if (response.IsSuccessStatusCode)
                {
                    var goleadores = await response.Content.ReadFromJsonAsync<List<GoleadorCLS>>();
                    return goleadores ?? new List<GoleadorCLS>();
                }
                else
                {
                    return new List<GoleadorCLS>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error al obtener goleadores: {ex.Message}");
                return new List<GoleadorCLS>();
            }
        }
    }
}
