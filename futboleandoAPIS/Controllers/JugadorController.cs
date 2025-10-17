using futboleandoAPIS.Models;
using futboleandoEntities;
using futboleandoEntities.Jugador;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using futboleandoEntities.Equipo;

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

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = (from j in _bd.Jugadors
                                join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                                where j.Idtorneo > 1
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,  
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno,
                                    apmaterno = j.Apmaterno,    
                                    nombrecompleto = (j.Nombre.Trim() + " " + j.Appaterno.Trim() + " " + j.Apmaterno.Trim()).Trim(),
                                    idequipo = e.Idequipo,
                                    nombreequipo = e.Nombre,
                                    fechanacimiento = (DateOnly)j.Fnacimiento
                                }).ToList();
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
                    appaterno = obj.Appaterno,
                    apmaterno = obj.Apmaterno,
                    nombrecompleto = (obj.Nombre.Trim() + " " + obj.Appaterno.Trim() + " " + obj.Apmaterno.Trim()).Trim()
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
                                where j.Idequipo == idequipo 
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno,
                                    apmaterno = j.Apmaterno,
                                    nombrecompleto = (j.Nombre.Trim() + " " + j.Appaterno.Trim() + " " + j.Apmaterno.Trim()).Trim(),
                                    idequipo = e.Idequipo,
                                    nombreequipo = e.Nombre,
                                    fechanacimiento = (DateOnly)j.Fnacimiento
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
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
