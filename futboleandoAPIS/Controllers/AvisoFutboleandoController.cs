using System;
using System.Linq;
using futboleandoAPIS.Models;
using futboleandoEntities.Aviso;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvisoFutboleandoController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;

        public AvisoFutboleandoController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var aviso = _bd.Avisofutboleandos
                    .OrderByDescending(a => a.Idavisofutboleando)
                    .Select(a => new AvisoFutboleandoCLS
                    {
                        idavisofutboleando = a.Idavisofutboleando,
                        titulomensaje = a.Titulomensaje,
                        mensaje = a.Mensaje,
                        fechamensaje = a.Fechamensaje.HasValue
                            ? a.Fechamensaje.Value.ToDateTime(TimeOnly.MinValue)
                            : null,
                        habilitado = a.Habilitado
                    })
                    .FirstOrDefault();

                if (aviso == null)
                {
                    return Ok(new AvisoFutboleandoCLS());
                }

                return Ok(aviso);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Telefono")]
        public IActionResult GetTelefono()
        {
            try
            {
                var telefono = _bd.Avisofutboleandos
                    .OrderByDescending(a => a.Idavisofutboleando)
                    .Select(a => a.Titulomensaje)
                    .FirstOrDefault();

                return Ok(telefono ?? string.Empty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
