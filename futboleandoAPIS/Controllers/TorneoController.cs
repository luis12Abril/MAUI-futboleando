using futboleandoAPIS.Models;
using futboleandoEntities.Torneo;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorneoController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;

        public TorneoController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = (from t in _bd.Torneos
                               join l in _bd.Ligas on t.Idliga equals l.Idliga
                               where t.Habilitado == 1 && t.Visible == 1
                               orderby t.Ordentorneo, t.Nombre
                               select new TorneoListCLS
                               {
                                   idtorneo = t.Idtorneo,
                                   nombre = t.Nombre ?? string.Empty,
                                   clavetorneo = t.Clavetorneo ?? string.Empty,
                                   idliga = t.Idliga ?? 0,
                                   nombreliga = l.Nombre ?? string.Empty,
                                   visible = t.Visible ?? 0
                               }).ToList();

                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("PorLiga/{idLiga}")]
        public IActionResult GetPorLiga(int idLiga)
        {
            try
            {
                var consulta = (from t in _bd.Torneos
                               join l in _bd.Ligas on t.Idliga equals l.Idliga
                               where t.Habilitado == 1 && t.Visible == 1 && t.Idliga == idLiga
                               orderby t.Ordentorneo, t.Nombre
                               select new TorneoListCLS
                               {
                                   idtorneo = t.Idtorneo,
                                   nombre = t.Nombre ?? string.Empty,
                                   clavetorneo = t.Clavetorneo ?? string.Empty,
                                   idliga = t.Idliga ?? 0,
                                   nombreliga = l.Nombre ?? string.Empty,
                                   visible = t.Visible ?? 0
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
                var torneo = (from t in _bd.Torneos
                             join l in _bd.Ligas on t.Idliga equals l.Idliga
                             where t.Idtorneo == id && t.Habilitado == 1
                             select new TorneoListCLS
                             {
                                 idtorneo = t.Idtorneo,
                                 nombre = t.Nombre ?? string.Empty,
                                 clavetorneo = t.Clavetorneo ?? string.Empty,
                                 idliga = t.Idliga ?? 0,
                                 nombreliga = l.Nombre ?? string.Empty,
                                 visible = t.Visible ?? 0
                             }).FirstOrDefault();

                if (torneo == null)
                    return NotFound();

                return Ok(torneo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
