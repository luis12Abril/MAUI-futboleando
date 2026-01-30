using futboleandoEntities.Cumpleañero;
using futboleandoEntities.Jugador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class CumpleañeroService
    {
        private readonly HttpClient _httpClient;

        public CumpleañeroService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Obtiene los cumpleañeros de las próximas 2 semanas por torneo
        /// </summary>
        public async Task<List<CumpleañeroCLS>> ListarCumpleañerosPorTorneo(int idTorneo, int? idEquipo = null)
        {
            try
            {
                var startTime = DateTime.Now;
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS SERVICE] Iniciando petición API para torneo {idTorneo}...");
                
                // ? CORREGIDO: Usar el endpoint correcto con manejo de respuesta
                var response = await _httpClient.GetAsync($"api/Jugador/PorTorneo/{idTorneo}");
                
                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS SERVICE] Error HTTP: {response.StatusCode}");
                    return new List<CumpleañeroCLS>();
                }
                
                var jugadores = await response.Content.ReadFromJsonAsync<List<JugadorListCLS>>();
                
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS SERVICE] Jugadores recibidos: {jugadores?.Count ?? 0}");
                
                if (jugadores == null || !jugadores.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS SERVICE] No se encontraron jugadores para el torneo {idTorneo}");
                    return new List<CumpleañeroCLS>();
                }

                // Contar jugadores con fecha de nacimiento
                var jugadoresConFecha = jugadores.Where(j => j.fnacimiento.HasValue).Count();
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS SERVICE] Jugadores con fecha de nacimiento: {jugadoresConFecha}");

                // Filtrar por equipo si se especifica
                if (idEquipo.HasValue && idEquipo.Value > 0)
                {
                    jugadores = jugadores.Where(j => j.idequipo == idEquipo.Value).ToList();
                    System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS SERVICE] Filtrado por equipo {idEquipo}: {jugadores.Count} jugadores");
                }

                // Calcular cumpleañeros
                var cumpleañeros = CalcularCumpleañeros(jugadores);

                var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS SERVICE] Procesados en {elapsed}ms - {cumpleañeros.Count} cumpleañeros encontrados");
                
                return cumpleañeros;
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"?? Petición cancelada o timeout: {ex.Message}");
                return new List<CumpleañeroCLS>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error de red: {ex.Message}");
                return new List<CumpleañeroCLS>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error al obtener cumpleañeros: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"? Stack trace: {ex.StackTrace}");
                return new List<CumpleañeroCLS>();
            }
        }

        /// <summary>
        /// Calcula los cumpleañeros de las próximas 2 semanas
        /// </summary>
        private List<CumpleañeroCLS> CalcularCumpleañeros(List<JugadorListCLS> jugadores)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Fecha actual: {hoy}");
            
            var cumpleañeros = new List<CumpleañeroCLS>();

            foreach (var jugador in jugadores.Where(j => j.fnacimiento.HasValue))
            {
                var fechaNac = jugador.fnacimiento!.Value;
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Procesando: {jugador.nombrecompleto} - Nació: {fechaNac}");
                
                // Calcular cumpleaños de este año
                var cumpleañosEsteAño = new DateOnly(hoy.Year, fechaNac.Month, fechaNac.Day);
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS]   Cumpleaños este año: {cumpleañosEsteAño}");
                
                // Si ya pasó, calcular para el próximo año
                if (cumpleañosEsteAño < hoy)
                {
                    cumpleañosEsteAño = new DateOnly(hoy.Year + 1, fechaNac.Month, fechaNac.Day);
                    System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS]   Ya pasó, próximo año: {cumpleañosEsteAño}");
                }

                // Calcular días hasta el cumpleaños
                var diasParaCumple = cumpleañosEsteAño.DayNumber - hoy.DayNumber;
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS]   Días para cumpleaños: {diasParaCumple}");

                // Solo incluir si es dentro de 14 días
                if (diasParaCumple >= 0 && diasParaCumple <= 14)
                {
                    var edad = hoy.Year - fechaNac.Year;
                    if (cumpleañosEsteAño > new DateOnly(hoy.Year, fechaNac.Month, fechaNac.Day))
                    {
                        edad++;
                    }

                    System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS]   ? AGREGADO - Edad que cumple: {edad}");

                    cumpleañeros.Add(new CumpleañeroCLS
                    {
                        idjugador = jugador.idjugador ?? 0,
                        nombrecompleto = jugador.nombrecompleto,
                        fechanacimiento = fechaNac,
                        nombreequipo = jugador.nombreequipo,
                        edad = edad,
                        esCumpleañosHoy = diasParaCumple == 0,
                        diasParaCumpleaños = diasParaCumple
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS]   ? Fuera de rango (días: {diasParaCumple})");
                }
            }

            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Total de cumpleañeros encontrados: {cumpleañeros.Count}");

            // Ordenar por días para cumpleaños
            return cumpleañeros.OrderBy(c => c.diasParaCumpleaños).ToList();
        }
    }
}
