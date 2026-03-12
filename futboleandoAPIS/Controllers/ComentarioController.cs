using futboleandoAPIS.Models;
using futboleandoEntities.Comentario;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComentarioController : ControllerBase
{
    private readonly DbA85d0bFutboleandobdContext _bd;

    public ComentarioController(DbA85d0bFutboleandobdContext bd)
    {
        _bd = bd;
    }

    [HttpGet("PorJuego/{idJuego}")]
    public IActionResult GetComentariosPorJuego(int idJuego)
    {
        try
        {
            var comentarios = (from c in _bd.Comentarios
                    join u in _bd.Usuarios on c.Idusuario equals u.Idusuario
                    where c.Idjuego == idJuego && c.Habilitado == 1
                    orderby c.Fechacomentario descending
                    select new ComentarioCLS
                    {
                        idcomentario = c.Idcomentario,
                        idjuego = c.Idjuego ?? 0,
                        comentario = c.Comentario1 ?? string.Empty,
                        idusuario = c.Idusuario ?? 0,
                        nombreusuario = u.Nombre ?? "",
                        fechacomentario = c.Fechacomentario ?? DateTime.MinValue
                    })
                .Take(30)
                .ToList();

            return Ok(comentarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public IActionResult PostComentario([FromBody] ComentarioCreateCLS comentario)
    {
        try
        {
            if (comentario == null || comentario.idjuego <= 0 || comentario.idusuario <= 0)
            {
                return BadRequest("Datos incompletos");
            }

            var texto = comentario.comentario?.Trim();
            if (string.IsNullOrWhiteSpace(texto))
            {
                return BadRequest("El comentario es requerido");
            }

            if (texto.Length > 100)
            {
                return BadRequest("El comentario supera los 100 caracteres");
            }

            var totalComentarios = _bd.Comentarios.Count(c => c.Idjuego == comentario.idjuego && c.Habilitado == 1);
            if (totalComentarios >= 30)
            {
                return Conflict("Se alcanzó el máximo de comentarios para este juego");
            }

            var entidad = new Comentario
            {
                Idjuego = comentario.idjuego,
                Idusuario = comentario.idusuario,
                Comentario1 = texto,
                Fechacomentario = DateTime.Now,
                Habilitado = 1
            };

            _bd.Comentarios.Add(entidad);
            _bd.SaveChanges();

            var nombreUsuario = _bd.Usuarios
                .Where(u => u.Idusuario == comentario.idusuario)
                .Select(u => u.Nombre)
                .FirstOrDefault() ?? string.Empty;

            var response = new ComentarioCLS
            {
                idcomentario = entidad.Idcomentario,
                idjuego = entidad.Idjuego ?? 0,
                comentario = entidad.Comentario1 ?? string.Empty,
                idusuario = entidad.Idusuario ?? 0,
                nombreusuario = nombreUsuario,
                fechacomentario = entidad.Fechacomentario ?? DateTime.MinValue
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
