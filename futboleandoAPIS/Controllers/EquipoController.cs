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

        // ✅ Endpoint sin parámetro - devuelve todos los equipos habilitados
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = (from j in _bd.Equipos
                                where j.Habilitado == 1
                                   && j.Nombre.Trim() != "_SIN EQUIPO"  // ✅ Excluir equipos sin asignar
                                select new EquipoListCLS
                                {
                                    idequipo = j.Idequipo,
                                    nombre = j.Nombre,
                                    representante = j.Representante,                                   
                                    golesfavor = j.Golesafavor,
                                    golescontra = j.Golesencontra,
                                    diferenciagoles = j.Difgoles,
                                    puntos = j.Puntos
                                }).OrderByDescending(e => e.puntos)
                                .ThenByDescending(e => e.diferenciagoles)
                                .ThenByDescending(e => e.golesfavor)
                                .ToList();
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ Endpoint con parámetro de torneo - devuelve equipos del torneo específico
        [HttpGet("PorTorneo/{idTorneo}")]
        public IActionResult GetPorTorneo(int idTorneo)
        {
            try
            {
                var consulta = (from j in _bd.Equipos
                                where j.Idtorneo == idTorneo 
                                   && j.Habilitado == 1
                                   && j.Nombre.Trim() != "_SIN EQUIPO"  // ✅ Excluir equipos sin asignar
                                select new EquipoListCLS
                                {
                                    idequipo = j.Idequipo,
                                    nombre = j.Nombre,
                                    representante = j.Representante,                                   
                                    golesfavor = j.Golesafavor,
                                    golescontra = j.Golesencontra,
                                    diferenciagoles = j.Difgoles,
                                    puntos = j.Puntos
                                }).OrderByDescending(e => e.puntos)  // ✅ ORDENAR POR PUNTOS (mayor a menor)
                                .ThenByDescending(e => e.diferenciagoles)  // ✅ Desempate por diferencia de goles
                                .ThenByDescending(e => e.golesfavor)  // ✅ Segundo desempate por goles a favor
                                .ToList();
                
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
