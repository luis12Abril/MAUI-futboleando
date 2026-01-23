using futboleandoAPIS.Models;
using futboleandoEntities.Estado;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;

        public EstadoController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // ? Primero obtener todos sin filtro para debug
                var todosLosEstados = _bd.Estados.ToList();
                
                var consulta = _bd.Estados
                    .Where(e => e.Habilitado == 1)
                    .OrderBy(e => e.Nombre)
                    .Select(e => new EstadoListCLS
                    {
                        idestado = e.Idestado,
                        nombre = e.Nombre ?? string.Empty
                    })
                    .ToList();

                // ? Si no hay estados habilitados, devolver todos para pruebas
                if (consulta.Count == 0)
                {
                    consulta = _bd.Estados
                        .OrderBy(e => e.Nombre)
                        .Select(e => new EstadoListCLS
                        {
                            idestado = e.Idestado,
                            nombre = e.Nombre ?? string.Empty
                        })
                        .ToList();
                }

                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ? Endpoint de prueba para verificar conexión
        [HttpGet("test")]
        public IActionResult Test()
        {
            try
            {
                var count = _bd.Estados.Count();
                var habilitados = _bd.Estados.Count(e => e.Habilitado == 1);
                
                return Ok(new 
                { 
                    message = "Conexión exitosa", 
                    totalEstados = count,
                    estadosHabilitados = habilitados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var estado = _bd.Estados
                    .Where(e => e.Idestado == id && e.Habilitado == 1)
                    .Select(e => new EstadoListCLS
                    {
                        idestado = e.Idestado,
                        nombre = e.Nombre ?? string.Empty
                    })
                    .FirstOrDefault();

                if (estado == null)
                    return NotFound();

                return Ok(estado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
