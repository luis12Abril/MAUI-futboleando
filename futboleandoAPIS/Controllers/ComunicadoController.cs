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

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // PASO 1: Obtener datos de la BD
                var datosDB = (from c in _bd.Comunicados
                                where c.Idtorneo == 1038
                                select new ComunicadoListCLS
                                {
                                    idcomunicado = c.Idcomunicado,
                                    comunicadocorto = c.Comunicadocorto,
                                    comunicadolargo = c.Comunicadolargo,
                                    fechacomunicado = c.Fechacomunicado,
                                    idtorneo = c.Idtorneo,
                                    habilitado = c.Habilitado

                                    
                                }).OrderByDescending(o => o.fechacomunicado)  // ✅ ORDENAR POR FECHAS (mayor a menor)
                                
                                .ToList();


                // PASO 2: Formatear fechas en memoria
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

                    // ✅ Asignar fecha formateada a la propiedad calculada
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

                //Return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
