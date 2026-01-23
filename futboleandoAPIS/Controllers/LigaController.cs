using futboleandoAPIS.Models;
using futboleandoEntities.Liga;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LigaController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;

        public LigaController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = (from l in _bd.Ligas
                               join m in _bd.Municipios on l.Idmunicipio equals m.Idmunicipio
                               where l.Habilitado == 1
                               orderby l.Nombre
                               select new LigaListCLS
                               {
                                   idliga = l.Idliga,
                                   nombre = l.Nombre ?? string.Empty,
                                   idmunicipio = l.Idmunicipio ?? 0,
                                   nombremunicipio = m.Nombre ?? string.Empty
                               }).ToList();

                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("PorMunicipio/{idMunicipio}")]
        public IActionResult GetPorMunicipio(int idMunicipio)
        {
            try
            {
                var consulta = (from l in _bd.Ligas
                               join m in _bd.Municipios on l.Idmunicipio equals m.Idmunicipio
                               where l.Habilitado == 1 && l.Idmunicipio == idMunicipio
                               orderby l.Nombre
                               select new LigaListCLS
                               {
                                   idliga = l.Idliga,
                                   nombre = l.Nombre ?? string.Empty,
                                   idmunicipio = l.Idmunicipio ?? 0,
                                   nombremunicipio = m.Nombre ?? string.Empty
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
