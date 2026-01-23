using futboleandoAPIS.Models;
using futboleandoEntities;
using futboleandoEntities.Jugador;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using futboleandoEntities.Equipo;
using Microsoft.EntityFrameworkCore;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;
        public JugadorController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        // ✅ Endpoint sin parámetro - devuelve todos los jugadores habilitados
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = (from j in _bd.Jugadors
                                join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                                where j.Habilitado == 1 
                                   && j.Nombre.Trim() != "GOL A FAVOR DEL EQUIPO"  // ✅ Excluir autogoles
                                   && e.Nombre.Trim() != "_SIN EQUIPO"  // ✅ También excluye jugadores sin equipo
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno,
                                    apmaterno = j.Apmaterno,
                                    nombreequipo = e.Nombre,
                                    nombrecompleto = (j.Nombre.Trim() + " " + j.Appaterno.Trim() + " " + j.Apmaterno.Trim()).Trim(),
                                    fnacimiento = (DateOnly)j.Fnacimiento,
                                    goles = j.Goles ?? 0
                                })
                                .OrderByDescending(j => j.goles)
                                .ThenBy(j => j.nombrecompleto)
                                .ToList();
                
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ Endpoint con parámetro de torneo - devuelve jugadores del torneo específico
        [HttpGet("PorTorneo/{idTorneo}")]
        public IActionResult GetPorTorneo(int idTorneo)
        {
            try
            {
                var consulta = (from j in _bd.Jugadors
                                join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                                where j.Idtorneo == idTorneo 
                                   && j.Habilitado == 1
                                   && j.Nombre.Trim() != "GOL A FAVOR DEL EQUIPO"  // ✅ Excluir autogoles
                                   && e.Nombre.Trim() != "_SIN EQUIPO"  // ✅ También excluye jugadores sin equipo
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno,
                                    apmaterno = j.Apmaterno,
                                    nombreequipo = e.Nombre,
                                    nombrecompleto = (j.Nombre.Trim() + " " + j.Appaterno.Trim() + " " + j.Apmaterno.Trim()).Trim(),
                                    fnacimiento = (DateOnly)j.Fnacimiento,
                                    goles = j.Goles ?? 0
                                })
                                .OrderByDescending(j => j.goles)  // ✅ Ordenar por goles (goleadores primero)
                                .ThenBy(j => j.nombrecompleto)    // ✅ Desempate alfabético
                                .ToList();
                
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var obj = _bd.Jugadors.FirstOrDefault(j => j.Idjugador == id);
                if (obj == null)
                {
                    return NotFound();
                }
                return Ok(new JugadorFormCLS
                {
                    idjugador = obj.Idjugador,
                    nombre = obj.Nombre,
                    appaterno = obj.Appaterno
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("listarjugadoresporequipo/{idequipo}")]
        public IActionResult ListarJugadoresPorEquipo(int idequipo)
        {
            try
            {
                var consulta = (from j in _bd.Jugadors
                                join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                                where j.Idequipo == idequipo && j.Habilitado == 1
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno
                                }).ToList();
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]JugadorFormCLS oJugadorFormCLS)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var obJugador = _bd.Jugadors.FirstOrDefault(j => j.Idjugador == id);
                if (obJugador == null)
                {
                    return NotFound();
                }
                obJugador.Habilitado = 0;                
                _bd.SaveChanges();
                return Ok("Se elimino correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
