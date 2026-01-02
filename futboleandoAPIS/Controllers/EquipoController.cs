using futboleandoAPIS.Models;
using futboleandoEntities.Equipo;
using futboleandoEntities.Jugador;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;
        public EquipoController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = (from j in _bd.Equipos                               
                              
                                select new EquipoListCLS
                                {
                                    idequipo = j.Idequipo,
                                    nombre = j.Nombre
                                }).ToList();
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }   
    }
}
