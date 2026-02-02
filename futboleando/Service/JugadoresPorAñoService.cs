using futboleandoEntities.JugadoresPorAño;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class JugadoresPorAñoService
    {
        private readonly HttpClient _httpClient;

        public JugadoresPorAñoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ? Listar equipos del torneo para el picker
        public async Task<List<EquipoSimpleCLS>> ListarEquiposPorTorneo(int idTorneo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Jugador/EquiposPorTorneo/{idTorneo}");
                
                if (response.IsSuccessStatusCode)
                {
                    var equipos = await response.Content.ReadFromJsonAsync<List<EquipoSimpleCLS>>();
                    return equipos ?? new List<EquipoSimpleCLS>();
                }
                else
                {
                    return new List<EquipoSimpleCLS>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error al obtener equipos: {ex.Message}");
                return new List<EquipoSimpleCLS>();
            }
        }

        // ? Listar jugadores agrupados por año de nacimiento
        public async Task<List<JugadoresPorAñoCLS>> ListarJugadoresPorAño(int idTorneo, int? idEquipo = null)
        {
            try
            {
                string url = $"api/Jugador/JugadoresPorAño/{idTorneo}";
                
                // Agregar parámetro de equipo si existe
                if (idEquipo.HasValue && idEquipo.Value > 0)
                {
                    url += $"?idEquipo={idEquipo.Value}";
                }

                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var jugadores = await response.Content.ReadFromJsonAsync<List<JugadoresPorAñoCLS>>();
                    return jugadores ?? new List<JugadoresPorAñoCLS>();
                }
                else
                {
                    return new List<JugadoresPorAñoCLS>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error al obtener jugadores por año: {ex.Message}");
                return new List<JugadoresPorAñoCLS>();
            }
        }
    }
}
