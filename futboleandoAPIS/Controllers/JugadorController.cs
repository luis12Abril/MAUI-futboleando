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
                                where j.Idtorneo == 1068
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,  
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno,
                                    apmaterno = j.Apmaterno,    
                                    combrecompleto = j.Nombre + " " + j.Appaterno + " " + j.Apmaterno,                                    
                                    nombreequipo = e.Nombre                                   
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
                return Ok();
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
