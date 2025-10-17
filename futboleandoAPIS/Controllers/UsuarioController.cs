using futboleandoAPIS.Models;
using futboleandoEntities;
using futboleandoEntities.Jugador;
using futboleandoEntities.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;
        public UsuarioController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = _bd.Usuarios.Where(p => p.Idtipousuario == 3).Select(p => new UsuarioListCLS
                {
                    idusuario = p.Idusuario,
                    nombre = p.Nombre,
                    idtipousuario = p.Idtipousuario ?? 0, // Si es null, asigna 0
                    visitas = p.Visitas ?? 0,
                    visitascel = p.Visitascel ?? 0

                }).ToList();
                return Ok(consulta);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("listarusuariosmasvisitascelular")]
        public IActionResult ListarUsuariosMasVisitasCelular()
        {
            try
            {
                var consulta = _bd.Usuarios.Where(p => p.Visitascel > p.Visitas).Select(p => new UsuarioMasVisitasCelularListCLS
                {
                    idusuario = p.Idusuario,
                    nombre = p.Nombre,
                    idtipousuario = p.Idtipousuario ?? 0, // Si es null, asigna 0
                    visitas = p.Visitas ?? 0,
                    visitascel = p.Visitascel ?? 0,
                    fechaalta = (DateTime)p.Fechaalta

                }).ToList();
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost]
        public IActionResult Post()
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
