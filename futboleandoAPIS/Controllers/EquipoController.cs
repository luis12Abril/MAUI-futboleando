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
                                where j.Idtorneo == 1052
                                select new EquipoListCLS
                                {
                                    idequipo = j.Idequipo,
                                    nombre = j.Nombre,
                                    representante = j.Representante,                                   
                                    golesfavor = j.Golesafavor,
                                    golescontra = j.Golesencontra,
                                    puntos = j.Puntos
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
