using futboleandoEntities.Juego;
using futboleandoEntities.Jornada;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace futboleando.Service
{
    public class JuegoService
    {
        private readonly HttpClient _httpClient;

        public event Func<Task> OnChange;

        public JuegoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void NotificarChange()
        {
            OnChange?.Invoke();
        }

        // ? Listar juegos por torneo
        public async Task<ObservableCollection<JuegoListCLS>> ListarJuegosPorTorneo(int idTorneo)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JuegoListCLS>>($"api/Juego/PorTorneo/{idTorneo}");
                if (response != null)
                {
                    return new ObservableCollection<JuegoListCLS>(response);
                }
                return new ObservableCollection<JuegoListCLS>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<JuegoListCLS>();
            }
        }

        // ? Obtener detalles de un juego específico
        public async Task<JuegoDetallesCLS?> ObtenerDetallesJuego(int idJuego)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<JuegoDetallesCLS>($"api/Juego/Detalles/{idJuego}");
                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerDetallesJuego: {ex.Message}");
                return null;
            }
        }

        // ? Listar juegos por torneo y jornada
        public async Task<ObservableCollection<JuegoListCLS>> ListarJuegosPorTorneoYJornada(int idTorneo, int idJornada)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JuegoListCLS>>($"api/Juego/PorTorneoYJornada/{idTorneo}/{idJornada}");
                if (response != null)
                {
                    return new ObservableCollection<JuegoListCLS>(response);
                }
                return new ObservableCollection<JuegoListCLS>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<JuegoListCLS>();
            }
        }

        // ? Listar jornadas por torneo
        public async Task<ObservableCollection<JornadaListCLS>> ListarJornadasPorTorneo(int idTorneo)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JornadaListCLS>>($"api/Juego/JornadasPorTorneo/{idTorneo}");
                if (response != null)
                {
                    return new ObservableCollection<JornadaListCLS>(response);
                }
                return new ObservableCollection<JornadaListCLS>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<JornadaListCLS>();
            }
        }
    }
}
