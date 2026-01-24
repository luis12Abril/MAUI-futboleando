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
                                select new
                                {
                                    j.Idequipo,
                                    j.Nombre,
                                    j.Representante,
                                    j.Fotoequipo,
                                    j.Golesafavor,
                                    j.Golesencontra,
                                    j.Difgoles,
                                    j.Puntos
                                }).AsEnumerable()  // ✅ Ejecutar consulta SQL primero
                                .OrderByDescending(e => e.Puntos ?? 0)
                                .ThenByDescending(e => e.Difgoles ?? 0)
                                .ThenByDescending(e => e.Golesafavor ?? 0)
                                .ToList();

                // ✅ Convertir a EquipoListCLS devolviendo foto como string Base64
                var resultado = consulta.Select(e => new EquipoListCLS
                {
                    idequipo = e.Idequipo,
                    nombre = e.Nombre,
                    representante = e.Representante,
                    foto = LimpiarFotoBase64(e.Fotoequipo),  // ✅ Devuelve string limpio
                    golesfavor = e.Golesafavor,
                    golescontra = e.Golesencontra,
                    diferenciagoles = e.Difgoles,
                    puntos = e.Puntos
                }).ToList();

                return Ok(resultado);
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
                                select new
                                {
                                    j.Idequipo,
                                    j.Nombre,
                                    j.Representante,
                                    j.Fotoequipo,
                                    j.Golesafavor,
                                    j.Golesencontra,
                                    j.Difgoles,
                                    j.Puntos
                                }).AsEnumerable()  // ✅ Ejecutar consulta SQL primero
                                .OrderByDescending(e => e.Puntos ?? 0)
                                .ThenByDescending(e => e.Difgoles ?? 0)
                                .ThenByDescending(e => e.Golesafavor ?? 0)
                                .ToList();

                // ✅ Convertir a EquipoListCLS devolviendo foto como string Base64
                var resultado = consulta.Select(e => new EquipoListCLS
                {
                    idequipo = e.Idequipo,
                    nombre = e.Nombre,
                    representante = e.Representante,
                    foto = LimpiarFotoBase64(e.Fotoequipo),  // ✅ Devuelve string limpio
                    golesfavor = e.Golesafavor,
                    golescontra = e.Golesencontra,
                    diferenciagoles = e.Difgoles,
                    puntos = e.Puntos
                }).ToList();
                
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ Método auxiliar para limpiar foto Base64 (elimina prefijo data:image)
        private string LimpiarFotoBase64(string fotoBase64)
        {
            if (string.IsNullOrWhiteSpace(fotoBase64))
                return null;

            try
            {
                // ✅ Eliminar el prefijo "data:image/jpeg;base64," si existe
                string base64Limpio = fotoBase64;
                
                if (fotoBase64.StartsWith("data:image/"))
                {
                    var indexComa = fotoBase64.IndexOf(",");
                    if (indexComa > 0)
                    {
                        base64Limpio = fotoBase64.Substring(indexComa + 1);
                    }
                }

                return base64Limpio;
            }
            catch (Exception)
            {
                // Si falla, devolver null
                return null;
            }
        }
    }
}
