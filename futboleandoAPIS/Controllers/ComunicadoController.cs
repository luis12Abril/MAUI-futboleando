using System.Globalization;
using futboleandoAPIS.Models;
using futboleandoEntities;
using futboleandoEntities.Comunicado;
using futboleandoEntities.Equipo;
using futboleandoEntities.Jugador;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComunicadoController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;
        public ComunicadoController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        // ✅ Endpoint sin parámetro - devuelve todos los comunicados habilitados
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var datosDB = (from c in _bd.Comunicados
                                where c.Habilitado == 1
                                select new ComunicadoListCLS
                                {
                                    idcomunicado = c.Idcomunicado,
                                    comunicadocorto = c.Comunicadocorto,
                                    comunicadolargo = c.Comunicadolargo,
                                    fechacomunicado = c.Fechacomunicado,
                                    idtorneo = c.Idtorneo,
                                    habilitado = c.Habilitado
                                })
                                .OrderByDescending(o => o.fechacomunicado)
                                .ToList();

                var cultura = new CultureInfo("es-ES");
                var resultado = datosDB.Select(c =>
                {
                    var comunicado = new ComunicadoListCLS
                    {
                        idcomunicado = c.idcomunicado,
                        comunicadocorto = c.comunicadocorto ?? string.Empty,
                        comunicadolargo = c.comunicadolargo ?? string.Empty,
                        fechacomunicado = c.fechacomunicado,
                        idtorneo = c.idtorneo,
                        habilitado = c.habilitado
                    };

                    if (c.fechacomunicado.HasValue)
                    {
                        var fecha = c.fechacomunicado.Value.ToDateTime(TimeOnly.MinValue);
                        string diaSemana = cultura.TextInfo.ToTitleCase(fecha.ToString("dddd", cultura));
                        string dia = fecha.ToString("dd");
                        string mes = cultura.DateTimeFormat.GetAbbreviatedMonthName(fecha.Month);
                        mes = char.ToUpper(mes[0]) + mes.Substring(1);
                        string anio = fecha.ToString("yyyy");

                        comunicado.fechacomunicadoformateada = $"{diaSemana} {dia}/{mes}/{anio}";
                    }

                    return comunicado;
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ Endpoint con parámetro de torneo - devuelve comunicados del torneo específico
        [HttpGet("PorTorneo/{idTorneo}")]
        public IActionResult GetPorTorneo(int idTorneo)
        {
            try
            {
                var datosDB = (from c in _bd.Comunicados
                                where c.Idtorneo == idTorneo && c.Habilitado == 1
                                select new ComunicadoListCLS
                                {
                                    idcomunicado = c.Idcomunicado,
                                    comunicadocorto = c.Comunicadocorto,
                                    comunicadolargo = c.Comunicadolargo,
                                    fechacomunicado = c.Fechacomunicado,
                                    idtorneo = c.Idtorneo,
                                    habilitado = c.Habilitado
                                })
                                .OrderByDescending(o => o.fechacomunicado)  // ✅ ORDENAR POR FECHAS (más reciente primero)
                                .ToList();

                var cultura = new CultureInfo("es-ES");
                var resultado = datosDB.Select(c =>
                {
                    var comunicado = new ComunicadoListCLS
                    {
                        idcomunicado = c.idcomunicado,
                        comunicadocorto = c.comunicadocorto ?? string.Empty,
                        comunicadolargo = c.comunicadolargo ?? string.Empty,
                        fechacomunicado = c.fechacomunicado,
                        idtorneo = c.idtorneo,
                        habilitado = c.habilitado
                    };

                    if (c.fechacomunicado.HasValue)
                    {
                        var fecha = c.fechacomunicado.Value.ToDateTime(TimeOnly.MinValue);
                        string diaSemana = cultura.TextInfo.ToTitleCase(fecha.ToString("dddd", cultura));
                        string dia = fecha.ToString("dd");
                        string mes = cultura.DateTimeFormat.GetAbbreviatedMonthName(fecha.Month);
                        mes = char.ToUpper(mes[0]) + mes.Substring(1);
                        string anio = fecha.ToString("yyyy");

                        comunicado.fechacomunicadoformateada = $"{diaSemana} {dia}/{mes}/{anio}";
                    }

                    return comunicado;
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
