using futboleandoEntities.Juego;
using futboleandoEntities.Jornada;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Linq;

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

        // ? Listar juegos donde un jugador anoto gol
        public async Task<ObservableCollection<JuegoGolesJugadorCLS>> ListarJuegosConGolesJugador(int idTorneo, int idJugador)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JuegoGolesJugadorCLS>>($"api/Juego/GolesPorJugador/{idTorneo}/{idJugador}");
                if (response != null)
                {
                    if (response.Count > 0)
                    {
                        return new ObservableCollection<JuegoGolesJugadorCLS>(response);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return await ObtenerJuegosConGolesJugadorFallback(idTorneo, idJugador);
        }

        private async Task<ObservableCollection<JuegoGolesJugadorCLS>> ObtenerJuegosConGolesJugadorFallback(int idTorneo, int idJugador)
        {
            try
            {
                var juegos = await ListarJuegosPorTorneo(idTorneo);
                var juegosJugados = juegos
                    .Where(j => string.Equals(j.nombreestatusjuego?.Trim(), "JUGADO", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (juegosJugados.Count == 0)
                {
                    return new ObservableCollection<JuegoGolesJugadorCLS>();
                }

                var resultados = new List<JuegoGolesJugadorCLS>();
                var detalleTasks = juegosJugados
                    .Select(async juego => new { juego, detalle = await ObtenerDetallesJuego(juego.idjuego) })
                    .ToList();

                var detalles = await Task.WhenAll(detalleTasks);

                foreach (var item in detalles)
                {
                    if (item.detalle == null)
                    {
                        continue;
                    }

                    var golesJugador = 0;
                    var golEquipo01 = item.detalle.golesEquipo01.FirstOrDefault(g => g.idjugador == idJugador);
                    if (golEquipo01 != null)
                    {
                        golesJugador += golEquipo01.goles;
                    }

                    var golEquipo02 = item.detalle.golesEquipo02.FirstOrDefault(g => g.idjugador == idJugador);
                    if (golEquipo02 != null)
                    {
                        golesJugador += golEquipo02.goles;
                    }

                    if (golesJugador == 0)
                    {
                        continue;
                    }

                    resultados.Add(new JuegoGolesJugadorCLS
                    {
                        idjuego = item.juego.idjuego,
                        idjornada = item.juego.idjornada,
                        nombrejornada = item.juego.nombrejornada,
                        idequipo01 = item.juego.idequipo01,
                        nombreequipo01 = item.juego.nombreequipo01,
                        golesequipo01 = item.juego.golesequipo01,
                        idequipo02 = item.juego.idequipo02,
                        nombreequipo02 = item.juego.nombreequipo02,
                        golesequipo02 = item.juego.golesequipo02,
                        fhorario = item.juego.fhorario,
                        idcampo = item.juego.idcampo,
                        nombrecampo = item.juego.nombrecampo,
                        nombrearbitro = item.juego.nombrearbitro,
                        idestatusjuego = item.juego.idestatusjuego,
                        nombreestatusjuego = item.juego.nombreestatusjuego,
                        resequipo01 = item.juego.resequipo01,
                        resequipo02 = item.juego.resequipo02,
                        idtorneo = item.juego.idtorneo,
                        golesjugador = golesJugador
                    });
                }

                var ordenados = resultados
                    .OrderByDescending(j => j.fhorario ?? DateTime.MinValue)
                    .ToList();

                return new ObservableCollection<JuegoGolesJugadorCLS>(ordenados);
            }
            catch (Exception ex)
            {
                return new ObservableCollection<JuegoGolesJugadorCLS>();
            }
        }
    }
}
