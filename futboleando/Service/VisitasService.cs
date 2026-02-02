using futboleandoEntities.Visitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class VisitasService
    {
        private readonly HttpClient _httpClient;

        public VisitasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ? Obtener totales de visitas (excluyendo admin)
        public async Task<VisitasTotalesCLS> ObtenerVisitasTotales()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Usuario/VisitasTotales");
                
                if (response.IsSuccessStatusCode)
                {
                    var totales = await response.Content.ReadFromJsonAsync<VisitasTotalesCLS>();
                    return totales ?? new VisitasTotalesCLS();
                }
                else
                {
                    return new VisitasTotalesCLS();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error al obtener totales: {ex.Message}");
                return new VisitasTotalesCLS();
            }
        }

        // ? Obtener tipos de usuario para el picker
        public async Task<List<TipoUsuarioSimpleCLS>> ObtenerTiposUsuario()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Usuario/TiposUsuario");
                
                if (response.IsSuccessStatusCode)
                {
                    var tipos = await response.Content.ReadFromJsonAsync<List<TipoUsuarioSimpleCLS>>();
                    return tipos ?? new List<TipoUsuarioSimpleCLS>();
                }
                else
                {
                    return new List<TipoUsuarioSimpleCLS>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error al obtener tipos de usuario: {ex.Message}");
                return new List<TipoUsuarioSimpleCLS>();
            }
        }

        // ? Obtener visitas por usuario con filtro opcional
        public async Task<List<VisitaUsuarioCLS>> ObtenerVisitasPorUsuario(int? idTipoUsuario = null)
        {
            try
            {
                string url = "api/Usuario/VisitasPorUsuario";
                
                // Agregar parámetro de tipo de usuario si existe
                if (idTipoUsuario.HasValue && idTipoUsuario.Value > 0)
                {
                    url += $"?idTipoUsuario={idTipoUsuario.Value}";
                }

                System.Diagnostics.Debug.WriteLine($"?? Llamando a: {_httpClient.BaseAddress}{url}");

                var response = await _httpClient.GetAsync(url);
                
                System.Diagnostics.Debug.WriteLine($"?? Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var visitas = await response.Content.ReadFromJsonAsync<List<VisitaUsuarioCLS>>();
                    System.Diagnostics.Debug.WriteLine($"? Visitas recibidas del API: {visitas?.Count ?? 0}");
                    return visitas ?? new List<VisitaUsuarioCLS>();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"? Error del servidor: {errorContent}");
                    return new List<VisitaUsuarioCLS>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error al obtener visitas por usuario: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"? StackTrace: {ex.StackTrace}");
                return new List<VisitaUsuarioCLS>();
            }
        }
    }
}
