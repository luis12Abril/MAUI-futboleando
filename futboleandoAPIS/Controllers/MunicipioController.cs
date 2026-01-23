using futboleandoAPIS.Models;
using futboleandoEntities.Municipio;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipioController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;

        public MunicipioController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = (from m in _bd.Municipios
                               join e in _bd.Estados on m.Idestado equals e.Idestado
                               where m.Habilitado == 1
                               orderby m.Nombre
                               select new MunicipioListCLS
                               {
                                   idmunicipio = m.Idmunicipio,
                                   nombre = m.Nombre ?? string.Empty,
                                   idestado = m.Idestado ?? 0,
                                   nombreestado = e.Nombre ?? string.Empty
                               }).ToList();

                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("PorEstado/{idEstado}")]
        public IActionResult GetPorEstado(int idEstado)
        {
            try
            {
                var consulta = (from m in _bd.Municipios
                               join e in _bd.Estados on m.Idestado equals e.Idestado
                               where m.Habilitado == 1 && m.Idestado == idEstado
                               orderby m.Nombre
                               select new MunicipioListCLS
                               {
                                   idmunicipio = m.Idmunicipio,
                                   nombre = m.Nombre ?? string.Empty,
                                   idestado = m.Idestado ?? 0,
                                   nombreestado = e.Nombre ?? string.Empty
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
